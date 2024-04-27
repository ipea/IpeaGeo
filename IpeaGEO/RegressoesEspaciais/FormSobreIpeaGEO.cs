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
    public partial class FormSobreIpeaGEO : Form
    {
        public FormSobreIpeaGEO()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSobreIpeaGEO_Load(object sender, EventArgs e)
        {
            richTextBox1.DetectUrls = true;
            
            string
            texto = "                                       IpeaGEO\n";
            texto += "                            Versão 2.1.15_04_17, 17 Abril 2015\n\n";
            texto += "                               <http:www.ipea.gov.br>\n\n";

            texto += "           Copyright (C) 2010-2013 Instituto de Pesquisa Econômica Aplicada, \n\n";

            texto += "Este programa é um software livre desenvolvido pela Assessoria de Métodos\n";
            texto += "Quantitativos (ASMEQ) da Diretoria de Estudos Regionais, Urbanos e Ambientais (DIRUR)\n";
            texto += "do Instituto de Pesquisa Econômica e Aplicada - Ipea.\n\n";

            texto += "Você pode redistribuí-lo e/ou modificá-lo dentro dos termos da Licença Pública\n";
            texto += "Geral da GNU publicada pela Fundação do Software Livre (FSF), versão 3.\n\n";

            texto += "Este programa é distribuído na esperança de que possa ser útil, no entanto não\n"; 
            texto += "oferece NENHUMA GARANTIA. Este programa também não oferece garantia implícita de\n";
            texto += "ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. \n\n";

            texto += "Para maiores detalhes acesse a Licença Pública Geral Menor GNU através do caminho: ";
            texto += "                              Ajuda -> Licença.\n\n\n";


            texto += "O IpeaGEO também utiliza os seguintes componentes:\n\n";
            texto += "Random Project - Site: http://www.codeproject.com/Articles/15102/NET-random-number-generators-and-distributions\n";
            texto += "NetTopology - Site: https://code.google.com/p/nettopologysuite/\n";
            texto += "ZedGraph - Site: http://sourceforge.net/projects/zedgraph/\n";
            texto += "XPTabControlCS - Site: https://github.com/tmbx/csutils/tree/master/XPTabControlCS\n";
            texto += "SharpMap - Site: http://sharpmap.codeplex.com/\n";
            texto += "ITextSharp - Site: http://itextpdf.com/\n";
            texto += "Combinatorial - Site: http://www.codeproject.com/Articles/2781/Combinatorial-algorithms-in-C\n\n\n";


            texto += "Email para contato: ipeageo@ipea.gov.br\n\n";

            texto += "Responsáveis técnicos: \n\n";

            texto += "Alexandre Xavier Ywata de Carvalho, Ph.D.\n";
            texto += "Pedro Henrique Melo Albuquerque, D.Sc.\n\n";
            

            texto += "Equipe de desenvolvimento: \n\n";

            texto += "Alex Rodrigues do Nascimento \n";
            texto += "Camilo Rey Laureto  \n";
            texto += "Cauê de Castro Dobbin  \n";
            texto += "Demerson André Polli  \n";
            texto += "Fabio Augusto Scalet Medina  \n";
            texto += "Gabriela Drummond Marques da Silva  \n";
            texto += "Gilberto Rezende de Almeida Júnior  \n";
            texto += "Guilherme Costa Chadud Moreira  \n";
            texto += "Gustavo Gomes Basso  \n";
            texto += "Igor Ferreira do Nascimento  \n";
            texto += "Luis Felipe Biato de Carvalho \n";
            texto += "Luiz Felipe Dantas Guimarães  \n";
            texto += "Marcius Correia Lima Filho  \n";
            texto += "Marina Garcia Pena  \n";
            texto += "Rafael Dantas Guimarães \n";


            this.richTextBox1.Text = texto;
        }
    }
}
