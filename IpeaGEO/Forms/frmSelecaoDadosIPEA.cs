using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;
using IpeaGeo.Web;

namespace IpeaGeo.Forms
{
    public partial class frmSelecaoDadosIPEA : Form 
    {

        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private string strExtensao = "";
        private string strEnderecoBase = "";
        private DataSet ds_tabelas_csv = new System.Data.DataSet();

        Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

        private List<DataResource> resources;

        public frmSelecaoDadosIPEA()
        {
            InitializeComponent();
            resources = IpeaDataSelection.parseXmlDataConfig();
            FormaçãoTreeView();

        } // constructor

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        } //btnClose_Click()

        #region Formação TreeView
        private void FormaçãoTreeView()
        {
            foreach (DataResource resource in resources)
            {
                treeView1.Nodes.Add(resource.label);
                treeView1.Nodes[treeView1.Nodes.Count - 1].Name = resource.label;
                if (resource.items != null)
                {
                    foreach (DataResourceItem item in resource.items)
                    {
                        treeView1.Nodes[resource.label].Nodes.Add(item.label);
                        treeView1.Nodes[resource.label].Nodes[treeView1.Nodes[resource.label].Nodes.Count - 1].Name = item.label;
                    } //foreach
                } //if
            } // foreach
        } // FormaçãoTreeView()
        #endregion 

        #region Metadados RichTextBox

        // Metodo para poder selecionar o no com o botao direito mouse!
        private void treeView1_AfterSelect_1(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            string node = treeView1.SelectedNode.FullPath;
            char[] seps = { '\\' };
            string[] names = node.Split(seps);

            foreach (DataResource resource in resources)
                if (resource.label == names[0])
                {
                    if (names.Length == 1)
                        richTextBox1.Text = (resource.metadata != null) ? resource.metadata : "";
                    else if (resource.items != null)
                        foreach (DataResourceItem item in resource.items)
                            if (item.label == names[1])
                            {
                                richTextBox1.Text = (item.metadata != null) ? item.metadata : "";
                                break;
                            } // if
                    break;
                } // if
        } // treeView1_AfterSelect_1()
        #endregion 

        #region Leitura das Tabelas em CSV
        public static void TransferCSVToTable(ref DataTable dt1, string filePath)
        {
            string[] csvRows = System.IO.File.ReadAllLines(filePath);
            string[] fields = null;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(';');
                if (dt1.Columns.Count == 0)
                    for (int i = 0; i < fields.Length; i++)
                        dt1.Columns.Add(fields[i]);
                else
                {
                    DataRow row = dt1.NewRow();
                    row.ItemArray = fields;
                    dt1.Rows.Add(row);
                } // else
            } // foreach
        } // TransferCSVToTable()
        #endregion

        private string m_txt_metadados = "";

        private void frmDados_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(this.tabPage2);
            this.txtPesquisar.ForeColor = Color.Silver;

            //impede que os split containers mudem a razao de divisao entre eles//
            spc_1.IsSplitterFixed = true;
            spc_2.IsSplitterFixed = true;

            try
            {
                clsUtilTools mUtils1 = new clsUtilTools();

                //Cria um DataTable
                DataTable dt1 = new DataTable();

                // acha o endereco de onde o aplicativo esta sendo lido
                string strEndereco1 = Application.ExecutablePath;

                FileInfo fiExcel1 = new FileInfo(strEndereco1);

                ArrayList nomes = new ArrayList();
                nomes.Add("Arqui1");
                
                //string strEndCompleto = "";           
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        #region Importar

        public string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome_node = null;
                string node = null;

                if (treeView1.SelectedNode != null)
                {
                    nome_node = treeView1.SelectedNode.Name;
                    node = treeView1.SelectedNode.FullPath;
                }
                else if (listView1.SelectedItems.Count != 0)
                {
                    nome_node = listView1.SelectedItems[0].ToString();
                    nome_node = nome_node.Remove(0, nome_node.IndexOf("{") + 1);
                    nome_node = nome_node.Remove(nome_node.Length - 1);

                    foreach (DataResource resource in resources)
                        if (resource.items == null)
                        {
                            if (resource.label == nome_node)
                            {
                                node = nome_node;
                                break;
                            }
                        }
                        else
                        {
                            foreach (DataResourceItem item in resource.items)
                                if (item.label == nome_node)
                                {
                                    node = resource.label + "\\" + item.label;
                                    break;
                                }
                        }

                } // else if
                else MessageBox.Show("Nenhum elemento selecionado", "Aviso");

                /*string node = treeView1.SelectedNode.FullPath;*/
                char[] seps = { '\\' };
                string[] names = node.Split(seps);

                foreach (DataResource resource in resources)
                    if (resource.label == names[0])
                    {
                        if (names.Length == 1)
                            ((IPEAGEOMDIParent)this.MdiParent).AbrirDados(resource);
                        else if (resource.items != null)
                            foreach (DataResourceItem item in resource.items)
                                if (item.label == names[1])
                                {
                                    ((IPEAGEOMDIParent)this.MdiParent).AbrirDados(item, resource.url);
                                    break;
                                } // if

                        break;
                    } // if
                //frmMapaTematico mapa = new frmMapaTematico();
                //mapa.ClearXML();
            } // try
            catch { }
        } // btnImportar_Click()

        #endregion
                
        bool txtBox = false;
        private void txtPesquisar_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtPesquisar.ForeColor = Color.Black;

            if (txtBox == false)
            {
                txtPesquisar.Text = "";
                txtBox = true;
            } // if
        } // txtPesquisar_MouseClick()

        //--muda o cursor quando este estiver em cima do tree view--//
        private void treeView1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor = Cursors.Default;            
        } // treeView1_MouseMove()

        ArrayList algoritmo = new ArrayList();
        ArrayList trecho = new ArrayList();
        ArrayList meta_dados = new ArrayList();
        ArrayList lista = new ArrayList();

        ArrayList index = new ArrayList();
        
        // criado para dar valor a variavel pesq da btnPesquisa_Click (no lugar da variavel procura)
        public string pesq
        {
            get { return txtPesquisar.Text; }
        }     

#warning Reescrever lendo a partir do XML.
        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            listView1.Clear();

            try
            {
                if (txtPesquisar.Text != "")
                {
                    //apos apertar o botao pesquisar aparece o wait cursor ate a finalizar a acao de pesquisa
                    Cursor.Current = Cursors.WaitCursor;

                    int n_tabs = tabControl1.TabCount;

                    if (n_tabs < 2)
                        tabControl1.TabPages.Add(tabPage2);
                    tabControl1.SelectedIndex = 1;
                    treeView1.CollapseAll();

                    // Se ds_tabelas_csv estiver vazio, retorna null, senão recebe o nome da primeira tabela.
                    // TODO dt1 ficou null e não foi usado. Avaliar se não é conveniente remover.
                    DataTable dt1 = (ds_tabelas_csv.Tables.Count == 0) ? null : (DataTable)ds_tabelas_csv.Tables[0];

                    string pesquisa = (txtPesquisar.Text).ToLower();
                    string tentativa = "";

                    ArrayList dim1 = new ArrayList();
                    ArrayList dim2 = new ArrayList();
                    ArrayList st_encontrado = new ArrayList();

#warning Avaliar se não é conveniente remover.
                    if (dt1 != null)
                        for (int i = 0; i < dt1.Columns.Count - 2; i++)
                            for (int j = 0; j < ds_tabelas_csv.Tables[i + 1].Rows.Count; j++)
                            {
                                tentativa = ((string)ds_tabelas_csv.Tables[i + 1].Rows[j][1]);
                                if (EncontrarString(pesquisa, tentativa.ToLower()))
                                {
                                    dim1.Add(i);
                                    dim2.Add(j);
                                    st_encontrado.Add(tentativa);
                                } // if
                            } // for

                    # region Ferramenta de Pesquisa
                    string input_teste = ""; ;
                    int index_teste;


                    index.Clear();
                    listView1.Clear();

                    foreach (DataResource elemento in resources)
                    {

                        if (elemento.items != null)
                        {
                            foreach (DataResourceItem item in elemento.items)
                            {
                                if (Fonetizar(item.metadata.ToString() + " " + item.label.ToString()).IndexOf(" " + Fonetizar(pesq)) >= 0)
                                {
                                    richTextBox2.Text = null;
                                    listView1.Items.Add(item.label.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (Fonetizar(elemento.label.ToString() + " " + elemento.metadata.ToString()).IndexOf(" " + Fonetizar(pesq)) >= 0)
                            {
                                richTextBox2.Text = null;
                                listView1.Items.Add(elemento.label.ToString());
                            }
                        }
                    }
                }
                else
                {

                }
               Cursor.Current = Cursors.Default;
            } // try

            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            } // catch
        } // btnPesquisa_Click()

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            treeView1.SelectedNode = null;
            if (listView1.SelectedItems.Count != 0)
            {
                foreach (DataResource item in resources)
                {
                    if (item.items != null)
                    {
                        foreach (DataResourceItem elemento in item.items)
                        {
                            if (listView1.SelectedItems[0].ToString().IndexOf(elemento.label.ToString()) >= 0)
                            {
                                richTextBox2.Enabled = true;
                                richTextBox2.Clear();
                                richTextBox2.BackColor = System.Drawing.SystemColors.Window;
                                richTextBox2.Text = elemento.metadata.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (listView1.SelectedItems[0].ToString().IndexOf(item.label.ToString()) >= 0)
                        {
                            richTextBox2.Enabled = true;
                            richTextBox2.Clear();
                            richTextBox2.BackColor = System.Drawing.SystemColors.Window;
                            richTextBox2.Text = item.metadata.ToString();
                        }
                    }
                }
            } //if
        } // listView1_ItemSelectionChanged()
                #endregion

        private bool EncontrarString(string pesquisa, string tentativa)
        {
            double valor_corte = 1000;
            if (DistStrings(pesquisa, tentativa) < valor_corte)
                return true;

            return false;
        } // EncontrarString()

        private double DistStrings(string pesquisa, string tentativa)
        {
            double res = Double.PositiveInfinity;
            double indice = tentativa.IndexOf(pesquisa);
            if (indice >= 0) res = 0;
            return res;
        } //DistString()

        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            txtPesquisar.ForeColor = Color.Black;
        } // txtPesquisar_TextChanged()

        private void txtPesquisar_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                if (txtPesquisar.TextLength >= 0)
                    btnPesquisa.PerformClick();
        } // txtPesquisar_KeyUp()

        string output = string.Empty;
        string output1 = string.Empty;
        string output2 = string.Empty;
        string output3 = string.Empty;
        string procura = string.Empty;
        string resultado = string.Empty; 
        string resultado1 = string.Empty;
        string resultado2 = string.Empty;
        char consoante;
        char complemento;
        int length;

        #region Algoritmo de Pesquisa Fonética
        public string CapitalizeVowels(string input)
        {
            
            if (string.IsNullOrEmpty(input)) //since a string is a class object, it could be null
                return string.Empty;
            else
            {                
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == 'a' || input[i] == 'e' ||
                        input[i] == 'i' || input[i] == 'o' ||
                        input[i] == 'u')
                        output += input[i].ToString().ToUpper(); //Vowel
                    else
                        output += input[i].ToString().ToLower(); //Not vowel
                } // for
                
                return output;           
            } // else
        } // CapitalizeVowels()

        public string Fonetizar(string input, bool consulta = false)
        {
            input = RemoveAcentos(input.ToUpperInvariant());

            if (input.Equals("H")) input = "AGA";

            input = SomenteLetras(input);
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = FonetizarParticula(input);
            input = TrataConsoanteMuda(input, consoante, complemento);

            //Eliminar palavras especiais
            input = input.Replace(" LTDA ", " ");

            //Eliminar preposições
            var preposicoes = new[] { " DE ", " DA ", " DO ", " AS ", " OS ", " AO ", " NA ", " NO ", " DOS ", " DAS ", " AOS ", " NAS ", " NOS ", " COM " };
            input = preposicoes.Aggregate(input, (current, preposicao) => current.Replace(preposicao, " "));

            //Converte algarismos romanos para números
            var algRomanos = new[] { " V ", " I ", " IX ", " VI ", " IV ", " II ", " VII ", " III ", " X ", " VIII " };
            var numeros = new[] { " 5 ", " 1 ", " 9 ", " 6 ", " 4 ", " 2 ", " 7 ", " 3 ", " 10 ", " 8 " };
            for (int i = 0; i < algRomanos.Length; i++)
                input = input.Replace(algRomanos[i], numeros[i]);

            //Converte numeros para literais
            var algarismosExtenso = new[] { "ZERO", "UM", "DOIS", "TRES", "QUATRO", "CINCO", "SEIS", "SETE", "OITO", "NOVE" };
            for (int i = 0; i < 10; i++)
                input = input.Replace(i.ToString(), algarismosExtenso[i]);

            //Elimina preposições e artigos
            var letras = new[] { " A ", " B ", " C ", " D ", " E ", " F ", " G ", " H ", " I ", " J ", " K ", " L ", " M ", " N ", " O ", " P ", " Q ", " R ", " S ", " T ", " U ", " V ", " X ", " Z ", " W ", " Y " };
            input = letras.Aggregate(input, (current, letra) => current.Replace(letra, " "));

            input = input.Trim();
            var particulas = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var output = new string[particulas.Length];
            for (var i = 0; i < particulas.Length; i++)
                output[i] = FonetizarParticula(particulas[i]);

            string output1 = string.Join(" ", output).Trim();
            if (consulta)
                output1 = "%" + string.Join("%%", output) + "%";

            if (input.Length < 200)
                procura = input;
            else
                output2 = input;
            return input;
        } // Fonetizar()

        public string SomenteLetras(string output1)
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVXZWY ";
            var letraAnt = output1[0];
            output3 = "";
            foreach (var letraT in output1)
                foreach (var letraC in letras.Where(letraC => letraC == letraT).TakeWhile(letraC => letraAnt != ' ' || letraT != ' '))
                {
                    output3 += letraC;
                    letraAnt = letraT;
                    break;
                } // foreach

            return output3.ToUpperInvariant();
        } // SomenteLetras()

        public string FonetizarParticula(string input)
        {
            string aux2;
            int j;
            const string letras = "ABPCKQDTEIYFVWGJLMNOURSZX9";
            const string codFonetico = "123444568880AABCDEEGAIJJL9";

            input = input.ToUpperInvariant();
            string output3 = input[0].ToString();

            //Elimina os caracteres repetidos
            for (int i = 1; i < input.Length; i++)
                if (input[i - 1] != input[i])
                    output3 += input[i];

            //Iguala os fonemas parecidos
            if (output3[0].Equals('W'))
                if (output3[1].Equals('I'))
                    output3 = output3.Remove(0, 1).Insert(0, "U");
                else if ("A,E,O,U".Contains(output3[1]))
                    output3 = output3.Remove(0, 1).Insert(0, "V");

            var caracteres = new[]
                {
                    "TSCH", "SCH", "TSH", "TCH", "SH", "CH", "LH", "NH", "PH", "GN", "MN", "SCE", "SCI", "SCY"
                    , "CS", "KS", "PS", "TS", "TZ", "XS", "CE", "CI", "CY", "GE", "GI", "GY", "GD", "CK", "PC"
                    , "QU", "SC", "SK", "XC", "SQ", "CT", "GT", "PT"
                };

            var caracteresSub = new[]
                {
                    "XXXX", "XXX", "XXX", "XXX", "XX", "XX", "LI", "NN", "FF", "NN", "NN", "SSI", "SSI",
                    "SSI", "SS", "SS", "SS", "SS", "SS", "SS", "SE", "SI", "SI", "JE", "JI", "JI", "DD",
                    "QQ", "QQ", "QQ", "SQ", "SQ", "SQ", "99", "TT", "TT", "TT"
                };

            for (int i = 0; i < caracteres.Length; i++)
                output3 = output3.Replace(caracteres[i], caracteresSub[i]);

            //Trata consoantes mudas
            output3 = TrataConsoanteMuda(output3, 'B', 'I');
            output3 = TrataConsoanteMuda(output3, 'D', 'I');
            output3 = TrataConsoanteMuda(output3, 'P', 'I');

            //Trata as letras
            //Retira letras iguais
            if (output3[0].Equals('H'))
            {
                aux2 = Convert.ToString(output3[1]);
                j = 2;
            } // if
            else
            {
                aux2 = Convert.ToString(output3[0]);
                j = 1;
            } // else

            while (j < output3.Length)
            {
                if (output3[j] != output3[j - 1] && output3[j] != 'H')
                    aux2 += output3[j];

                j++;
            } // while

            output3 = aux2;

            //Transforma letras em códigos fonéticos
            //.Select(chr => letras.IndexOf(chr)).Aggregate(string.Empty, (current, n) => current + codFonetico[n]);
            return output3;
        } // FonetizarParticula()

        public string TrataConsoanteMuda(string input, char consoante, char complemento)
        {
            var i = input.IndexOf(consoante);
            while (i > -1)
            {
                if (i >= input.Length - 1 || !"AEIOU".Contains(input[i + 1]))
                {
                    input = input.Insert(i + 1, Convert.ToString(complemento));
                    i++;
                } // if
                i = input.IndexOf(consoante, ++i);
            } // while
            return input;
        } // TrataConsoanteMuda()

        public string SubstituiTerminacao(string input)
        {
            input = RemoveAcentos(input);

            var terminacao = new[] { "N", "B", "D", "T", "W", "AM", "OM", "OIM", "UIM", "CAO", "AO", "OEM", "ONS", "EIA", "X", "US", "TH" };
            var terminacaoSub = new[] { "M", " ", " ", " ", " ", "N ", "N ", "N  ", "N  ", "SSN", "N ", "N  ", "N  ", "IA ", "S", "OS", "TI" };
            var tamanhoMinStr = new[] { 2, 3, 3, 3, 3, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 3 };
            int tamanho = 0;
            do
            {
                for (int i = 0; i < terminacao.Length; i++)
                {
                    if (input.EndsWith(terminacao[i]) && input.Length >= tamanhoMinStr[i])
                    {
                        var startIndex = input.Length - terminacao[i].Length;
                        input = input.Remove(startIndex, terminacao[i].Length)
                            .Insert(startIndex, terminacaoSub[i]);
                    } // if
                    else if (input.Length < tamanhoMinStr[i])
                    {
                        tamanho = tamanhoMinStr[i];
                        break;
                    } // else if
                } // for
            } while (input.EndsWith("N") && input.Length >= tamanho);
            return input;
        } // SubstituiTerminacao()

        public string RemoveAcentos(string input)
        {
            const string comAcento = "áÁàÀâÂãÃéÉèÈêÊíÍìÌîÎóÓòÒôÔõÕúÚùÙûÛüÜçÇñÑ";
            const string semAcento = "AAAAAAAAEEEEEEIIIIIIOOOOOOOOUUUUUUUUCCNN";

            for (var i = 0; i < comAcento.Length; i++)
                input = input.Replace(comAcento[i], semAcento[i]);
            
            return input;
        } // RemoveAcentos()

        #endregion

        public string Intervalo(string input, int index)//, int index-1, int index+1)
        {
            length = procura.Length;

            resultado1 = resultado1.Remove(0, index + length);
            if (index <= 0) resultado1 = "";
            
            return resultado1;
        } // Intervalo()
    } // class
 } // namespace

        
    




      
    



    







