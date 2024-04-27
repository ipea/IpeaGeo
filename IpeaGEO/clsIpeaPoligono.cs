using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpeaMatrix;
using System.Windows.Forms;

namespace IpeaGEO
{
    public class clsIpeaPoligono
    {
        public clsIpeaPoligono()
        {
        }

        #region Mudanças Alexandre
        //private double m_epsilon = 1.0e-6;
        private double m_epsilon = 1.0e-5;

        private int m_indice_cluster = 0;
        private string m_nome = "";
        private int m_posicao = -1;
        private double m_X_centroide = 0.0;
        private double m_Y_centroide = 0.0;
        private double m_area = 0.0;
        private double m_perimetro = 0.0;
        private int m_num_pontos = 0;
        private double m_bounding_box_x_min = 0.0;
        private double m_bounding_box_x_max = 0.0;
        private double m_bounding_box_y_min = 0.0;
        private double m_bounding_box_y_max = 0.0;
        private int m_num_vizinhos = 0;
        private int[] m_lista_indices_vizinhos = new int[0];
        protected double[] m_lista_distancias_vizinhos = new double[0];
        private double[] m_lista_pesos_vizinhos = new double[0];

        private string[] m_lista_vizinhos = new string[0];
        private double[] m_X = new double[0];
        private double[] m_Y = new double[0];

        #region Variáveis internas
 

        public clsIpeaPoligono Clone()
        {
            clsIpeaPoligono res = new clsIpeaPoligono();

            res.m_nome = this.m_nome;
            res.m_posicao = this.m_posicao;
            res.m_X_centroide = this.m_X_centroide;
            res.m_Y_centroide = this.m_Y_centroide;
            res.m_area = this.m_area;
            res.m_perimetro = this.m_perimetro;
            res.m_num_pontos = this.m_num_pontos;
            res.m_bounding_box_x_max = this.m_bounding_box_x_max;
            res.m_bounding_box_x_min = this.m_bounding_box_x_min;
            res.m_bounding_box_y_max = this.m_bounding_box_y_max;
            res.m_bounding_box_y_min = this.m_bounding_box_y_min;
            res.m_num_vizinhos = this.m_num_vizinhos;
            res.m_indice_cluster = this.m_indice_cluster;

            res.m_lista_indices_vizinhos = new int[this.m_lista_indices_vizinhos.GetLength(0)];
            res.m_lista_vizinhos = new string[this.m_lista_vizinhos.GetLength(0)];
            for (int i = 0; i < this.m_lista_vizinhos.GetLength(0); i++)
            {
                res.m_lista_vizinhos[i] = this.m_lista_vizinhos[i];
                res.m_lista_indices_vizinhos[i] = this.m_lista_indices_vizinhos[i];
            }

            res.m_lista_distancias_vizinhos = new double[this.m_lista_distancias_vizinhos.GetLength(0)];
            for (int i = 0; i < this.m_lista_distancias_vizinhos.GetLength(0); i++)
                res.m_lista_distancias_vizinhos[i] = this.m_lista_distancias_vizinhos[i];

            res.m_lista_pesos_vizinhos = new double[this.m_lista_pesos_vizinhos.GetLength(0)];
            for (int i = 0; i < this.m_lista_pesos_vizinhos.GetLength(0); i++)
                res.m_lista_pesos_vizinhos[i] = this.m_lista_pesos_vizinhos[i];

            res.m_X = new double[this.m_X.GetLength(0)];
            res.m_Y = new double[this.m_Y.GetLength(0)];
            for (int i = 0; i < this.m_Y.GetLength(0); i++)
            {
                res.m_X[i] = this.m_X[i];
                res.m_Y[i] = this.m_Y[i];
            }

            //------------- poligonos no cluster --------------//
            res.m_indices_poligonos_no_cluster = new int[this.m_indices_poligonos_no_cluster.GetLength(0)];
            for (int i = 0; i < this.m_indices_poligonos_no_cluster.GetLength(0); i++)
                res.m_indices_poligonos_no_cluster[i] = this.m_indices_poligonos_no_cluster[i];

            return res;
        }

        public int IndiceCluster
        {
            get { return this.m_indice_cluster; }
            set { this.m_indice_cluster = value; }
        }

        public int Count
        {
            get { return this.m_num_pontos; }
        }

        #endregion

        #region variáveis internas específicas para modelagem de dados de painel

        protected int nobs = 0;
        protected double[,] Ydata = new double[0, 0];
        protected double[,] Xdata = new double[0, 0];

        public int NObs_dados
        {
            get { return this.nobs; }
        }

        public double[,] Y_dados
        {
            get { return Ydata; }
            set { this.Ydata = value; }
        }

        public double[,] X_dados
        {
            get { return Xdata; }
            set { this.Xdata = value; }
        }

        #endregion

        #region Funções específicas para análise de clusterização

        protected int[] m_indices_poligonos_no_cluster = new int[0];
        public int[] IndicesPoligonosNoCluster
        {
            get { return this.m_indices_poligonos_no_cluster; }
            set { this.m_indices_poligonos_no_cluster = value; }
        }

        public void ClearIndicesPoligonosNoCluster()
        {
            this.m_indices_poligonos_no_cluster = new int[0];
        }

        public void AddIndicesPoligonosNoCluster(int[] v)
        {
            for (int i = 0; i < v.GetLength(0); i++)
                this.AddIndicePoligonoNoCluster(v[i]);
        }

        public void AddIndicePoligonoNoCluster(int v)
        {
            if (!this.EstaNaListaClusters(v))
            {
                int[] aux = new int[this.m_indices_poligonos_no_cluster.GetLength(0)];
                for (int i = 0; i < aux.GetLength(0); i++)
                    aux[i] = this.m_indices_poligonos_no_cluster[i];

                this.m_indices_poligonos_no_cluster = new int[aux.GetLength(0) + 1];
                for (int i = 0; i < aux.GetLength(0); i++)
                    this.m_indices_poligonos_no_cluster[i] = aux[i];

                this.m_indices_poligonos_no_cluster[aux.GetLength(0)] = v;
            }
        }

        protected bool EstaNaListaClusters(int v)
        {
            for (int i = 0; i < this.m_indices_poligonos_no_cluster.GetLength(0); i++)
            {
                if (this.m_indices_poligonos_no_cluster[i] == v) return true;
            }
            return false;
        }

        #endregion

        #region Funções de manipulação dos vizinhos

        public double[] ListaPesosVizinhos
        {
            get
            {
                double[] res = new double[this.m_num_vizinhos];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = this.m_lista_pesos_vizinhos[i];

                return res;
            }
            set { this.m_lista_pesos_vizinhos = value; }
        }

        public void AdicionaPesosVizinhos(double peso,int iVizinho)
        {
            m_lista_pesos_vizinhos[iVizinho] = peso;
        }

        public double[] ListaDistanciasVizinhos
        {
            get
            {
                double[] res = new double[this.m_num_vizinhos];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = this.m_lista_distancias_vizinhos[i];

                return res;
            }
            set { this.m_lista_distancias_vizinhos = value; }
        }

        public string[] ListaVizinhos
        {
            get { return this.m_lista_vizinhos; }
        }

        public int[] ListaIndicesVizinhos
        {
            get
            {
                int[] res = new int[this.m_num_vizinhos];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = this.m_lista_indices_vizinhos[i];

                return res;
            }
        }

        /// <summary>
        /// Lista o número de vizinhos do polígono
        /// </summary>
        public int NumeroVizinhos
        {
            get { return this.m_num_vizinhos; }
        }

        public void DeleteVizinho(int indice)
        {
            int posicao = -1;
            for (int i = 0; i < this.m_lista_indices_vizinhos.GetLength(0); i++)
            {
                if (this.m_lista_indices_vizinhos[i] == indice)
                {
                    posicao = i;
                    break;
                }
            }

            if (posicao >= 0)
            {
                if (this.m_num_vizinhos <= 1)
                {
                    this.m_num_vizinhos = 0;
                    this.m_lista_indices_vizinhos = new int[0];
                    this.m_lista_vizinhos = new string[0];
                    this.m_lista_distancias_vizinhos = new double[0];
                    this.m_lista_pesos_vizinhos = new double[0];

                }
                else
                {
                    string[] temp = new string[this.m_num_vizinhos - 1];
                    int[] temp1 = new int[this.m_num_vizinhos - 1];
                    double[] temp2 = new double[this.m_num_vizinhos - 1];
                    double[] temp3 = new double[this.m_num_vizinhos - 1];
                    int k = 0;

                    for (int i = 0; i < this.m_lista_indices_vizinhos.GetLength(0); i++)
                    {
                        if (i != posicao)
                        {
                            temp[k] = this.m_lista_vizinhos[i];
                            temp1[k] = this.m_lista_indices_vizinhos[i];
                            temp2[k] = this.m_lista_distancias_vizinhos[i];
                            temp3[k] = this.m_lista_pesos_vizinhos[i];
                            k++;
                        }
                    }

                    this.m_num_vizinhos--;

                    this.m_lista_vizinhos = new string[this.m_num_vizinhos];
                    this.m_lista_indices_vizinhos = new int[this.m_num_vizinhos];
                    this.m_lista_distancias_vizinhos = new double[this.m_num_vizinhos];
                    this.m_lista_pesos_vizinhos = new double[this.m_num_vizinhos];


                    for (int i = 0; i < temp.GetLength(0); i++)
                    {
                        this.m_lista_vizinhos[i] = temp[i];
                        this.m_lista_indices_vizinhos[i] = temp1[i];
                        this.m_lista_distancias_vizinhos[i] = temp2[i];
                        this.m_lista_pesos_vizinhos[i] = temp2[i];
                    }

                    temp = null;
                    temp1 = null;
                    temp2 = null;
                }
            }
        }

        //public void AddListaVizinhos(string[] lista, int[] indices)
        //{
        //   for (int i = 0; i < lista.GetLength(0); i++)
        //        this.AddVizinho(lista[i], indices[i]);
        //}

        public void AddListaVizinhos(int[] indices)
        {
            double[] dist = new double[indices.GetLength(0)];
            double[] peso = new double[indices.GetLength(0)];

            for (int i = 0; i < dist.GetLength(0); i++)
            {
                dist[i] = 0.0;
                peso[i] = 0.0;

            }

            this.AddListaVizinhos(indices, dist, peso);
        }

        public void AddListaVizinhos(int[] indices, double[] distancias_novos_vizinhos, double[]  pesos_novos_vizinhos)
        {
            if (indices.GetLength(0) != distancias_novos_vizinhos.GetLength(0))
                throw new Exception("Problemas com dimensão dos vetores de índices e de distâncias");

            int counter = 0;
            for (int i = 0; i < indices.GetLength(0); i++)
            {
                if (indices[i] != this.m_indice_cluster && !this.EstaNaListaVizinhos(indices[i]))
                    counter++;
            }

            int[] temp1 = new int[this.m_num_vizinhos];
            string[] temp = new string[this.m_num_vizinhos];
            double[] temp2 = new double[this.m_num_vizinhos];
            double[] temp3 = new double[this.m_num_vizinhos];


            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                temp[i] = this.m_lista_vizinhos[i];
                temp1[i] = this.m_lista_indices_vizinhos[i];
                temp2[i] = this.m_lista_distancias_vizinhos[i];
                temp3[i] = this.m_lista_pesos_vizinhos[i];

            }

            this.m_lista_vizinhos = new string[counter + this.m_num_vizinhos];
            this.m_lista_indices_vizinhos = new int[counter + this.m_num_vizinhos];
            this.m_lista_distancias_vizinhos = new double[counter + this.m_num_vizinhos];
            this.m_lista_pesos_vizinhos = new double[counter + this.m_num_vizinhos];


            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                this.m_lista_vizinhos[i] = temp[i];
                this.m_lista_indices_vizinhos[i] = temp1[i];
                this.m_lista_distancias_vizinhos[i] = temp2[i];
                this.m_lista_pesos_vizinhos[i] = temp3[i];
            }

            counter = this.m_num_vizinhos;
            for (int i = 0; i < indices.GetLength(0); i++)
            {
                if (indices[i] != this.m_indice_cluster && !this.EstaNaListaVizinhos(indices[i]))
                {
                    this.m_lista_vizinhos[counter] = "Polígono " + indices[i];
                    this.m_lista_indices_vizinhos[counter] = indices[i];
                    this.m_lista_distancias_vizinhos[counter] = distancias_novos_vizinhos[i];
                    this.m_lista_pesos_vizinhos[counter] = pesos_novos_vizinhos[i];
                    counter++;
                }
            }

            temp = null;
            temp1 = null;
            temp2 = null;
            temp3 = null;

            this.m_num_vizinhos = this.m_lista_indices_vizinhos.GetLength(0);
        }

        /// <summary>
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        public void AddVizinho(int indice)
        {
            this.AddVizinho(indice, 0.0,0.0);
        }

        /// <summary>
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        public void AddVizinho(int indice, double distancia_novo_vizinho, double peso_novo_vizinho)
        {
            if (!this.EstaNaListaVizinhos(indice) && (indice != this.m_indice_cluster))
            {
                string[] temp = new string[this.m_num_vizinhos];
                int[] temp1 = new int[this.m_num_vizinhos];
                double[] temp2 = new double[this.m_num_vizinhos];
                double[] temp3 = new double[this.m_num_vizinhos];


                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    temp[i] = this.m_lista_vizinhos[i];
                    temp1[i] = this.m_lista_indices_vizinhos[i];
                    temp2[i] = this.m_lista_distancias_vizinhos[i];
                    temp3[i] = this.m_lista_pesos_vizinhos[i];

                }

                this.m_lista_vizinhos = new string[this.m_num_vizinhos + 1];
                this.m_lista_indices_vizinhos = new int[this.m_num_vizinhos + 1];
                this.m_lista_distancias_vizinhos = new double[this.m_num_vizinhos + 1];
                this.m_lista_pesos_vizinhos = new double[this.m_num_vizinhos + 1];


                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    this.m_lista_vizinhos[i] = temp[i];
                    this.m_lista_indices_vizinhos[i] = temp1[i];
                    this.m_lista_distancias_vizinhos[i] = temp2[i];
                    this.m_lista_pesos_vizinhos[i] = temp3[i];
                }

                this.m_lista_vizinhos[this.m_num_vizinhos] = "Polígono " + indice.ToString();
                this.m_lista_indices_vizinhos[this.m_num_vizinhos] = indice;
                this.m_lista_distancias_vizinhos[this.m_num_vizinhos] = distancia_novo_vizinho;
                this.m_lista_distancias_vizinhos[this.m_num_vizinhos] = peso_novo_vizinho;

                this.m_num_vizinhos++;

                temp = null;
                temp1 = null;
                temp2 = null;
                temp3 = null;
            }
        }

        /// <summary>
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        public void AddVizinho(string v, int indice)
        {
            this.AddVizinho(v, indice, 0.0,1.0);
        }


        

        /// <summary>
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        public void AddVizinho(string v, int indice, double distancia_novo_vizinho,double peso_novo_vizinho)
        {
            if (!this.EstaNaListaVizinhos(v) && (v != this.m_nome))
            {
                string[] temp = new string[this.m_num_vizinhos];
                int[] temp1 = new int[this.m_num_vizinhos];
                double[] temp2 = new double[this.m_num_vizinhos];
                double[] temp3 = new double[this.m_num_vizinhos];


                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    temp[i] = this.m_lista_vizinhos[i];
                    temp1[i] = this.m_lista_indices_vizinhos[i];
                    temp2[i] = this.m_lista_distancias_vizinhos[i];
                    temp3[i] = this.m_lista_pesos_vizinhos[i];

                }

                this.m_lista_vizinhos = new string[this.m_num_vizinhos + 1];
                this.m_lista_indices_vizinhos = new int[this.m_num_vizinhos + 1];
                this.m_lista_distancias_vizinhos = new double[this.m_num_vizinhos + 1];
                this.m_lista_pesos_vizinhos = new double[this.m_num_vizinhos + 1];


                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    this.m_lista_vizinhos[i] = temp[i];
                    this.m_lista_indices_vizinhos[i] = temp1[i];
                    this.m_lista_distancias_vizinhos[i] = temp2[i];
                    this.m_lista_pesos_vizinhos[i] = temp3[i];

                }

                this.m_lista_vizinhos[this.m_num_vizinhos] = v;
                this.m_lista_indices_vizinhos[this.m_num_vizinhos] = indice;
                this.m_lista_distancias_vizinhos[this.m_num_vizinhos] = distancia_novo_vizinho;
                this.m_lista_pesos_vizinhos[this.m_num_vizinhos] = peso_novo_vizinho;

                this.m_num_vizinhos++;

                temp = null;
                temp1 = null;
                temp2 = null;
                temp3 = null;
            }
        }

        /// <summary>
        /// Verifica se um determinado polígono é vizinho ou não
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool EstaNaListaVizinhos(string v)
        {
            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                if (v == this.m_lista_vizinhos[i]) return true;
            }
            return false;
        }

        /// <summary>
        /// Verifica se um determinado polígono é vizinho ou não
        /// </summary>
        public bool EstaNaListaVizinhos(int v)
        {
            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                if (v == this.m_lista_indices_vizinhos[i]) return true;
            }
            return false;
        }

        /// <summary>
        /// Espeficica a lista de todos os vizinhos
        /// </summary>
        public void AddListaAllVizinhos(int[] indices)
        {
            double[] dist = new double[indices.GetLength(0)];
            double[] peso = new double[indices.GetLength(0)];

            for (int i = 0; i < dist.GetLength(0); i++)
            {
                dist[i] = 0.0;
                peso[i] = 0.0;

            }

            this.AddListaAllVizinhos(indices, dist,peso);
        }

        /// <summary>
        /// Espeficica a lista de todos os vizinhos
        /// </summary>
        public void AddListaAllVizinhos(int[] indices, double[] distancias_vizinhos, double[] pesos_vizinhos)
        {
            if (indices.GetLength(0) != distancias_vizinhos.GetLength(0))
                throw new Exception("Dimensões dos vetores de índices e de distâncias não confererem");

            int counter = 0;
            for (int i = 0; i < indices.GetLength(0); i++)
                if (indices[i] != this.m_indice_cluster) counter++;

            this.m_num_vizinhos = counter;
            this.m_lista_indices_vizinhos = new int[counter];
            this.m_lista_vizinhos = new string[counter];
            this.m_lista_distancias_vizinhos = new double[counter];
            this.m_lista_pesos_vizinhos = new double[counter];


            counter = 0;
            for (int i = 0; i < indices.GetLength(0); i++)
            {
                if (indices[i] != this.m_indice_cluster)
                {
                    this.m_lista_indices_vizinhos[counter] = indices[i];
                    this.m_lista_vizinhos[counter] = "Polígono " + indices[i].ToString();
                    this.m_lista_distancias_vizinhos[counter] = distancias_vizinhos[i];
                    counter++;
                }
            }

            //m_num_vizinhos = indices.GetLength(0);
            //for (int i = 0; i < m_num_vizinhos; i++)
            //{
            //    m_lista_vizinhos[i] = "Polígono " + indices[i].ToString();
            //    m_lista_indices_vizinhos[i] = indices[i];
            //}
        }

        /// <summary>
        /// Espeficica a lista de todos os vizinhos
        /// </summary>
        public void AddListaAllVizinhos(string[] vlist, int[] indices)
        {
            double[] dist = new double[indices.GetLength(0)];
            double[] peso = new double[indices.GetLength(0)];

            for (int i = 0; i < dist.GetLength(0); i++)
            {
                dist[i] = 0.0;
                peso[i] = 0.0;

            }

            this.AddListaAllVizinhos(vlist, indices, dist,peso);
        }

        /// <summary>
        /// Espeficica a lista de todos os vizinhos
        /// </summary>
        public void AddListaAllVizinhos(string[] vlist, int[] indices, double[] distancias_vizinhos, double[] pesos_vizinhos)
        {
            if (indices.GetLength(0) != distancias_vizinhos.GetLength(0) ||
                indices.GetLength(0) != vlist.GetLength(0))
                throw new Exception("Dimensões dos vetores de índices e de distâncias não confererem");

            int counter = 0;
            for (int i = 0; i < indices.GetLength(0); i++)
                if (indices[i] != this.m_indice_cluster) counter++;

            this.m_num_vizinhos = counter;
            this.m_lista_indices_vizinhos = new int[counter];
            this.m_lista_vizinhos = new string[counter];
            this.m_lista_distancias_vizinhos = new double[counter];
            this.m_lista_pesos_vizinhos = new double[counter];


            counter = 0;
            for (int i = 0; i < indices.GetLength(0); i++)
            {
                if (indices[i] != this.m_indice_cluster)
                {
                    this.m_lista_indices_vizinhos[counter] = indices[i];
                    this.m_lista_vizinhos[counter] = vlist[i];
                    this.m_lista_distancias_vizinhos[counter] = distancias_vizinhos[i];
                    this.m_lista_pesos_vizinhos[counter] = pesos_vizinhos[i];

                    counter++;
                }
            }

            //m_num_vizinhos = vlist.GetLength(0);
            //for (int i = 0; i < m_num_vizinhos; i++)
            //{
            //    m_lista_vizinhos[i] = vlist[i];
            //    m_lista_indices_vizinhos[i] = indices[i];
            //}
        }

        public void UpdateDistancia(string v, double nova_distancia)
        {
            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                if (v == this.m_lista_vizinhos[i])
                    this.m_lista_distancias_vizinhos[i] = nova_distancia;
            }
        }

        public void UpdateDistancia(int indice, double nova_distancia)
        {
            for (int i = 0; i < this.m_num_vizinhos; i++)
            {
                if (indice == this.m_lista_indices_vizinhos[i])
                    this.m_lista_distancias_vizinhos[i] = nova_distancia;
            }
        }

        #endregion

        #region Funções de geoferenciamento

        public double Area
        {
            get { return this.m_area; }
            set { this.m_area = value; }
        }

        public double Perimetro
        {
            get { return this.m_perimetro; }
            set { this.m_perimetro = value; }
        }

        public double YCentroide
        {
            get { return this.m_Y_centroide; }
            set { this.m_Y_centroide = value; }
        }

        public double XCentroide
        {
            get { return this.m_X_centroide; }
            set { this.m_X_centroide = value; }
        }

        public string Nome
        {
            get { return m_nome; }
            set { this.m_nome = value; }
        }

        /// <summary>
        /// Identifica qual a posição do polígono de acordo com a base (A posição no SharpMap é igual ao número do polígono).
        /// </summary>
        public int PosicaoNoDataTable
        {
            get { return m_posicao; }
            set { this.m_posicao = value; }
        }

        public double X(int i) { return this.m_X[i]; }
        public double Y(int i) { return this.m_Y[i]; }

        public double[] GetArrayX() { return this.m_X; }
        public double[] GetArrayY() { return this.m_Y; }

        public void AddCoordenadasX(double[] vx)
        {
            m_num_pontos = vx.GetLength(0);
            this.m_X = new double[vx.GetLength(0)];
            this.m_bounding_box_x_max = vx[0];
            this.m_bounding_box_x_min = vx[0];
            for (int i = 0; i < this.m_X.GetLength(0); i++)
            {
                this.m_X[i] = vx[i];
                if (this.m_bounding_box_x_min > vx[i]) this.m_bounding_box_x_min = vx[i];
                if (this.m_bounding_box_x_max < vx[i]) this.m_bounding_box_x_max = vx[i];
            }

            this.m_epsilon = Math.Min(this.m_bounding_box_x_max - this.m_bounding_box_x_min,
                this.m_bounding_box_y_max - this.m_bounding_box_y_min) / 1.0e+8;
        }

        public void AddCoordenadasY(double[] vy)
        {
            m_num_pontos = vy.GetLength(0);
            this.m_Y = new double[vy.GetLength(0)];
            this.m_bounding_box_y_max = vy[0];
            this.m_bounding_box_y_min = vy[0];
            for (int i = 0; i < this.m_Y.GetLength(0); i++)
            {
                this.m_Y[i] = vy[i];
                if (this.m_bounding_box_y_min > vy[i]) this.m_bounding_box_y_min = vy[i];
                if (this.m_bounding_box_y_max < vy[i]) this.m_bounding_box_y_max = vy[i];
            }

            this.m_epsilon = Math.Min(this.m_bounding_box_x_max - this.m_bounding_box_x_min,
                this.m_bounding_box_y_max - this.m_bounding_box_y_min) / 1.0e+8;
        }

        public double BoundingBoxXMin
        {
            get { return this.m_bounding_box_x_min; }
        }

        public double BoundingBoxXMax
        {
            get { return this.m_bounding_box_x_max; }
        }

        public double BoundingBoxYMin
        {
            get { return this.m_bounding_box_y_min; }
        }

        public double BoundingBoxYMax
        {
            get { return this.m_bounding_box_y_max; }
        }

        public bool IsInPoligono(double x, double y)
        {
            if (x > this.m_bounding_box_x_max - this.m_epsilon) return false;
            if (x < this.m_bounding_box_x_min + this.m_epsilon) return false;
            if (y < this.m_bounding_box_y_min + this.m_epsilon) return false;
            if (y > this.m_bounding_box_y_max - this.m_epsilon) return false;

            int n_cruzamentos = 0;
            double beta = 0.0;
            double alpha = 0.0;

            double y1 = 0.0;
            double y2 = 0.0;
            double x1 = 0.0;
            double x2 = 0.0;

            for (int i = 0; i < this.m_num_pontos; i++)
            {
                y1 = this.m_Y[i];
                x1 = this.m_X[i];

                if (i < this.m_num_pontos - 1)
                {
                    y2 = this.m_Y[i + 1];
                    x2 = this.m_X[i + 1];
                }
                else
                {
                    y2 = this.m_Y[0];
                    x2 = this.m_X[0];
                }

                if (y1 <= y && y2 >= y)
                {
                    if (y1 == y2)
                    {
                        if ((y1 - y) * (y2 - y) < 0.0) return true;
                        else continue;
                    }

                    if (x1 == x2)
                    {
                        if (x1 == x) return true;
                        else
                        {
                            if (x1 > x)
                            {
                                n_cruzamentos++;
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    beta = (y2 - y1) / (x2 - x1);
                    alpha = y1 - x1 * beta;
                    //if ((y - alpha) / beta > x)
                    if (y - alpha - beta * x > 0)
                    {
                        n_cruzamentos++;
                    }
                    if ((y - alpha) / beta == x) return true;
                }
            }

            if (Math.Floor((double)n_cruzamentos / 2.0) != (double)n_cruzamentos / 2.0) return true;

            return false;
        }

        #endregion 


        #endregion


        private string m_ID = "";
        /// <summary>
        /// Variável ID
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        /// <param name="v"></param>
        public void AddVizinho(string v)
        {
            if (!this.EstaNaListaVizinhos(v))
            {
                string[] temp = new string[this.m_num_vizinhos];
                for (int i = 0; i < temp.GetLength(0); i++) temp[i] = this.m_lista_vizinhos[i];

                this.m_lista_vizinhos = new string[this.m_num_vizinhos + 1];
                for (int i = 0; i < temp.GetLength(0); i++) this.m_lista_vizinhos[i] = temp[i];
                this.m_lista_vizinhos[this.m_num_vizinhos] = v;
                this.m_num_vizinhos++;
                temp = null;
            }
        }

    
        public void AddListaAllVizinhos(string[] vlist)
        {
            m_num_vizinhos = vlist.GetLength(0);
            for (int i = 0; i < m_num_vizinhos; i++) m_lista_vizinhos[i] = vlist[i];
        }

    }
}
