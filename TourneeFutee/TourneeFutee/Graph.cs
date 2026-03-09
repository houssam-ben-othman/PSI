namespace TourneeFutee
{
    public class Graph
    {


        private bool directed; // graphe orienté ou non
        private int order; // nombre de sommets du graphe (obligatoirement un entier)
        private float noEdgeValue; // poids de l'arc
        private Matrix adjacence; // matrice d'adjacence du graphe
        private Dictionary<string, int> vertexIndices; // dictionnaire associant à chaque nom de sommet son indice dans la matrice d'adjacence
        private Dictionary<string, float> vertexValues; // dictionnaire associant à chaque nom de sommet sa valeur


        // --- Construction du graphe ---

        // Contruit un graphe (`directed`=true => orienté)
        // La valeur `noEdgeValue` est le poids modélisant l'absence d'un arc (0 par défaut)
        public Graph(bool directed, float noEdgeValue = 0)
        {
            this.directed = directed;
            this.noEdgeValue = noEdgeValue;
            this.order = 0;
            this.adjacence = new Matrix(0, 0, noEdgeValue);
            this.vertexIndices = new Dictionary<string, int>();
            this.vertexValues = new Dictionary<string, float>();
        }


        // --- Propriétés ---

        // Propriété : ordre du graphe
        // Lecture seule
        public int Order
        {
            get { return this.order; }    
                    // pas de set
        }

        // Propriété : graphe orienté ou non
        // Lecture seule
        public bool Directed
        {
            get{return this.directed; }   
                    // pas de set
        }
        public float NoEdgeValue
        {
            get{return this.noEdgeValue; }
            set{this.noEdgeValue = value; }// Rmq : Pourrais être supprimé si pas utile
        }
        public Matrix Adajcence
        {
            get { return this.adjacence; }
            set { this.adjacence = value; }     
        }   
        public Dictionary<string, int> VertexIndices
        {
            get { return this.vertexIndices; }
            set{this.vertexIndices = value; }// Rmq : Pourrais être supprimé si pas utile
        }
        public Dictionary<string, float> VertexValues
        {
            get { return this.vertexValues; }
            set { this.vertexValues = value; }// Rmq : Pourrais être supprimé si pas utile
        }


        // --- Gestion des sommets ---

        // Ajoute le sommet de nom `name` et de valeur `value` (0 par défaut) dans le graphe
        // Lève une ArgumentException s'il existe déjà un sommet avec le même nom dans le graphe
        public void AddVertex(string name, float value = 0)
        {
            if (vertexIndices.ContainsKey(name))
                throw new ArgumentException("A vertex with the same name already exists.", nameof(name));
            int newIndex = order;
            adjacence.AddRow(newIndex);
            adjacence.AddColumn(newIndex);
            vertexIndices[name] = newIndex;
            vertexValues[name] = value;
            order++;
        }


        // Supprime le sommet de nom `name` du graphe (et tous les arcs associés)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void RemoveVertex(string name)
        {
            // TODO : implémenter
            if (!vertexIndices.TryGetValue(name, out int index))
                throw new ArgumentException("Sommet non trouvé", nameof(name));
            adjacence.RemoveRow(index);  
            adjacence.RemoveColumn(index); 
            vertexIndices.Remove(name);
            vertexValues.Remove(name);
            order--;
        }

        // Renvoie la valeur du sommet de nom `name`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public float GetVertexValue(string nomsommet) // O(1)
        {
            if (this.vertexValues.ContainsKey(nomsommet)==false) // O(1)
            {
                throw new ArgumentException("Le sommet n'existe pas dans le graphe."); // O(1)
            }
            return this.vertexValues[nomsommet];// O(1)
        }

        // Affecte la valeur du sommet de nom `name` à `value`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void SetVertexValue(string nomsommet, float valeur) // O(1)
        {
            if (this.vertexValues.ContainsKey(nomsommet)==false) // O(1)
            {
                throw new ArgumentException("Le sommet n'existe pas dans le graphe."); // O(1)
            }
            this.vertexValues[nomsommet] = valeur; // O(1)
        }


        // Renvoie la liste des noms des voisins du sommet de nom `vertexName`
        // (si ce sommet n'a pas de voisins, la liste sera vide)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public List<string> GetNeighbors(string vertexName)
        {
            List<string> neighborNames = new List<string>();

            // TODO : implémenter
            if (!vertexIndices.TryGetValue(vertexName, out int vertexIndex))
                throw new ArgumentException("Sommet non trouvé", nameof(vertexName));
            for (int j = 0; j < order; j++)
            {
                float weight = adjacence.GetValue(vertexIndex, j);

                if (weight != noEdgeValue)
                {
                    string neighborName = vertexIndices.FirstOrDefault(kvp => kvp.Value == j).Key;
                    if (neighborName != null)
                        neighborNames.Add(neighborName);
                }
            }
            return neighborNames;
        }

        // --- Gestion des arcs ---

        /* Ajoute un arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`, avec le poids `weight` (1 par défaut)
         * Si le graphe n'est pas orienté, ajoute aussi l'arc inverse, avec le même poids
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - il existe déjà un arc avec ces extrémités
         */
        public void AddEdge(string sourceName, string destinationName, float weight = 1)
        {
            // TODO : implémenter
            if (!vertexIndices.TryGetValue(sourceName, out int sourceIndex) ||
                !vertexIndices.TryGetValue(destinationName, out int destIndex))
                throw new ArgumentException("Sommet(s) non trouvé(s)");
            if (adjacence.GetValue(sourceIndex, destIndex) != noEdgeValue)
                throw new ArgumentException("Arc déjà existant");
            adjacence.SetValue(sourceIndex, destIndex, weight);
            if (directed==false)
                adjacence.SetValue(destIndex, sourceIndex, weight);
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName)
        {
            // TODO : implémenter
            if (!vertexIndices.TryGetValue(sourceName, out int sourceIndex) ||
                !vertexIndices.TryGetValue(destinationName, out int destIndex))
                throw new ArgumentException("Sommet(s) non trouvé(s)");
            if (adjacence.GetValue(sourceIndex, destIndex) == noEdgeValue)
                throw new ArgumentException("Arc inexistant");
            adjacence.SetValue(sourceIndex, destIndex, noEdgeValue);
            if (directed==false)
                adjacence.SetValue(destIndex, sourceIndex, noEdgeValue);
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName)
        {
            if (this.vertexIndices.TryGetValue(sourceName, out int sourceIndex)==false)
            {
                throw new ArgumentException("Le sommet source n'existe pas.", nameof(sourceName));
            }
            if (this.vertexIndices.TryGetValue(destinationName, out int destIndex)==false)
            {
                throw new ArgumentException("Le sommet destination n'existe pas.", nameof(destinationName));
            }
            float weight = this.adjacence.GetValue(sourceIndex, destIndex);
            if (weight == this.noEdgeValue)
            {
                throw new ArgumentException("L'arc n'existe pas entre ces deux sommets.");
            }
            return weight;
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight)
        {
            if (this.vertexIndices.TryGetValue(sourceName, out int sourceIndex)==false)
            {
                throw new ArgumentException("Le sommet source n'existe pas.", nameof(sourceName));
            }
            if (this.vertexIndices.TryGetValue(destinationName, out int destIndex)==false)
            {
                throw new ArgumentException("Le sommet destination n'existe pas.", nameof(destinationName));
            }
            this.adjacence.SetValue(sourceIndex, destIndex, weight);
            if (this.directed==false)
            {
                this.adjacence.SetValue(destIndex, sourceIndex, weight);
            }
        }
        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
    }


}
