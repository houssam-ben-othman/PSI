namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        private Graph graph; // graphe modélisant le problème de voyageur de commerce à résoudre
        private Tour bestTour; // meilleure tournée trouvée jusqu'à présent
        private Matrix costMatrix; // matrice de coûts utilisée pour les calculs intermédiaires
        public int NbCities; // nombre de villes du problème

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            // TODO : implémenter
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            // TODO : implémenter
            return new Tour();
        }

        // --- Méthodes utilitaires réalisant des étapes de l'algorithme de Little


        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            float somme = 0;
            for (int i = 0; i < m.NbRows; i++)
            {
                float minRow = float.MaxValue;
                for (int j = 0; j < m.NbColumns; j++)
                {
                    float val = m.GetValue(i, j);
                    if (val < minRow)
                    {
                        minRow = val;
                    }
                }
                if (minRow > 0 && minRow < float.MaxValue)
                {
                    somme += minRow;
                    for (int j = 0; j < m.NbColumns; j++)
                    {
                        float val = m.GetValue(i, j);
                        if (val < float.PositiveInfinity)
                        {
                            m.SetValue(i, j, val - minRow);
                        }
                    }
                }
            }

            for (int j = 0; j < m.NbColumns; j++)
            {
                float minCol = float.MaxValue;
                for (int i = 0; i < m.NbRows; i++)
                {
                    float val = m.GetValue(i, j);
                    if (val < minCol)
                    {
                        minCol = val;
                    }
                }
                if (minCol > 0 && minCol < float.MaxValue)
                {
                    somme += minCol;
                    for (int i = 0; i < m.NbRows; i++)
                    {
                        float val = m.GetValue(i, j);
                        if (val < float.PositiveInfinity)
                        {
                            m.SetValue(i, j, val - minCol);
                        }
                    }
                }
            }

            return somme;
        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            float maxvalue = -1;
            int indicei = -1;
            int indicej = -1;

            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    if (m.GetValue(i, j) == 0)
                    {
                        float minRow = float.MaxValue;
                        for (int col = 0; col < m.NbColumns; col++)
                        {
                            if (col != j) // On ignore la colonne du zéro
                            {
                                float val = m.GetValue(i, col);
                                if (val < minRow)
                                {
                                    minRow = val;
                                }
                            }
                        }

                        float minCol = float.MaxValue;
                        for (int row = 0; row < m.NbRows; row++)
                        {
                            if (row != i) // On ignore la ligne du zéro
                            {
                                float val = m.GetValue(row, j);
                                if (val < minCol)
                                {
                                    minCol = val;
                                }
                            }
                        }
                        float regret = minRow + minCol;
                        if (regret > maxvalue)
                        {
                            maxvalue = regret;
                            indicei = i;
                            indicej = j;
                        }
                    }
                }
            }
            return (indicei, indicej, maxvalue);

        }

        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {

            // TODO : implémenter
            return false;   
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
        

    }
}
