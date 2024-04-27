using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.Drawing;
using System.Collections;


namespace IpeaGEO
{
    class clsStatisticScan
    {

        #region Funções operacionais
        /// <summary>
        /// Calcula a distancia pelo método de Haversine
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double calDistancia(SharpMap.Geometries.Point from, SharpMap.Geometries.Point to)
        {
            double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1X = from.X / 180 * Math.PI;
            double p1Y = from.Y / 180 * Math.PI;
            double p2X = to.X / 180 * Math.PI;
            double p2Y = to.Y / 180 * Math.PI;

            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }
        /// <summary>
        /// Calcula a distancia pelo método de Haversine
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double calDistancia(SharpMap.Geometries.Point from, PointF to)
        {
            double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1X = from.X / 180 * Math.PI;
            double p1Y = from.Y / 180 * Math.PI;
            double p2X = to.X / 180 * Math.PI;
            double p2Y = to.Y / 180 * Math.PI;

            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }

        /// <summary>
        /// Calcula a distancia Euclidiana
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double calDistanciaEuclidiana(SharpMap.Geometries.Point from, SharpMap.Geometries.Point to)
        {
            double p1X = from.X;
            double p1Y = from.Y;
            double p2X = to.X ;
            double p2Y = to.Y ;

            return Math.Sqrt((p1Y - p2Y) * (p1Y - p2Y) + (p1X - p2X) * (p1X - p2X));
        }
        /// <summary>
        /// Calcula a distancia Euclidiana
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double calDistanciaEuclidiana(SharpMap.Geometries.Point from, PointF to)
        {
            double p1X = from.X ;
            double p1Y = from.Y ;
            double p2X = to.X ;
            double p2Y = to.Y;

            return Math.Sqrt((p1Y - p2Y) * (p1Y - p2Y) + (p1X - p2X) * (p1X - p2X));
        }

        private bool existeIntersecao(int[,] intTodos, int[] vetor, int coluna)
        {
            if (coluna > 0)
            {
                for (int i = 0; i < coluna; i++)
                {
                    for (int j = 0; j < intTodos.GetLength(0); j++)
                    {
                        if (intTodos[j, i] == vetor[j])
                        {
                            return (true);
                        }
                        else if (intTodos[j, i] < 0 || vetor[j] < 0) break;
                    }
                }
            }
            return (false);
        }
        private bool existeIntersecao(int[,] intTodos, int vetor, int coluna)
        {
            if (coluna > 0)
            {
                for (int i = 0; i < coluna; i++)
                {
                    for (int j = 0; j < intTodos.GetLength(0); j++)
                    {
                        if (intTodos[j, i] == vetor)
                        {
                            return (true);
                        }
                        else if (intTodos[j, i] < 0 || vetor < 0) break;
                    }
                }
            }
            return (false);
        }

        private void comparaListas(ref int[,] intTodos, double[] maxLLR, double LLR, int[] vetor)
        {
            for (int i = 0; i < maxLLR.Length; i++)
            {
                if (maxLLR[i] < LLR && existeIntersecao(intTodos, vetor, i) == false)
                {
                    maxLLR[i] = LLR;
                    limpaLista(ref intTodos, i);
                    for (int j = 0; j < vetor.Length; j++)
                    {
                        intTodos[j, i] = vetor[j];
                        if (vetor[j] < 0) break;
                    }
                }
            }
        }

        private double dblValores(DataTable dataTable, string Id, string strId, string strVariavel)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (dataTable.Rows[i][strId].ToString() == Id)
                {
                    return (Convert.ToDouble(dataTable.Rows[i][strVariavel]));
                }
            }

            return (double.NaN);
        }

        private double distanciaEuclidiana(double x0, double x1, double y0, double y1)
        {
            return (Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2)));
        }
        private double min(double v1, double v2)
        {
            if (v1 < v2) return (v1);
            else return (v2);

        }
        /// <summary>
        /// Haversine Formula - Para o calculo de distancia entre pontos.
        /// </summary>
        /// <param name="lon0">Longitude0 in spherical coordinates </param>
        /// <param name="lon1">Longitude1 in spherical coordinates</param>
        /// <param name="lat0">Latitude0 in spherical coordinates</param>
        /// <param name="lat1">Latitude1 in spherical coordinates</param>
        /// <returns></returns>
        private double distanciaGreatCircle(double lon0, double lon1, double lat0, double lat1)
        {
            //Haversine Formula (from R.W. Sinnott, "Virtues of the Haversine", Sky and Telescope, vol. 68, no. 2, 1984, p. 159):
            //http://www.movable-type.co.uk/scripts/gis-faq-5.1.html

            double R = 6367;//Kilometros
            double dlon = lon1 - lon0;
            double dlat = lat1 - lat0;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat0) * Math.Cos(lat1) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(min(1, Math.Sqrt(a)));
            double d = R * c;
            return (d);

        }

        private void limpaLista(ref int[] strLista)
        {
            for (int i = 0; i < strLista.Length; i++) strLista[i] = -1;
        }
        private void limpaLista(ref int[,] strLista, int coluna)
        {
            for (int i = 0; i < strLista.GetLength(0); i++) strLista[i, coluna] = -1;
        }

        //Lambda(z) para a Binomial
        private double lambdaZbinomial(double N, double C, double nz, double cz)
        {
            double numerador = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0;
            double denominador = 0, d1 = 0, d2 = 0;

            if ((cz / nz) > ((C - cz) / (N - nz)))
            {
                //numerador
                p1 = cz * (Math.Log(cz) - Math.Log(nz));
                p2 = (nz - cz) * (Math.Log(nz - cz) - Math.Log(nz));
                p3 = (C - cz) * (Math.Log(C - cz) - Math.Log(N - nz));
                p4 = (N - nz - C + cz) * (Math.Log(N - nz - C + cz) - Math.Log(N - nz));
                numerador = p1 + p2 + p3 + p4;
                //denominador
                d1 = C * Math.Log(C);
                d2 = (N - C) * Math.Log(N - C) - N * Math.Log(N);
                denominador = d1 + d2;
                //saida
                return (numerador / denominador);
            }
            else
            {
                return 0.0;
            }
        }
        private double lambdaZpoisson(double N, double C, double nz, double cz)
        {
            if (cz > (nz * (C / N)))
            {
                return (cz * (Math.Log(cz) - Math.Log(nz * (C / N))) + Math.Log(C - cz) - Math.Log(C - (nz * (C / N))));
            }
            else
            {
                return 0.0;
            }
        }

        private int indicemenorValor(double[] vetor)
        {
            double min = vetor[0];
            int iIndex = 0;
            for (int i = 0; i < vetor.Length; i++)
            {
                if (vetor[i] <= min)
                {
                    min = vetor[i];
                    iIndex = i;
                }
            }
            return (iIndex);
        }

        private double Minimo(double[] vetor)
        {
            double min = vetor[0];
            int iIndex = 0;
            for (int i = 0; i < vetor.Length; i++)
            {
                if (vetor[i] <= min)
                {
                    min = vetor[i];
                    iIndex = i;
                }
            }
            return (min);
        }

        #endregion


        #region Inferência Estatística 

        /// <summary>
        /// Estima o vetor de parametros theta=(mu,beta) a notação esta definida em http://en.wikipedia.org/wiki/Gumbel_distribution
        /// </summary>
        /// <param name="vetorDados"></param>
        /// <returns></returns>
        private double[] parametrosGumbel(double[] vetorDados)
        {
            double soma = 0;

            //Parametro B
            double soma2 = 0;

            for (int i = 0; i < vetorDados.Length; i++) 
            {
                soma += vetorDados[i];
                soma2 += vetorDados[i]*vetorDados[i];
            }
            double desvio = Math.Sqrt((soma2/vetorDados.Length)-Math.Pow(soma/vetorDados.Length,2));

            double bhat = (desvio * 6) / Math.PI;

            double mu = (soma / vetorDados.Length) + ((6 * desvio * 0.5772156649015328606) / Math.PI);

            double[] parametros = new double[2];
            parametros[0] = mu;
            parametros[1] = bhat;

            return (parametros);
        }

        private double pValor(double p, double[] parametros)
        {
            double mu = parametros[0];
            double beta = parametros[1];
            double p1 = (mu - p) / beta;
            double p2 = -Math.Exp(p1);
            double p3=Math.Exp(p2);
            return(1 - p3);
        }

        #endregion


        #region Estatística Scan




        private void guardaMelhorP(double dblRazaoVerossimilhanca, ArrayList iPoligonos, ref int[,] strRegioes, ref double[] maxLLR, int numCluster)
        {
            double minimo = Minimo(maxLLR);

            if (dblRazaoVerossimilhanca >= minimo || dblRazaoVerossimilhanca==0)
            {
                //Encontra o menor
                int iIndice = indicemenorValor(maxLLR);

                //Guarda o valor no vetor LLR
                maxLLR[iIndice] = dblRazaoVerossimilhanca;

                //Guarda os valores do novo Cluster
                for (int i = 0; i < strRegioes.GetLength(0); i++)
                {
                    if (i < iPoligonos.Count)
                    {
                        strRegioes[i, iIndice] = (int)iPoligonos[i];
                    }
                    else
                    {
                        strRegioes[i, iIndice] = -1;
                    }
                }
            }
        }

       

        private bool pontoNoMapa(SharpMap.Layers.VectorLayer layMapa, double pPontoX, double pPontoY)
        {
            SharpMap.Geometries.Point p = new SharpMap.Geometries.Point(pPontoX, pPontoY);

            for (int i = 0; i < layMapa.DataSource.GetFeatureCount(); i++)
            {
                //Captura o FeatureDataRow
                FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)i);

                //Captura a geometria
                SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                //Geometria do poligono
                Polygon poligono0 = shape0 as SharpMap.Geometries.Polygon;

                //Classe mapas
                clsMapa clsMapa = new clsMapa();

                //Verifica se o ponto está no shape
                if(poligono0!=null)if(clsMapa.PointInPolygon(p, poligono0) == true)
                {
                    return (true);
                }
            }
            return (false);
        }



        public int[] EstatisticaScanBinomial(DataTable dataTable, string strEndereco, int numCluster, string strCasos, string strTotal, ref ProgressBar progressBar, ref Label lblStatus, ref double[] maxLLR, double numPpop, double dblRaioMinimo, double dblRaioMaximo, int numGrid, int numSimulacoes, string strIdMapa, string strId, ref double[] vetPvalor, ref double[] vetMonteCarlo)
        {
            //Coloca o status do Label
            lblStatus.Text = "Criando os pontos de Grid 1 de 4.";
            lblStatus.Refresh();
            progressBar.Minimum = 0;
            progressBar.Maximum = numGrid;

            //Inicializa o resultado
            int[,] strRegioes = new int[dataTable.Rows.Count, numCluster];

            //Definindo um layer
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("TEMP");

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Coloca o número de GRIDS como um valor par
            if (numGrid % 2 != 0) numGrid++;
            
            //Eixos veritcais
            int iVertical = (int)Math.Sqrt(numGrid);
            int iHorizontal = iVertical;
             
            //Limites do mapa BoundindBox
            BoundingBox bBox = layMapa.Envelope;

            //Passo 1.1: Gerando os pontos GRID
            List<PointF> pontosGrid = new List<PointF>();

            double pX = bBox.Min.X, pY = bBox.Min.Y;
            double fatorX=(bBox.Max.X-bBox.Min.X)/iHorizontal;
            double fatorY=(bBox.Max.Y-bBox.Min.Y)/iVertical;
            for (int iV = 0; iV < iVertical; iV++)
            {
                if (iV == 0 && pY < bBox.Max.Y)
                {
                    pY = bBox.Min.Y;
                    for (int iH = 0; iH < iHorizontal; iH++)
                    {
                        if (iH == 0)
                        {
                            pX = bBox.Min.X;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                        else if (iH > 0 && pX < bBox.Max.X)
                        {
                            pX += fatorX;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                    }
                }
                else if(iV > 0 && pY < bBox.Max.Y)
                {
                    pY += fatorY;
                    for (int iH = 0; iH < iHorizontal; iH++)
                    {
                        if (iH == 0)
                        {
                            pX = bBox.Min.X;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                        else if (iH > 0 && pX < bBox.Max.X)
                        {
                            pX += fatorX;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                    }
                }
            }

            //Coloca o status do Label
            lblStatus.Text = "Criando os pontos Grid 2 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = pontosGrid.Count;

            //Passo 1.2: Para cada ponto do GRID calcule a distancia aos outros pontos e memorize
            ArrayList listaDistancias = new ArrayList();
            int[] listaPoligonos = new int[dataTable.Rows.Count];
            ArrayList listaPoligonosID = new ArrayList();


            double N = 0, C = 0;

            for (int i = 0; i < pontosGrid.Count; i++)
            {
                double[] arDistancias = new double[dataTable.Rows.Count];

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    //Número do poligono
                    int iPoligono0 = Convert.ToInt32(dataTable.Rows[j]["Mapa" + strIdMapa]);

                    //Captura o FeatureDataRow
                    FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)iPoligono0);

                    //Captura a geometria
                    SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                    //Geometria do poligono
                    Polygon poligono0 = shape0 as SharpMap.Geometries.Polygon;
               
                    if (i == 0)
                    {
                        listaPoligonos[j] = j;
                        if (dataTable.Rows[j][strCasos].ToString() != "")
                        {
                            C += Convert.ToDouble(dataTable.Rows[j][strCasos]);
                        }
                        if (dataTable.Rows[j][strTotal].ToString() != "")
                        {
                            N += Convert.ToDouble(dataTable.Rows[j][strTotal]);
                        }
                        listaPoligonosID.Add(dataTable.Rows[j][strId].ToString());
                    }

                    if (poligono0 != null)
                    {
                        arDistancias[j] = calDistancia(poligono0.Centroid, pontosGrid[i]);
                        //arDistancias[j] = calDistanciaEuclidiana(poligono0.Centroid, pontosGrid[i]);
                    }
                    else
                    {
                        BoundingBox bBoxCentro = shape0.GetBoundingBox();
                        double XCentro = bBoxCentro.Min.X + ((bBoxCentro.Max.X - bBoxCentro.Min.X) / 2);
                        double YCentro = bBoxCentro.Min.Y + ((bBoxCentro.Max.Y - bBoxCentro.Min.Y) / 2);
                        SharpMap.Geometries.Point pCentro = new SharpMap.Geometries.Point(XCentro,YCentro);
                        arDistancias[j] = calDistancia(pCentro, pontosGrid[i]);
                        //arDistancias[j] = calDistanciaEuclidiana(pCentro, pontosGrid[i]);
                    }
                }
                listaDistancias.Add(arDistancias);

                //Incrementa a progress bar
                progressBar.Increment(1);
            }

            //Coloca o status do Label
            lblStatus.Text = "Buscando os conglomerados 3 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = numGrid;

            //Para cada Lista de Distancias fazer o Scan
            for(int i=0;i<listaDistancias.Count;i++)
            {
                //Poligonos que compõe o Cluster
                ArrayList arCluster = new ArrayList();

                double dblRazaoVerossimilhanca = 0;
                double cz = 0, nz = 0;

                //Guarda as distâncias
                double[] distancias = (double[])listaDistancias[i];

                //Guarda a lista de poligonos original
                int[] listaPoligonosTemp = new int[dataTable.Rows.Count];
                for (int y = 0; y < dataTable.Rows.Count; y++) listaPoligonosTemp[y] = y;

                //Ordena as distâncias
                Array.Sort(distancias, listaPoligonosTemp);

                //Variáveis de controle
                double dblProp=0;
                int contador=0;

                //Começa a busca
                do
                {
                    if (distancias[contador] >= dblRaioMinimo)
                    {
                        //Guarda os poligonos coprendidos (Número da linha no DataTable)
                        arCluster.Add(listaPoligonosTemp[contador]);

                        //Atualiza os casos e a população
                        if (dataTable.Rows[listaPoligonosTemp[contador]][strCasos].ToString() != "") cz += Convert.ToDouble(dataTable.Rows[listaPoligonosTemp[contador]][strCasos]);
                        if (dataTable.Rows[listaPoligonosTemp[contador]][strTotal].ToString() != "") nz += Convert.ToDouble(dataTable.Rows[listaPoligonosTemp[contador]][strTotal]);

                        //Proporção da população
                        dblProp = 100 * (nz / N);

                        //Calcula a log likelihood ratio para a distribuição Binomial
                        dblRazaoVerossimilhanca = lambdaZbinomial(N, C, nz, cz);

                        //Guarda o melhor valor no vetor
                        guardaMelhorP(dblRazaoVerossimilhanca, arCluster, ref strRegioes, ref maxLLR, numCluster);
                    }

                    //Incrementa o contador
                    contador++;

                } while (dblProp < numPpop && distancias[contador] <= dblRaioMaximo);

                //Incrementa a progress bar
                progressBar.Increment(1);                
            }


            //Guarda o vetor de clusters
            int[] iVetor = new int[strRegioes.GetLength(0)];
            for (int c = 0; c < numCluster; c++)
            {
                for (int i = 0; i < strRegioes.GetLength(0); i++)
                {
                    if (strRegioes[i, c] > -1)
                    {
                        iVetor[Convert.ToInt32(dataTable.Rows[strRegioes[i, c]]["Mapa" + strIdMapa])] = numCluster - c;
                    }
                    else break;
                }
            }

            //Coloca o status do Label
            lblStatus.Text = "Fazendo as simulações de Monte Carlo 4 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = numSimulacoes;

            //Vetor de verossimilhanças
            double[] dblVerossimilhancas = new double[numSimulacoes];

            //Simulações de Monte Carlo
            Random rnd = new Random();

            for (int s = 0; s < numSimulacoes; s++)
            {
                //Gera os valores aleatórios
                int[] listaPoligonosRandom = new int[dataTable.Rows.Count];
                double[] rndVetor = new double[dataTable.Rows.Count];

                double[] MaxLLRrnd = new double[1];
                MaxLLRrnd[0]=- 99999;

                //Para cada Lista de Distancias fazer o Scan
                for (int i = 0; i < listaDistancias.Count; i++)
                {
                    //Poligonos que compõe o Cluster
                    ArrayList arCluster = new ArrayList();

                    double dblRazaoVerossimilhanca = 0;
                    double cz = 0, nz = 0;

                    //Guarda as distâncias
                    double[] distancias = (double[])listaDistancias[i];

                    //Guarda a lista de poligonos original
                    for (int y = 0; y < dataTable.Rows.Count; y++)
                    {
                        listaPoligonosRandom[y] = y;
                        rndVetor[y] = rnd.NextDouble();
                    }

                    //Ordena os valores aleatórios
                    Array.Sort(rndVetor, listaPoligonosRandom);

                    //Ordena as distâncias
                    Array.Sort(distancias, listaPoligonosRandom);

                    //Variáveis de controle
                    double dblProp = 0;
                    int contador = 0;

                    //Começa a busca
                    do
                    {
                        if (distancias[contador] >= dblRaioMinimo)
                        {
                            //Guarda os poligonos coprendidos (Número da linha no DataTable)
                            arCluster.Add(listaPoligonosRandom[contador]);

                            //Atualiza os casos e a população
                            if (dataTable.Rows[listaPoligonosRandom[contador]][strCasos].ToString() != "") cz += Convert.ToDouble(dataTable.Rows[listaPoligonosRandom[contador]][strCasos]);
                            if (dataTable.Rows[listaPoligonosRandom[contador]][strTotal].ToString() != "") nz += Convert.ToDouble(dataTable.Rows[listaPoligonosRandom[contador]][strTotal]);

                            //Proporção da população
                            dblProp = 100 * (nz / N);

                            //Calcula a log likelihood ratio para a distribuição Binomial
                            dblRazaoVerossimilhanca = lambdaZbinomial(N, C, nz, cz);

                            //Guarda o melhor valor no vetor
                            guardaMelhorP(dblRazaoVerossimilhanca, arCluster, ref strRegioes, ref MaxLLRrnd, 1);
                        }

                        //Incrementa o contador
                        contador++;

                    } while (dblProp < numPpop && distancias[contador] <= dblRaioMaximo);
                }

                //Incrementa a ProgressBar
                progressBar.Increment(1);

                //Guarda a verossimilhança
                dblVerossimilhancas[s] = MaxLLRrnd[0];
            }

            vetMonteCarlo = dblVerossimilhancas;

            //Calcula a significância
            double[] parametros = new double[2];
            parametros = parametrosGumbel(dblVerossimilhancas);

            for (int c = 0; c < numCluster; c++)
            {
                vetPvalor[c] = pValor(maxLLR[c], parametros);
            }

            //Fecha o shapeFile
            //shapefile.Close();

            return (iVetor);
        }

        public int[] EstatisticaScanPoisson(DataTable dataTable, string strEndereco, int numCluster, string strCasos, string strTotal, ref ProgressBar progressBar, ref Label lblStatus, ref double[] maxLLR, double numPpop, double dblRaioMinimo, double dblRaioMaximo, int numGrid, int numSimulacoes, string strIdMapa, string strId, ref double[] vetPvalor, ref double[] vetMonteCarlo)
        {
            //Coloca o status do Label
            lblStatus.Text = "Criando os pontos de Grid 1 de 4.";
            lblStatus.Refresh();
            progressBar.Minimum = 0;
            progressBar.Maximum = numGrid;

            //Inicializa o resultado
            int[,] strRegioes = new int[dataTable.Rows.Count, numCluster];

            //Definindo um layer
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("TEMP");

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Coloca o número de GRIDS como um valor par
            if (numGrid % 2 != 0) numGrid++;

            //Eixos veritcais
            int iVertical = (int)Math.Sqrt(numGrid);
            int iHorizontal = iVertical;

            //Limites do mapa BoundindBox
            BoundingBox bBox = layMapa.Envelope;

            //Passo 1.1: Gerando os pontos GRID
            List<PointF> pontosGrid = new List<PointF>();

            double pX = bBox.Min.X, pY = bBox.Min.Y;
            double fatorX = (bBox.Max.X - bBox.Min.X) / iHorizontal;
            double fatorY = (bBox.Max.Y - bBox.Min.Y) / iVertical;
            for (int iV = 0; iV < iVertical; iV++)
            {
                if (iV == 0 && pY < bBox.Max.Y)
                {
                    pY = bBox.Min.Y;
                    for (int iH = 0; iH < iHorizontal; iH++)
                    {
                        if (iH == 0)
                        {
                            pX = bBox.Min.X;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                        else if (iH > 0 && pX < bBox.Max.X)
                        {
                            pX += fatorX;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                    }
                }
                else if (iV > 0 && pY < bBox.Max.Y)
                {
                    pY += fatorY;
                    for (int iH = 0; iH < iHorizontal; iH++)
                    {
                        if (iH == 0)
                        {
                            pX = bBox.Min.X;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                        else if (iH > 0 && pX < bBox.Max.X)
                        {
                            pX += fatorX;
                            pontosGrid.Add(new PointF((float)pX, (float)pY));
                            //Incrementa a progress bar
                            progressBar.Increment(1);
                        }
                    }
                }
            }

            //Coloca o status do Label
            lblStatus.Text = "Criando os pontos Grid 2 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = pontosGrid.Count;

            //Passo 1.2: Para cada ponto do GRID calcule a distancia aos outros pontos e memorize
            ArrayList listaDistancias = new ArrayList();
            int[] listaPoligonos = new int[dataTable.Rows.Count];
            ArrayList listaPoligonosID = new ArrayList();


            double N = 0, C = 0;

            for (int i = 0; i < pontosGrid.Count; i++)
            {
                double[] arDistancias = new double[dataTable.Rows.Count];

                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    //Número do poligono
                    int iPoligono0 = Convert.ToInt32(dataTable.Rows[j]["Mapa" + strIdMapa]);

                    //Captura o FeatureDataRow
                    FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)iPoligono0);

                    //Captura a geometria
                    SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                    //Geometria do poligono
                    Polygon poligono0 = shape0 as SharpMap.Geometries.Polygon;

                    if (i == 0)
                    {
                        listaPoligonos[j] = j;
                        if (dataTable.Rows[j][strCasos].ToString() != "") C += Convert.ToDouble(dataTable.Rows[j][strCasos]);
                        if (dataTable.Rows[j][strTotal].ToString() != "") N += Convert.ToDouble(dataTable.Rows[j][strTotal]);
                        listaPoligonosID.Add(dataTable.Rows[j][strId].ToString());
                    }

                    if (poligono0 != null)
                    {
                        arDistancias[j] = calDistancia(poligono0.Centroid, pontosGrid[i]);
                        //arDistancias[j] = calDistanciaEuclidiana(poligono0.Centroid, pontosGrid[i]);
                    }
                    else
                    {
                        BoundingBox bBoxCentro = shape0.GetBoundingBox();
                        double XCentro = bBoxCentro.Min.X + ((bBoxCentro.Max.X - bBoxCentro.Min.X) / 2);
                        double YCentro = bBoxCentro.Min.Y + ((bBoxCentro.Max.Y - bBoxCentro.Min.Y) / 2);
                        SharpMap.Geometries.Point pCentro = new SharpMap.Geometries.Point(XCentro, YCentro);
                        arDistancias[j] = calDistancia(pCentro, pontosGrid[i]);
                        //arDistancias[j] = calDistanciaEuclidiana(pCentro, pontosGrid[i]);
                    }
                }
                listaDistancias.Add(arDistancias);

                //Incrementa a progress bar
                progressBar.Increment(1);
            }

            //Coloca o status do Label
            lblStatus.Text = "Buscando os conglomerados 3 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = numGrid;

            //Para cada Lista de Distancias fazer o Scan
            for (int i = 0; i < listaDistancias.Count; i++)
            {
                //Poligonos que compõe o Cluster
                ArrayList arCluster = new ArrayList();

                double dblRazaoVerossimilhanca = 0;
                double cz = 0, nz = 0;

                //Guarda as distâncias
                double[] distancias = (double[])listaDistancias[i];

                //Guarda a lista de poligonos original
                int[] listaPoligonosTemp = new int[dataTable.Rows.Count];
                for (int y = 0; y < dataTable.Rows.Count; y++) listaPoligonosTemp[y] = y;

                //Ordena as distâncias
                Array.Sort(distancias, listaPoligonosTemp);

                //Variáveis de controle
                double dblProp = 0;
                int contador = 0;

                //Começa a busca
                do
                {
                    if (distancias[contador] >= dblRaioMinimo)
                    {
                        //Guarda os poligonos coprendidos (Número da linha no DataTable)
                        arCluster.Add(listaPoligonosTemp[contador]);

                        //Atualiza os casos e a população
                        if (dataTable.Rows[listaPoligonosTemp[contador]][strCasos].ToString() != "") cz += Convert.ToDouble(dataTable.Rows[listaPoligonosTemp[contador]][strCasos]);
                        if (dataTable.Rows[listaPoligonosTemp[contador]][strTotal].ToString() != "") nz += Convert.ToDouble(dataTable.Rows[listaPoligonosTemp[contador]][strTotal]);

                        //Proporção da população
                        dblProp = 100 * (nz / N);

                        //Calcula a log likelihood ratio para a distribuição Binomial
                        dblRazaoVerossimilhanca = lambdaZpoisson(N, C, nz, cz);

                        //Guarda o melhor valor no vetor
                        guardaMelhorP(dblRazaoVerossimilhanca, arCluster, ref strRegioes, ref maxLLR, numCluster);
                    }

                    //Incrementa o contador
                    contador++;

                } while (dblProp < numPpop && distancias[contador] <= dblRaioMaximo);

                //Incrementa a progress bar
                progressBar.Increment(1);
            }


            //Guarda o vetor de clusters
            int[] iVetor = new int[strRegioes.GetLength(0)];
            for (int c = 0; c < numCluster; c++)
            {
                for (int i = 0; i < strRegioes.GetLength(0); i++)
                {
                    if (strRegioes[i, c] > -1)
                    {
                        iVetor[Convert.ToInt32(dataTable.Rows[strRegioes[i, c]]["Mapa" + strIdMapa])] = numCluster - c;
                    }
                    else break;
                }
            }

            //Coloca o status do Label
            lblStatus.Text = "Fazendo as simulações de Monte Carlo 4 de 4.";
            lblStatus.Refresh();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = numSimulacoes;

            //Vetor de verossimilhanças
            double[] dblVerossimilhancas = new double[numSimulacoes];

            //Simulações de Monte Carlo
            Random rnd = new Random();

            for (int s = 0; s < numSimulacoes; s++)
            {
                //Gera os valores aleatórios
                int[] listaPoligonosRandom = new int[dataTable.Rows.Count];
                double[] rndVetor = new double[dataTable.Rows.Count];

                double[] MaxLLRrnd = new double[1];
                MaxLLRrnd[0] = -99999;

                //Para cada Lista de Distancias fazer o Scan
                for (int i = 0; i < listaDistancias.Count; i++)
                {
                    //Poligonos que compõe o Cluster
                    ArrayList arCluster = new ArrayList();

                    double dblRazaoVerossimilhanca = 0;
                    double cz = 0, nz = 0;

                    //Guarda as distâncias
                    double[] distancias = (double[])listaDistancias[i];

                    //Guarda a lista de poligonos original
                    for (int y = 0; y < dataTable.Rows.Count; y++)
                    {
                        listaPoligonosRandom[y] = y;
                        rndVetor[y] = rnd.NextDouble();
                    }

                    //Ordena os valores aleatórios
                    Array.Sort(rndVetor, listaPoligonosRandom);

                    //Ordena as distâncias
                    Array.Sort(distancias, listaPoligonosRandom);

                    //Variáveis de controle
                    double dblProp = 0;
                    int contador = 0;

                    //Começa a busca
                    do
                    {
                        if (distancias[contador] >= dblRaioMinimo)
                        {
                            //Guarda os poligonos coprendidos (Número da linha no DataTable)
                            arCluster.Add(listaPoligonosRandom[contador]);

                            //Atualiza os casos e a população
                            if (dataTable.Rows[listaPoligonosRandom[contador]][strCasos].ToString() != "") cz += Convert.ToDouble(dataTable.Rows[listaPoligonosRandom[contador]][strCasos]);
                            if (dataTable.Rows[listaPoligonosRandom[contador]][strTotal].ToString() != "") nz += Convert.ToDouble(dataTable.Rows[listaPoligonosRandom[contador]][strTotal]);

                            //Proporção da população
                            dblProp = 100 * (nz / N);

                            //Calcula a log likelihood ratio para a distribuição Binomial
                            dblRazaoVerossimilhanca = lambdaZpoisson(N, C, nz, cz);

                            //Guarda o melhor valor no vetor
                            guardaMelhorP(dblRazaoVerossimilhanca, arCluster, ref strRegioes, ref MaxLLRrnd, 1);
                        }

                        //Incrementa o contador
                        contador++;

                    } while (dblProp < numPpop && distancias[contador] <= dblRaioMaximo);
                }

                //Incrementa a ProgressBar
                progressBar.Increment(1);

                //Guarda a verossimilhança
                dblVerossimilhancas[s] = MaxLLRrnd[0];
            }

            vetMonteCarlo = dblVerossimilhancas;

            //Calcula a significância
            double[] parametros = new double[2];
            parametros = parametrosGumbel(dblVerossimilhancas);

            for (int c = 0; c < numCluster; c++)
            {
                vetPvalor[c] = pValor(maxLLR[c], parametros);
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (iVetor);
        }
        #endregion

    }
}
