using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGeo
{
    public partial class UserControlRegressaoInstrumentos : UserControl
    {
        public UserControlRegressaoInstrumentos()
        {
            InitializeComponent();
        }

        #region variaveis

        private string[] listavariaveisoriginal = new string[0];
        private string[] listavariaveis = new string[0];
        private string[] lista_dep_variaveis = new string[0];
        private string[] lista_indep_variaveis = new string[0];
        private string[] lista_inst_variaveis = new string[0];

        #endregion
        
        #region methods

        public void EsconderSelecaoInstrumentos(bool colapsa)
        {
            splitContainer4.Panel2Collapsed = colapsa;
        }

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
            try
            {
                string[] auxlista = lista;

                this.listavariaveis = auxlista;
                this.listavariaveisoriginal = auxlista;

                this.lstVariaveisDisponiveis.Items.Clear();
                this.lstVariaveisDisponiveis.Items.AddRange(listavariaveis);
                this.lstVariaveisDisponiveis.SelectedIndex = 0;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
        
        #region propriedades

        public void ZeraControle()
        {
            this.lstVariaveisDisponiveis.Items.Clear();
            this.lstVariaveisIndependentes.Items.Clear();
            this.lstVariaveisInstrumentais.Items.Clear();
            this.lstVariavelDependente.Items.Clear();

            listavariaveisoriginal = new string[0];
            listavariaveis = new string[0];
            lista_dep_variaveis = new string[0];
            lista_indep_variaveis = new string[0];
            lista_inst_variaveis = new string[0];
        }

        public bool SelecaoInstrumentosDisponivel
        {
            set
            {
                if (value)
                {
                    this.lstVariaveisInstrumentais.Enabled = true;

                    btnAddAllVariaveisInstrumentais.Enabled = true;
                    btnAddVariavelInstrumental.Enabled = true;
                    btnRemoveAllVariaveisInstrumentais.Enabled = true;
                    btnRemoveVariavelInstrumental.Enabled = true;
                }
                else
                {
                    //btnAddAllVariaveisInstrumentais.Enabled = true;
                    //btnAddVariavelInstrumental.Enabled = true;
                    //btnRemoveAllVariaveisInstrumentais.Enabled = true;
                    //btnRemoveVariavelInstrumental.Enabled = true;

                    this.RemoveAllVariaveisInstrumentais();
                    this.lstVariaveisInstrumentais.Enabled = false;

                    btnAddAllVariaveisInstrumentais.Enabled = false;
                    btnAddVariavelInstrumental.Enabled = false;
                    btnRemoveAllVariaveisInstrumentais.Enabled = false;
                    btnRemoveVariavelInstrumental.Enabled = false;

                    splitContainer4.Panel2Collapsed = true;
                }
            }
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

        public string[] VariavelDependente
        {
            get { return this.lista_dep_variaveis; }
            set
            {
                if (value.GetLength(0) > 0)
                {
                    string snow = "";
                    if (this.lista_dep_variaveis.GetLength(0) > 0)
                    {
                        snow = this.lista_dep_variaveis[0];
                    }

                    this.lstVariavelDependente.Items.Clear();
                    string s = value[0];
                    this.lstVariavelDependente.Items.Add(s);

                    this.lista_dep_variaveis = new string[1];
                    this.lista_dep_variaveis[0] = s;

                    this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, s);
                    this.lstVariaveisDisponiveis.Items.Clear();
                    this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);

                    if (snow != "")
                    {
                        this.RetornaVariableToDataList(snow);
                    }
                }
            }
        }

        public string[] VariaveisIndependentes
        {
            get { return this.lista_indep_variaveis; }
            set
            {
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

        public string[] VariaveisInstrumentais
        {
            get { return this.lista_inst_variaveis; }
            set
            {
                if (value.GetLength(0) == 1)
                {
                    string s = value[0];
                    string[] aux = this.lista_inst_variaveis;
                    int nobs = this.lista_inst_variaveis.GetLength(0) + 1;
                    this.lista_inst_variaveis = new string[nobs];
                    for (int i = 0; i < nobs - 1; i++)
                    {
                        this.lista_inst_variaveis[i] = aux[i];
                    }
                    this.lista_inst_variaveis[nobs - 1] = s;

                    this.lstVariaveisInstrumentais.Items.Clear();
                    this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

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

                        string[] aux = this.lista_inst_variaveis;
                        int nobs = this.lista_inst_variaveis.GetLength(0) + nselecionado;

                        this.lista_inst_variaveis = new string[nobs];

                        for (int i = 0; i < aux.GetLength(0); i++)
                        {
                            this.lista_inst_variaveis[i] = aux[i];
                        }
                        for (int i = 0; i < nselecionado; i++)
                        {
                            this.lista_inst_variaveis[i + aux.GetLength(0)] = s[i];
                        }

                        this.lstVariaveisInstrumentais.Items.Clear();
                        this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                        for (int i = nselecionado - 1; i >= 0; i--)
                            this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, s[i]);

                        this.lstVariaveisDisponiveis.Items.Clear();
                        this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                    }
                }
            }
        }

        #endregion

        #region eventos

        private void btnAddResponseVariable_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            if (this.lstVariaveisDisponiveis.SelectedIndex >= 0 
                && this.lstVariaveisDisponiveis.SelectedIndex < this.lstVariaveisDisponiveis.Items.Count)
            {
                string snow = "";
                if (this.lista_dep_variaveis.GetLength(0) > 0)
                {
                    snow = this.lista_dep_variaveis[0];
                }

                this.lstVariavelDependente.Items.Clear();
                int item = this.lstVariaveisDisponiveis.SelectedIndex;
                string s = this.listavariaveis[item];

                if (s.Length > 3 && s.Substring(0, 4) == "W_x_")
                {
                    MessageBox.Show("Uma variável da forma W*X não pode ser adicionada como variável dependente!");
                    return;
                }

                this.lstVariavelDependente.Items.Add(s);

                this.lista_dep_variaveis = new string[1];
                this.lista_dep_variaveis[0] = s;

                this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item);
                this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, "W_x_" + s);

                this.lstVariaveisDisponiveis.Items.Clear();
                this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);

                if (snow != "")
                {
                    this.RetornaVariableToDataList(snow);
                    if (clt.PosicaoNaLista(this.listavariaveisoriginal, "W_x_" + snow) >= 0)
                    {
                        this.RetornaVariableToDataList("W_x_" + snow);
                    }
                }
            }
        }

        private void btnRemoveResponseVariable_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            if (this.lista_dep_variaveis.GetLength(0) > 0)
            {
                string s = this.lista_dep_variaveis[0];
                
                this.RetornaVariableToDataList(s);
                if (clt.PosicaoNaLista(this.listavariaveisoriginal, "W_x_" + s) >= 0)
                {
                    this.RetornaVariableToDataList("W_x_" + s);
                }

                this.lista_dep_variaveis = new string[0];
                this.lstVariavelDependente.Items.Clear();
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

                        lista_indep_variaveis = clt.EliminaStringsDuplicadas(lista_indep_variaveis);
                        this.ReordenaListaVariaveis(ref lista_indep_variaveis, lista_indep_variaveis);

                        this.lstVariaveisIndependentes.Items.Clear();
                        this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                        //for (int i = nselecionado - 1; i >= 0; i--)
                        //    this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item[i]);

                        //this.lstVariaveisDisponiveis.Items.Clear();
                        //this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                    }
                }
            }
        }

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

                    //this.RetornaVariableToDataList(s);
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

                        //for (int i = 0; i < nselected; i++)
                        //{
                        //    this.RetornaVariableToDataList(s[i]);
                        //}
                    }
                }
            }
        }

        private void btnAddAllVariaveisIndependentes_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            int nvars = this.listavariaveis.GetLength(0);
            int item = 0;
            string s = "";
            string[] aux = new string[0];
            int nobs = 0;

            this.lista_indep_variaveis = new string[nvars];
            for (int i = 0; i < nvars; i++) this.lista_indep_variaveis[i] = this.listavariaveis[i];

            lista_indep_variaveis = clt.EliminaStringsDuplicadas(lista_indep_variaveis);
            this.ReordenaListaVariaveis(ref lista_indep_variaveis, lista_indep_variaveis);

            this.lstVariaveisIndependentes.Items.Clear();
            this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

            /*
            for (int i = 0; i < nvars; i++)
            {
                item = 0;
                s = this.listavariaveis[item];
                aux = this.lista_indep_variaveis;
                nobs = this.lista_indep_variaveis.GetLength(0) + 1;
                this.lista_indep_variaveis = new string[nobs];
                for (int k = 0; k < nobs - 1; k++)
                {
                    this.lista_indep_variaveis[k] = aux[k];
                }
                this.lista_indep_variaveis[nobs - 1] = s;

                this.lstVariaveisIndependentes.Items.Clear();
                this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);

                this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item);
                this.lstVariaveisDisponiveis.Items.Clear();
                this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
            }
            */
        }

        private void btnRemoveAllVariaveisIndependentes_Click(object sender, EventArgs e)
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

                //this.RetornaVariableToDataList(s);
            }
        }

        private void btnAddVariavelInstrumental_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            if (this.lstVariaveisDisponiveis.SelectedIndex >= 0
                && this.lstVariaveisDisponiveis.SelectedIndex < this.lstVariaveisDisponiveis.Items.Count)
            {
                if (this.lstVariaveisDisponiveis.SelectedItems.Count == 1)
                {
                    int item = this.lstVariaveisDisponiveis.SelectedIndex;
                    string s = this.listavariaveis[item];
                    string[] aux = this.lista_inst_variaveis;
                    int nobs = this.lista_inst_variaveis.GetLength(0) + 1;
                    this.lista_inst_variaveis = new string[nobs];
                    for (int i = 0; i < nobs - 1; i++)
                    {
                        this.lista_inst_variaveis[i] = aux[i];
                    }
                    this.lista_inst_variaveis[nobs - 1] = s;

                    lista_inst_variaveis = clt.EliminaStringsDuplicadas(lista_inst_variaveis);
                    this.ReordenaListaVariaveis(ref lista_inst_variaveis, lista_inst_variaveis);

                    this.lstVariaveisInstrumentais.Items.Clear();
                    this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

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

                        //string[] aux = this.lista_indep_variaveis;
                        string[] aux = this.lista_inst_variaveis;

                        //int nobs = this.lista_indep_variaveis.GetLength(0) + nselecionado;
                        int nobs = this.lista_inst_variaveis.GetLength(0) + nselecionado;

                        //this.lista_indep_variaveis = new string[nobs];
                        this.lista_inst_variaveis = new string[nobs];

                        for (int i = 0; i < aux.GetLength(0); i++)
                        {
                            //this.lista_indep_variaveis[i] = aux[i];
                            this.lista_inst_variaveis[i] = aux[i];
                        }
                        for (int i = 0; i < nselecionado; i++)
                        {
                            //this.lista_indep_variaveis[i + aux.GetLength(0)] = s[i];
                            this.lista_inst_variaveis[i + aux.GetLength(0)] = s[i];
                        }

                        lista_inst_variaveis = clt.EliminaStringsDuplicadas(lista_inst_variaveis);
                        this.ReordenaListaVariaveis(ref lista_inst_variaveis, lista_inst_variaveis);

                        //this.lstVariaveisIndependentes.Items.Clear();
                        //this.lstVariaveisIndependentes.Items.AddRange(this.lista_indep_variaveis);
                        this.lstVariaveisInstrumentais.Items.Clear();
                        this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                        //for (int i = nselecionado - 1; i >= 0; i--)
                        //    this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item[i]);

                        //this.lstVariaveisDisponiveis.Items.Clear();
                        //this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
                    }
                }
            }
        }

        private void btnRemoveVariavelInstrumental_Click(object sender, EventArgs e)
        {
            if (this.lstVariaveisInstrumentais.SelectedIndex >= 0
                && this.lstVariaveisInstrumentais.SelectedIndex < this.lstVariaveisInstrumentais.Items.Count)
            {
                if (this.lstVariaveisInstrumentais.SelectedIndices.Count == 1)
                {
                    int item = this.lstVariaveisInstrumentais.SelectedIndex;
                    string s = this.lista_inst_variaveis[item];
                    this.DeleteItemFromString(ref this.lista_inst_variaveis, this.lista_inst_variaveis, item);

                    this.lstVariaveisInstrumentais.Items.Clear();
                    this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                    //this.RetornaVariableToDataList(s);
                }
                else
                {
                    if (this.lstVariaveisInstrumentais.SelectedIndices.Count > 1)
                    {
                        int nselected = this.lstVariaveisInstrumentais.SelectedIndices.Count;
                        int[] item = new int[nselected];
                        string[] s = new string[nselected];
                        for (int i = 0; i < nselected; i++)
                        {
                            item[i] = this.lstVariaveisInstrumentais.SelectedIndices[i];
                            s[i] = this.lista_inst_variaveis[item[i]];
                        }

                        for (int i = nselected - 1; i >= 0; i--)
                        {
                            this.DeleteItemFromString(ref this.lista_inst_variaveis, this.lista_inst_variaveis, item[i]);
                        }

                        this.lstVariaveisInstrumentais.Items.Clear();
                        this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                        //for (int i = 0; i < nselected; i++)
                        //{
                        //    this.RetornaVariableToDataList(s[i]);
                        //}
                    }
                }
            }
        }

        private void btnAddAllVariaveisInstrumentais_Click(object sender, EventArgs e)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

            int nvars = this.listavariaveis.GetLength(0);
            int item = 0;
            string s = "";
            string[] aux = new string[0];
            int nobs = 0;

            this.lista_inst_variaveis = new string[nvars];
            for (int i = 0; i < nvars; i++) this.lista_inst_variaveis[i] = this.listavariaveis[i];

            lista_inst_variaveis = clt.EliminaStringsDuplicadas(lista_inst_variaveis);
            this.ReordenaListaVariaveis(ref lista_inst_variaveis, lista_inst_variaveis);

            this.lstVariaveisInstrumentais.Items.Clear();
            this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

            /*
            for (int i = 0; i < nvars; i++)
            {
                item = 0;
                s = this.listavariaveis[item];
                aux = this.lista_inst_variaveis;
                nobs = this.lista_inst_variaveis.GetLength(0) + 1;
                this.lista_inst_variaveis = new string[nobs];
                for (int k = 0; k < nobs - 1; k++)
                {
                    this.lista_inst_variaveis[k] = aux[k];
                }
                this.lista_inst_variaveis[nobs - 1] = s;

                this.lstVariaveisInstrumentais.Items.Clear();
                this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                this.DeleteItemFromString(ref this.listavariaveis, this.listavariaveis, item);
                this.lstVariaveisDisponiveis.Items.Clear();
                this.lstVariaveisDisponiveis.Items.AddRange(this.listavariaveis);
            }
            */
        }

        private void btnRemoveAllVariaveisInstrumentais_Click(object sender, EventArgs e)
        {
            this.RemoveAllVariaveisInstrumentais();
        }

        private void RemoveAllVariaveisInstrumentais()
        {
            int item = 0;
            string s = "";
            int nvars = this.lstVariaveisInstrumentais.Items.Count;

            for (int i = 0; i < nvars; i++)
            {
                item = 0;
                s = this.lista_inst_variaveis[item];
                this.DeleteItemFromString(ref this.lista_inst_variaveis, this.lista_inst_variaveis, item);

                this.lstVariaveisInstrumentais.Items.Clear();
                this.lstVariaveisInstrumentais.Items.AddRange(this.lista_inst_variaveis);

                //this.RetornaVariableToDataList(s);
            }
        }

        #endregion
    }
}
