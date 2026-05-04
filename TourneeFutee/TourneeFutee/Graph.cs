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
        public void AddVertex(string name, float value = 0) // O(n)
        {
            if (vertexIndices.ContainsKey(name) == true) // O(1)
            {
                throw new ArgumentException("Un sommet avec le même nom existe déja.", nameof(name)); // O(1)
            }
            int newIndex = order; // O(1)
            adjacence.AddRow(newIndex); // O(n)
            adjacence.AddColumn(newIndex); // O(n)
            vertexIndices[name] = newIndex; // O(1)
            vertexValues[name] = value; // O(1)
            order++; // O(1)
        }


        // Supprime le sommet de nom `name` du graphe (et tous les arcs associés)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void RemoveVertex(string name) // O(n)
        {
            // TODO : implémenter // O(1)
            if (vertexIndices.TryGetValue(name, out int index) == false) // O(1)
            {
                throw new ArgumentException("Sommet non trouvé", nameof(name)); // O(1)
            }
            adjacence.RemoveRow(index);  // O(n)
            adjacence.RemoveColumn(index); // O(n)
            vertexIndices.Remove(name); // O(1)
            vertexValues.Remove(name); // O(1)
            order--; // O(1)
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
        public List<string> GetNeighbors(string vertexName) // O(n^2)
        {
            List<string> neighborNames = new List<string>(); // O(1)
            int vertexIndex;
            try
            {
                vertexIndex = vertexIndices[vertexName]; // O(1)
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Sommet non trouvé"); // O(1)
            }
            for (int j = 0; j < order; j++) // O(n)
            {
                float weight = adjacence.GetValue(vertexIndex, j); // O(1)
                if (weight != noEdgeValue) // O(1)
                {
                    foreach (var i in vertexIndices) // O(n)
                    {
                        if (i.Value == j) // O(1)
                        {
                            neighborNames.Add(i.Key); // O(1)
                            break; // O(1)
                        }
                    }
                }
            }
            return neighborNames; // O(1)
        }

        // --- Gestion des arcs ---

        /* Ajoute un arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`, avec le poids `weight` (1 par défaut)
         * Si le graphe n'est pas orienté, ajoute aussi l'arc inverse, avec le même poids
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - il existe déjà un arc avec ces extrémités
         */
        public void AddEdge(string sourceName, string destinationName, float weight = 1) // O(1)
        {
            try // O(1)
            {
                int i = vertexIndices[sourceName]; // O(1)
                int j = vertexIndices[destinationName]; // O(1)
                if (adjacence.GetValue(i, j) != noEdgeValue) // O(1)
                {
                    throw new ArgumentException("Cet arc existe déja"); // O(1)
                }
                adjacence.SetValue(i, j, weight); // O(1)

                if (!directed) // O(1)
                {
                    adjacence.SetValue(j, i, weight); // O(1)
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Sommet non trouvé"); // O(1)
            }
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName) // O(1)
        {
            try // O(1)
            {
                int i = vertexIndices[sourceName]; // O(1)
                int j = vertexIndices[destinationName]; // O(1)
                if (adjacence.GetValue(i, j) == noEdgeValue) // O(1)
                {
                    throw new ArgumentException("l'arc n'existe pas"); // O(1)
                }
                adjacence.SetValue(i, j, noEdgeValue); // O(1)
                if (!directed) // O(1)
                {
                    adjacence.SetValue(j, i, noEdgeValue); // O(1)
                }
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Sommet non trouvé"); // O(1)
            }
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName) // O(1)
        {
            if (this.vertexIndices.TryGetValue(sourceName, out int sourceIndex) == false) // O(1)
            {
                throw new ArgumentException("Le sommet source n'existe pas.", nameof(sourceName)); // O(1)
            }
            if (this.vertexIndices.TryGetValue(destinationName, out int destIndex) == false) // O(1)
            {
                throw new ArgumentException("Le sommet destination n'existe pas.", nameof(destinationName)); // O(1)
            }
            float weight = this.adjacence.GetValue(sourceIndex, destIndex); // O(1)
            if (weight == this.noEdgeValue) // O(1)
            {
                throw new ArgumentException("L'arc n'existe pas entre ces deux sommets."); // O(1)
            }
            return weight; // O(1)
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight) // O(1)
        {
            if (this.vertexIndices.TryGetValue(sourceName, out int sourceIndex) == false) // O(1)
            {
                throw new ArgumentException("Le sommet source n'existe pas.", nameof(sourceName)); // O(1)
            }
            if (this.vertexIndices.TryGetValue(destinationName, out int destIndex) == false) // O(1)
            {
                throw new ArgumentException("Le sommet destination n'existe pas.", nameof(destinationName)); // O(1)
            }
            this.adjacence.SetValue(sourceIndex, destIndex, weight); // O(1)
            if (this.directed == false) // O(1)
            {
                this.adjacence.SetValue(destIndex, sourceIndex, weight); // O(1)
            }
        }
        public bool ContainsVertex(string name)
        {
            return vertexIndices.ContainsKey(name);
        }
        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
    }
}
