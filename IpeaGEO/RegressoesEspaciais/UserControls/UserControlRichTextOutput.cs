using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.RegressoesEspaciais.UserControls
{
    public partial class UserControlRichTextOutput : UserControl
    {
        public UserControlRichTextOutput()
        {
            InitializeComponent();
        }

        public string Texto
        {
            get
            {
                return this.richTextBoxResultadosEstimacao.Text;
            }
            set
            {
            	this.richTextBoxResultadosEstimacao.Text = value;
            }
        }

        private void salvarTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxResultadosEstimacao.SelectAll();
                Application.DoEvents();

                //string v = richTextBoxResultadosEstimacao.SelectedText;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarParaTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxResultadosEstimacao.SelectAll();

                string texto = richTextBoxResultadosEstimacao.SelectedText;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarSeleçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = richTextBoxResultadosEstimacao.SelectedText;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tamanhoDaFonteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormTamanhoFonte frm = new FormTamanhoFonte();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.richTextBoxResultadosEstimacao.SelectAll();
                    this.richTextBoxResultadosEstimacao.SelectionFont = new Font("Courier New", frm.TamanhoFonte);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
