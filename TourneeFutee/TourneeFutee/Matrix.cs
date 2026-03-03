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
        public void AddRow(int i)//--> Pire cas : O(n+m) || Meilleur Cas : O(n)
        {
            if (i < 0 || i > nbRows)//--> O(1)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Indice hors limites");//--> O(1)
            }
            List<float> Defaut = new List<float>(); //--> O(1) 
            for (int x = 0; x < this.nbColumns; x++) //--> O(n) 
            {
                Defaut.Add(defaultValue);//--> O(1)
            }
            matrice.Insert(i, Defaut); //--> Pire Cas (Insertion debut/millieu) O(m) || Meilleur Cas (Insertion fin) : O(1)
            nbRows++; //--> O(1)
            }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)//--> Pire Cas : O(n*m) || Meilleur Cas : O(n)
        {
            if (j < 0 || j > nbColumns)//--> O(1)
            {
                throw new ArgumentOutOfRangeException(nameof(j), "Indice hors limites"); //--> O(1)
            }
            foreach (var row in matrice)//O(m)
            {
                row.Insert(j, defaultValue); //--> Pire Cas (Insertion début/millieu) : O(n) || Meilleur Cas (Insertion fin) : O(1)
            }
            nbColumns++;//--> O(1)
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
