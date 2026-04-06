namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        private Graph graph; // graphe modélisant le problème de voyageur de commerce à résoudre
        private Tour MeilleurTour; // meilleure tournée trouvée jusqu'à présent
        private Matrix MatriceCout; // matrice de coûts utilisée pour les calculs intermédiaires
        public int NbCities; // nombre de villes du problème

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {

            this.graph = graph;
            this.MeilleurTour = new Tour();
            this.NbCities = graph.Order;
            this.MatriceCout = new Matrix(graph.Order, graph.Order, float.PositiveInfinity);
            for (int i = 0; i < graph.Order; i++)
            {
                for (int j = 0; j < graph.Order; j++)
                {
                    float val = graph.Adajcence.GetValue(i, j);
                    if (val == graph.NoEdgeValue || i == j)
                    {
                        this.MatriceCout.SetValue(i, j, float.PositiveInfinity);
                    }
                    else
                    {
                        this.MatriceCout.SetValue(i, j, val);
                    }
                }
            }
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            Matrix Matrix = new Matrix(NbCities, NbCities, float.PositiveInfinity);
            for (int i = 0; i < NbCities; i++)
            {
                for (int j = 0; j < NbCities; j++)
                {
                    Matrix.SetValue(i, j, MatriceCout.GetValue(i, j));
                }
            }
            float Bound = ReduceMatrix(Matrix);
            Branch(Matrix, Bound, new List<(string, string)>());
            return MeilleurTour;
        }
        private Matrix CopieMatrice(Matrix m)
        {
            Matrix copie = new Matrix(m.NbRows, m.NbColumns, float.PositiveInfinity);
            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    copie.SetValue(i, j, m.GetValue(i, j));
                }
            }
            return copie;
        }

        private string IndexToName(int index)
        {
            foreach (KeyValuePair<string, int> nom in graph.VertexIndices)
            {
                if (nom.Value == index)
                {
                    return nom.Key;
                }
            }
            return null;
        }

        private void Branch(Matrix currentMatrix, float lowerBound, List<(string, string)> includedSegments)
        {
            if (lowerBound >= MeilleurTour.Cost)
            {
                return;
            }
            if (includedSegments.Count == NbCities)
            {
                if (lowerBound < MeilleurTour.Cost)
                {
                    MeilleurTour = new Tour();
                    MeilleurTour.Cost = lowerBound;
                    foreach (var seg in includedSegments)
                    {
                        MeilleurTour.AddSegment(seg.Item1, seg.Item2);
                    }
                }
                return;
            }

            if (includedSegments.Count == NbCities - 1)
            {
                List<string> sources = new List<string>();
                List<string> destinations = new List<string>();
                foreach ((string,string) seg in includedSegments)
                {
                    sources.Add(seg.Item1);
                    destinations.Add(seg.Item2);
                }

                string DerSrc = null;
                string DerDst = null;

                foreach (KeyValuePair<string,int> ville in graph.VertexIndices)
                {
                    if (!sources.Contains(ville.Key))
                    {
                        DerSrc = ville.Key;
                    }
                    if (!destinations.Contains(ville.Key))
                    {
                        DerDst = ville.Key;
                    }
                }

                if (DerSrc != null && DerDst != null)
                {
                    if (graph.VertexIndices.TryGetValue(DerSrc, out int si) && graph.VertexIndices.TryGetValue(DerDst, out int di))
                    {
                        float CoutArete = MatriceCout.GetValue(si, di);
                        if (CoutArete != float.PositiveInfinity)
                        {
                            float CoutTot = lowerBound + currentMatrix.GetValue(si, di);
                            if (CoutTot == float.PositiveInfinity)
                            {
                                CoutTot = lowerBound;
                            }
                            if (CoutTot < MeilleurTour.Cost)
                            {
                                MeilleurTour = new Tour();
                                MeilleurTour.Cost = CoutTot;
                                foreach (var seg in includedSegments)
                                {
                                    MeilleurTour.AddSegment(seg.Item1, seg.Item2);
                                }
                                MeilleurTour.AddSegment(DerSrc, DerDst);
                            }
                        }
                    }
                }
                return;
            }
            (int regret_i, int regret_j, float dgaf) = GetMaxRegret(currentMatrix);
            if (regret_i == -1 || regret_j == -1)
            {
                return;
            }
            string segSrc = IndexToName(regret_i);
            string segDst = IndexToName(regret_j);

            Matrix inclMatrix = CopieMatrice(currentMatrix);

            for (int k = 0; k < NbCities; k++)
            {
                inclMatrix.SetValue(regret_i, k, float.PositiveInfinity);
            }
            for (int k = 0; k < NbCities; k++)
            {
                inclMatrix.SetValue(k, regret_j, float.PositiveInfinity);
            }
            List<(string, string)> newIncluded = new List<(string, string)>(includedSegments) { (segSrc, segDst) };

            if (newIncluded.Count < NbCities)
            {
                string FinChaine = segDst;
                bool advanced = true;
                while (advanced)
                {
                    advanced = false;
                    foreach (var s in newIncluded)
                    {
                        if (s.Item1 == FinChaine)
                        {
                            FinChaine = s.Item2;
                            advanced = true;
                            break;
                        }
                    }
                }

                string DebutChaine = segSrc;
                advanced = true;
                while (advanced)
                {
                    advanced = false;
                    foreach (var s in newIncluded)
                    {
                        if (s.Item2 == DebutChaine)
                        {
                            DebutChaine = s.Item1;
                            advanced = true; 
                            break;
                        }
                    }
                }

                if (graph.VertexIndices.TryGetValue(FinChaine, out int forbidI) && graph.VertexIndices.TryGetValue(DebutChaine, out int forbidJ))
                {
                    inclMatrix.SetValue(forbidI, forbidJ, float.PositiveInfinity);
                }


                float inclBound = lowerBound + ReduceMatrix(inclMatrix);
                Branch(inclMatrix, inclBound, newIncluded);


                Matrix exclMatrix = CopieMatrice(currentMatrix);
                exclMatrix.SetValue(regret_i, regret_j, float.PositiveInfinity);
                float exclBound = lowerBound + ReduceMatrix(exclMatrix);
                Branch(exclMatrix, exclBound, includedSegments);
            }
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
            string villededépart = segment.destination;
            int taillechaine = 1;
            bool continu = true;
            while (continu)
            {
                if (villededépart == segment.source)
                {
                    if (taillechaine < nbCities)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                continu = false;
                foreach (var s in includedSegments)
                {
                    if (s.source == villededépart)
                    {
                        villededépart = s.destination;
                        taillechaine++;
                        continu = true;
                        break;
                    }
                }
            }
            return false;
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 


    }
}
