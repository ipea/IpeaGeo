using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace IpeaGEO
{
    class clsReport
    {

        //TODO: COLOCAR O LOGO DO IPEA NO CODIGO AO INVES DE OBJETO

        public string MapaTematicoRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavel)
        {

                string strHTML=@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>"+
                @"<html xmlns='http://www.w3.org/1999/xhtml'>"+
                @"<head>"+
                @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />"+
                @"<title>Relatório</title>"+
                @"</head>"+
                @"<body>"+
                @"<p align='center'>&nbsp;</p>" +
                @"<p align='center'>&nbsp;</p>" +
                @"<p align='center'>&nbsp;</p>" +
                @"<p align='center'>&nbsp;</p>" +
                @"<h1 align='center'>IpeaGEO 1.0.0 </h1>"+
                @"<p align='center'>&nbsp;</p>"+
                @"<table width='829' border='1' align='center'>"+
                @"  <caption>"+
                @"    <strong>Informações gerais </strong>"+
                @"  </caption>"+
                @"  <tr>"+
                @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>"+
                @"    <td width='615'>"+numPoligonos.ToString()+"</td>"+
                @"  </tr>"+
                @"  <tr>"+
                @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>"+
                @"    <td>"+strMapa+"</td>"+
                @"  </tr>"+
                @"  <tr>"+
                @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>"+
                @"    <td>"+strBase+"</td>"+
                @"  </tr>"+
                @"  <tr>"+
                @"    <td><strong>M&eacute;todo</strong></td>"+
                @"    <td>"+strMetodo+"</td>"+
                @"  </tr>"+
                @"  <tr>" +
                @"    <td><strong>Vari&aacute;vel</strong></td>" +
                @"    <td>" + strVariavel + "</td>" +
                @"  </tr>" +
                @"</table>"+
                @"<p align='center'>&nbsp;</p>"+

                @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
                @"<div align='center'>"+
                @"  <table width='489' border='1'>"+
                @"    <caption>"+
                @"      <strong>Metodologia:"+strMetodo+"</strong>"+
                @"    </caption>"+
                @"    <tr>"+
                @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>"+
                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>"+
                @"      <td width='361'><div align='center'><strong>Intervalo</strong></div></td>"+
                @"    </tr>";

                strHTML +=
                @"    <tr>" +
                @"      <td>Classe 0 </td>" +
                @"      <td bgcolor='" + strCores[0] + "'>&nbsp;</td>" +
                @"      <td><div align='center'> Menor que " + strClasses[0].ToString() + "</div></td>" +
                @"    </tr>";
                for(int i=1;i<strClasses.Length-1;i++)
                {
                    strHTML+=
                    @"    <tr>"+
                    @"      <td>Classe"+i.ToString()+"</td>"+
                    @"      <td bgcolor='"+strCores[i]+"'>&nbsp;</td>"+
                    @"      <td><div align='center'>" + strClasses[i - 1].ToString() + " --| " + strClasses[i].ToString() + "</div></td>" +
                    @"    </tr>";
                }
                strHTML +=
                @"    <tr>" +
                @"      <td>Classe"+Convert.ToString(strClasses.Length - 1)+ "</td>" +
                @"      <td bgcolor='" + strCores[strClasses.Length - 1] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[strClasses.Length - 2].ToString() + " --| " + strClasses[strClasses.Length - 1].ToString() + "</div></td>" +
                @"    </tr>";

                strHTML+=@"  </table>"+
                @"</div>"+
                @"<p align='left'>&nbsp;</p>"+
                @"</body>"+
                @"</html>";

                return (strHTML);
        }
        public string EstatisticaScanRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavelBase, string strVariavelEvento, string strSimulacoes, string strPontosGrid, string strRaioMax, string strRarioMin, string strProp, string strHistograma)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 1.0.0 </h1>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  </caption>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>M&eacute;todo</strong></td>" +
            @"    <td>" + strMetodo + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;vel Base</strong></td>" +
            @"    <td>" + strVariavelBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;vel Evento</strong></td>" +
            @"    <td>" + strVariavelEvento + "</td>" +
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'>&nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='489' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"      <td width='361'><div align='center'><strong>P-valor</strong></div></td>" +
            @"    </tr>";

            for (int i = 0; i < strClasses.Length; i++)
            {
                strHTML +=
                @"    <tr>" +
                @"      <td>Classe" + i.ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i+1] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i].ToString().Substring(0,6) + "</div></td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +

            @"<p align='center'><img src='" + strHistograma + "' width='824' height='613' /></p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<div align='center'>" +
            @"  <table width='495' border='1'>" +
            @"    <caption>" +
            @"      <strong>Monte Carlo      </strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='185'><strong>Simula&ccedil;&otilde;es</strong></td>" +
            @"      <td width='294'>"+strSimulacoes+"</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Pontos grid</strong> </td>" +
            @"      <td>"+strPontosGrid+"</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Raio m&aacute;ximo (Km) </strong></td>" +
            @"      <td>"+strRaioMax+"</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Raio m&iacute;nimo (Km) </strong></td>" +
            @"      <td>"+strRarioMin+"</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Propor&ccedil;&atilde;o m&aacute;xima (Base) </strong></td>" +
            @"      <td>"+strProp+"</td>" +
            @"    </tr>" +
            @"  </table>" +
            @"</div>" +
            @"<p align='center'>Obs: Utilizou-se a distribui&ccedil;&atilde;o generalizada de valores extremos. </p>" +

            @"</body>" +
            @"</html>";

            return (strHTML);
        }


        public string IndicesDeDependenciaEspacialGlobais(string strBase, string strMapa, int numPoligonos, string strTipoVizinhanca, string[] strVariaveisSelecionadasQuantitativas, string strVariavelPopulacao, int intNumeroDeSimulacoes, string strTipoDoPeso, double[] dblIndiceGeary, double[] dblpValorIndiceGeary, double[] dblIndiceGetis, double[] dblpValorIndiceGetis, double[] dblIndiceMoran, double[] dblpValorIndiceMoran, double[] dblIndiceMoranSimples, double[] dblpValorIndiceMoranSimples, double[] dblIndiceRogerson, double[] dblpValorIndiceRogerson, double[] dblIndiceTango, double[] dblpValorIndiceTango)
        {
            int iFor = strVariaveisSelecionadasQuantitativas.Length;
            if (dblIndiceRogerson.Length  != 0 || dblIndiceTango.Length != 0) iFor--;
                       
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 1.0.0 </h1>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  </caption>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Tipo de vizinhan&ccedil;a</strong></td>" +
            @"    <td>" + strTipoVizinhanca + "</td>" +
            @"  </tr>";

            if(strVariavelPopulacao!="")
            {
                strHTML += @"  <tr>" +
                @"    <td><strong>Variavel populacional</strong></td>" +
                @"    <td>" + strVariavelPopulacao + "</td>" +
                @"  </tr>";
            }

            if (dblIndiceMoran[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Moran</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceMoran[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceMoran[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }
            if (dblIndiceMoranSimples[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Moran Simples</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceMoranSimples[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceMoranSimples[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }

            if (dblIndiceGeary[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Geary</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceGeary[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceGeary[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }
            if (dblIndiceGetis[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Getis-Ord Gi*</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML += @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceGetis[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceGetis[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }
            if (dblIndiceRogerson.Length != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Rogerson</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML += @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceRogerson[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceRogerson[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }
            if (dblIndiceTango.Length != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Tango</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    strHTML += @"  <tr>" +
                    @"    <td width='200'><strong>" + strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dblIndiceTango[i].ToString() + "</td>" +
                    @"    <td width='200'>P-valor: " + dblpValorIndiceTango[i].ToString() + "</td>" +
                    @"  </tr>";
                }
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"</body>" +
            @"</html>";

            return (strHTML);

        }

        public string AnaliseDeConglomerados(string strBase, string strMapa, string strMapaImagem,string numClusters,string iMinkowsky ,int numPoligonos, string strMetodo,string strDistancia,bool blEspacial, string[] strVariaveisSelecionadas,string[] strVetorPesos ,string[] strCores)
        {
            string strVariaveis = "";
            string strPesos = "";

            for (int i = 0; i < strVariaveisSelecionadas.Length - 1; i++) strVariaveis += strVariaveisSelecionadas[i] + ", ";
            strVariaveis += strVariaveisSelecionadas[strVariaveisSelecionadas.Length - 1];
            for (int i = 0; i < strVetorPesos.Length - 1; i++) strPesos += strVetorPesos[i] + ", ";
            strPesos += strVetorPesos[strVetorPesos.Length - 1];


            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 1.0.0 </h1>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  </caption>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>M&eacute;todo</strong></td>" +
            @"    <td>" + strMetodo + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Dist&acirc;ncia</strong></td>" +
            @"    <td>" + strDistancia + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;veis</strong></td>" +
            @"    <td>" + strVariaveis + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Pesos das vari&aacute;veis</strong></td>" +
            @"    <td>" + strPesos + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>N&uacute;mero de conglomerados</strong></td>" +
            @"    <td>" + numClusters + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Fator Minkowsky</strong></td>" +
            @"    <td>" + iMinkowsky + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Conglomerado espacial</strong></td>" +
            @"    <td>" + blEspacial.ToString() + "</td>" +
            @"  </tr>" +
            
            @"</table>" +
            @"<p align='center'>&nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='200' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='40'><div align='center'><strong>Conglomerado</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"    </tr>";

            for (int i = 0; i < strCores.Length; i++)
            {
                strHTML +=
                @"    <tr>" +
                @"      <td>Conglomerado " + i.ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string AnaliseDeConglomeradosEspacial(string strBase, string strMapa, string strMapaImagem, string numClusters, string iMinkowsky, int numPoligonos, string strMetodo, string strDistancia,string strEML, bool blEspacial, string[] strVariaveisSelecionadas, string[] strCores, string strVizinhanca, string strNumeroConglomerados)
        {
            string strVariaveis = "";

            for (int i = 0; i < strVariaveisSelecionadas.Length - 1; i++) strVariaveis += strVariaveisSelecionadas[i] + ", ";
            strVariaveis += strVariaveisSelecionadas[strVariaveisSelecionadas.Length - 1];


            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 1.0.0 </h1>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  </caption>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Tipo de vizinhan&ccedil;a</strong></td>" +
            @"    <td>" + strVizinhanca + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>M&eacute;todo</strong></td>" +
            @"    <td>" + strMetodo + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Dist&acirc;ncia</strong></td>" +
            @"    <td>" + strDistancia + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;veis</strong></td>" +
            @"    <td>" + strVariaveis + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>N&uacute;mero de conglomerados</strong></td>" +
            @"    <td>" + numClusters + "</td>" +
            @"  </tr>";
            if(strDistancia=="Minkowsky")
            {
                strHTML += @"  <tr>" +
                @"    <td><strong>Fator Minkowsky</strong></td>" +
                @"    <td>" + iMinkowsky + "</td>" +
                @"  </tr>";
             }
            if (strMetodo == "EML")
            {
                strHTML += @"  <tr>" +
                @"    <td><strong>Fator EML</strong></td>" +
                @"    <td>" + strEML + "</td>" +
                @"  </tr>";       
            }
            strHTML += @"  <tr>" +
            @"    <td><strong>Conglomerado espacial</strong></td>" +
            @"    <td>" + blEspacial.ToString() + "</td>" +
            @"  </tr>"+
            @"</table>" +
            @"<p align='center'>&nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='200' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='40'><div align='center'><strong>Conglomerado</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"    </tr>";

            for (int i = 0; i < strCores.Length; i++)
            {
                strHTML +=
                @"    <tr>" +
                @"      <td>Conglomerado " + i.ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +
            @"<p align='center'><img src='" + strNumeroConglomerados + "' width='824' height='613' /></p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string IndiceDeDependenciaLocal(string strBase, string strMapa,int numPoligonos, string[] strMapaImagemLisa, string[] strMapaImagemGetis, string[] strMapaImagemEscore, string[] strEspalha, string strTipoVizinhnaca, string strTipoCorrecao, string strConfiabilidade, string strPopulacao, string[] strVariaveisSelecionadas, string[] strCores)
        {
            string strVariaveis = "";

            for (int i = 0; i < strVariaveisSelecionadas.Length - 1; i++) strVariaveis += strVariaveisSelecionadas[i] + ", ";
            strVariaveis += strVariaveisSelecionadas[strVariaveisSelecionadas.Length - 1];
            string[] strClasse = new string[5] { "Não significativo", "Alto-Alto", "Alto-Baixo", "Baixo-Baixo", "Baixo-Alto" };

            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 1.0.0 </h1>" +
            @"<p align='center'>&nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  </caption>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Tipo de vizinhan&ccedil;a</strong></td>" +
            @"    <td>" + strTipoVizinhnaca + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;veis</strong></td>" +
            @"    <td>" + strVariaveis + "</td>" +
            @"  </tr>"+
            @"  <tr>" +
            @"    <td width='198'><strong>Nivel de confiabilidade: </strong></td>" +
            @"    <td width='615'>" + strConfiabilidade + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td width='198'><strong>M&eacute;todo de corre&ccedil;ão: </strong></td>" +
            @"    <td width='615'>" + strTipoCorrecao + "</td>" +
            @"  </tr>"
            ;
            if (strPopulacao != null)
            {
                strHTML += 
                @"  <tr>" +
                @"    <td width='198'><strong>População: </strong></td>" +
                @"    <td width='615'>" + strPopulacao + "</td>" +
                @"  </tr>";
            }
            strHTML += @"</table>" + @"<p align='center'>&nbsp;</p>";

            if (strMapaImagemLisa[0] != null)
            {
                for (int m = 0; m < strMapaImagemLisa.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + strMapaImagemLisa[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: LISA </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"  </table>" + @"</div>" + @"<p align='left'>&nbsp;</p>"+ @"<p align='center'>&nbsp;</p>"+@"  <caption>" +
                                @"    <strong>Espalhamento de Moran: " + strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + strEspalha[m] + "' width='824' height='613' /></p>" + @"</table>" + @"<p align='center'>&nbsp;</p>"+@"<p align='center'>&nbsp;</p>";
                }
            }
            if (strMapaImagemGetis[0] !=null)
            {
                for (int m = 0; m < strMapaImagemGetis.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + strMapaImagemGetis[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: Getis-Ord Gi* </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"</table>" + @"<p align='center'>&nbsp;</p>";
                    strHTML += @"<p align='center'>&nbsp;</p>";
                }
            }
            if (strMapaImagemEscore[0] != null)
            {
                for (int m = 0; m < strMapaImagemEscore.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + strMapaImagemEscore[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: Escore </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"</table>" + @"<p align='center'>&nbsp;</p>";
                    strHTML += @"<p align='center'>&nbsp;</p>";
                }
            }

            strHTML += @"<p align='center'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }
    }
}
