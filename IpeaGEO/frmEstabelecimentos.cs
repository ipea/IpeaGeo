using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace IpeaGEO
{
    public partial class frmEstabelecimentos : Form
    {
        public frmEstabelecimentos()
        {
            InitializeComponent();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            DataSet dsTemp = dsDados;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Filter = "Access (*.mdb)|*.mdb|Excel (*.xls)|*.xls|XML (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                string strFile = saveFileDialog1.FileName;

                ExportData exporta = new ExportData();

                if (strExtensao == ".XLS")
                {
                    exporta.exportToExcel(dsTemp, strFile);
                }
                else if (strExtensao == ".XML")
                {
                    dsTemp.WriteXml(strFile);
                }
                else if (strExtensao == ".MDB")
                {
                    //Cria o arquivo MDB
                    exporta.exportaToAccess(dsTemp, strFile, this.Name);
                }
                Cursor.Current = Cursors.Default;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Cria as variáveis
            dsDados.Tables[0].Columns.Add("Nome");
            dsDados.Tables[0].Columns.Add("Latitude");
            dsDados.Tables[0].Columns.Add("Longitude");
            dsDados.Tables[0].Columns.Add("Site");
            dsDados.Tables[0].Columns.Add("Endereço");
            dsDados.Tables[0].Columns.Add("Localidade");
            dsDados.Tables[0].Columns.Add("CEP");
            dsDados.Tables[0].Columns.Add("País");
            dsDados.Tables[0].Columns.Add("Telefone");

            
            //Faz a conexão
            string link = "http://maps.google.com/maps?f=l&hl=pt-BR&geocode=&q=" + txtEstabelecimento.Text + "&near=" + txtLocalidade.Text + "&view=text";
            System.Net.WebClient client = new System.Net.WebClient();
            string page = client.DownloadString(link);

            //Número de páginas
            int iPaginasInicio = page.IndexOf("aproximadamente <b>");
            int iPaginasFim = page.IndexOf("</b> para");
            string str = page.Substring(iPaginasInicio + 19, iPaginasFim - iPaginasInicio - 19);

            //Começa o loop das páginas
            int final = Convert.ToInt32(str) / 10;

            //Inicializa a progress bar
            progressBar1.Minimum = 0;
            progressBar1.Maximum = Convert.ToInt32(str);

            //Estabelcimentos por página
            int iEstab = 10;

            for (int i = 0; i < final + 1; i++)
            {
                if (i == final)
                {
                    iEstab = Convert.ToInt32(str) - final * 10;
                }
                
                //Faz a conexão
                string strPagina = Convert.ToString((i * 10));
                link = "http://maps.google.com/maps?f=l&hl=pt-BR&geocode=&q=" + txtEstabelecimento.Text + "&near=" + txtLocalidade.Text + "&view=text&start=" + strPagina;
                page = client.DownloadString(link);

                for (int j = 0; j < iEstab; j++)
                {
                    //Cria uma nova linha
                    DataRow dLinha = dsDados.Tables[0].NewRow();

                    string strInicioPage="class=\"fn org\" dir=ltr>";
                    page = page.Substring(page.IndexOf(strInicioPage));
                    //Variaveis
                    int iInicio = 0;
                    int iFim = 0;

                    //Estabelecimento
                    string strNome = "";
                    iInicio = page.IndexOf("ltr>");
                    if (iInicio < 0)
                    {
                        strNome = "";
                    }
                    else
                    {
                        string strFiltro = page.Substring(iInicio);
                        iFim = strFiltro.IndexOf("</a>");
                        if (iFim < 0)
                        {
                            strNome = "";
                        }
                        else
                        {
                            strNome = strFiltro.Substring(4, iFim-4);
                        }
                    }

                    strNome = strNome.Replace("</b>", " ");
                    strNome = strNome.Replace("<b>", " ");


                    dLinha["Nome"] = strNome;

                    //Captura Coordenadas
                    string strCoordenads = "";
                    iInicio = page.IndexOf("{points:[");
                    if (iInicio < 0)
                    {
                        strCoordenads = "";
                    }
                    else
                    {
                        string strFiltro = page.Substring(iInicio);
                        iFim = strFiltro.IndexOf("},chr");
                        if (iFim < 0)
                        {
                            strCoordenads = "";
                        }
                        else
                        {
                            strCoordenads = strFiltro.Substring(9, iFim);
                        }
                    }

                    //Latitude
                    string strLatitude = strCoordenads.Substring(strCoordenads.IndexOf("{latlng:{lat:") + 13, strCoordenads.IndexOf(",") - strCoordenads.IndexOf("{latlng:{lat:") - 13);
                    dLinha["Latitude"] = strLatitude;

                    //Longitude
                    string strLongitude = strCoordenads.Substring(strCoordenads.IndexOf(",lng:") + 5, strCoordenads.IndexOf("},chr:") - strCoordenads.IndexOf(",lng:") - 5);
                    dLinha["Longitude"] = strLongitude;

                    //Captura Site
                    string strSite = "";
                    string strDummy1 = "/local_url?q=";
                    iInicio = page.IndexOf(strDummy1);
                    if (iInicio < 0)
                    {
                        strSite = "";
                    }
                    else
                    {
                        string strFiltro1 = page.Substring(iInicio);
                        iFim = strFiltro1.IndexOf("&amp;");
                        if (iFim < 0)
                        {
                            strSite = "";
                        }
                        else
                        {
                            strSite = strFiltro1.Substring(strDummy1.Length, iFim - strDummy1.Length);
                        }
                    }
                    dLinha["Site"] = strSite;

                    //Captura Endereço
                    string strEndereco = "";
                    string strDummy2 = "street-address";
                    iInicio = page.IndexOf(strDummy2);
                    if (iInicio < 0)
                    {
                        strEndereco = "";
                    }
                    else
                    {
                        string strFiltro2 = page.Substring(iInicio);
                        iFim = strFiltro2.IndexOf("</span>");
                        if (iFim < 0)
                        {
                            strEndereco = "";
                        }
                        else
                        {
                            strEndereco = strFiltro2.Substring(strDummy2.Length + 2, iFim - strDummy2.Length - 2);
                        }
                    }
                    dLinha["Endereço"] = strEndereco;

                    //Captura Localidade
                    string strLocalidade = "";
                    string strDummy3 = "locality";
                    iInicio = page.IndexOf(strDummy3);
                    if (iInicio < 0)
                    {
                        strLocalidade = "";
                    }
                    else
                    {
                        string strFiltro3 = page.Substring(iInicio);
                        iFim = strFiltro3.IndexOf("</span>");
                        if (iFim < 0)
                        {
                            strLocalidade = "";
                        }
                        else
                        {
                            strLocalidade = strFiltro3.Substring(strDummy3.Length + 2, iFim - strDummy3.Length - 2);
                        }
                    }
                    dLinha["Localidade"] = strLocalidade;

                    //Captura CEP
                    string strCEP = "";
                    string strDummy4 = "postal-code";
                    iInicio = page.IndexOf(strDummy4);
                    if (iInicio < 0)
                    {
                        strCEP = "";
                    }
                    else
                    {
                        string strFiltro4 = page.Substring(iInicio);
                        iFim = strFiltro4.IndexOf("</span>");
                        if (iFim < 0)
                        {
                            strCEP = "";
                        }
                        else
                        {
                            strCEP = strFiltro4.Substring(strDummy4.Length + 2, iFim - strDummy4.Length - 2);
                        }
                    }
                    dLinha["CEP"] = strCEP;

                    //Captura País
                    string strPais = "";
                    string strDummy5 = "country-name";
                    iInicio = page.IndexOf(strDummy5);
                    if (iInicio < 0)
                    {
                        strPais = "";
                    }
                    else
                    {
                        string strFiltro5 = page.Substring(iInicio);
                        iFim = strFiltro5.IndexOf("</span>");
                        if (iFim < 0)
                        {
                            strPais = "";
                        }
                        else
                        {
                            strPais = strFiltro5.Substring(strDummy5.Length + 2, iFim - strDummy5.Length - 2);
                        }
                    }
                    dLinha["País"] = strPais;

                    //Captura Telefone
                    string strTelefone = "";
                    string strDummy6 = "tel";
                    iInicio = page.IndexOf(strDummy6);
                    if (iInicio < 0)
                    {
                        strTelefone = "";
                    }
                    else
                    {
                        string strFiltro6 = page.Substring(iInicio);
                        iFim = strFiltro6.IndexOf("</nobr>");
                        if (iFim < 0)
                        {
                            strTelefone = "";
                        }
                        else
                        {
                            strTelefone = strFiltro6.Substring(strDummy6.Length + 7, iFim - strDummy6.Length - 7);
                        }
                    }
                    dLinha["Telefone"] = strTelefone;

                    //Adiciona a linha
                    dsDados.Tables[0].Rows.Add(dLinha);
                    progressBar1.Increment(1);

                    //Arruma a string page
                    page = page.Substring(page.IndexOf(strInicioPage) + strInicioPage.Length + 1);
                }
            }

            dataGridView1.DataSource = dsDados.Tables[0];
            dataGridView1.Refresh();
        }
    }
}
