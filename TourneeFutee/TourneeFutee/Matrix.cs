namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        private int nbRows; // nombre de lignes de la matrice
        private int nbColumns; // nombre de colonnes de la matrice
        private float defaultValue;// valeur par défaut utilisée pour remplir les nouvelles cases
        private List<List<float>> matrice;// matrice de float

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        public Matrix(int nbRows=0, int nbColumns = 0, float defaultValue=0)
        {
            if (nbRows < 0 )
            {
                throw new ArgumentOutOfRangeException(nameof(nbRows));
            }
            if (nbColumns < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nbColumns));
            }
            this.nbRows = nbRows;
            this.nbColumns = nbColumns;
            this.defaultValue = defaultValue;
            this.matrice = new List<List<float>>();
            if (nbColumns > 0 && nbRows>0)
            {
                for (int i = 0; i < nbRows; i++)
                {
                    List<float> row = new List<float>();
                    for (int j = 0; j < nbColumns; j++)
                    {
                        row.Add(defaultValue);
                    }
                    matrice.Add(row);
                }
            }
         
        }

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule
        public List<List<float>> Matrice
        {
            get { return this.matrice; }
            set { this.matrice = value; }
        }

        public float DefaultValue
        {
            get { return this.defaultValue; }
                 // pas de set
        }

        // Propriété : nombre de lignes
        // Lecture seule
        public int NbRows
        {
            get { return this.nbRows; }
                 // pas de set
        }

        // Propriété : nombre de colonnes
        // Lecture seule
        public int NbColumns
        {
            get { return this.nbColumns; }
                 // pas de set
        }

        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            if (i < 0 || i > nbRows) 
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Indice hors limites"); // verifie que i est dans les limites
            }
            List<float> Defaut = new List<float>();
            for (int x = 0; x < this.nbColumns; x++) 
            {
                Defaut.Add(defaultValue);
            }
            matrice.Insert(i, Defaut); // insère la nouvelle ligne à l'indice i
            nbRows++;
            }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            if (j < 0 || j > nbColumns)
            {
                throw new ArgumentOutOfRangeException(nameof(j), "Indice hors limites");
            }
            foreach (var row in matrice)
            {
                row.Insert(j, defaultValue);
            }
            nbColumns++;
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i) 
        {
            if (i < 0||i>=this.nbRows) 
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Indice hors limites"); // verifie que i est dans les limites
            }
            else 
            {
                this.matrice.RemoveAt(i);  // supprime la ligne à l'indice i
                this.nbRows--; 
            }
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j) 
        {
            if (j < 0||j>=this.nbColumns) 
            {
                throw new ArgumentOutOfRangeException(nameof(j), "Indice hors limites");  // verifie que j est dans les limites
            }
            else 
            {
                for (int i = 0; i < this.nbRows; i++)  
                {
                    this.matrice[i].RemoveAt(j);  // supprime la colonne à l'indice j pour chaque ligne
                }
                this.nbColumns--; 
            }
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            if (i < 0 || i >= this.nbRows)
            {
                throw new ArgumentOutOfRangeException(nameof(i)); // verifie que i est dans les limites
            }
            else if (j < 0 || j >= this.nbColumns)
            {
                throw new ArgumentOutOfRangeException(nameof(j)); // verifie que j est dans les limites
            }
            else
            {
                return matrice[i][j];
            }         
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            if (i < 0 || i >= this.nbRows)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }
            else if (j < 0 || j >= this.nbColumns)
            {
                throw new ArgumentOutOfRangeException(nameof(j));
            }
            else
            {
                matrice[i][j] = v;
            }
                
        }

        // Affiche la matrice
        public void Print()
        {
            foreach(List<float> floats in this.matrice)
            {
                AfficherListe(floats);
            }
        }

        public void AfficherListe(List<float> liste)
        {
            foreach (float f in liste)
            {
                Console.WriteLine(f);
            }
        }
        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
