using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;


namespace TourneeFutee
{
    /// <summary>
    /// Service de persistance permettant de sauvegarder et charger
    /// des graphes et des tournées dans une base de données MySQL.
    /// </summary>
    public class ServicePersistance
    {
        // ─────────────────────────────────────────────────────────────────────
        // Attributs privés
        // ─────────────────────────────────────────────────────────────────────

        private readonly string _connectionString;

        // TODO : si vous avez besoin de maintenir une connexion ouverte,
        //        ajoutez un attribut MySqlConnection ici.

        // ─────────────────────────────────────────────────────────────────────
        // Constructeur
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Instancie un service de persistance et se connecte automatiquement
        /// à la base de données <paramref name="dbname"/> sur le serveur
        /// à l'adresse IP <paramref name="serverIp"/>.
        /// Les identifiants sont définis par <paramref name="user"/> (utilisateur)
        /// et <paramref name="pwd"/> (mot de passe).
        /// </summary>
        /// <param name="serverIp">Adresse IP du serveur MySQL.</param>
        /// <param name="dbname">Nom de la base de données.</param>
        /// <param name="user">Nom d'utilisateur.</param>
        /// <param name="pwd">Mot de passe.</param>
        /// <exception cref="Exception">Levée si la connexion échoue.</exception>
        public ServicePersistance(string serverIp, string dbname, string user, string pwd)
        {
            // TODO : initialiser et ouvrir la connexion à la base de données
            // Exemple :
            _connectionString = $"server={serverIp};database={dbname};uid={user};pwd={pwd};";
            using var connection = new MySqlConnection(_connectionString); 
            // using var permet de ne pas oublier de fermer la connexion et de ne pas utiliser finally connexion.Close
            try
            {
                connection.Open();
                Console.WriteLine("Connected to Tournee Futee Database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }      
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes publiques
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Sauvegarde le graphe <paramref name="g"/> en base de données
        /// (sommets et arcs inclus) et renvoie son identifiant.
        /// </summary>
        /// <param name="g">Le graphe à sauvegarder.</param>
        /// <returns>Identifiant du graphe en base de données (AUTO_INCREMENT).</returns>
        public uint SaveGraph(Graph g)
        {
            // TODO : implémenter la sauvegarde du graphe
            //
            // Ordre recommandé :
            //   1. INSERT dans la table Graphe -> récupérer l'id avec LAST_INSERT_ID()
            //   2. Pour chaque sommet de g : INSERT dans Sommet (valeur + graphe_id)
            //      -> conserver la correspondance sommet C# <-> id BdD
            //   3. Pour chaque arc de la matrice d'adjacence (poids != +inf) :
            //      INSERT dans Arc (sommet_source_id, sommet_dest_id, poids, graphe_id)
            //
            // Exemple pour récupérer l'id généré :
            //   uint id = Convert.ToUInt32(cmd.ExecuteScalar());
            using (var conn = OpenConnection())
            {
                //Insérer le graphe
                string sqlGraphe = @"INSERT INTO Graphe (est_oriente, nom, nb_sommet) VALUES (@oriente, @nom, @nb); SELECT LAST_INSERT_ID();";
                var cmdG = new MySqlCommand(sqlGraphe, conn);
                cmdG.Parameters.AddWithValue("@oriente", g.Directed ? 1 : 0);
                cmdG.Parameters.AddWithValue("@nom", "Graphe");
                cmdG.Parameters.AddWithValue("@nb", g.Order);
                uint grapheId = Convert.ToUInt32(cmdG.ExecuteScalar()); // Récupérer l'id du graphe inséré pour l'associer aux sommets et arcs

                //Insérer les sommets triés par indice matriciel
                // On mémorise indice matriciel → id BdD pour l'insertion des arcs
                var idBdDParIndice = new Dictionary<int, uint>();
                foreach (var kv in g.VertexIndices.OrderBy(kv => kv.Value))
                {
                    string nom = kv.Key;
                    int indice = kv.Value;
                    float valeur = g.GetVertexValue(nom);
                    string sqlSommet = @"INSERT INTO Sommet (graphe_id, nom, valeur, indice) VALUES (@gid, @nom, @val, @idx); SELECT LAST_INSERT_ID();";
                    var cmdS = new MySqlCommand(sqlSommet, conn);
                    cmdS.Parameters.AddWithValue("@gid", grapheId);
                    cmdS.Parameters.AddWithValue("@nom", nom);
                    cmdS.Parameters.AddWithValue("@val", valeur);
                    cmdS.Parameters.AddWithValue("@idx", indice);
                    idBdDParIndice[indice] = Convert.ToUInt32(cmdS.ExecuteScalar()); // Récupérer l'id du sommet inséré pour l'associer à son indice dans la matrice
                }

                //Insérer les arcs
                for (int i = 0; i < g.Order; i++)
                {
                    for (int j = 0; j < g.Order; j++)
                    {
                        if (i == j) continue;
                        // Graphe non orienté : on ne stocke que i < j
                        if (!g.Directed && j < i) continue;
                        float poids = g.Adajcence.GetValue(i, j);
                        // Un arc existe ssi son poids diffère de NoEdgeValue
                        if (poids != g.NoEdgeValue)
                        {
                            string sqlArc = @"INSERT INTO Arc (graphe_id, sommet_source, sommet_dest, poids) VALUES (@gid, @src, @dst, @p);"; 
                            var cmdA = new MySqlCommand(sqlArc, conn);
                            cmdA.Parameters.AddWithValue("@gid", grapheId);
                            cmdA.Parameters.AddWithValue("@src", idBdDParIndice[i]);
                            cmdA.Parameters.AddWithValue("@dst", idBdDParIndice[j]);
                            cmdA.Parameters.AddWithValue("@p", poids);
                            cmdA.ExecuteNonQuery();  
                        }
                    }
                }
                return grapheId;
            }
        }

        /// <summary>
        /// Charge depuis la base de données le graphe identifié par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Graph"/>.
        /// </summary>
        /// <param name="id">Identifiant du graphe à charger.</param>
        /// <returns>Instance de <see cref="Graph"/> reconstituée.</returns>
        public Graph LoadGraph(uint id)
        {
            // TODO : implémenter le chargement du graphe
            //
            // Ordre recommandé :
            //   1. SELECT dans Graphe WHERE id = @id -> récupérer IsOriented, etc.
            //   2. SELECT dans Sommet WHERE graphe_id = @id -> reconstruire les sommets
            //      (respecter l'ordre d'insertion pour que les indices de la matrice
            //       correspondent à ceux sauvegardés)
            //   3. SELECT dans Arc WHERE graphe_id = @id -> reconstruire la matrice
            //      d'adjacence en utilisant les correspondances sommet_id <-> indice
            using (var conn = OpenConnection())
            {
                //Données du graphe
                bool estOriente = false;
                string sqlG = "SELECT est_oriente FROM Graphe WHERE id = @id;";
                var cmdG = new MySqlCommand(sqlG, conn);
                cmdG.Parameters.AddWithValue("@id", id);
                using (var r = cmdG.ExecuteReader())
                {
                    if (!r.Read())
                        throw new Exception($"Graphe id={id} introuvable en base.");
                    estOriente = r.GetInt32("est_oriente") == 1;
                }

                //Charger les sommets triés par indice
                var nomParIndice = new SortedDictionary<int, string>();
                var valeurParIndice = new Dictionary<int, float>();
                var nomParIdBdD = new Dictionary<uint, string>();
                string sqlS = @"SELECT id, nom, valeur, indice FROM Sommet WHERE graphe_id = @gid ORDER BY indice;";
                var cmdS = new MySqlCommand(sqlS, conn);
                cmdS.Parameters.AddWithValue("@gid", id);
                using (var r = cmdS.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int idx = r.GetInt32("indice");
                        uint sid = r.GetUInt32("id");
                        string nm = r.GetString("nom");
                        float val = r.IsDBNull(r.GetOrdinal("valeur")) ? 0f : r.GetFloat("valeur"); // Traiter le cas où la valeur est null en base (si la colonne autorise null)
                        nomParIndice[idx] = nm;
                        valeurParIndice[idx] = val;
                        nomParIdBdD[sid] = nm;
                    }
                }

                //Construire le graphe et ajouter les sommets
                Graph g = new Graph(estOriente, 0);
                foreach (var kv in nomParIndice)
                {
                    g.AddVertex(kv.Value, valeurParIndice[kv.Key]);
                }
               
                //Charger les arcs et reconstruire la matrice
                string sqlA = @"SELECT sommet_source, sommet_dest, poids FROM Arc WHERE graphe_id = @gid;";
                var cmdA = new MySqlCommand(sqlA, conn);
                cmdA.Parameters.AddWithValue("@gid", id);
                using (var r = cmdA.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string src = nomParIdBdD[r.GetUInt32("sommet_source")];
                        string dst = nomParIdBdD[r.GetUInt32("sommet_dest")];
                        float poids = r.GetFloat("poids");
                        g.AddEdge(src, dst, poids); // La méthode AddEdge gère à la fois les graphes orientés et non orientés
                    }
                }
                return g;
            }
        }

        /// <summary>
        /// Sauvegarde la tournée <paramref name="t"/> (effectuée dans le graphe
        /// identifié par <paramref name="graphId"/>) en base de données
        /// et renvoie son identifiant.
        /// </summary>
        /// <param name="graphId">Identifiant BdD du graphe dans lequel la tournée a été calculée.</param>
        /// <param name="t">La tournée à sauvegarder.</param>
        /// <returns>Identifiant de la tournée en base de données (AUTO_INCREMENT).</returns>
        public uint SaveTour(uint graphId, Tour t)
        {
            using (var conn = OpenConnection())
            {
                string sqlTour = @"INSERT INTO Tournee (graphe_id, cout_total) VALUES (@gid, @cout); SELECT LAST_INSERT_ID();";
                var cmdT = new MySqlCommand(sqlTour, conn);
                cmdT.Parameters.AddWithValue("@gid", graphId);
                cmdT.Parameters.AddWithValue("@cout", t.Cost);
                uint tourId = Convert.ToUInt32(cmdT.ExecuteScalar());
                var nomVersId = new Dictionary<string, uint>();
                string sqlSommet = "SELECT id, nom FROM Sommet WHERE graphe_id = @gid;";
                var cmdS = new MySqlCommand(sqlSommet, conn);
                cmdS.Parameters.AddWithValue("@gid", graphId);
                using (var r = cmdS.ExecuteReader())
                {
                    while (r.Read())
                    {
                        nomVersId[r.GetString("nom")] = r.GetUInt32("id"); // Mémoriser la correspondance nom sommet 
                    }
                }
                for (int i = 0; i < t.Vertices.Count; i++)
                {
                    string sqlEtape = @"INSERT INTO EtapeTournee (tournee_id, numero_ordre, sommet_id) VALUES (@tid, @ordre, @sid);";
                    var cmdE = new MySqlCommand(sqlEtape, conn);
                    cmdE.Parameters.AddWithValue("@tid", tourId);
                    cmdE.Parameters.AddWithValue("@ordre", i);
                    cmdE.Parameters.AddWithValue("@sid", nomVersId[t.Vertices[i]]); // Associer le sommet de l'étape à son id en base de données
                    cmdE.ExecuteNonQuery();
                }
                return tourId;
            }
        }

        /// <summary>
        /// Charge depuis la base de données la tournée identifiée par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Tour"/>.
        /// </summary>
        /// <param name="id">Identifiant de la tournée à charger.</param>
        /// <returns>Instance de <see cref="Tour"/> reconstituée.</returns>
        public Tour LoadTour(uint id)
        {
            using (var conn = OpenConnection())
            {
                uint graphId;
                float coutTotal;
                string sqlTour = "SELECT graphe_id, cout_total FROM Tournee WHERE id = @id;";
                var cmdT = new MySqlCommand(sqlTour, conn);
                cmdT.Parameters.AddWithValue("@id", id);
                using (var r = cmdT.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        throw new Exception($"Tournée id={id} introuvable.");
                    }
                    graphId = r.GetUInt32("graphe_id");
                    coutTotal = r.GetFloat("cout_total");
                }
                var idVersNom = new Dictionary<uint, string>();
                string sqlSommet = "SELECT id, nom FROM Sommet WHERE graphe_id = @gid;";
                var cmdS = new MySqlCommand(sqlSommet, conn);
                cmdS.Parameters.AddWithValue("@gid", graphId);
                using (var r = cmdS.ExecuteReader())
                {
                    while (r.Read())
                    {
                        idVersNom[r.GetUInt32("id")] = r.GetString("nom"); // Mémoriser la correspondance id sommet 
                    }
                }
                List<string> sequence = new List<string>();
                string sqlEtape = @"SELECT sommet_id FROM EtapeTournee WHERE tournee_id = @tid ORDER BY numero_ordre;";
                var cmdE = new MySqlCommand(sqlEtape, conn);
                cmdE.Parameters.AddWithValue("@tid", id);
                using (var r = cmdE.ExecuteReader())
                {
                    while (r.Read())
                    {
                        uint sommetId = r.GetUInt32("sommet_id");
                        sequence.Add(idVersNom[sommetId]);
                    }
                }
                return new Tour(sequence, coutTotal); // Utilise un constructeur de Tour qui prend la séquence de sommets et le coût total 
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes utilitaires privées (à compléter selon vos besoins)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Crée et retourne une nouvelle connexion MySQL ouverte.
        /// Encadrez toujours l'appel dans un bloc using pour garantir la fermeture.
        /// </summary>
        private MySqlConnection OpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
