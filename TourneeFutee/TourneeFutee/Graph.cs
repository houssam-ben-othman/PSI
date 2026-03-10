namespace TourneeFutee
{
    public class Graph
    {

        // TODO : ajouter tous les attributs que vous jugerez pertinents 
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
            get { return this.order; }    // TODO : implémenter
                    // pas de set
        }

        // Propriété : graphe orienté ou non
        // Lecture seule
        public bool Directed
        {
            get{return this.directed; }   // TODO : implémenter
                    // pas de set
        }
        public float NoEdgeValue
        {
            get{return this.noEdgeValue; }
            set{this.noEdgeValue = value; }// Pourrais être supprimé si pas utile
        }
        public Matrix Adajcence
        {
            get { return this.adjacence; }
            set { this.adjacence = value; }     
        }   
        public Dictionary<string, int> VertexIndices
        {
            get { return this.vertexIndices; }
            set{this.vertexIndices = value; }// Pourrais être supprimé si pas utile
        }
        public Dictionary<string, float> VertexValues
        {
            get { return this.vertexValues; }
            set { this.vertexValues = value; }// Pourrais être supprimé si pas utile
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
        public float GetVertexValue(string name)
        {
            // TODO : implémenter
            if (!vertexValues.TryGetValue(name, out float value))
                throw new ArgumentException("Sommet non trouvé", nameof(name));
            return value;
        }

        // Affecte la valeur du sommet de nom `name` à `value`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void SetVertexValue(string name, float value)
        {
            // TODO : implémenter
            if (!vertexValues.TryGetValue(name, out _))
                throw new ArgumentException("Sommet non trouvé", nameof(name));
            vertexValues[name] = value;
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
            try
            {
                int i = vertexIndices[sourceName];
                int j = vertexIndices[destinationName];
                if(adjacence.GetValue(i, j)!=noEdgeValue)
                {
                    throw new ArgumentException("Cet arc existe déja");
                }
                adjacence.SetValue(i, j, weight);

                if (!directed)
                {
                    adjacence.SetValue(j, i, weight);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Sommet non trouvé");
            }
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName)
        {
            try
            {
                int i = vertexIndices[sourceName];
                int j = vertexIndices[destinationName];
                if(adjacence.GetValue(i,j)==noEdgeValue)
                {
                    throw new ArgumentException("l'arc n'existe pas");
                }
                adjacence.SetValue(i, j, noEdgeValue);
                if(!directed)
                {
                    adjacence.SetValue(j,i, noEdgeValue);
                }
            }
            catch (KeyNotFoundException) 
            {
                throw new ArgumentException("Sommet non trouvé");
            }
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName)
        {
            // TODO : implémenter
            return 0.0f;
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight)
        {
            // TODO : implémenter
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
