using System;
using System.Text;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public class clsIpeaShape : IpeaGeo.RegressoesEspaciais.clsIpeaShape
    {
        public clsIpeaShape()
            : base()
        {
        }

        public string RetornaMatrizTodasDistanciasParaSalvamento()
        {
            if (m_matriz_all_distances == null)
            {
                GerarMatrizTodasDistancias();
            }

            StringBuilder res = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = i+1; j < this.Count; j++)
                {
                    res.Append((this[i].Label == "" ? (i + 1).ToString() : this[i].Label) + "\t"
                        + (this[j].Label == "" ? (j + 1).ToString() : this[j].Label) + "\t" 
                        + m_matriz_all_distances[i, j].ToString() + "\n"); 
                }
            }

            return res.ToString();
        }

        public clsIpeaShape Clone()
        {
            clsIpeaShape res = new clsIpeaShape();
            res.m_nome = this.m_nome;
            res.m_tipo_vizinhanca = this.m_tipo_vizinhanca;
            res.m_ordem_vizinhanca = this.m_ordem_vizinhanca;
            res.m_num_poligonos = this.m_num_poligonos;
            res.m_poligonos = new clsIpeaPoligono[this.m_poligonos.GetLength(0)];

            for (int i = 0; i < this.m_poligonos.GetLength(0); i++)
            {
                res.m_poligonos[i] = (clsIpeaPoligono)this.m_poligonos[i].Clone();
            }

            return res;
        }

        public clsIpeaPoligono this[int i]
        {
            get { return (clsIpeaPoligono)this.m_poligonos[i]; }
            set { this.m_poligonos[i] = value; }
        }

        #region Função para definição dos vizinhos

        #region Chamada das definições dos vizinhos

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos)
        {
            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca)
        {
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos, ref ProgressBar pgBar)
        {
            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos, ref ToolStripProgressBar pgBar)
        {
            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, ref ProgressBar pgBar)
        {
            switch (tipo_vizinhanca)
            {
                //Matriz de vizinhança do tipo QUEEN NÃO NORMALIZADA
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NÃO NORMALIZADA
                case 2:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
                //Matriz de vizinhança do tipo QUEEN NORMALIZADA
                case 3:
                    this.DefinicaoVizinhosQueenNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NORMALIZADA
                case 4:
                    this.DefinicaoVizinhosRookNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, ref ToolStripProgressBar pgBar)
        {
            switch (tipo_vizinhanca)
            {
                //Matriz de vizinhança do tipo QUEEN NÃO NORMALIZADA
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NÃO NORMALIZADA
                case 2:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
                //Matriz de vizinhança do tipo QUEEN NORMALIZADA
                case 3:
                    this.DefinicaoVizinhosQueenNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NORMALIZADA
                case 4:
                    this.DefinicaoVizinhosRookNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
                    break;
            }
        }

        #endregion

        #region Vizinhança do tipo queen (um vértico em comum)

        protected void DefinicaoVizinhosQueen(ref clsIpeaShape shp_dados)
        {
            shp_dados.TipoVizinhanca = "Queen";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i);
                }
            }
        }

        protected void DefinicaoVizinhosQueen(ref clsIpeaShape shp_dados, ref ToolStripProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Queen";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i);
                    }
                }
            }
         
            prBar.Value = 0;
            //prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosQueen(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Queen";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea=new clsAreaPerimetroCentroide();

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        double distancia=clsarea.distancia(shp_dados[i].YCentroide,shp_dados[i].XCentroide,shp_dados[j].YCentroide,shp_dados[j].XCentroide, false);
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia,1.0,shp_dados[j].PosicaoNoDataTable);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia,1.0,shp_dados[i].PosicaoNoDataTable);
                    }
                }
            }
         
            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosQueenNormalizada(ref clsIpeaShape shp_dados)
        {
            shp_dados.TipoVizinhanca = "Queen Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            for (int i = 0; i < shp_dados.Count; i++)
            {
                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].ListaPesosVizinhos[v] = peso;
            }
        }

        protected void DefinicaoVizinhosQueenNormalizada(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Queen Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                    }
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++)
                {
                    shp_dados[i].AdicionaPesosVizinhos(peso,v);
                }
            }            

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosQueenNormalizada(ref clsIpeaShape shp_dados, ref ToolStripProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Queen Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                        shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                        sai = true;
                                        break;
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                    }
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++)
                {
                    shp_dados[i].AdicionaPesosVizinhos(peso, v);
                }
            }

            prBar.Value = 0;
            //prBar.Refresh();
            
            Application.DoEvents();
        }

        #endregion

        #region Vizinhança do tipo rook (uma aresta em comum)

        protected void DefinicaoVizinhosRook(ref clsIpeaShape shp_dados)
        {
            shp_dados.TipoVizinhanca = "Rook";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }
        }

        protected void DefinicaoVizinhosRook(ref clsIpeaShape shp_dados, ref ToolStripProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Rook";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            prBar.Value = 0;
            //prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosRook(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Rook";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosRookNormalizada(ref clsIpeaShape shp_dados)
        {
            shp_dados.TipoVizinhanca = "Rook Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].ListaPesosVizinhos[v] = peso;
            }
        }

        protected void DefinicaoVizinhosRookNormalizada(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Rook Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].ListaPesosVizinhos[v] = peso;
            }

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosRookNormalizada(ref clsIpeaShape shp_dados, ref ToolStripProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Rook Normalizada";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;

            double[] vx1;
            double[] vy1;

            double[] vx2;
            double[] vy2;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            bool pode_ser = false;

            prBar.Maximum = shp_dados.Count;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                vx1 = shp_dados[i].GetArrayX();
                vy1 = shp_dados[i].GetArrayY();

                for (int j = i + 1; j < shp_dados.Count; j++)
                {
                    if (!((shp_dados[i].BoundingBoxXMax < shp_dados[j].BoundingBoxXMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxXMin > shp_dados[j].BoundingBoxXMax + this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMax < shp_dados[j].BoundingBoxYMin - this.m_epsilon_distancia)
                        || (shp_dados[i].BoundingBoxYMin > shp_dados[j].BoundingBoxYMax + this.m_epsilon_distancia)))
                    {
                        sai = false;
                        pode_ser = false;

                        vx2 = shp_dados[j].GetArrayX();
                        vy2 = shp_dados[j].GetArrayY();

                        for (int k1 = 0; k1 < vx1.GetLength(0); k1++)
                        {
                            for (int k2 = 0; k2 < vx2.GetLength(0); k2++)
                            {
                                if (Math.Abs(vx1[k1] - vx2[k2]) < this.m_epsilon_distancia)
                                {
                                    if (Math.Abs(vy1[k1] - vy2[k2]) < this.m_epsilon_distancia)
                                    {
                                        if (pode_ser)
                                        {
                                            double distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            double novopeso = 1 / ((double)shp_dados[i].ListaVizinhos.Length);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, novopeso, shp_dados[j].PosicaoNoDataTable);
                                            sai = true;
                                            pode_ser = false;
                                            break;
                                        }
                                        else
                                        {
                                            pode_ser = true;
                                        }
                                    }
                                }
                            }
                            if (sai) break;
                        }
                    }
                }
            }

            for (int j = shp_dados.Count - 1; j >= 0; j--)
            {
                for (int i = j - 1; i >= 0; i--)
                {
                    double distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    double novopeso = 1 / ((double)shp_dados[j].ListaVizinhos.Length);
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].ListaPesosVizinhos[v] = peso;
            }

            prBar.Value = 0;
            //prBar.Refresh();
            Application.DoEvents();
        }

        #endregion

        #endregion          
    }
}
