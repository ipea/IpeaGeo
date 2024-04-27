using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsIpeaShape
    {
        public clsIpeaShape()
        {
            m_hora_criacao = DateTime.Now;
        }

        protected TipoContiguidade m_tipo_contiguidade = TipoContiguidade.Rook;
        public TipoContiguidade TipoContiguidade
        {
            get
            {
                return m_tipo_contiguidade;
            }
            set
            {
            	m_tipo_contiguidade = value;
            }
        }

        protected DateTime m_hora_criacao;
        public DateTime HoraCriacao
        {
            get
            {
                return m_hora_criacao;
            }
            set
            {
            	m_hora_criacao = value;
            }
        }

        public void DeleteListaVizinhos(ref clsIpeaShape shp_dados)
        {
            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].DeleteAllVizinhosAt();
                //shp_dados[i].DeleteAllVizinhos();
            }
        }

        private bool m_coordenadas_em_radianos = false;
        public bool CoordenadasEmRadianos
        {
            set { m_coordenadas_em_radianos = value; }
            get { return m_coordenadas_em_radianos; }
        }

        protected void AjusteEpsilonDistancia(ref clsIpeaShape shp_shape)
        {
            double max_x = double.NegativeInfinity;
            double min_x = double.PositiveInfinity;
            double max_y = double.NegativeInfinity;
            double min_y = double.PositiveInfinity;

            for (int i = 0; i < shp_shape.Count; i++)
            {
                if (min_x > shp_shape[i].BoundingBoxXMin) min_x = shp_shape[i].BoundingBoxXMin;
                if (min_y > shp_shape[i].BoundingBoxYMin) min_y = shp_shape[i].BoundingBoxYMin;
                if (max_x < shp_shape[i].BoundingBoxXMax) max_x = shp_shape[i].BoundingBoxXMax;
                if (max_y < shp_shape[i].BoundingBoxYMax) max_y = shp_shape[i].BoundingBoxYMax;
            }

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            double max_dist = clsarea.distancia(min_x, min_y, max_x, max_y, m_coordenadas_em_radianos);

            m_epsilon_distancia = max_dist * 1e-9;
        }

        //protected double m_epsilon_distancia = 1e-7;
        protected double m_epsilon_distancia = 1e-5;

        protected int m_num_poligonos = 0;
        protected clsIpeaPoligono[] m_poligonos = new clsIpeaPoligono[0];
        protected string m_nome = "";
        protected string m_tipo_vizinhanca = "";
        protected int m_ordem_vizinhanca = 1;

        public void ConvertFromIpeaGEOShape(IpeaGeo.clsIpeaShape c)
        {
            this.Nome = c.Nome;
            this.TipoVizinhanca = c.TipoVizinhanca;
            this.OrdemVizinhanca = c.OrdemVizinhanca;
            this.Count = c.Count;

            this.m_poligonos = new clsIpeaPoligono[c.Poligonos.GetLength(0)];
            clsIpeaPoligono pol = new clsIpeaPoligono();
            for (int i = 0; i < this.Poligonos.GetLength(0); i++)
            {
                pol = (clsIpeaPoligono)c.Poligonos[i];
                this.m_poligonos[i] = new clsIpeaPoligono();
                this.m_poligonos[i].ConvertFromIpeaGEOPoligono(pol);
            }
        }

        public clsIpeaPoligono[] Poligonos
        {
            get
            {
                return this.m_poligonos;
            }
            set
            {
            	this.m_poligonos = value;
            }
        }

        public IpeaGeo.clsIpeaShape ConvertToIpeaGEOShape()
        {
            IpeaGeo.clsIpeaShape res = new IpeaGeo.clsIpeaShape();

            res.Nome = this.Nome;
            res.TipoVizinhanca = this.TipoVizinhanca;
            res.OrdemVizinhanca = this.OrdemVizinhanca;
            res.Count = this.Count;

            res.Poligonos = new clsIpeaPoligono[this.Poligonos.GetLength(0)];
            for (int i = 0; i < this.Poligonos.GetLength(0); i++)
            {
                res.Poligonos[i] = this.Poligonos[i].ConvertToIpeaGEOPoligono();
            }

            return res;
        }

        public bool m_tipo_distancia = false;
        public bool TipoDistancia
        {
            get { return this.m_tipo_distancia; }
            set
            {
                this.m_tipo_distancia = value;
            }
        }

        protected FileMatrix m_matriz_all_distances = null;
        public FileMatrix MatrizAllDistances
        {
            get { return this.m_matriz_all_distances; }
            set { this.m_matriz_all_distances = value; }
        }

        public void GerarMatrizTodasDistancias()
        {
            clsAreaPerimetroCentroide cap = new clsAreaPerimetroCentroide();

            this.TipoDistancia = true;
            this.m_tipo_distancia = true;

            m_matriz_all_distances = new FileMatrix(this.Count, this.Count);

            for (int i = 0; i < this.Count; i++)
            {
                for (int j = i + 1; j < this.Count; j++)
                {
                    m_matriz_all_distances[i, j] = cap.distancia(this[i].YCentroide, this[i].XCentroide, this[j].YCentroide, this[j].XCentroide, false);
                    m_matriz_all_distances[j, i] = m_matriz_all_distances[i, j];
                }
            }
        }

        public clsIpeaShape Clone()
        {
            clsIpeaShape res = new clsIpeaShape();
            res.m_hora_criacao = this.m_hora_criacao;
            res.m_nome = this.m_nome;
            res.m_tipo_vizinhanca = this.m_tipo_vizinhanca;
            res.m_ordem_vizinhanca = this.m_ordem_vizinhanca;
            res.m_num_poligonos = this.m_num_poligonos;
            res.m_poligonos = new clsIpeaPoligono[this.m_poligonos.GetLength(0)];

            for (int i = 0; i < this.m_poligonos.GetLength(0); i++)
            {
                res.m_poligonos[i] = this.m_poligonos[i].Clone();
            }

            return res;
        }

        #region Funções básicas

        public string TipoVizinhanca
        {
            get { return this.m_tipo_vizinhanca; }
            set
            {
                this.m_tipo_vizinhanca = value;
            }
        }
 
        public int OrdemVizinhanca
        {
            get { return this.m_ordem_vizinhanca; }
            set
            {
                this.m_ordem_vizinhanca = Math.Max(1, value);
            }
        }

        public string Nome
        {
            get { return m_nome; }
            set { this.m_nome = value; }
        }

        public int Count
        {
            get { return this.m_num_poligonos; }
            set { this.m_num_poligonos = value; }
        }

        public void AddPoligono(clsIpeaPoligono pol)
        {
            clsIpeaPoligono[] temp = new clsIpeaPoligono[this.m_num_poligonos];
            for (int i = 0; i < this.m_num_poligonos; i++) temp[i] = this.m_poligonos[i];

            this.m_poligonos = new clsIpeaPoligono[this.m_num_poligonos + 1];
            for (int i = 0; i < this.m_num_poligonos; i++) this.m_poligonos[i] = temp[i];

            this.m_poligonos[this.m_num_poligonos] = pol;

            this.m_num_poligonos++;
            temp = null;
        }

        public clsIpeaPoligono this[int i]
        {
            get { return this.m_poligonos[i]; }
            set { this.m_poligonos[i] = value; }
        }

        #endregion

        #region Função para definição dos vizinhos

        #region Limpa toda a definição de vizinhos

        protected void limpaVizinhosAtuais(ref clsIpeaShape shp_dados)
        {
            for (int i = 0; i < shp_dados.Count; i++)
            {
                
            }
        }

        #endregion

        #region Funçoes de importação e exportaçao das vizinhanças

        public string VizinhosToString(string nome_tipo_vizinhança)
        {
            StringBuilder sb = new StringBuilder("<<ESTRUTURA DE VIZINHANCA PARA ARQUIVO SHAPE>>\n", 10 * this.Count * 20);
            sb.Append(DateTime.Today.ToLongDateString() + "\n" + DateTime.Now.ToLongTimeString() + "\n");
            sb.Append("Vizinhanca do tipo " + nome_tipo_vizinhança + "\n\n");

            for (int i = 0; i < this.Count; i++)
            {
                sb.Append("[" + this[i].Nome + "]\n");
                sb.Append("<");
                for (int j = 0; j < this[i].ListaVizinhos.GetLength(0); j++)
                {
                    if (j < this[i].ListaVizinhos.GetLength(0) - 1)
                        sb.Append(this[i].ListaVizinhos[j] + ", ");
                    else
                        sb.Append(this[i].ListaVizinhos[j]);
                }
                sb.Append(">\n");
            }

            string res = sb.ToString();
            sb = null;

            return res;
        }

        #endregion

        #region Chamada das definições dos vizinhos

        public void DefinicaoVizinhosFromMatrizEsparsa(ref clsIpeaShape shp_dados, clsMatrizEsparsa Wesparsa)
        {
            clsMatrizEsparsa W;
            if (Wesparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm) W = Wesparsa;
            else
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                W = fme.CompressColumn2TripletForm(Wesparsa);
            }

            double[] ax = W.x;
            int[] ai = W.i;
            int[] ap = W.p;
            
            DeleteListaVizinhos(ref shp_dados);

            double distancia, peso;
            int i, j;

            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();

            for (int k = 0; k <ax.GetLength(0); k++)
            {
                peso = ax[k];
                i = ai[k];
                j = ap[k];

                distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, peso, shp_dados[j].PosicaoNoDataTable);
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos, ref ProgressBar pgBar)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, string[] nomes_poligonos, ref ToolStripProgressBar pgBar)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            for (int i = 0; i < shp_dados.Count; i++)
            {
                shp_dados[i].Nome = nomes_poligonos[i];
            }
            switch (tipo_vizinhanca)
            {
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, ref ProgressBar pgBar)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            switch (tipo_vizinhanca)
            {
                //Matriz de vizinhança do tipo QUEEN NÃO NORMALIZADA
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NÃO NORMALIZADA
                case 2:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
                //Matriz de vizinhança do tipo QUEEN NORMALIZADA
                case 3:
                    this.DefinicaoVizinhosQueenNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NORMALIZADA
                case 4:
                    this.DefinicaoVizinhosRookNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        public void DefinicaoVizinhos(ref clsIpeaShape shp_dados, int tipo_vizinhanca, ref ToolStripProgressBar pgBar)
        {
            AjusteEpsilonDistancia(ref shp_dados);
            DeleteListaVizinhos(ref shp_dados);

            switch (tipo_vizinhanca)
            {
                //Matriz de vizinhança do tipo QUEEN NÃO NORMALIZADA
                case 1:
                    this.DefinicaoVizinhosQueen(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NÃO NORMALIZADA
                case 2:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
                //Matriz de vizinhança do tipo QUEEN NORMALIZADA
                case 3:
                    this.DefinicaoVizinhosQueenNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Queen;
                    break;
                //Matriz de vizinhança do tipo ROOK NORMALIZADA
                case 4:
                    this.DefinicaoVizinhosRookNormalizada(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
                default:
                    this.DefinicaoVizinhosRook(ref shp_dados, ref pgBar);
                    shp_dados.TipoContiguidade = TipoContiguidade.Rook;
                    break;
            }
        }

        #endregion

        #region Adicionando vizinhos para os polígonos sem vizinhos 

        /// <summary>
        /// Função para correr todos os vizinhos de um determinado shape, checando se há algum polígono sem vizinhos. Se houver polígono sem vizinhos, 
        /// a função checa qual o polígono mais próximo, em termos de distância, e adiciona esse polígono mais próximo à lista de vizinhos do 
        /// polígono originalmente sem vizinhos. 
        /// </summary>
        /// <param name="shp_dados">Referência para o objeto shape.</param>
        public void AdicionaVizinhoProximo(ref clsIpeaShape shp_dados)
        {
            clsAreaPerimetroCentroide clsarea = new clsAreaPerimetroCentroide();
            double min_dist = 0.0;
            double dist = 0.0;
            int min_j = 0;

            for (int i = 0; i < shp_dados.Count; i++)
            {
                if (shp_dados[i].ListaVizinhos.Length <= 0)
                {
                    min_dist = double.PositiveInfinity;
                    min_j = -1;
                    for (int j=0; j<shp_dados.Count; j++)
                    {
                        if (j != i)
                        {
                            dist = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                            if (dist < min_dist)
                            {
                                min_dist = dist;
                                min_j = j;
                            }
                        }
                    }

                    dist = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[min_j].YCentroide, shp_dados[min_j].XCentroide, false);
                    shp_dados[i].AddVizinho(shp_dados[min_j].Nome, min_j, dist, 1.0, shp_dados[min_j].PosicaoNoDataTable);
                    shp_dados[min_j].AddVizinho(shp_dados[i].Nome, i, dist, 1.0, shp_dados[i].PosicaoNoDataTable);
                }
            }

            if (shp_dados.TipoVizinhanca == "Rook Normalizada" || shp_dados.TipoVizinhanca == "Queen Normalizada")
            {
                //Normaliza
                for (int i = 0; i < shp_dados.Count; i++)
                {
                    if (shp_dados[i].NumeroVizinhos > 0)
                    {
                        double peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                        for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].SetPesoVizinho(v, peso);
                    }
                    else
                    {
                    }
                }
            }
        }

        #endregion

        #region Vizinhança do tipo queen (um vértice em comum)

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

            prBar.Maximum = shp_dados.Count + shp_dados.Count;

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
                prBar.Increment(1);
                Application.DoEvents();

                for (int i = j - 1; i >= 0; i--)
                {
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i);
                    }
                }
            }           

            prBar.Value = 0;
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosQueen(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Queen";
            shp_dados.OrdemVizinhanca = 1;

            double distancia;

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
                                        distancia=clsarea.distancia(shp_dados[i].YCentroide,shp_dados[i].XCentroide,shp_dados[j].YCentroide,shp_dados[j].XCentroide, false);
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
                        distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
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

            double distancia, novopeso, peso;

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
                                        distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].SetPesoVizinho(v, peso);
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

            double peso, distancia, novopeso;

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
                                        distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        novopeso = 1.0;
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
                        distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        novopeso = 1.0;
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                    }
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
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

            double peso, distancia, novopeso;

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
                                        distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                        novopeso = 1.0;
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
                        distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        novopeso = 1.0;
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                    }
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.00 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++)
                {
                    shp_dados[i].AdicionaPesosVizinhos(peso, v);
                }
            }

            prBar.Value = 0;
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

            double novopeso, distancia;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
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

            double novopeso, distancia;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            prBar.Value = 0;
            Application.DoEvents();
        }

        protected void DefinicaoVizinhosRook(ref clsIpeaShape shp_dados, ref ProgressBar prBar)
        {
            shp_dados.TipoVizinhanca = "Rook";
            shp_dados.OrdemVizinhanca = 1;

            string[] lista = new string[0];
            bool sai = false;
            double distancia;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            shp_dados[i].AddVizinho(shp_dados[j].Nome, j, distancia, 1.0, shp_dados[j].PosicaoNoDataTable);
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
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome))
                    {
                        distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                        shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, 1.0, shp_dados[i].PosicaoNoDataTable);
                    }
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

            double distancia, peso, novopeso;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.0 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].SetPesoVizinho(v, peso);
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
            double distancia;
            double novopeso;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            double peso;
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.0 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].SetPesoVizinho(v, peso);
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

            double peso, distancia, novopeso;

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
                                            distancia = clsarea.distancia(shp_dados[i].YCentroide, shp_dados[i].XCentroide, shp_dados[j].YCentroide, shp_dados[j].XCentroide, false);
                                            novopeso = 1.0;
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
                    distancia = clsarea.distancia(shp_dados[j].YCentroide, shp_dados[j].XCentroide, shp_dados[i].YCentroide, shp_dados[i].XCentroide);
                    novopeso = 1.0;
                    if (shp_dados[i].EstaNaListaVizinhos(shp_dados[j].Nome)) shp_dados[j].AddVizinho(shp_dados[i].Nome, i, distancia, novopeso, shp_dados[i].PosicaoNoDataTable);
                }
            }

            //Normaliza
            for (int i = 0; i < shp_dados.Count; i++)
            {
                peso = (1.0 / (double)(shp_dados[i].NumeroVizinhos));
                for (int v = 0; v < shp_dados[i].NumeroVizinhos; v++) shp_dados[i].SetPesoVizinho(v, peso);
            }

            prBar.Value = 0;
            Application.DoEvents();
        }

        #endregion

        #endregion
    }
}
