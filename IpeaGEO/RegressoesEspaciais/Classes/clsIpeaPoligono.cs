using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsIpeaPoligono
    {
        public clsIpeaPoligono()
        {
        }

        /// <summary>
        /// Lista de indicadores especificando se os subpolígonos adicionados à estrutura são inner ring ou outer ring. Os subpolígonos
        /// do interior serão chamados de 'InteriorRing', e do exterior serão chamados de 'ExteriorRing'. 
        /// </summary>
        public string[] TiposSubpoligonos
        {
            get { return this.m_tipos_subpoligonos; }
            set { this.m_tipos_subpoligonos = value; }
        }

        public void AddRing(string tipo)
        {
            string[] novo = new string[this.m_tipos_subpoligonos.GetLength(0) + 1];
            for (int i = 0; i < this.m_tipos_subpoligonos.GetLength(0); i++) { novo[i] = this.m_tipos_subpoligonos[i]; }
            novo[this.m_tipos_subpoligonos.GetLength(0)] = tipo;
            m_tipos_subpoligonos = novo;
        }

        //protected double m_epsilon = 1.0e-6;
        protected double m_epsilon = 1.0e-5;
        protected string[] m_tipos_subpoligonos = new string[0];

        protected int m_indice_cluster = 0;
        protected string m_nome = "";
        protected string m_label = "";
        protected int m_posicao = -1;
        protected double m_X_centroide = double.NaN;
        protected double m_Y_centroide = double.NaN;
        protected double m_area = -1.0;
        protected double m_perimetro = 0.0;
        protected int m_num_pontos = 0;
        protected double m_bounding_box_x_min = 0.0;
        protected double m_bounding_box_x_max = 0.0;
        protected double m_bounding_box_y_min = 0.0;
        protected double m_bounding_box_y_max = 0.0;
        protected int m_num_vizinhos = 0;
        protected int[] m_lista_indices_vizinhos = new int[0];
        protected double[] m_lista_distancias_vizinhos = new double[0];
        protected double[] m_lista_distancias = new double[0];
        protected double[] m_lista_pesos_vizinhos = new double[0];
        protected double[] m_lista_id_datatable_vizinhos = new double[0];
        protected int m_num_total_poligonos = 0;

        protected string[] m_lista_vizinhos = new string[0];
        protected double[] m_X = new double[0];
        protected double[] m_Y = new double[0];

        public double[] ListaDistancias
        {
            get
            {
                return this.m_lista_distancias;
            }
            set
            {
                this.m_lista_distancias = value;
            }
        }

        public void SetX(double[] x)
        {
            m_X = new double[x.GetLength(0)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m_X[i] = x[i];
            }
        }

        public void SetY(double[] y)
        {
            m_Y = new double[y.GetLength(0)];
            for (int i = 0; i < y.GetLength(0); i++)
            {
                m_Y[i] = y[i];
            }
        }

        public void ConvertFromIpeaGEOPoligono(clsIpeaPoligono c)
        {
            this.Nome = c.Nome;
            this.Label = c.Label;
            this.PosicaoNoDataTable = c.PosicaoNoDataTable;
            this.YCentroide = c.YCentroide;
            this.XCentroide = c.XCentroide;
            this.Area = c.Area;
            this.Perimetro = c.Perimetro;
            this.Count = c.Count;
            this.BoundingBoxXMax = c.BoundingBoxXMax;
            this.BoundingBoxXMin = c.BoundingBoxXMin;
            this.BoundingBoxYMax = c.BoundingBoxYMax;
            this.BoundingBoxYMin = c.BoundingBoxYMin;
            this.NumeroVizinhos = c.NumeroVizinhos;
            this.IndiceCluster = c.IndiceCluster;

            this.ListaIndicesVizinhos = new int[c.ListaIndicesVizinhos.GetLength(0)];
            this.ListaVizinhos = new string[c.ListaVizinhos.GetLength(0)];
            this.ListaDistanciasVizinhos = new double[c.ListaDistanciasVizinhos.GetLength(0)];
            this.ListaPesosVizinhos = new double[c.ListaPesosVizinhos.GetLength(0)];
            this.IndicesPoligonosNoCluster = new int[c.IndicesPoligonosNoCluster.GetLength(0)];
            this.TiposSubpoligonos = new string[c.TiposSubpoligonos.GetLength(0)];
            this.ListaDistancias = new double[c.ListaDistancias.GetLength(0)];

            for (int i = 0; i < c.ListaIndicesVizinhos.GetLength(0); i++) { this.ListaIndicesVizinhos[i] = c.ListaIndicesVizinhos[i]; }
            for (int i = 0; i < c.ListaVizinhos.GetLength(0); i++) { this.ListaVizinhos[i] = c.ListaVizinhos[i]; }
            for (int i = 0; i < c.ListaDistanciasVizinhos.GetLength(0); i++) { this.ListaDistanciasVizinhos[i] = c.ListaDistanciasVizinhos[i]; }
            for (int i = 0; i < c.ListaPesosVizinhos.GetLength(0); i++) { this.ListaPesosVizinhos[i] = c.ListaPesosVizinhos[i]; }
            for (int i = 0; i < c.IndicesPoligonosNoCluster.GetLength(0); i++) { this.IndicesPoligonosNoCluster[i] = c.IndicesPoligonosNoCluster[i]; }
            for (int i = 0; i < c.TiposSubpoligonos.GetLength(0); i++) { this.TiposSubpoligonos[i] = c.TiposSubpoligonos[i]; }
            for (int i = 0; i < c.ListaDistancias.GetLength(0); i++) { this.ListaDistancias[i] = c.ListaDistancias[i]; }

            this.SetX(c.GetArrayX()); 
            this.SetY(c.GetArrayY());
        }

        public clsIpeaPoligono ConvertToIpeaGEOPoligono()
        {
            clsIpeaPoligono c = new clsIpeaPoligono();

            c.Nome = this.Nome;
            c.Label = this.Label;
            c.PosicaoNoDataTable = this.PosicaoNoDataTable;
            c.YCentroide = this.YCentroide;
            c.XCentroide = this.XCentroide;
            c.Area = this.Area;
            c.Perimetro = this.Perimetro;
            c.Count = this.Count;
            c.BoundingBoxXMax = this.BoundingBoxXMax;
            c.BoundingBoxXMin = this.BoundingBoxXMin;
            c.BoundingBoxYMax = this.BoundingBoxYMax;
            c.BoundingBoxYMin = this.BoundingBoxYMin;
            c.NumeroVizinhos = this.NumeroVizinhos;
            c.IndiceCluster = this.IndiceCluster;

            c.ListaIndicesVizinhos = new int[this.ListaIndicesVizinhos.GetLength(0)];
            c.ListaVizinhos = new string[this.ListaVizinhos.GetLength(0)];
            c.ListaDistanciasVizinhos = new double[this.ListaDistanciasVizinhos.GetLength(0)];
            c.ListaPesosVizinhos = new double[this.ListaPesosVizinhos.GetLength(0)];
            c.IndicesPoligonosNoCluster = new int[this.IndicesPoligonosNoCluster.GetLength(0)];
            c.TiposSubpoligonos = new string[this.TiposSubpoligonos.GetLength(0)];
            c.ListaDistancias = new double[this.ListaDistancias.GetLength(0)];

            for (int i = 0; i < this.ListaIndicesVizinhos.GetLength(0); i++) { c.ListaIndicesVizinhos[i] = this.ListaIndicesVizinhos[i]; }
            for (int i = 0; i < this.ListaVizinhos.GetLength(0); i++) { c.ListaVizinhos[i] = this.ListaVizinhos[i]; }
            for (int i = 0; i < this.ListaDistanciasVizinhos.GetLength(0); i++) { c.ListaDistanciasVizinhos[i] = this.ListaDistanciasVizinhos[i]; }
            for (int i = 0; i < this.ListaPesosVizinhos.GetLength(0); i++) { c.ListaPesosVizinhos[i] = this.ListaPesosVizinhos[i]; }
            for (int i = 0; i < this.IndicesPoligonosNoCluster.GetLength(0); i++) { c.IndicesPoligonosNoCluster[i] = this.IndicesPoligonosNoCluster[i]; }
            for (int i = 0; i < this.TiposSubpoligonos.GetLength(0); i++) { c.TiposSubpoligonos[i] = this.TiposSubpoligonos[i]; }
            for (int i = 0; i < this.ListaDistancias.GetLength(0); i++) { c.ListaDistancias[i] = this.ListaDistancias[i]; }

            c.SetX(this.GetArrayX()); 
            c.SetY(this.GetArrayY());

            return c;
        }

        #region Variáveis internas

        public string Label
        {
            get { return this.m_label; }
            set { this.m_label = value; }
        }

        public clsIpeaPoligono Clone()
        {
            clsIpeaPoligono res = new clsIpeaPoligono();

            res.m_tipos_subpoligonos = new string[this.m_tipos_subpoligonos.GetLength(0)];
            for (int i = 0; i < res.m_tipos_subpoligonos.GetLength(0); i++)
            {
                res.m_tipos_subpoligonos[i] = this.m_tipos_subpoligonos[i];
            }

            res.m_nome = this.m_nome;
            res.m_label = this.m_label;
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

            res.m_lista_distancias = new double[this.m_lista_distancias.GetLength(0)];
            for (int i = 0; i < this.m_lista_distancias.GetLength(0); i++)
                res.m_lista_distancias[i] = this.m_lista_distancias[i];

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
            set
            {
            	this.m_num_pontos = value;
            }
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

        public void SetPesoVizinho(int i, double valor)
        {
            this.m_lista_pesos_vizinhos[i] = valor;
        }

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

        public double[] ListaIDnoDataTableVizinhos
        {
            get
            {
                double[] res = new double[this.m_num_vizinhos];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = this.m_lista_id_datatable_vizinhos[i];

                return res;
            }
            set { this.m_lista_id_datatable_vizinhos = value; }
        }

        public void AdicionaPesosVizinhos(double peso, int iVizinho)
        {
            m_lista_pesos_vizinhos[iVizinho] = peso;
        }

        public double[] ListaDistanciasVizinhos
        {
            get
            {
                //double[] res = new double[this.m_num_vizinhos];
                //for (int i = 0; i < res.GetLength(0); i++)
                //    res[i] = this.m_lista_distancias_vizinhos[i];

                //return res;

                return m_lista_distancias_vizinhos;
            }
            set 
            { 
                this.m_lista_distancias_vizinhos = value; 
            }
        }

        public string[] ListaVizinhos
        {
            get { return this.m_lista_vizinhos; }
            set
            {
            	this.m_lista_vizinhos = value;
            }
        }

        public int[] ListaIndicesVizinhos
        {
            get
            {
                //int[] res = new int[this.m_num_vizinhos];
                //for (int i = 0; i < res.GetLength(0); i++)
                //    res[i] = this.m_lista_indices_vizinhos[i];

                //return res;

                return this.m_lista_indices_vizinhos;
            }
            set
            {
                //int[] res = new int[value.GetLength(0)];
                //for (int i = 0; i < res.GetLength(0); i++)
                //    res[i] = value[i];

                //this.m_lista_indices_vizinhos = res;

                this.m_lista_indices_vizinhos = value;
            }
        }

        /// <summary>
        /// Lista o número de vizinhos do polígono
        /// </summary>
        public int NumeroVizinhos
        {
            get { return this.m_num_vizinhos; }
            set
            {
            	this.m_num_vizinhos = value;
            }
        }

        public void DeleteAllVizinhosAt()
        {
            for (int i = this.m_num_vizinhos - 1; i >= 0; i--)
            {
                this.DeleteVizinhoAt(i);
            }
        }

        public void DeleteAllVizinhos()
        {
            int[] lista_vizinhos = (int[])this.m_lista_indices_vizinhos.Clone();
            for (int i = this.m_num_vizinhos-1; i >= 0; i--)
            {
                this.DeleteVizinho(lista_vizinhos[i]);
            }
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
        
        public void DeleteVizinhoAt(int posicao)
        {
            if (posicao >= 0 && posicao < m_lista_indices_vizinhos.GetLength(0))
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
        /// Adiciona um vizinho a lista de polígonos adjacentes
        /// </summary>
        /// <param name="v"></param>
        /// <param name="indice"></param>
        /// <param name="distancia_novo_vizinho"></param>
        /// <param name="peso_novo_vizinho"></param>
        public void AddVizinho(string v, int indice, double distancia_novo_vizinho, double peso_novo_vizinho, int iposicao_datatable_vizinho)
        {
            if (!this.EstaNaListaVizinhos(v) && (v != this.m_nome))
            {
                string[] temp = new string[this.m_num_vizinhos];
                int[] temp1 = new int[this.m_num_vizinhos];
                double[] temp2 = new double[this.m_num_vizinhos];
                double[] temp3 = new double[this.m_num_vizinhos];
                double[] temp4 = new double[this.m_num_vizinhos];

                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    temp[i] = this.m_lista_vizinhos[i];
                    temp1[i] = this.m_lista_indices_vizinhos[i];
                    temp2[i] = this.m_lista_distancias_vizinhos[i];
                    temp3[i] = this.m_lista_pesos_vizinhos[i];
                    temp4[i] = this.m_lista_id_datatable_vizinhos[i];

                }

                this.m_lista_vizinhos = new string[this.m_num_vizinhos + 1];
                this.m_lista_indices_vizinhos = new int[this.m_num_vizinhos + 1];
                this.m_lista_distancias_vizinhos = new double[this.m_num_vizinhos + 1];
                this.m_lista_pesos_vizinhos = new double[this.m_num_vizinhos + 1];
                this.m_lista_id_datatable_vizinhos = new double[this.m_num_vizinhos + 1];

                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    this.m_lista_vizinhos[i] = temp[i];
                    this.m_lista_indices_vizinhos[i] = temp1[i];
                    this.m_lista_distancias_vizinhos[i] = temp2[i];
                    this.m_lista_pesos_vizinhos[i] = temp3[i];
                    this.m_lista_id_datatable_vizinhos[i] = temp4[i];

                }

                this.m_lista_vizinhos[this.m_num_vizinhos] = v;
                this.m_lista_indices_vizinhos[this.m_num_vizinhos] = indice;
                this.m_lista_distancias_vizinhos[this.m_num_vizinhos] = distancia_novo_vizinho;
                this.m_lista_pesos_vizinhos[this.m_num_vizinhos] = peso_novo_vizinho;
                this.m_lista_id_datatable_vizinhos[this.m_num_vizinhos] = iposicao_datatable_vizinho;

                this.m_num_vizinhos++;

                temp = null;
                temp1 = null;
                temp2 = null;
                temp3 = null;
                temp4 = null;
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

        protected bool m_estrutura_triangulos_definida = false;
        protected TriangulosFromPoligonos m_estrutura_triangulos = new TriangulosFromPoligonos();

        protected void DefineEstruturaTriangulos()
        {
            ArrayList x_temp = new ArrayList();
            ArrayList y_temp = new ArrayList();
            bool external_ring = true;

            for (int i = 0; i < m_X.GetLength(0); i++)
            {
                if (!double.IsNaN(m_X[i]))
                {
                    x_temp.Add(m_X[i]);
                    y_temp.Add(m_Y[i]);
                }
                if (double.IsNaN(m_X[i]) || i == m_X.GetLength(0) - 1)
                {
                    double[] vx = new double[x_temp.Count];
                    double[] vy = new double[x_temp.Count];
                    for (int k = 0; k < x_temp.Count; k++)
                    {
                        vx[k] = Convert.ToDouble(x_temp[k]);
                        vy[k] = Convert.ToDouble(y_temp[k]);
                    }
                    m_estrutura_triangulos.AdicionaTriangulosFromPoligono(vx, vy, external_ring);
                    x_temp.Clear();
                    y_temp.Clear();
                    external_ring = false;
                }
            }
            m_estrutura_triangulos_definida = true;
        }

        protected void CalculaArea()
        {
            if (!m_estrutura_triangulos_definida)
            {
                DefineEstruturaTriangulos();
            }
            this.m_area = m_estrutura_triangulos.AreaTotal();
        }

        protected void CalculaCentroids()
        {
            if (!m_estrutura_triangulos_definida)
            {
                DefineEstruturaTriangulos();
            }
            double[] res = m_estrutura_triangulos.CentroidCoords();
            this.m_X_centroide = res[0];
            this.m_Y_centroide = res[1];
        }

        public double Area
        {
            get
            {
                if (this.m_area < 0.0 || double.IsNaN(m_area))
                {
                    CalculaArea();
                }
                return this.m_area;
            }
            set { this.m_area = value; }
        }

        public double Perimetro
        {
            get { return this.m_perimetro; }
            set { this.m_perimetro = value; }
        }

        public double YCentroide
        {
            get
            {
                if (Double.IsNaN(this.m_Y_centroide))
                {
                    CalculaCentroids();
                }
                return this.m_Y_centroide;
            }
            set { this.m_Y_centroide = value; }
        }

        public double XCentroide
        {
            get
            {
                if (Double.IsNaN(this.m_X_centroide))
                {
                    CalculaCentroids();
                }
                return this.m_X_centroide;
            }
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
            set
            {
            	this.m_bounding_box_x_min = value;
            }
        }

        public double BoundingBoxXMax
        {
            get { return this.m_bounding_box_x_max; }
            set
            {
            	this.m_bounding_box_x_max = value;
            }
        }

        public double BoundingBoxYMin
        {
            get { return this.m_bounding_box_y_min; }
            set
            {
            	this.m_bounding_box_y_min = value;
            }
        }

        public double BoundingBoxYMax
        {
            get { return this.m_bounding_box_y_max; }
            set
            {
            	this.m_bounding_box_y_max = value;
            }
        }

        public bool IsInPoligono(double x, double y)
        {
            if (!m_estrutura_triangulos_definida)
            {
                DefineEstruturaTriangulos();
            }

            if (x > this.m_bounding_box_x_max - this.m_epsilon) return false;
            if (x < this.m_bounding_box_x_min + this.m_epsilon) return false;
            if (y < this.m_bounding_box_y_min + this.m_epsilon) return false;
            if (y > this.m_bounding_box_y_max - this.m_epsilon) return false;

            return m_estrutura_triangulos.IsInPoligono(x, y);
        }

        #endregion 
        
        public void Determine_Nobs(int nobs)
        {
            this.m_num_total_poligonos = nobs;
            this.m_lista_distancias = new double[m_num_total_poligonos];
            this.ListaDistancias = new double[m_num_total_poligonos];
        }

        public void AddListaDistancias(int posicao, double distancia)
        {
            this.m_lista_distancias[posicao] = distancia;
            this.ListaDistancias[posicao] = distancia;
        }

        protected string m_ID = "";
        /// <summary>
        /// Variável ID
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set { this.m_ID = value; }
        }

        protected int m_IDSharpMap = -1;
        /// <summary>
        /// Variável ID
        /// </summary>
        public int IDSharpMap
        {
            get { return m_IDSharpMap; }
            set { this.m_IDSharpMap = value; }
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
