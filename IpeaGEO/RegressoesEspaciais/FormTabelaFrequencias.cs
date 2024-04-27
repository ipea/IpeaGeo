﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormTabelaFrequencias : Form
    {
        public FormTabelaFrequencias()
        {
            InitializeComponent();
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDados
        {
            set
            {
                m_dt_dados = value;

                clsUtilTools clt = new clsUtilTools();

                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                userControlSelecaoVariaveis1.ZeraControle();
                userControlSelecaoVariaveis1.VariaveisList = variaveis;
                userControlSelecaoVariaveis1.VariaveisDB = variaveis;
            }
            get { return m_dt_dados; }
        }

        private void FormTabelaFrequencias_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl1.TabPages.Contains(this.tabPage2))
                    this.tabControl1.TabPages.Remove(this.tabPage2);

                userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis disponíveis na tabela";
                userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0) <= 0)
                    throw new Exception("Selecione pelo menos uma variável.");

                BLEstatisticasDescritivas ble = new BLEstatisticasDescritivas();
                ble.TabelaDados = this.m_dt_dados;
                ble.VariaveisCategoricas = this.userControlSelecaoVariaveis1.VariaveisIndependentes;
                ble.NumDecimais = Convert.ToInt32(this.nudNumCasasDecimais.Value);

                ble.GeraTabelaFrequencias();

                userControlRichTextOutput1.Texto = ble.OutputText;

                if (!this.tabControl1.TabPages.Contains(this.tabPage2))
                {
                    this.tabControl1.TabPages.Add(this.tabPage2);
                }
                this.tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
