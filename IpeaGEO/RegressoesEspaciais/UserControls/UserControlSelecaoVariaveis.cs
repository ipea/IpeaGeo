using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class UserControlSelecaoVariaveis : UserControl
    {
        public UserControlSelecaoVariaveis()
        {
            InitializeComponent();
        }

        #region variaveis

        private string[] listavariaveisoriginal = new string[0];
        private string[] listavariaveis = new string[0];
        private string[] lista_dep_variaveis = new string[0];
        private string[] lista_indep_variaveis = new string[0];
        //private string[] lista_inst_variaveis = new string[0];

        #endregion

        #region propriedades

        public string LabelListBoxEsquerda
        {
            set
            {
            	this.grbVariaveisDisponiveis.Text = value;
            }
        }

        public string LabelListBoxDireita
        {
            set
            {
            	this.groupBox1.Text = value;
            }
        }

        public bool PermiteSelecaoMultipla
        {
            set 
            {
                if (value)
                {
                    this.lstVariaveisDisponiveis.SelectionMode = SelectionMode.MultiExtended;
                    this.lstVariaveisIndependentes.SelectionMode = SelectionMode.MultiExtended;
                }
                else
                {
                    this.lstVariaveisDisponiveis.SelectionMode = SelectionMode.One;
                    this.lstVariaveisIndependentes.SelectionMode = SelectionMode.One;
                }
            }
        }

        public bool SelecaoVariaveisDisponivel
        {
            set
            {
                if (value)
                {
                    this.lstVariaveisIndependentes.Enabled = true;

                    this.btnAddAllVariaveisIndependentes.Enabled = true;
                    this.btnAddVariavelIndependente.Enabled = true;
                    this.btnRemoveAllVariaveisIndependentes.Enabled = true;
                    this.btnRemoveVariavelIndependente.Enabled = true;
                }
                else
                {
                    //this.btnAddAllVariaveisIndependentes.Enabled = true;
                    //this.btnAddVariavelIndependente.Enabled = true;
                    //this.btnRemoveAllVariaveisIndependentes.Enabled = true;
                    //this.btnRemoveVariavelIndependente.Enabled = true;

                    this.RemoveAllVariaveisSelecionadas();
                    this.lstVariaveisIndependentes.Enabled = false;

                    this.btnAddAllVariaveisIndependentes.Enabled = 
                    this.btnAddVariavelIndependente.Enabled = 
                    this.btnRemoveAllVariaveisIndependentes.Enabled = 
                    this.btnRemoveVariavelIndependente.Enabled = false;
                }
            }
        }

        private void RemoveAllVariaveisSelecionadas()
        {
            int item = 0;
            string s = "";
            int nvars = this.lstVariaveisIndependentes.Items.Count;

            for (int i = 0; i < nvars; i++)
            {
                item = 0;
                s = this.lista_indep_variaveis[item];
                this.DeleteItemFromString(ref this.lista_indep_variaveis, this.lista_indep_variaveis, item);

                this.lstVariaveisIndependentes.Items.Clear();
                this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                this.RetornaVariableToDataList(s);
            }
        }

        public void UnselectAll()
        {
            this.RemoveAllVariaveisIndependentes();
        }

        public void ZeraControle()
        {
            this.lstVariaveisDisponiveis.Items.Clear();
            this.lstVariaveisIndependentes.Items.Clear();

            listavariaveisoriginal = new string[0];
            listavariaveis = new string[0];
            lista_dep_variaveis = new string[0];
            lista_indep_variaveis = new string[0];
            //lista_inst_variaveis = new string[0];
        }

        private string[] m_variaveis;
        public string[] VariaveisList
        {
            get { return this.m_variaveis; }
            set
            {
                this.m_variaveis = value;
                if (this.m_variaveis != null)
                {
                    this.CarregaVariavies(VariaveisList);
                }
            }
        }

        private string[] m_variaveisDB;
        public string[] VariaveisDB
        {
            get { return this.m_variaveisDB; }
            set { this.m_variaveisDB = value; }
        }

        public string[] VariaveisIndependentes
        {
            get { return this.lista_indep_variaveis; }
            set
            {
                RemoveAllVariaveisIndependentes();

                if (value.GetLength(0) == 1)
                {
                    string s = value[0];
                    string[] aux = this.lista_indep_variaveis;
                    int nobs = this.lista_indep_variaveis.GetLength(0) + 1;
                    this.lista_indep_variaveis = new string[nobs];
                    for (int i = 0; i < nobs - 1; i++)
                    {
                        this.lista_indep_variaveis[i] = aux[i];
                    }
                    this.lista_indep_variaveis[nobs - 1] = s;

                    this.lstVariaveisIndependentes.Items.Clear();
                    this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                    this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, s);
                    this.lstVariaveisDisponiveis.Items.Clear();
                    this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                }
                else
                {
                    if (value.GetLength(0) > 1)
                    {
                        int nselecionado = value.GetLength(0);
                        string[] s = new string[nselecionado];

                        for (int i = 0; i < s.GetLength(0); i++)
                        {
                            s[i] = value[i];
                        }

                        string[] aux = this.lista_indep_variaveis;
                        int nobs = this.lista_indep_variaveis.GetLength(0) + nselecionado;
                        this.lista_indep_variaveis = new string[nobs];
                        for (int i = 0; i < aux.GetLength(0); i++)
                        {
                            this.lista_indep_variaveis[i] = aux[i];
                        }
                        for (int i = 0; i < nselecionado; i++)
                        {
                            this.lista_indep_variaveis[i + aux.GetLength(0)] = s[i];
                        }

                        this.lstVariaveisIndependentes.Items.Clear();
                        this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                        for (int i = nselecionado - 1; i >= 0; i--)
                            this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, s[i]);

                        this.lstVariaveisDisponiveis.Items.Clear();
                        this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                    }
                }
            }
        }

        #endregion

        #region methods

        private void DeleteItemFromString(ref string[] sout, string[] sin, string s)
        {
            int item = -1;
            for (int i = 0; i < sin.GetLength(0); i++)
            {
                if (sin[i] == s)
                {
                    item = i;
                    break;
                }
            }

            if (item < 0 || item > sin.GetLength(0) - 1) sout = sin;
            else
            {
                int nobs = sin.GetLength(0) - 1;
                sout = new string[nobs];

                if (item == 0) { for (int i = 0; i < nobs; i++) sout[i] = sin[i + 1]; }
                else
                {
                    if (item == nobs) { for (int i = 0; i < nobs; i++) sout[i] = sin[i]; }
                    else
                    {
                        for (int i = 0; i < item; i++) sout[i] = sin[i];
                        for (int i = item + 1; i <= nobs; i++) sout[i - 1] = sin[i];
                    }
                }
            }
        }

        private void DeleteItemFromString(ref string[] sout, string[] sin, int item)
        {
            if (item < 0 || item > sin.GetLength(0) - 1) sout = sin;
            else
            {
                int nobs = sin.GetLength(0) - 1;
                sout = new string[nobs];

                if (item == 0) { for (int i = 0; i < nobs; i++) sout[i] = sin[i + 1]; }
                else
                {
                    if (item == nobs) { for (int i = 0; i < nobs; i++) sout[i] = sin[i]; }
                    else
                    {
                        for (int i = 0; i < item; i++) sout[i] = sin[i];
                        for (int i = item + 1; i <= nobs; i++) sout[i - 1] = sin[i];
                    }
                }
            }
        }

        private void RetornaVariableToDataList(string s)
        {
            int pos = this.RetornaPosicaoListaOriginal(s);

            string[] snew = new string[this.listavariaveis.GetLength(0) + 1];

            int min = this.listavariaveisoriginal.GetLength(0) - 1;
            int max = 0;
            for (int i = 0; i < this.listavariaveis.GetLength(0); i++)
            {
                if (min > this.RetornaPosicaoListaOriginal(this.listavariaveis[i]))
                {
                    min = this.RetornaPosicaoListaOriginal(this.listavariaveis[i]);
                }
                if (max < this.RetornaPosicaoListaOriginal(this.listavariaveis[i]))
                {
                    max = this.RetornaPosicaoListaOriginal(this.listavariaveis[i]);
                }
            }

            int ponto = 0;
            if (pos > max)
            {
                for (int i = 0; i < this.listavariaveis.GetLength(0); i++) snew[i] = this.listavariaveis[i];
                snew[this.listavariaveis.GetLength(0)] = s;
            }
            else
            {
                if (pos < min)
                {
                    for (int i = 0; i < this.listavariaveis.GetLength(0); i++) snew[i + 1] = this.listavariaveis[i];
                    snew[0] = s;
                }
                else
                {
                    for (int i = 0; i < this.listavariaveis.GetLength(0); i++)
                    {
                        if (pos > this.RetornaPosicaoListaOriginal(this.listavariaveis[i]))
                        {
                            snew[i] = this.listavariaveis[i];
                        }
                        else
                        {
                            ponto = i;
                            break;
                        }
                    }
                    snew[ponto] = s;
                    for (int i = ponto + 1; i <= this.listavariaveis.GetLength(0); i++)
                    {
                        snew[i] = this.listavariaveis[i - 1];
                    }
                }
            }

            clsUtilTools clt = new clsUtilTools();
            snew = clt.EliminaStringsDuplicadas(snew);

            this.listavariaveis = snew;
            this.lstVariaveisDisponiveis.Items.Clear();
            this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
        }

        private int RetornaPosicaoListaOriginal(string s)
        {
            int r = 0;
            int i = 0;
            for (i = 0; i < this.listavariaveisoriginal.GetLength(0); i++)
            {
                if (s == this.listavariaveisoriginal[i])
                {
                    r = i;
                    break;
                }
            }
            return r;
        }

        private void CarregaVariavies(string[] lista)
        {
            string[] auxlista = lista;

            this.listavariaveis = auxlista;
            this.listavariaveisoriginal = auxlista;

            this.lstVariaveisDisponiveis.Items.Clear();
            this.lstVariaveisDisponiveis.Items.AddRange(listavariaveis);
            this.lstVariaveisDisponiveis.SelectedIndex = 0;
        }

        //TODO: está certo? (Cauê)
   
        public void addEvent (EventHandler evento)
        {
            this.btnAddVariavelIndependente.Click += evento;
            this.btnAddAllVariaveisIndependentes.Click += evento;
            this.btnRemoveAllVariaveisIndependentes.Click += evento;
            this.btnRemoveVariavelIndependente.Click += evento;
        }

        #endregion

        #region eventos

        private void ReordenaListaVariaveis(ref string[] sout, string[] sin)
        {
            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;
            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, sin);

            sout = new string[variaveis_na_base.GetLength(0) + Wvariaveis_na_base.GetLength(0)];
            for (int i = 0; i < variaveis_na_base.GetLength(0); i++)
            {
                sout[i] = variaveis_na_base[i];
            }

            for (int i = 0; i < Wvariaveis_na_base.GetLength(0); i++)
            {
                sout[i + variaveis_na_base.GetLength(0)] = "W_x_" + Wvariaveis_na_base[i];
            }
        }

        private void SeparaBlocosVariaveis(out string[] variaveis_na_base, out string[] Wvariaveis_na_base, string[] lista_variaveis)
        {
            ArrayList xvs = new ArrayList();
            ArrayList wsvs = new ArrayList();

            for (int i = 0; i < lista_variaveis.GetLength(0); i++)
            {
                if (lista_variaveis[i].Length > 3 && lista_variaveis[i].Substring(0, 4) == "W_x_")
                {
                    wsvs.Add(lista_variaveis[i].Substring(4));
                }
                else
                {
                    xvs.Add(lista_variaveis[i]);
                }
            }

            variaveis_na_base = new string[xvs.Count];
            Wvariaveis_na_base = new string[wsvs.Count];

            for (int i = 0; i < xvs.Count; i++)
            {
                variaveis_na_base[i] = xvs[i].ToString();
            }

            for (int i = 0; i < wsvs.Count; i++)
            {
                Wvariaveis_na_base[i] = wsvs[i].ToString();
            }
        }

        private void btnAddVariavelIndependente_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            if (this.lstVariaveisDisponiveis.SelectedIndex >= 0
                && this.lstVariaveisDisponiveis.SelectedIndex < this.lstVariaveisDisponiveis.Items.Count)
            {
                if (this.lstVariaveisDisponiveis.SelectedItems.Count == 1)
                {
                    int item = this.lstVariaveisDisponiveis.SelectedIndex;
                    //Aqui tem um BUG!!!
                    string s = this.listavariaveis[item];
                    string[] aux = this.lista_indep_variaveis;
                    int nobs = this.lista_indep_variaveis.GetLength(0) + 1;
                    this.lista_indep_variaveis = new string[nobs];
                    for (int i = 0; i < nobs - 1; i++)
                    {
                        this.lista_indep_variaveis[i] = aux[i];
                    }
                    this.lista_indep_variaveis[nobs - 1] = s;


                    lista_indep_variaveis = clt.EliminaStringsDuplicadas(lista_indep_variaveis);
                    this.ReordenaListaVariaveis(ref lista_indep_variaveis, lista_indep_variaveis);

                    this.lstVariaveisIndependentes.Items.Clear();
                    this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                    //this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item);
                    //this.lstVariaveisDisponiveis.Items.Clear();
                    //this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                }
                else
                {
                    if (this.lstVariaveisDisponiveis.SelectedItems.Count > 1)
                    {
                        int nselecionado = this.lstVariaveisDisponiveis.SelectedItems.Count;
                        int[] item = new int[nselecionado];
                        string[] s = new string[nselecionado];

                        for (int i = 0; i < s.GetLength(0); i++)
                        {
                            item[i] = this.lstVariaveisDisponiveis.SelectedIndices[i];
                            s[i] = this.listavariaveis[item[i]];
                        }

                        string[] aux = this.lista_indep_variaveis;
                        int nobs = this.lista_indep_variaveis.GetLength(0) + nselecionado;
                        this.lista_indep_variaveis = new string[nobs];
                        for (int i = 0; i < aux.GetLength(0); i++)
                        {
                            this.lista_indep_variaveis[i] = aux[i];
                        }
                        for (int i = 0; i < nselecionado; i++)
                        {
                            this.lista_indep_variaveis[i + aux.GetLength(0)] = s[i];
                        }

                        this.lstVariaveisIndependentes.Items.Clear();
                        this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                        for (int i = nselecionado - 1; i >= 0; i--)
                            this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item[i]);

                        this.lstVariaveisDisponiveis.Items.Clear();
                        this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                    }
                }
            }
        }

        //TODO: esta certo? (Cauê)

        private void btnRemoveVariavelIndependente_Click(object sender, EventArgs e)
        {
            if (this.lstVariaveisIndependentes.SelectedIndex >= 0
                && this.lstVariaveisIndependentes.SelectedIndex < this.lstVariaveisIndependentes.Items.Count)
            {
                if (this.lstVariaveisIndependentes.SelectedIndices.Count == 1)
                {
                    int item = this.lstVariaveisIndependentes.SelectedIndex;
                    string s = this.lista_indep_variaveis[item];
                    this.DeleteItemFromString(ref this.lista_indep_variaveis, this.lista_indep_variaveis, item);

                    this.lstVariaveisIndependentes.Items.Clear();
                    this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);
                }

                //for (int i = 0; i < lstVariaveisIndependentes.SelectedItems.Count; i++)
                //    lstVariaveisDisponiveis.Items.Add(lstVariaveisIndependentes.SelectedItems[i]);

                //for (int i = 0; i < lstVariaveisIndependentes.SelectedItems.Count; i++)
                //{
                //    this.DeleteItemFromString(ref this.lista_indep_variaveis, this.lista_indep_variaveis, lstVariaveisIndependentes.SelectedItems[i].ToString());
                //    lstVariaveisIndependentes.Items.Remove(lstVariaveisIndependentes.SelectedItems[i]);                
                //} 
            }

            else
            {
                if (this.lstVariaveisIndependentes.SelectedIndices.Count > 1)
                {
                    int nselected = this.lstVariaveisIndependentes.SelectedIndices.Count;
                    int[] item = new int[nselected];
                    string[] s = new string[nselected];
                    for (int i = 0; i < nselected; i++)
                    {
                        item[i] = this.lstVariaveisIndependentes.SelectedIndices[i];
                        s[i] = this.lista_indep_variaveis[item[i]];
                    }

                    for (int i = nselected - 1; i >= 0; i--)
                    {
                        this.DeleteItemFromString(ref this.lista_indep_variaveis, this.lista_indep_variaveis, item[i]);
                    }

                    this.lstVariaveisIndependentes.Items.Clear();
                    this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);
                }
            }

        }

        private void btnAddAllVariaveisIndependentes_Click(object sender, EventArgs e)
        {
            if (lstVariaveisDisponiveis.Items.Count > 0)
            {
                RemoveAllVariaveisIndependentes();
                lstVariaveisIndependentes.Items.AddRange(lstVariaveisDisponiveis.Items);
                lstVariaveisDisponiveis.Items.Clear();
                this.lista_indep_variaveis = new string[lstVariaveisIndependentes.Items.Count];
                for (int k = 0; k < lista_indep_variaveis.GetLength(0); k++)
                    this.lista_indep_variaveis[k] = lstVariaveisIndependentes.Items[k].ToString();
            }           
        }

        private void RemoveAllVariaveisIndependentes()
        {
            if (lstVariaveisIndependentes.Items.Count > 0)
            {
                int item = 0;
                string s = "";
                int nvars = this.lstVariaveisIndependentes.Items.Count;

                for (int i = 0; i < nvars; i++)
                {
                    item = 0;
                    s = this.lista_indep_variaveis[item];
                    this.DeleteItemFromString(ref this.lista_indep_variaveis, this.lista_indep_variaveis, item);

                    this.lstVariaveisIndependentes.Items.Clear();
                    this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                    this.RetornaVariableToDataList(s);
                }
            }
        }

        private void btnRemoveAllVariaveisIndependentes_Click(object sender, EventArgs e)
        {
            RemoveAllVariaveisIndependentes();
        }

        #endregion


    }
}
