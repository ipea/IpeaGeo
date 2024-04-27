using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormIndicadoresDependenciaEspacial : Form
    {
        public FormIndicadoresDependenciaEspacial()
        {
            InitializeComponent();
        }

        private void FormIndicadoresDependenciaEspacial_Load(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Remove(tabPage2);

            if (tabControl1.TabPages.Contains(tabPage3))
                this.tabControl1.TabPages.Remove(tabPage3);

            if (tabControl1.TabPages.Contains(tabPage4))
                this.tabControl1.TabPages.Remove(tabPage4);
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                ExecutarAnalises();
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region executar funções de cálculo das medidas de dependência espacial

        private void ExecutarAnalises()
        {

        }

        #endregion
    }
}
