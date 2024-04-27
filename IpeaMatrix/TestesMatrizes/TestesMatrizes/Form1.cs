using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IpeaMatrix;

namespace TestesMatrizes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Matrix a = new Matrix(2, 2);
                Matrix b = new Matrix(2, 2);

                a[0, 0] = -1.0;

                Matrix c = -a;

                a[3, 6] = 123.0;

                Matrix w = 2.0*Mfunc.Ident(4);

                Matrix vinv = Mfunc.Inv(w);

                w[0, 0] = 1;
                w[0, 1] = 11;
                w[0, 2] = 5;
                w[1, 0] = 2;
                w[1, 1] = 12;
                w[1, 2] = 2;
                w[2, 0] = 3;
                w[2, 1] = 13;
                w[2, 2] = 20;

                vinv = Mfunc.Inv(w);
                Matrix vinv1 = Mfunc.GaussJordanInv(w);

                Matrix diff = vinv - vinv1;
                double ndiff = Mfunc.Norm(diff);

                double d = Mfunc.Det(w);

                c = vinv1 * w;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
