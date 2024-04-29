using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace IpeaGeo.Classes
{
    class clsArmazenamentoDados 
    {
        //Variaveis chave para o salavemento
        private static Color[] cores;
        private static String[] legendas; 
        private static string variaveis;
        private static string metodo_u;
        private static int cor1;
        private static double zoom_mapa;
        public double zoom_m = zoom_mapa;
        private static double[] centro;
        public double[] centro_map = centro;
        public Color[] cores_tem = cores;
        public string variaveis_tem = variaveis;
        public string metodo_tem = metodo_u;
        public int cor_tem = cor1;
        public String[] legendas_tem = legendas;
        private static string enderecoMapa;

        private static string Shape;
        private static string Base;
        private static String[] Shapefile;
        private static string datafile;

        private static string tabela;
        private static string lblDados;
        private static string lblShape;
        public string tabela_concat = tabela;
        public string lblDados_concat = lblDados;
        public string lblShape_concat = lblShape;

        private string path = Path.GetDirectoryName(Application.ExecutablePath);

        private static  bool Leitura;
        public bool Leitura_efetuada = Leitura;
        private static bool mapa_tem;

        ExportData export = new ExportData();

        public void CapturaDados( Color[] cor, String[] legenda, string variavel, string metodo, int cor_selecioanda/*, bool clear*/) 
        {
            cores = cor;
            legendas = legenda;
            variaveis = variavel;
            metodo_u = metodo;
            cor1 = cor_selecioanda;                     
        }

        public void CapturaDados(string nomeShape, string nomeBase, string nomedatafile, double zoom, double[] posicao_map, string endshape)
        {
            Shape = nomeShape.Substring(nomeShape.IndexOf("[") + 1, nomeShape.IndexOf("]") - nomeShape.IndexOf("[") - 1);
            Shapefile = new String[4];
            Shapefile[0] = Shape + ".dbf";
            Shapefile[1] = Shape + ".prj";
            Shapefile[2] = Shape + ".shp";
            Shapefile[3] = Shape + ".shx";
            datafile = nomedatafile;
            zoom_mapa = zoom;
            centro = posicao_map;
            enderecoMapa = endshape;
            //salvarStatus();
        }
        public void CapturaDados(string label_dados, string label_shape, bool quantitativo)
        {
            lblDados = label_dados;
            lblShape = label_shape;
            mapa_tem = quantitativo;
            //salvarStatus();
        }

        #region Salvar xml
            
        public void salvarStatus(string nome, string salvo)
        {
            // cria um XML com o status da base e malha abertos.
            // observar a pasta em que o arquivo deverá ser salvo.
            XmlTextWriter xml = new XmlTextWriter(nome + ".xml", null);

            xml.Flush();

            //inicia o documento xml
            xml.WriteStartDocument();

            //cria identação
            xml.Formatting = Formatting.Indented;

            // começando o xml
            xml.WriteStartElement("Arquivo_XML");

            //primeiro elemento
            xml.WriteStartElement("Dados");
            xml.WriteElementString("Base_de_Dados", nome + ".txt");
            xml.WriteEndElement();

            //segundo elemento
            xml.WriteStartElement("Shape");
            xml.WriteElementString("Arquivo_Shape", Shape);
            xml.WriteElementString("Endereco_Shape", enderecoMapa);
            xml.WriteEndElement();

            //Complemento
            xml.WriteStartElement("ShapeFile");
            for (int i = 0; i < Shapefile.Length; i++)
            {
                xml.WriteElementString("Lista_de_Shape_Files", Shapefile[i]);
            }
            xml.WriteEndElement();

            //terceiro elemento
            xml.WriteStartElement("Tabela_de_Dados");
            xml.WriteElementString("Titulo_Tabela", salvo + "$");
            xml.WriteEndElement();
            
            //quarto elemento
            if (lblDados != null)
            {
                xml.WriteStartElement("Label_Dados");
                xml.WriteElementString("Variavel_Dados", lblDados);
                xml.WriteEndElement();
            }

            //quinto elemento 
            if (lblShape != null)
            {
                xml.WriteStartElement("Label_Shape");
                xml.WriteElementString("Variavel_Shape", lblShape);
                xml.WriteEndElement();
            }

            // sexto elemento
            if (cores != null)
            {
                xml.WriteStartElement("Cores");
                for (int i = 0; i < cores.Length; i++)
                {
                    xml.WriteElementString("Paleta_de_Cores", cores[i].ToArgb().ToString()); 
                }
                xml.WriteEndElement();
            }

            // setimo elemento 
            if (legendas != null)
            {
                xml.WriteStartElement("Legendas");
                for (int i = 0; i < legendas.Length; i++)
                {
                    xml.WriteElementString("Lista_de_Legendas", legendas[i]);
                }
                xml.WriteEndElement();
            }
            
            // oitavo elemento 
            if (variaveis != null)
            {
                xml.WriteStartElement("Titulo");
                xml.WriteElementString("Titulo_Legenda", variaveis);
                xml.WriteEndElement();
            }
            
            if (metodo_u != null)
            {
                xml.WriteStartElement("Metodo_Utilizado");
                xml.WriteElementString("Metodo", metodo_u);
                xml.WriteEndElement();
            }

            if (metodo_u != null)
            {
                xml.WriteStartElement("Cor");
                xml.WriteElementString("Cor_Selecionada", cor1.ToString());
                xml.WriteEndElement();
            }
            
            if (zoom_mapa != null)
            {
                xml.WriteStartElement("Zoom");
                xml.WriteElementString("Zoom_Mapa", zoom_mapa.ToString());
                xml.WriteEndElement();
            }

            if (centro != null)
            {
                xml.WriteStartElement("Centro");
                for (int i = 0; i < 2; i++)
                {
                    xml.WriteElementString("Centro_Mapa", centro[i].ToString());
                }
                xml.WriteEndElement();
            }

            xml.WriteStartElement("Mapa");
            if (mapa_tem == true)
            {
                xml.WriteElementString("Tipo_Mapa", "Quantitativo");
            }
            else
            {
                xml.WriteElementString("Tipo_Mapa", "Categórico");
            }
            xml.WriteEndElement();

            //encerrando o xml
            xml.WriteEndElement();

            //encerra o xml
            xml.Close();
        }

        #endregion

        #region Lendo XML

        public void LerXML(string nome)
        {
            // lê o arquivo XML e carrega a base e a malha (e faz o merge)

            XmlDocument xml = new XmlDocument();
            XmlNodeList xmlfile;

            try
            {
                xml.Load(nome);

                // retorna a base salva
                xmlfile = xml.GetElementsByTagName("Base_de_Dados");
                Base = xmlfile[0].InnerXml;

                // retorna o shape salvo
                xmlfile = xml.GetElementsByTagName("Arquivo_Shape");
                Shape = xmlfile[0].InnerXml;

                xmlfile = xml.GetElementsByTagName("Endereco_Shape");
                enderecoMapa = xmlfile[0].InnerXml;

                // retorna o shapefile
                xmlfile = xml.GetElementsByTagName("Lista_de_Shape_Files");
                Shapefile = new String[xmlfile.Count];
                for (int i = 0; i < xmlfile.Count; i++)
                {
                    Shapefile[i] = xmlfile[i].InnerXml;
                }

                xmlfile = xml.GetElementsByTagName("Titulo_Tabela");
                if (xmlfile.Count != 0)
                {
                    tabela = xmlfile[0].InnerXml;
                }

                xmlfile = xml.GetElementsByTagName("Variavel_Dados");
                if (xmlfile.Count != 0)
                {
                    lblDados = xmlfile[0].InnerXml;
                }

                xmlfile = xml.GetElementsByTagName("Variavel_Shape");
                if (xmlfile.Count != 0)
                {
                    lblShape = xmlfile[0].InnerXml;
                }

                // retorna o vetor de cores salvo
                xmlfile = xml.GetElementsByTagName("Paleta_de_Cores");
                cores = new Color[xmlfile.Count];
                for (int i = 0; i < xmlfile.Count; i++)
                {
                    cores[i] = Color.FromArgb(Convert.ToInt32(xmlfile[i].InnerXml));
                }

                // retorna o vetor de legendas salvo
                xmlfile = xml.GetElementsByTagName("Lista_de_Legendas");
                legendas = new String[xmlfile.Count];
                for (int i = 0; i < xmlfile.Count; i++)
                {
                    legendas[i] = xmlfile[i].InnerXml;
                }

                // retorna a variavel de título salva
                xmlfile = xml.GetElementsByTagName("Titulo_Legenda");
                if (xmlfile.Count != 0)
                {
                    variaveis = xmlfile[0].InnerXml;
                }

                // retorna o método utilizado
                xmlfile = xml.GetElementsByTagName("Metodo");
                if (xmlfile.Count != 0)
                {
                    metodo_u = xmlfile[0].InnerXml;
                }

                xmlfile = xml.GetElementsByTagName("Cor_Selecionada");
                if (xmlfile.Count != 0)
                {
                    cor1 = Convert.ToInt16(xmlfile[0].InnerXml);
                }

                xmlfile = xml.GetElementsByTagName("Zoom_Mapa");
                if (xmlfile.Count != 0)
                {
                    zoom_mapa = Convert.ToDouble(xmlfile[0].InnerXml);
                }

                xmlfile = xml.GetElementsByTagName("Centro_Mapa");
                if (xmlfile.Count != 0)
                {
                    centro = new double[2];
                    for (int i = 0; i < 2; i++)
                    {                        
                        centro[i] = Convert.ToDouble(xmlfile[i].InnerXml);
                    }
                }

                xmlfile = xml.GetElementsByTagName("Tipo_Mapa");
                if (xmlfile[0].InnerXml == "Quantitativo")
                    mapa_tem = true;
                else
                    mapa_tem = false;

                GerenciaDados_XML();
            }
            catch
            {
                Leitura = false;
                Leitura_efetuada = false;
                MessageBox.Show("Erro na leitura do arquivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Aplicando XML aberto 

        private void GerenciaDados_XML()
        {
            #region abre frmMapa
            IPEAGEOMDIParent parent = new IPEAGEOMDIParent();
            
            Leitura = true;
            Leitura_efetuada = true;

            string FileName = FileName = enderecoMapa; //path + @"\malhas\" + Shape + @".shp";
            frmMapa Mapa = new frmMapa();
            Mapa.Text = "Layer Principal [" + Path.GetFileNameWithoutExtension(FileName) + "]";
            Mapa.Name = Path.GetFileNameWithoutExtension(FileName);
            Mapa.MdiParent = IPEAGEOMDIParent.ActiveForm;
            Mapa.strEnderecoMapa = FileName;

            //Coloca as variaveis do mapa
            clsMapa clsMapa = new clsMapa();
            Mapa.strVariaveisMapa = clsMapa.informacaoVariaveis(FileName, 0);
            Mapa.Show();
            Mapa.HabilitaFuncoesBasesDadosIPEA = true;

            #endregion

            #region concatena tabela de dados com o shape

            tabela_concat = tabela;
            lblDados_concat = lblDados;
            lblShape_concat = lblShape;
            legendas_tem = legendas;

            if (tabela != null)
            {
                Mapa.XML(Shape, cores, Base, mapa_tem);
            }

            Leitura = false;
            Leitura_efetuada = false;

            #endregion
        }

        #endregion 

        public string Base_(string FileName)
        {
            FileName = Base;
            return FileName;
        }
    }
}

/*
 * <xml>
 *     <data file="c:\abc\abc\dados.xls" key = "ID_CIDADE"/>
 *     <shape file="c:\abc\abc\mapa.shp" key = "ID_CIDADE"/>
 *     <mapa_tematico titulo="titulo grafico">
 *         <legenda titulo = "titulo" categorias = "3" pos.x = "300" pos.y = "400">
 *             <categoria id = "1" cor = "vermelho" min = "0" max = "10" titulo = "AAA">
 *             <categoria id = "2" cor = "amarelo" min = "10" max = "20" titulo = "BBB">
 *             <categoria id = "3" cor = "azul" min = "20" max = "50" titulo = "CCC">
 *         </legenda>
 *     </mapa_tematico>
 * </xml>
 * 
 * 
 * 
 * 
 */
