using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace IpeaGeo
{
    class clsReport
    {
        public struct DependenciaGlobal
        {
            public string strBase, strMapa, strTipoVizinhanca, strVariavelPopulacao, 
                strTipoDoPeso;
            public string[] strVariaveisSelecionadasQuantitativas;
            public double[] dblIndiceGeary, dblpValorIndiceGeary, dblIndiceGetis, 
                dblpValorIndiceGetis, dblIndiceMoran, dblpValorIndiceMoran, 
                dblIndiceMoranSimples, dblpValorIndiceMoranSimples, 
                dblIndiceRogerson, dblpValorIndiceRogerson, dblIndiceTango, 
                dblpValorIndiceTango;
            public int numPoligonos, intNumeroDeSimulacoes;
        }

        public struct DependenciaLocal
        {
            public string strBase, strMapa, strTipoVizinhnaca, strTipoCorrecao,
                strConfiabilidade, strPopulacao;
            public string[] strMapaImagemLisa, strMapaImagemGetis, strMapaImagemGetis2,
                strMapaImagemEscore, strEspalha, strVariaveisSelecionadas, strCores;
            public int numPoligonos;
        }

        //TODO: COLOCAR O LOGO DO IPEA NO CODIGO AO INVES DE OBJETO

        public string MapaTematicoRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavel)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +
            @"<body>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            @"    <td><strong>Vari&aacute;vel</strong></td>" +
            @"    <td>" + strVariavel + "</td>" +
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='489' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"      <td width='361'><div align='center'><strong>Intervalo</strong></div></td>" +
            @"    </tr>";

            int size = 0;
            int tamanho = strClasses[0].ToString().IndexOf(",");
            int tamanhob = strClasses[0].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[0].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[0].ToString().Length;
            }

            strHTML +=
            @"    <tr>" +
            @"      <td>Classe 0 </td>" +
            @"      <td bgcolor='" + strCores[0] + "'>&nbsp;</td>" +
            @"      <td><div align='center'> Menor que " + strClasses[0].ToString().Substring(0, size) + "</div></td>" +
            @"    </tr>";

            for (int i = 1; i < strClasses.Length - 1; i++)
            {
                //Definindo tamanhos para as classes
                tamanho = strClasses[i - 1].ToString().IndexOf(",");
                tamanhob = strClasses[i - 1].ToString().IndexOf(".");

                if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
                {
                    if (tamanho == -1) tamanho = tamanhob;

                    size = tamanho + 7;
                    int total = strClasses[i - 1].ToString().Length;
                    if (size > total) size = total;
                }
                else
                {
                    size = strClasses[i - 1].ToString().Length;
                }

                //Definindo tamanhos para as classes
                int tamanho2 = strClasses[i].ToString().IndexOf(",");
                int tamanhob2 = strClasses[i].ToString().IndexOf(".");
                int size2 = 0;

                if (tamanho2 != -1 || tamanhob2 != -1) //Caso haja vírgulas
                {
                    if (tamanho2 == -1) tamanho2 = tamanhob2;

                    size2 = tamanho2 + 7;
                    int total = strClasses[i].ToString().Length;
                    if (size2 > total) size2 = total;
                }
                else
                {
                    size2 = strClasses[i].ToString().Length;
                }

                strHTML +=
                @"    <tr>" +
                @"      <td>Classe" + i.ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i - 1].ToString().Substring(0, size) + " --| " + strClasses[i].ToString().Substring(0, size2) + "</div></td>" +
                @"    </tr>";
            }

            //Definindo tamanhos para as classes
            tamanho = strClasses[strClasses.Length - 2].ToString().IndexOf(",");
            tamanhob = strClasses[strClasses.Length - 2].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[strClasses.Length - 2].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[strClasses.Length - 2].ToString().Length;
            }

            //Definindo tamanhos para as classes
            int tamanho3 = strClasses[strClasses.Length - 1].ToString().IndexOf(",");
            int tamanhob3 = strClasses[strClasses.Length - 1].ToString().IndexOf(".");
            int size3 = 0;

            if (tamanho3 != -1 || tamanhob3 != -1) //Caso haja vírgulas
            {
                if (tamanho3 == -1) tamanho3 = tamanhob3;

                size3 = tamanho3 + 7;
                int total = strClasses[strClasses.Length - 1].ToString().Length;
                if (size3 > total) size3 = total;
            }
            else
            {
                size3 = strClasses[strClasses.Length - 1].ToString().Length;
            }

            strHTML +=
            @"    <tr>" +
            @"      <td>Classe" + Convert.ToString(strClasses.Length - 1) + "</td>" +
            @"      <td bgcolor='" + strCores[strClasses.Length - 1] + "'>&nbsp;</td>" +
            @"      <td><div align='center'>" + strClasses[strClasses.Length - 2].ToString().Substring(0, size) + " --| " + strClasses[strClasses.Length - 1].ToString().Substring(0, size3) + "</div></td>" +
            @"    </tr>";

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string MapaTematicoRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavel, string[] legenda)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +
            @"<body>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            @"    <td><strong>Vari&aacute;vel</strong></td>" +
            @"    <td>" + strVariavel + "</td>" +
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='489' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"      <td width='361'><div align='center'><strong>Intervalo</strong></div></td>" +
            @"    </tr>";

            int diff = 0;
            int size = 0;
            int tamanho = strClasses[0].ToString().IndexOf(",");
            int tamanhob = strClasses[0].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[0].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[0].ToString().Length;
            }

            strHTML +=
            @"    <tr>" +
            @"      <td>" + legenda[0].ToString() + "</td>" +
            @"      <td bgcolor='" + strCores[0] + "'>&nbsp;</td>" +
            @"      <td><div align='center'> Menor que " + strClasses[0].ToString().Substring(0, size) + "</div></td>" +
            @"    </tr>";

            for (int i = 1; i < strClasses.Length - 1; i++)
            {
                //Definindo tamanhos para as classes
                tamanho = strClasses[i - 1].ToString().IndexOf(",");
                tamanhob = strClasses[i - 1].ToString().IndexOf(".");

                if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
                {
                    if (tamanho == -1) tamanho = tamanhob;

                    size = tamanho + 7;
                    int total = strClasses[i - 1].ToString().Length;
                    if (size > total) size = total;
                }
                else
                {
                    size = strClasses[i - 1].ToString().Length;
                }

                //Definindo tamanhos para as classes
                int tamanho2 = strClasses[i].ToString().IndexOf(",");
                int tamanhob2 = strClasses[i].ToString().IndexOf(".");
                int size2 = 0;

                if (tamanho2 != -1 || tamanhob2 != -1) //Caso haja vírgulas
                {
                    if (tamanho2 == -1) tamanho2 = tamanhob2;

                    size2 = tamanho2 + 7;
                    int total = strClasses[i].ToString().Length;
                    if (size2 > total) size2 = total;
                }
                else
                {
                    size2 = strClasses[i].ToString().Length;
                }

                strHTML +=
                @"    <tr>" +
                @"      <td>" + legenda[i].ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i - 1].ToString().Substring(0, size) + " --| " + strClasses[i].ToString().Substring(0, size2) + "</div></td>" +
                @"    </tr>";
            }

            //Definindo tamanhos para as classes
            tamanho = strClasses[strClasses.Length - 2].ToString().IndexOf(",");
            tamanhob = strClasses[strClasses.Length - 2].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[strClasses.Length - 2].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[strClasses.Length - 2].ToString().Length;
            }

            //Definindo tamanhos para as classes
            int tamanho3 = strClasses[strClasses.Length - 1].ToString().IndexOf(",");
            int tamanhob3 = strClasses[strClasses.Length - 1].ToString().IndexOf(".");
            int size3 = 0;

            if (tamanho3 != -1 || tamanhob3 != -1) //Caso haja vírgulas
            {
                if (tamanho3 == -1) tamanho3 = tamanhob3;

                size3 = tamanho3 + 7;
                int total = strClasses[strClasses.Length - 1].ToString().Length;
                if (size3 > total) size3 = total;
            }
            else
            {
                size3 = strClasses[strClasses.Length - 1].ToString().Length;
            }

            strHTML +=
            @"    <tr>" +
            @"      <td>" + legenda[legenda.Length - 1].ToString() + "</td>" +
            @"      <td bgcolor='" + strCores[strClasses.Length - 1] + "'>&nbsp;</td>" +
            @"      <td><div align='center'>" + strClasses[strClasses.Length - 2].ToString().Substring(0, size) + " --| " + strClasses[strClasses.Length - 1].ToString().Substring(0, size3) + "</div></td>" +
            @"    </tr>";

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }     
      
        public string MapaTematicoQualiRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, string[] strClasses, string[] strCores, string strVariavel, string[] legenda)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +
            @"<body>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            @"    <td><strong>Vari&aacute;vel</strong></td>" +
            @"    <td>" + strVariavel + "</td>" +
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='489' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"      <td width='361'><div align='center'><strong>Categorias</strong></div></td>" +
            @"    </tr>";

            int diff = 0;
            int size = 0;
            int tamanho = strClasses[0].ToString().IndexOf(",");
            int tamanhob = strClasses[0].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[0].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[0].ToString().Length;
            }

            for (int i = 0; i < strClasses.Length ; i++)
            {
                strHTML +=
                @"    <tr>" +
                @"      <td>" + legenda[i].ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i] + "</div></td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string MapaTematicoQualiRelatorio(string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, string[] strClasses, string[] strCores, string strVariavel)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +
            @"<body>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            @"    <td><strong>Vari&aacute;vel</strong></td>" +
            @"    <td>" + strVariavel + "</td>" +
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +

            @"<p align='center'><img src='" + strMapaImagem + "' width='824' height='613' /></p>" +
            @"<div align='center'>" +
            @"  <table width='489' border='1'>" +
            @"    <caption>" +
            @"      <strong>Metodologia:" + strMetodo + "</strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='69'><div align='center'><strong>Classe</strong></div></td>" +
            @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
            @"      <td width='361'><div align='center'><strong>Categorias</strong></div></td>" +
            @"    </tr>";

            int diff = 0;
            int size = 0;
            int tamanho = strClasses[0].ToString().IndexOf(",");
            int tamanhob = strClasses[0].ToString().IndexOf(".");

            if (tamanho != -1 || tamanhob != -1) //Caso haja vírgulas
            {
                if (tamanho == -1) tamanho = tamanhob;

                size = tamanho + 7;
                int total = strClasses[0].ToString().Length;
                if (size > total) size = total;
            }
            else
            {
                size = strClasses[0].ToString().Length;
            }

            for (int i = 0; i < strClasses.Length; i++)
            {
                strHTML +=
                @"    <tr>" +
                @"      <td>Classe" + i.ToString() + "</td>" +
                @"      <td bgcolor='" + strCores[i] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i] + "</div></td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string SegregacaoRelatorio(double[,] matrizindices, string[] nomes_variaveis, string[] nomes_indices, string[] nomesX, string[] nomesY, bool multigroup)
        {
            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +
            @"<body>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
                //@"<p align='center'> &nbsp;</p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Indices de Segregacao </strong>" +
            @"  </caption>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <tr>" +
            @"    <td>" + "Vari&aacute;veis" + "</td>";

            if (nomesX == null)
            {
                for (int i = 0; i < nomes_indices.Length; i++)
                {
                    strHTML += @"    <td>" + nomes_indices[i].ToString() + "</td>";
                }
                strHTML += @"  </tr>";
                if (!multigroup)
                {
                    for (int i = 0; i < nomes_variaveis.Length; i++)
                    {
                        strHTML += @"  <tr>" +
                                   @"    <td>" + nomes_variaveis[i].ToString() + "</td>";
                        for (int j = 0; j < nomes_indices.Length; j++)
                        {
                            int tam = matrizindices[i, j].ToString().Length;
                            if (tam > 8) tam = 8;
                            strHTML += @"    <td>" + matrizindices[i, j].ToString().Substring(0, tam) + "</td>";
                        }
                        strHTML += @"  </tr>";
                    }
                }
                else
                {
                    strHTML += @"  <tr>" +
                               @"    <td>" + "Grupo" + "</td>";
                    for (int j = 0; j < nomes_indices.Length; j++)
                    {
                        int tam = matrizindices[0, j].ToString().Length;
                        if (tam > 8) tam = 8;
                        strHTML += @"    <td>" + matrizindices[0, j].ToString().Substring(0, tam) + "</td>";
                    }
                    strHTML += @"  </tr>";
                }
            }
            else
            {
                for (int i = 0; i < nomes_indices.Length; i++)
                {
                    strHTML += @"    <td>" + nomes_indices[i].ToString() + "</td>";

                }
                strHTML += @"  </tr>";

                for (int i = 0; i < nomesX.Length; i++)
                {
                    strHTML += @"  <tr>" +
                               @"    <td>" + nomesX[i].ToString() + " - " + nomesY[i].ToString() + "</td>";
                    for (int j = 0; j < nomes_indices.Length; j++)
                    {
                        int tam = matrizindices[i, j].ToString().Length;
                        if (tam > 8) tam = 8;
                        strHTML += @"    <td>" + matrizindices[i, j].ToString().Substring(0, tam) + "</td>";
                    }
                    strHTML += @"  </tr>";
                }
            }

            strHTML += @"</table>";

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
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            @"<p align='center'> &nbsp;</p>" +

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
                @"      <td bgcolor='" + strCores[i + 1] + "'>&nbsp;</td>" +
                @"      <td><div align='center'>" + strClasses[i].ToString().Substring(0, 6) + "</div></td>" +
                @"    </tr>";
            }

            strHTML += @"  </table>" +
            @"</div>" +
            @"<p align='left'>&nbsp;</p>" +

            @"<p align='center'><img src='" + strHistograma + "' width='824' height='613' /></p>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<div align='center'>" +
            @"  <table width='495' border='1'>" +
            @"    <caption>" +
            @"      <strong>Monte Carlo      </strong>" +
            @"    </caption>" +
            @"    <tr>" +
            @"      <td width='185'><strong>Simula&ccedil;&otilde;es</strong></td>" +
            @"      <td width='294'>" + strSimulacoes + "</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Pontos grid</strong> </td>" +
            @"      <td>" + strPontosGrid + "</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Raio m&aacute;ximo (Km) </strong></td>" +
            @"      <td>" + strRaioMax + "</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Raio m&iacute;nimo (Km) </strong></td>" +
            @"      <td>" + strRarioMin + "</td>" +
            @"    </tr>" +
            @"    <tr>" +
            @"      <td><strong>Propor&ccedil;&atilde;o m&aacute;xima (Base) </strong></td>" +
            @"      <td>" + strProp + "</td>" +
            @"    </tr>" +
            @"  </table>" +
            @"</div>" +
            @"<p align='center'>Obs: Utilizou-se a distribui&ccedil;&atilde;o generalizada de valores extremos. </p>" +

            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string IndicesDeDependenciaEspacialGlobais(DependenciaGlobal dg)
        {
            int iFor = dg.strVariaveisSelecionadasQuantitativas.Length;
            if (dg.dblIndiceRogerson[0] > 999 && dg.dblIndiceTango[0] > 999)
            {
                dg.strVariavelPopulacao = "";
            }

            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  </caption>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + dg.numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + dg.strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + dg.strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Tipo de vizinhan&ccedil;a</strong></td>" +
            @"    <td>" + dg.strTipoVizinhanca + "</td>" +
            @"  </tr>";

            if (dg.strVariavelPopulacao != "")
            {
                strHTML += @"  <tr>" +
                @"    <td><strong>Variavel populacional</strong></td>" +
                @"    <td>" + dg.strVariavelPopulacao + "</td>" +
                @"  </tr>";
            }

            bool fez_moran = false;
            bool fez_geary = false;
            bool fez_getis = false;
            bool fez_tango = false;
            bool fez_rogerson = false;

            if (dg.dblIndiceMoran[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Moran</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    fez_moran = true;
                    int tam = dg.dblIndiceMoran[i].ToString().Length;
                    if (tam > 8) tam = 8;
                    int tam2 = dg.dblpValorIndiceMoran[i].ToString().Length;
                    if (tam2 > 8) tam2 = 8;

                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dg.dblIndiceMoran[i].ToString().Substring(0, tam) + "</td>" +
                    @"    <td width='200'>P-valor: " + dg.dblpValorIndiceMoran[i].ToString().Substring(0, tam2) + "</td>" +
                    @"  </tr>";
                }
            }
            
            if (dg.dblIndiceMoranSimples[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Moran Simples</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    fez_moran = true;
                    int tam = dg.dblIndiceMoran[i].ToString().Length;
                    if (tam > 8) tam = 8;
                    int tam2 = dg.dblpValorIndiceMoran[i].ToString().Length;
                    if (tam2 > 8) tam2 = 8;

                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dg.dblIndiceMoranSimples[i].ToString().Substring(0, tam) + "</td>" +
                    @"    <td width='200'>P-valor: " + dg.dblpValorIndiceMoranSimples[i].ToString().Substring(0, tam2) + "</td>" +
                    @"  </tr>";
                }
            }

            if (dg.dblIndiceGeary[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Geary</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    fez_geary = true;
                    int tam = dg.dblIndiceGeary[i].ToString().Length;
                    if (tam > 8) tam = 8;
                    int tam2 = dg.dblpValorIndiceGeary[i].ToString().Length;
                    if (tam2 > 8) tam2 = 8;

                    strHTML +=
                    @"  <tr>" +
                    @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dg.dblIndiceGeary[i].ToString().Substring(0, tam) + "</td>" +
                    @"    <td width='200'>P-valor: " + dg.dblpValorIndiceGeary[i].ToString().Substring(0, tam2) + "</td>" +
                    @"  </tr>";
                }
            }
            if (dg.dblIndiceGetis[0] != 0)
            {
                strHTML += @"<table width='650' border='1' align='center'>" +
                @"  <caption>" +
                @"    <strong>Indice de Getis-Ord Gi</strong>" +
                @"  </caption>";

                for (int i = 0; i < iFor; i++)
                {
                    fez_getis = true;
                    int tam = dg.dblIndiceGetis[i].ToString().Length;
                    if (tam > 8) tam = 8;
                    int tam2 = dg.dblpValorIndiceGetis[i].ToString().Length;
                    if (tam2 > 8) tam2 = 8;

                    strHTML += @"  <tr>" +
                    @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                    @"    <td width='200'>Indice : " + dg.dblIndiceGetis[i].ToString().Substring(0, tam) + "</td>" +
                    @"    <td width='200'>P-valor: " + dg.dblpValorIndiceGetis[i].ToString().Substring(0, tam2) + "</td>" +
                    @"  </tr>";
                }
            }

            if (dg.dblIndiceRogerson[0] != 0)
            {
                if (dg.dblIndiceRogerson[0] != 99999)
                {
                    strHTML += @"<table width='650' border='1' align='center'>" +
                    @"  <caption>" +
                    @"    <strong>Indice de Rogerson</strong>" +
                    @"  </caption>";

                    for (int i = 0; i < iFor; i++)
                    {
                        fez_rogerson = true;
                        int tam = dg.dblIndiceRogerson[i].ToString().Length;
                        if (tam > 8) tam = 8;
                        int tam2 = dg.dblpValorIndiceRogerson[i].ToString().Length;
                        if (tam2 > 8) tam2 = 8;

                        strHTML += @"  <tr>" +
                        @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                        @"    <td width='200'>Indice : " + dg.dblIndiceRogerson[i].ToString().Substring(0, tam) + "</td>" +
                        @"    <td width='200'>P-valor: " + dg.dblpValorIndiceRogerson[i].ToString().ToString().Substring(0, tam2) + "</td>" +
                        @"  </tr>";
                    }
                }
            }

            if (dg.dblIndiceTango[0] != 0)
            {
                if (dg.dblIndiceTango[0] != 99999)
                {
                    strHTML += @"<table width='650' border='1' align='center'>" +
                    @"  <caption>" +
                    @"    <strong>Indice de Tango</strong>" +
                    @"  </caption>";

                    for (int i = 0; i < iFor; i++)
                    {
                        fez_tango = true;
                        int tam = dg.dblIndiceTango[i].ToString().Length;
                        if (tam > 8) tam = 8;
                        int tam2 = dg.dblpValorIndiceTango[i].ToString().Length;
                        if (tam2 > 8) tam2 = 8;

                        strHTML += @"  <tr>" +
                        @"    <td width='200'><strong>" + dg.strVariaveisSelecionadasQuantitativas[i] + ": </strong></td>" +
                        @"    <td width='200'>Indice : " + dg.dblIndiceTango[i].ToString().Substring(0, tam) + "</td>" +
                        @"    <td width='200'>P-valor: " + dg.dblpValorIndiceTango[i].ToString().Substring(0, tam2) + "</td>" +
                        @"  </tr>";
                    }
                }
            }

            strHTML += @"  </table>" +

            @"</div>" +
            @"</body>" +
            @"</html>";

            return (strHTML);

        }

        public string AnaliseDeConglomeradosEspacial(string strBase, string strMapa, string strMapaImagem, string numClusters, string iMinkowsky, int numPoligonos, string strMetodo, string strDistancia, string strEML, bool blEspacial, string[] strVariaveisSelecionadas, string[] strCores, string strVizinhanca, string strNumeroConglomerados)
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
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
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
            
            if (strDistancia == "Minkowsky")
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
            @"  </tr>" +
            @"</table>" +
            @"<p align='center'> &nbsp;</p>" +

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
            @"<p align='center'> &nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }

        public string IndiceDeDependenciaLocal(DependenciaLocal dep)
        {
            string strVariaveis = "";

            for (int i = 0; i < dep.strVariaveisSelecionadas.Length - 1; i++) strVariaveis += dep.strVariaveisSelecionadas[i] + ", ";
            strVariaveis += dep.strVariaveisSelecionadas[dep.strVariaveisSelecionadas.Length - 1];
            string[] strClasse = new string[5] { "Não significativo", "Alto-Alto", "Alto-Baixo", "Baixo-Baixo", "Baixo-Alto" };

            string strHTML = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            @"<html xmlns='http://www.w3.org/1999/xhtml'>" +
            @"<head>" +
            @"<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />" +
            @"<title>Relatório</title>" +
            @"</head>" +

            @"<body>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<h1 align='center'>IpeaGEO 2.1 </h1>" +
            @"<p align='center'> &nbsp;</p>" +
            @"<table width='829' border='1' align='center'>" +
            @"  <caption>" +
            @"    <strong>Informações gerais </strong>" +
            @"  <tr>" +
            @"    <td width='198'><strong>N&uacute;mero de pol&iacute;gonos: </strong></td>" +
            @"    <td width='615'>" + dep.numPoligonos.ToString() + "</td>" +
            @"  </tr>" +
            @"  </caption>" +
            @"    <td><strong>Endere&ccedil;o da malha digital: </strong></td>" +
            @"    <td>" + dep.strMapa + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Endere&ccedil;o da base de dados: </strong></td>" +
            @"    <td>" + dep.strBase + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Tipo de vizinhan&ccedil;a</strong></td>" +
            @"    <td>" + dep.strTipoVizinhnaca + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td><strong>Vari&aacute;veis</strong></td>" +
            @"    <td>" + strVariaveis + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td width='198'><strong>Nivel de confiabilidade: </strong></td>" +
            @"    <td width='615'>" + dep.strConfiabilidade + "</td>" +
            @"  </tr>" +
            @"  <tr>" +
            @"    <td width='198'><strong>M&eacute;todo de corre&ccedil;ão: </strong></td>" +
            @"    <td width='615'>" + dep.strTipoCorrecao + "</td>" +
            @"  </tr>"
            ;
            
            if (dep.strPopulacao != null)
            {
                strHTML +=
                @"  <tr>" +
                @"    <td width='198'><strong>População: </strong></td>" +
                @"    <td width='615'>" + dep.strPopulacao + "</td>" +
                @"  </tr>";
            }
            strHTML += @"</table>" + @"<p align='center'> &nbsp;</p>";

            if (dep.strMapaImagemLisa[0] != null)
            {
                for (int m = 0; m < dep.strMapaImagemLisa.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + dep.strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + dep.strMapaImagemLisa[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: LISA </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < dep.strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + dep.strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"  </table>" + @"</div>" + @"<p align='left'>&nbsp;</p>" + @"<p align='center'> &nbsp;</p>" + @"  <caption>" +
                                @"    <strong>Espalhamento de Moran: " + dep.strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + dep.strEspalha[m] + "' width='824' height='613' /></p>" + @"</table>" + @"<p align='center'> &nbsp;</p>" + @"<p align='center'> &nbsp;</p>";
                }
            }
            if (dep.strMapaImagemGetis[0] != null)
            {
                for (int m = 0; m < dep.strMapaImagemGetis.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + dep.strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + dep.strMapaImagemGetis[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: Getis-Ord Gi* </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < dep.strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + dep.strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"</table>" + @"<p align='center'> &nbsp;</p>";
                    strHTML += @"<p align='center'> &nbsp;</p>";
                }
            }
            if (dep.strMapaImagemGetis2[0] != null)
            {
                for (int m = 0; m < dep.strMapaImagemGetis2.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + dep.strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + dep.strMapaImagemGetis2[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: Getis-Ord Gi </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < dep.strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + dep.strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"</table>" + @"<p align='center'> &nbsp;</p>";
                    strHTML += @"<p align='center'> &nbsp;</p>";
                }
            }
            if (dep.strMapaImagemEscore[0] != null)
            {
                for (int m = 0; m < dep.strMapaImagemEscore.Length; m++)
                {
                    strHTML += @"  <caption>" +
                                @"    <strong>Variável: " + dep.strVariaveisSelecionadas[m] + " </strong>" +
                                @"  </caption>" +
                                @"<p align='center'><img src='" + dep.strMapaImagemEscore[m] + "' width='824' height='613' /></p>" +
                                @"<div align='center'>" +
                                @"  <table width='200' border='1'>" +
                                @"    <caption>" +
                                @"      <strong>Metodologia: Escore </strong>" +
                                @"    </caption>" +
                                @"    <tr>" +
                                @"      <td width='40'><div align='center'><strong>Classificação</strong></div></td>" +
                                @"     <td width='37'><div align='center'><strong>Cor</strong></div></td>" +
                                @"    </tr>";

                    for (int i = 0; i < dep.strCores.Length; i++)
                    {
                        strHTML +=
                        @"    <tr>" +
                        @"      <td>" + strClasse[i] + "</td>" +
                        @"      <td bgcolor='" + dep.strCores[i] + "'>&nbsp;</td>" +
                        @"    </tr>";
                    }
                    strHTML += @"</table>" + @"<p align='center'> &nbsp;</p>";
                    strHTML += @"<p align='center'> &nbsp;</p>";
                }
            }

            strHTML += @"<p align='center'> &nbsp;</p>" +
            @"</body>" +
            @"</html>";

            return (strHTML);
        }
    }
}
