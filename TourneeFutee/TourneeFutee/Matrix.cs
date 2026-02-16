namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        private int nbRows;
        private int nbColumns;
        private float defaultValue;
        private List<List<float>> matrice;

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
                for (int j = 0; j < nbColumns; j++)
                {
                    List<float> Column = new List<float>();
                    for (int i = 0; i < nbRows; i++)
                    {
                        Column.Add(defaultValue);
                    }
                    matrice.Add(Column);
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
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Indice hors limites");
            }
            List<float> Defaut = new List<float>();
            for(int x = 0; x < this.nbColumns; x++)
            {
                Defaut[x] = defaultValue;
            }
            if (i == nbRows)
            {
                this.matrice.Add(Defaut);
            }
            else
            {
                this.matrice.Insert(i, Defaut);
            }
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            // TODO : implémenter
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            if (i < 0||i>=this.nbRows)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Indice hors limites");
            }
            else
            {
                this.matrice.RemoveAt(i);
                this.nbRows--;
            }
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            if (j < 0||j>=this.nbColumns)
            {
                throw new ArgumentOutOfRangeException(nameof(j), "Indice hors limites");
            }
            else
            {
                for (int i = 0; i < this.nbRows; i++)
                {
                    this.matrice[i].RemoveAt(j);
                }
                this.nbColumns--;
            }
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            // TODO : implémenter
            return 0.0f;
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
        }

        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
        }


        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
