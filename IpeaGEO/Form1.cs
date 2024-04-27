using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //label1.Text = "                    IpeaGEO\nAnálises espaciais geográficas 1.0.1";
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Alt && e.Control)
            {
                if (e.KeyValue == ((int)Keys.F10))
                {
                    //MessageBox.Show(e.KeyData.ToString());

                    //ZedGraph2.Form1 space = new ZedGraph2.Form1();
                    //space.MdiParent = this.MdiParent;                    
                    //space.Show();

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
