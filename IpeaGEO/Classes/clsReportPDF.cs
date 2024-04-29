using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace IpeaGeo.Classes
{
    class clsReportPDF
    {
        public struct GlobalDependence
        {
            public string pdfFile, database, shape, neighborhoodType, 
                populationVariables, weightKind;
            public string[] quantitativeSelectedVariables;
            public double[] gearyIndex, gearyPValue, getisIndex, 
                getisPValue, moranIndex, moranPValue, 
                simpleMoranIndex, simpleMoranPValue, rogersonIndex, 
                rogersonPValue, tangoIndex, tangoPValue;
            public int polygons, simulations;
        }

        public struct Regression
        {
            public string pdfFile, shape, regressionMethod, mapVar, finalShape,
                database, mapMethod;
            public string[] sBetas, mapColors;
            public double aic, bic, loglik, sigma2, rho, rhoT, rhoP, rhoDesv;
            public double[] dBetas, desv, t, pValue, mapClasses;
            public int polygons;
        }

        public void RelatorioPDF_Regressao(Regression rr)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(rr.pdfFile, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Regressão Método: " + rr.regressionMethod));

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2, 4);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(rr.polygons.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(rr.database);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(rr.finalShape);

            titulo = new Chunk("Método", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(rr.regressionMethod);

            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            documento.Add(new Paragraph(""));

            documento.Add(new Paragraph("Resultados"));

            //Adiciona tabela dos betas
            Table aTable = new Table(5, rr.sBetas.Length + 2); //Adiciono 1 para o rho, 1 para intercepto

            aTable.BorderColorLeft = iTextSharp.text.Color.WHITE;
            aTable.BorderColorRight = iTextSharp.text.Color.WHITE;

            titulo = new Chunk("Beta", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell a1 = new Cell();
            a1.Add(titulo);
            a1.BorderColorLeft = iTextSharp.text.Color.WHITE;
            aTable.AddCell(a1);

            titulo = new Chunk("Valor Estimado", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell a2 = new Cell();
            a2.Add(titulo);
            aTable.AddCell(a2);

            titulo = new Chunk("Estatística t", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell a3 = new Cell();
            a3.Add(titulo);
            aTable.AddCell(a3);

            titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell a4 = new Cell();
            a4.Add(titulo);
            aTable.AddCell(a4);

            titulo = new Chunk("Desvio Padrão", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell a5 = new Cell();
            a5.Add(titulo);
            a5.BorderColorRight = iTextSharp.text.Color.WHITE;
            aTable.AddCell(a5);

            Cell celldummy = new Cell("Intercepto");
            celldummy.BorderColorLeft = iTextSharp.text.Color.WHITE;
            aTable.AddCell(celldummy);

            aTable.AddCell(rr.dBetas[0].ToString());
            aTable.AddCell(rr.t[0].ToString());
            aTable.AddCell(rr.pValue[0].ToString());
            aTable.AddCell(rr.desv[0].ToString());

            for (int i = 0; i < rr.sBetas.Length; i++)
            {
                Cell cellbeta = new Cell(rr.sBetas[i].ToString());
                cellbeta.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellbeta.BorderColorLeft = iTextSharp.text.Color.WHITE;
                cellbeta.HorizontalAlignment = Element.ALIGN_CENTER;
                aTable.AddCell(cellbeta);

                Cell cellvalor = new Cell(rr.dBetas[i + 1].ToString());
                cellvalor.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellvalor.HorizontalAlignment = Element.ALIGN_CENTER;
                aTable.AddCell(cellvalor);

                Cell cellt = new Cell(rr.t[i + 1].ToString());
                cellt.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellt.HorizontalAlignment = Element.ALIGN_CENTER;
                aTable.AddCell(cellt);

                Cell cellp = new Cell(rr.pValue[i + 1].ToString());
                cellp.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellp.HorizontalAlignment = Element.ALIGN_CENTER;
                aTable.AddCell(cellp);

                Cell celldesv = new Cell(rr.desv[i + 1].ToString());
                celldesv.BackgroundColor = iTextSharp.text.Color.WHITE;
                celldesv.BorderColorRight = iTextSharp.text.Color.WHITE;
                celldesv.HorizontalAlignment = Element.ALIGN_CENTER;
                aTable.AddCell(celldesv);
            }
            
            Cell cellrho = new Cell("Rho");
            cellrho.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellrho.BorderColorLeft = iTextSharp.text.Color.WHITE;
            cellrho.HorizontalAlignment = Element.ALIGN_CENTER;
            aTable.AddCell(cellrho);

            Cell cellrhoval = new Cell(rr.rho.ToString());
            cellrhoval.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellrhoval.HorizontalAlignment = Element.ALIGN_CENTER;
            aTable.AddCell(cellrhoval);

            Cell cellrhot = new Cell(rr.rhoT.ToString());
            cellrhot.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellrhot.HorizontalAlignment = Element.ALIGN_CENTER;
            aTable.AddCell(cellrhot);

            Cell cellrhop = new Cell(rr.rhoP.ToString());
            cellrhop.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellrhop.HorizontalAlignment = Element.ALIGN_CENTER;
            aTable.AddCell(cellrhop);

            Cell cellrhodesv = new Cell(rr.rhoDesv.ToString());
            cellrhodesv.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellrhodesv.BorderColorRight = iTextSharp.text.Color.WHITE;
            cellrhodesv.HorizontalAlignment = Element.ALIGN_CENTER;
            aTable.AddCell(cellrhodesv);

            aTable.Cellpadding = 5;
            aTable.Cellspacing = 5;

            documento.Add(aTable);

            documento.Add(new Paragraph("Estatísticas"));

            //Adiciona tabela dos outros resultados da regressao
            Table bTable = new Table(4, 2);

            bTable.BorderColorRight = iTextSharp.text.Color.WHITE;
            bTable.BorderColorLeft = iTextSharp.text.Color.WHITE;

            titulo = new Chunk("AIC", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell b1 = new Cell();
            b1.Add(titulo);
            b1.BorderColorLeft = iTextSharp.text.Color.WHITE;
            bTable.AddCell(b1);

            titulo = new Chunk("BIC", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell b2 = new Cell();
            b2.Add(titulo);
            bTable.AddCell(b2);

            titulo = new Chunk("Log Likelihood", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell b3 = new Cell();
            b3.Add(titulo);
            bTable.AddCell(b3);

            titulo = new Chunk("Sigma 2", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell b4 = new Cell();
            b4.Add(titulo);
            b4.BorderColorRight = iTextSharp.text.Color.WHITE;
            bTable.AddCell(b4);


            Cell cellaic = new Cell(rr.aic.ToString());
            cellaic.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellaic.HorizontalAlignment = Element.ALIGN_CENTER;
            cellaic.BorderColorLeft = iTextSharp.text.Color.WHITE;
            bTable.AddCell(cellaic);

            Cell cellbic = new Cell(rr.bic.ToString());
            cellbic.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellbic.HorizontalAlignment = Element.ALIGN_CENTER;
            bTable.AddCell(cellbic);

            Cell cellloglik = new Cell(rr.loglik.ToString());
            cellloglik.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellloglik.HorizontalAlignment = Element.ALIGN_CENTER;
            bTable.AddCell(cellloglik);

            Cell cellsigma = new Cell(rr.sigma2.ToString());
            cellsigma.BackgroundColor = iTextSharp.text.Color.WHITE;
            cellsigma.HorizontalAlignment = Element.ALIGN_CENTER;
            cellsigma.BorderColorRight = iTextSharp.text.Color.WHITE;
            bTable.AddCell(cellsigma);

            bTable.Cellspacing = 5;
            bTable.Cellpadding = 5;

            documento.Add(bTable);

            // Nova pagina
            documento.NewPage();

            // Adiciona imagem do mapa
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(rr.shape);
            documento.Add(gif);

            documento.Add(new Paragraph("Mapa de " + rr.mapVar + ", Metodologia: " + rr.mapMethod));
            //Adiciona a legenda do mapa
            Table cTable = new Table(3);

            cTable.Width = 45;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[3];
            width_percentages[0] = 22;
            width_percentages[1] = 18;
            width_percentages[2] = 60;
            cTable.SetWidths(width_percentages);

            titulo = new Chunk("Classes", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c1 = new Cell();
            c1.Add(titulo);
            cTable.AddCell(c1);

            titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c2 = new Cell();
            c2.Add(titulo);
            cTable.AddCell(c2);

            titulo = new Chunk("Intervalos", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c3 = new Cell();
            c3.Add(titulo);
            cTable.AddCell(c3);

            for (int i = 0; i < rr.mapClasses.Length; i++)
            {
                int numero = i + 1;

                Cell cellclasse = new Cell(numero.ToString());
                cellclasse.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellclasse.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellclasse);

                //Trabalhando com cores
                string totalcor = rr.mapColors[i].Substring(1); //Retirei o #
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellcor);

                if (i == 0)
                {
                    Cell cellintervalo = new Cell("a partir de  " + rr.mapClasses[i].ToString());
                    cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                    cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cTable.AddCell(cellintervalo);
                }

                else
                {
                    if (i != rr.mapClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(rr.mapClasses[i - 1].ToString() + "  a  " + rr.mapClasses[i].ToString());
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                    else if (i == rr.mapClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(rr.mapClasses[i].ToString() + "  ou maior");
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                }
            }
            
            cTable.Cellpadding = 5;
            cTable.Cellspacing = 5;

            documento.Add(cTable);

            documento.Close();
        }
        
        public void RelatorioPDF_Segregacao(string enderecopdf, double[,] valores, string[] nomes_indices, string[] nomes_variaveis)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório Índices de Segregacao");
            documento.Add(p);

            int linhas = nomes_variaveis.Length + 1;
            int colunas = nomes_indices.Length + 1;

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(colunas, linhas);

            titulo = new Chunk("Variáveis", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);

            for (int i = 0; i < nomes_indices.Length; i++)
            {
                oTable.AddCell(nomes_indices[i].ToString());
            }

            for (int i = 0; i < linhas - 1; i++)
            {
                oTable.AddCell(nomes_variaveis[i].ToString());

                for (int j = 0; j < colunas - 1; j++)
                {
                    oTable.AddCell(valores[i, j].ToString());

                }
            }

            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            documento.Close();
        }

        public void RelatorioPDF_MapaTematicoMM(string enderecopdf, string strMapaImagem, string strMetodo, double[] strClasses, string[] strCores, string strVariavel)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Mapa Temático"));

            documento.Add(new Paragraph("Metodologia: " + strMetodo));

            //Adiciona imagem do mapa
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagem);
            documento.Add(gif);

            documento.Add(new Paragraph("Mapa de " + strVariavel));

            //Adiciona a legenda do mapa
            Table cTable = new Table(3);

            cTable.Width = 45;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[3];
            width_percentages[0] = 22;
            width_percentages[1] = 18;
            width_percentages[2] = 60;
            cTable.SetWidths(width_percentages);

            titulo = new Chunk("Classes", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c1 = new Cell();
            c1.Add(titulo);
            cTable.AddCell(c1);

            titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c2 = new Cell();
            c2.Add(titulo);
            cTable.AddCell(c2);

            titulo = new Chunk("Intervalos", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c3 = new Cell();
            c3.Add(titulo);
            cTable.AddCell(c3);

            for (int i = 0; i < strClasses.Length; i++)
            {
                int numero = i + 1;

                Cell cellclasse = new Cell(numero.ToString());
                cellclasse.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellclasse.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellclasse);

                //Trabalhando com cores. O string vem como #FFFFFF
                string totalcor = strCores[i].Substring(1); //Retirei o #
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellcor);

                if (i == 0)
                {
                    Cell cellintervalo = new Cell("a partir de  " + strClasses[i].ToString());
                    cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                    cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cTable.AddCell(cellintervalo);
                }

                else
                {
                    if (i != strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i - 1].ToString() + "  a  " + strClasses[i].ToString());
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                    else if (i == strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i].ToString() + "  ou maior");
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                }
            }
            
            cTable.Cellpadding = 5;
            cTable.Cellspacing = 5;

            documento.Add(cTable);

            documento.Close();
        }

        public void RelatorioPDF_MapaTematico(string enderecopdf, string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavel)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Mapa Temático"));

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2, 4);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(numPoligonos.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(strBase);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(strMapa);

            titulo = new Chunk("Metodologia", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(strMetodo);

            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            documento.Add(new Paragraph(""));

            documento.NewPage();

            //Adiciona imagem do mapa
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagem);
            documento.Add(gif);

            documento.Add(new Paragraph("Mapa de " + strVariavel));
            //Adiciona a legenda do mapa
            Table cTable = new Table(3);

            cTable.Width = 45;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[3];
            width_percentages[0] = 22;
            width_percentages[1] = 18;
            width_percentages[2] = 60;
            cTable.SetWidths(width_percentages);

            titulo = new Chunk("Classes", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c1 = new Cell();
            c1.Add(titulo);
            cTable.AddCell(c1);

            titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c2 = new Cell();
            c2.Add(titulo);
            cTable.AddCell(c2);

            titulo = new Chunk("Intervalos", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c3 = new Cell();
            c3.Add(titulo);
            cTable.AddCell(c3);

            for (int i = 0; i < strClasses.Length; i++)
            {
                int numero = i + 1;

                Cell cellclasse = new Cell(numero.ToString());
                cellclasse.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellclasse.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellclasse);

                //Trabalhando com cores. O string vem como #FFFFFF
                string totalcor = strCores[i].Substring(1); //Retirei o #
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellcor);

                if (i == 0)
                {
                    Cell cellintervalo = new Cell("a partir de  " + strClasses[i].ToString());
                    cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                    cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cTable.AddCell(cellintervalo);
                }

                else
                {
                    if (i != strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i - 1].ToString() + "  a  " + strClasses[i].ToString());
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                    else if (i == strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i].ToString() + "  ou maior");
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                }
            }
            
            cTable.Cellpadding = 5;
            cTable.Cellspacing = 5;

            documento.Add(cTable);

            documento.Close();
        }

        public void RelatorioPDF_MapaTematico(string enderecopdf, string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavel, string[] legendas)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Mapa Temático"));

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2, 4);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(numPoligonos.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(strBase);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(strMapa);

            titulo = new Chunk("Metodologia", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(strMetodo);

            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            documento.Add(new Paragraph(""));

            documento.NewPage();

            //Adiciona imagem do mapa
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagem);
            documento.Add(gif);

            documento.Add(new Paragraph("Mapa de " + strVariavel));
            
            //Adiciona a legenda do mapa
            Table cTable = new Table(3);

            cTable.Width = 45;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[3];
            width_percentages[0] = 22;
            width_percentages[1] = 18;
            width_percentages[2] = 60;
            cTable.SetWidths(width_percentages);

            titulo = new Chunk("Legenda", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c1 = new Cell();
            c1.Add(titulo);
            cTable.AddCell(c1);

            titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c2 = new Cell();
            c2.Add(titulo);
            cTable.AddCell(c2);

            titulo = new Chunk("Intervalos", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c3 = new Cell();
            c3.Add(titulo);
            cTable.AddCell(c3);

            for (int i = 0; i < strClasses.Length; i++)
            {
                int numero = i + 1;

                Cell cellclasse = new Cell(legendas[i].ToString());
                cellclasse.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellclasse.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellclasse);

                //Trabalhando com cores. O string vem como #FFFFFF
                string totalcor = strCores[i].Substring(1); //Retirei o #
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellcor);
                
                if (i == 0)
                {
                    Cell cellintervalo = new Cell("a partir de  " + strClasses[i].ToString());
                    cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                    cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cTable.AddCell(cellintervalo);
                }

                else
                {
                    if (i != strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i - 1].ToString() + "  a  " + strClasses[i].ToString());
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                    else if (i == strClasses.Length - 1)
                    {
                        Cell cellintervalo = new Cell(strClasses[i].ToString() + "  ou maior");
                        cellintervalo.BackgroundColor = iTextSharp.text.Color.WHITE;
                        cellintervalo.HorizontalAlignment = Element.ALIGN_CENTER;
                        cTable.AddCell(cellintervalo);
                    }
                }
            }
            cTable.Cellpadding = 5;
            cTable.Cellspacing = 5;

            documento.Add(cTable);

            documento.Close();
        }

        public void RelatorioPDF_Conglomerados(string enderecopdf, string strBase, string strMapa, string strMapaImagem, string numClusters, string iMinkowsky, int numPoligonos, string strMetodo, string strDistancia, bool blEspacial, string[] strVariaveisSelecionadas, string strvizinhanca, string[] strCores, string strCCC)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Conglomerados Espaciais"));

            string strVariaveis = "";
            string strPesos = "";

            for (int i = 0; i < strVariaveisSelecionadas.Length - 1; i++) strVariaveis += strVariaveisSelecionadas[i] + ", ";
            strVariaveis += strVariaveisSelecionadas[strVariaveisSelecionadas.Length - 1];

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2, 10);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(numPoligonos.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(strBase);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(strMapa);

            titulo = new Chunk("Método", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(strMetodo);

            titulo = new Chunk("Distância", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o5 = new Cell();
            o5.Add(titulo);
            oTable.AddCell(o5);
            oTable.AddCell(strDistancia);

            titulo = new Chunk("Variáveis", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o6 = new Cell();
            o6.Add(titulo);
            oTable.AddCell(o6);
            oTable.AddCell(strVariaveis);

            titulo = new Chunk("Tipo de Vizinhança", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o7 = new Cell();
            o7.Add(titulo);
            oTable.AddCell(o7);
            oTable.AddCell(strvizinhanca);

            titulo = new Chunk("Número de Conglomerados", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o8 = new Cell();
            o8.Add(titulo);
            oTable.AddCell(o8);
            oTable.AddCell(numClusters);

            titulo = new Chunk("Fator Minkowski", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o9 = new Cell();
            o9.Add(titulo);
            oTable.AddCell(o9);
            oTable.AddCell(iMinkowsky);

            titulo = new Chunk("Conglomerado Espacial", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o10 = new Cell();
            o10.Add(titulo);
            oTable.AddCell(o10);
            oTable.AddCell(blEspacial.ToString());

            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            documento.Add(new Paragraph(""));

            documento.NewPage();

            //Adiciona imagem do mapa
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagem);
            documento.Add(gif);

            documento.Add(new Paragraph("Metodologia:  " + strMetodo));
            //Adiciona a legenda do mapa
            Table cTable = new Table(2);

            cTable.Width = 30;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[2];
            width_percentages[0] = 50;
            width_percentages[1] = 50;
            cTable.SetWidths(width_percentages);

            titulo = new Chunk("Conglomerado", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c1 = new Cell();
            c1.Add(titulo);
            cTable.AddCell(c1);

            titulo = new Chunk("Cor", FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell c2 = new Cell();
            c2.Add(titulo);
            cTable.AddCell(c2);

            for (int i = 0; i < strCores.Length; i++)
            {
                int numero = i + 1;

                Cell cellclasse = new Cell(numero.ToString());
                cellclasse.BackgroundColor = iTextSharp.text.Color.WHITE;
                cellclasse.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellclasse);

                //Trabalhando com cores. O string vem como #FFFFFF
                string totalcor = strCores[i].Substring(1); //Retirei o #
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                cTable.AddCell(cellcor);
            }

            cTable.Cellpadding = 5;
            cTable.Cellspacing = 5;

            documento.Add(cTable);

            //Adiciona imagem dos critérios de parada
            iTextSharp.text.Image gif2 = iTextSharp.text.Image.GetInstance(strCCC);
            documento.Add(gif2);

            documento.Close();
        }

        public void RelatorioPDF_DependenciaGlobal(GlobalDependence rdg)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(rdg.pdfFile, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Dependência Global"));

            int iFor = rdg.quantitativeSelectedVariables.Length - 1;

            //Se não existir indice de tango ou rogerson
            if (rdg.rogersonIndex[0] > 999 || rdg.tangoIndex[0] > 999)
            {
                iFor++;
                rdg.populationVariables = "";
            }

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(rdg.polygons.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(rdg.database);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(rdg.shape);

            titulo = new Chunk("Tipo de Vizinhança", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(rdg.neighborhoodType);

            if (rdg.populationVariables != "")
            {
                titulo = new Chunk("Variável Populacional", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell o5 = new Cell();
                o5.Add(titulo);
                oTable.AddCell(o5);
                oTable.AddCell(rdg.populationVariables);
            }
            oTable.Cellspacing = 5;
            oTable.Cellpadding = 5;

            documento.Add(oTable);

            //Nova tabela

            if (rdg.moranIndex[0] != 0)
            {
                documento.Add(new Paragraph("Indice de Moran"));
                Table moranTable = new Table(3);

                titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mm1 = new Cell();
                mm1.Add(titulo);
                moranTable.AddCell(mm1);

                titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mn1 = new Cell();
                mn1.Add(titulo);
                moranTable.AddCell(mn1);

                titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mo1 = new Cell();
                mo1.Add(titulo);
                moranTable.AddCell(mo1);

                for (int i = 0; i < iFor; i++)
                {
                    moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                    moranTable.AddCell(rdg.moranIndex[i].ToString());
                    moranTable.AddCell(rdg.moranPValue[i].ToString());
                }

                moranTable.Cellpadding = 5;
                moranTable.Cellspacing = 5;
                documento.Add(moranTable);
            }

            if (rdg.simpleMoranIndex[0] != 0)
            {
                documento.Add(new Paragraph("Indice de Moran Simples"));
                Table moranTable = new Table(3);

                titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mm1 = new Cell();
                mm1.Add(titulo);
                moranTable.AddCell(mm1);

                titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mn1 = new Cell();
                mn1.Add(titulo);
                moranTable.AddCell(mn1);

                titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mo1 = new Cell();
                mo1.Add(titulo);
                moranTable.AddCell(mo1);

                for (int i = 0; i < iFor; i++)
                {
                    moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                    moranTable.AddCell(rdg.simpleMoranIndex[i].ToString());
                    moranTable.AddCell(rdg.simpleMoranPValue[i].ToString());
                }

                moranTable.Cellpadding = 5;
                moranTable.Cellspacing = 5;
                documento.Add(moranTable);
            }

            if (rdg.gearyIndex[0] != 0)
            {
                documento.Add(new Paragraph("Indice de Geary"));
                Table moranTable = new Table(3);

                titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mm1 = new Cell();
                mm1.Add(titulo);
                moranTable.AddCell(mm1);

                titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mn1 = new Cell();
                mn1.Add(titulo);
                moranTable.AddCell(mn1);

                titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mo1 = new Cell();
                mo1.Add(titulo);
                moranTable.AddCell(mo1);

                for (int i = 0; i < iFor; i++)
                {
                    moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                    moranTable.AddCell(rdg.gearyIndex[i].ToString());
                    moranTable.AddCell(rdg.gearyPValue[i].ToString());
                }

                moranTable.Cellpadding = 5;
                moranTable.Cellspacing = 5;
                documento.Add(moranTable);
            }

            if (rdg.getisIndex[0] != 0)
            {
                documento.Add(new Paragraph("Indice de Getis"));
                Table moranTable = new Table(3);

                titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mm1 = new Cell();
                mm1.Add(titulo);
                moranTable.AddCell(mm1);

                titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mn1 = new Cell();
                mn1.Add(titulo);
                moranTable.AddCell(mn1);

                titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell mo1 = new Cell();
                mo1.Add(titulo);
                moranTable.AddCell(mo1);

                for (int i = 0; i < iFor; i++)
                {
                    moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                    moranTable.AddCell(rdg.getisIndex[i].ToString());
                    moranTable.AddCell(rdg.getisPValue[i].ToString());

                }
                moranTable.Cellpadding = 5;
                moranTable.Cellspacing = 5;
                documento.Add(moranTable);
            }

            if (rdg.rogersonIndex[0] < 999)
            {
                if (rdg.rogersonIndex[0] != 0)
                {
                    documento.Add(new Paragraph("Indice de Rogerson"));
                    Table moranTable = new Table(3);

                    titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mm1 = new Cell();
                    mm1.Add(titulo);
                    moranTable.AddCell(mm1);

                    titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mn1 = new Cell();
                    mn1.Add(titulo);
                    moranTable.AddCell(mn1);

                    titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mo1 = new Cell();
                    mo1.Add(titulo);
                    moranTable.AddCell(mo1);

                    for (int i = 0; i < iFor; i++)
                    {
                        moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                        moranTable.AddCell(rdg.rogersonIndex[i].ToString());
                        moranTable.AddCell(rdg.rogersonPValue[i].ToString());
                    }

                    moranTable.Cellpadding = 5;
                    moranTable.Cellspacing = 5;
                    documento.Add(moranTable);
                }
            }

            if (rdg.tangoIndex[0] < 999)
            {
                if (rdg.tangoIndex[0] != 0)
                {
                    documento.Add(new Paragraph("Indice de Tango"));
                    Table moranTable = new Table(3);

                    titulo = new Chunk("Variável Quantitativa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mm1 = new Cell();
                    mm1.Add(titulo);
                    moranTable.AddCell(mm1);

                    titulo = new Chunk("Indice", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mn1 = new Cell();
                    mn1.Add(titulo);
                    moranTable.AddCell(mn1);

                    titulo = new Chunk("p-valor", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell mo1 = new Cell();
                    mo1.Add(titulo);
                    moranTable.AddCell(mo1);

                    for (int i = 0; i < iFor; i++)
                    {
                        moranTable.AddCell(rdg.quantitativeSelectedVariables[i]);
                        moranTable.AddCell(rdg.tangoIndex[i].ToString());
                        moranTable.AddCell(rdg.tangoPValue[i].ToString());
                    }

                    moranTable.Cellpadding = 5;
                    moranTable.Cellspacing = 5;
                    documento.Add(moranTable);
                }
            }

            documento.Close();
        }

        public void RelatorioPDF_DependenciaLocal(string enderecopdf, string strBase, string strMapa, int numPoligonos, string[] strMapaImagemLisa, string[] strMapaImagemGetis, string[] strMapaImagemGetis2, string[] strMapaImagemEscore, string[] strEspalha, string strTipoVizinhnaca, string strTipoCorrecao, string strConfiabilidade, string strPopulacao, string[] strVariaveisSelecionadas, string[] strCores)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            string strVariaveis = "";

            for (int i = 0; i < strVariaveisSelecionadas.Length - 1; i++) strVariaveis += strVariaveisSelecionadas[i] + ", ";
            strVariaveis += strVariaveisSelecionadas[strVariaveisSelecionadas.Length - 1];
            string[] strClasse = new string[5] { "Não significativo", "Alto-Alto", "Alto-Baixo", "Baixo-Baixo", "Baixo-Alto" };

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Dependência Local"));

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(numPoligonos.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(strBase);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(strMapa);

            titulo = new Chunk("Tipo de Vizinhança", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(strTipoVizinhnaca);

            titulo = new Chunk("Variáveis", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o5 = new Cell();
            o5.Add(titulo);
            oTable.AddCell(o5);
            oTable.AddCell(strVariaveis);

            titulo = new Chunk("Nível de Confiabilidade", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o6 = new Cell();
            o6.Add(titulo);
            oTable.AddCell(o6);
            oTable.AddCell(strConfiabilidade);

            titulo = new Chunk("Método de Correção", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o7 = new Cell();
            o7.Add(titulo);
            oTable.AddCell(o7);
            oTable.AddCell(strTipoCorrecao);

            if (strPopulacao != null)
            {
                titulo = new Chunk("População", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                Cell o8 = new Cell();
                o8.Add(titulo);
                oTable.AddCell(o8);
                oTable.AddCell(strPopulacao);
            }

            oTable.Cellpadding = 5;
            oTable.Cellspacing = 5;
            documento.Add(oTable);
            documento.NewPage();

            //Adicionando mapas LISA
            if (strMapaImagemLisa[0] != null)
            {
                for (int m = 0; m < strMapaImagemLisa.Length; m++)
                {
                    documento.Add(new Paragraph("Variável:  " + strVariaveisSelecionadas[m]));

                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagemLisa[m]);
                    documento.Add(gif);

                    documento.Add(new Paragraph("Metodologia LISA"));

                    Table tabelalocal = new Table(2);

                    tabelalocal.Width = 45;
                    int[] larguras = new int[2];
                    larguras[0] = 60;
                    larguras[1] = 40;
                    tabelalocal.SetWidths(larguras);

                    titulo = new Chunk("Classificação", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t1 = new Cell();
                    t1.Add(titulo);
                    tabelalocal.AddCell(t1);

                    titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t2 = new Cell();
                    t2.Add(titulo);
                    tabelalocal.AddCell(t2);

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        tabelalocal.AddCell(strClasse[i]);

                        //Trabalhando com cores
                        string totalcor = strCores[i].Substring(1); 
                        string R = totalcor.Substring(0, 2);
                        string G = totalcor.Substring(2, 2);
                        string B = totalcor.Substring(4, 2);

                        int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                        int Gint = Convert.ToInt32(G, 16);
                        int Bint = Convert.ToInt32(B, 16);

                        Cell cellcor = new Cell();
                        cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                        cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabelalocal.AddCell(cellcor);
                    }
                    
                    tabelalocal.Cellspacing = 5;
                    tabelalocal.Cellpadding = 5;
                    documento.Add(tabelalocal);
                    documento.NewPage();

                    //Mais um mapa para LISA
                    documento.Add(new Paragraph("Espalhamento de Moran: " + strVariaveisSelecionadas[m]));
                    gif = iTextSharp.text.Image.GetInstance(strEspalha[m]);
                    gif.ScalePercent(80);
                    gif.Alignment = Element.ALIGN_CENTER;
                    documento.Add(gif);

                    documento.NewPage();
                }
            }

            //Adicionando mapas GETIS
            if (strMapaImagemGetis[0] != null)
            {
                for (int m = 0; m < strMapaImagemGetis.Length; m++)
                {
                    documento.Add(new Paragraph("Variável:  " + strVariaveisSelecionadas[m]));

                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagemGetis[m]);
                    documento.Add(gif);

                    documento.Add(new Paragraph("Metodologia Getis Ord*"));

                    Table tabelalocal = new Table(2);

                    tabelalocal.Width = 45;
                    int[] larguras = new int[2];
                    larguras[0] = 60;
                    larguras[1] = 40;
                    tabelalocal.SetWidths(larguras);

                    titulo = new Chunk("Classificação", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t1 = new Cell();
                    t1.Add(titulo);
                    tabelalocal.AddCell(t1);

                    titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t2 = new Cell();
                    t2.Add(titulo);
                    tabelalocal.AddCell(t2);

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        tabelalocal.AddCell(strClasse[i]);

                        //Trabalhando com cores
                        string totalcor = strCores[i].Substring(1); 
                        string R = totalcor.Substring(0, 2);
                        string G = totalcor.Substring(2, 2);
                        string B = totalcor.Substring(4, 2);

                        int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                        int Gint = Convert.ToInt32(G, 16);
                        int Bint = Convert.ToInt32(B, 16);

                        Cell cellcor = new Cell();
                        cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                        cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabelalocal.AddCell(cellcor);
                    }
                    
                    tabelalocal.Cellspacing = 5;
                    tabelalocal.Cellpadding = 5;
                    documento.Add(tabelalocal);

                    documento.NewPage();
                }
            }

            //Adicionando mapas GETIS2
            if (strMapaImagemGetis2[0] != null)
            {
                for (int m = 0; m < strMapaImagemGetis2.Length; m++)
                {
                    documento.Add(new Paragraph("Variável:  " + strVariaveisSelecionadas[m]));

                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagemGetis2[m]);
                    documento.Add(gif);

                    documento.Add(new Paragraph("Metodologia Getis Ord"));

                    Table tabelalocal = new Table(2);

                    tabelalocal.Width = 45;
                    int[] larguras = new int[2];
                    larguras[0] = 60;
                    larguras[1] = 40;
                    tabelalocal.SetWidths(larguras);

                    titulo = new Chunk("Classificação", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t1 = new Cell();
                    t1.Add(titulo);
                    tabelalocal.AddCell(t1);

                    titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t2 = new Cell();
                    t2.Add(titulo);
                    tabelalocal.AddCell(t2);

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        tabelalocal.AddCell(strClasse[i]);

                        //Trabalhando com cores
                        string totalcor = strCores[i].Substring(1); 
                        string R = totalcor.Substring(0, 2);
                        string G = totalcor.Substring(2, 2);
                        string B = totalcor.Substring(4, 2);

                        int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                        int Gint = Convert.ToInt32(G, 16);
                        int Bint = Convert.ToInt32(B, 16);

                        Cell cellcor = new Cell();
                        cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                        cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabelalocal.AddCell(cellcor);
                    }
                    
                    tabelalocal.Cellspacing = 5;
                    tabelalocal.Cellpadding = 5;
                    documento.Add(tabelalocal);

                    documento.NewPage();
                }
            }

            //Adicionando mapas ESCORE
            if (strMapaImagemEscore[0] != null)
            {
                for (int m = 0; m < strMapaImagemEscore.Length; m++)
                {
                    documento.Add(new Paragraph("Variável:  " + strVariaveisSelecionadas[m]));

                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagemEscore[m]);
                    documento.Add(gif);

                    documento.Add(new Paragraph("Metodologia Escore"));

                    Table tabelalocal = new Table(2);

                    tabelalocal.Width = 45;
                    int[] larguras = new int[2];
                    larguras[0] = 60;
                    larguras[1] = 40;
                    tabelalocal.SetWidths(larguras);

                    titulo = new Chunk("Classificação", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t1 = new Cell();
                    t1.Add(titulo);
                    tabelalocal.AddCell(t1);

                    titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
                    Cell t2 = new Cell();
                    t2.Add(titulo);
                    tabelalocal.AddCell(t2);

                    for (int i = 0; i < strCores.Length; i++)
                    {
                        tabelalocal.AddCell(strClasse[i]);

                        //Trabalhando com cores
                        string totalcor = strCores[i].Substring(1); 
                        string R = totalcor.Substring(0, 2);
                        string G = totalcor.Substring(2, 2);
                        string B = totalcor.Substring(4, 2);

                        int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                        int Gint = Convert.ToInt32(G, 16);
                        int Bint = Convert.ToInt32(B, 16);

                        Cell cellcor = new Cell();
                        cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                        cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabelalocal.AddCell(cellcor);
                    }
                    
                    tabelalocal.Cellspacing = 5;
                    tabelalocal.Cellpadding = 5;
                    documento.Add(tabelalocal);

                    documento.NewPage();
                }
            }

            documento.Close();
        }

        public void RelatorioPDF_Scan(string enderecopdf, string strBase, string strMapa, string strMapaImagem, int numPoligonos, string strMetodo, double[] strClasses, string[] strCores, string strVariavelBase, string strVariavelEvento, string strSimulacoes, string strPontosGrid, string strRaioMax, string strRarioMin, string strProp, string strHistograma)
        {
            Document documento = new Document(iTextSharp.text.PageSize.A2);

            PdfWriter.GetInstance(documento, new FileStream(enderecopdf, FileMode.Create));

            documento.Open();

            //Adiciona o titulo do relatorio
            Chunk titulo = new Chunk("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));

            Paragraph title = new Paragraph("IpeaGEO 2.1", FontFactory.GetFont(FontFactory.COURIER, 30, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            title.Alignment = Element.ALIGN_CENTER;

            documento.Add(title);

            Paragraph p = new Paragraph("Relatório");
            documento.Add(p);
            documento.Add(new Paragraph("Scan"));

            //Adiciona a tabela com informações gerais
            Table oTable = new Table(2);

            titulo = new Chunk("Número de Polígonos", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o1 = new Cell();
            o1.Add(titulo);
            oTable.AddCell(o1);
            oTable.AddCell(numPoligonos.ToString());

            titulo = new Chunk("Endereço base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o2 = new Cell();
            o2.Add(titulo);
            oTable.AddCell(o2);
            oTable.AddCell(strBase);

            titulo = new Chunk("Endereço mapa", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o3 = new Cell();
            o3.Add(titulo);
            oTable.AddCell(o3);
            oTable.AddCell(strMapa);

            titulo = new Chunk("Método", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o4 = new Cell();
            o4.Add(titulo);
            oTable.AddCell(o4);
            oTable.AddCell(strMetodo);

            titulo = new Chunk("Variável Base", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o5 = new Cell();
            o5.Add(titulo);
            oTable.AddCell(o5);
            oTable.AddCell(strVariavelBase);

            titulo = new Chunk("Variável Evento", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell o6 = new Cell();
            o6.Add(titulo);
            oTable.AddCell(o6);
            oTable.AddCell(strVariavelEvento);

            oTable.Cellpadding = 5;
            oTable.Cellspacing = 5;
            documento.Add(oTable);
            documento.NewPage();

            //Adicionando a imagem

            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(strMapaImagem);
            documento.Add(gif);

            documento.Add(new Paragraph("Metodologia:  " + strMetodo));

            Table tabelacluster = new Table(3);

            tabelacluster.Width = 45;

            //Vetor com as porcentagens de largura das colunas
            int[] width_percentages = new int[3];
            width_percentages[0] = 22;
            width_percentages[1] = 18;
            width_percentages[2] = 60;
            tabelacluster.SetWidths(width_percentages);

            titulo = new Chunk("Classe", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell t1 = new Cell();
            t1.Add(titulo);
            tabelacluster.AddCell(t1);

            titulo = new Chunk("Cores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell t2 = new Cell();
            t2.Add(titulo);
            tabelacluster.AddCell(t2);

            titulo = new Chunk("p-valores", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell t3 = new Cell();
            t3.Add(titulo);
            tabelacluster.AddCell(t3);

            for (int i = 0; i < strClasses.Length; i++)
            {
                tabelacluster.AddCell(i.ToString());

                //Trabalhando com cores
                string totalcor = strCores[i + 1].Substring(1); 
                string R = totalcor.Substring(0, 2);
                string G = totalcor.Substring(2, 2);
                string B = totalcor.Substring(4, 2);

                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                int Gint = Convert.ToInt32(G, 16);
                int Bint = Convert.ToInt32(B, 16);

                Cell cellcor = new Cell();
                cellcor.BackgroundColor = new iTextSharp.text.Color(Rint, Gint, Bint);
                cellcor.HorizontalAlignment = Element.ALIGN_CENTER;
                tabelacluster.AddCell(cellcor);

                tabelacluster.AddCell(strClasses[i].ToString().Substring(0, 6));
            }
            
            tabelacluster.Cellspacing = 5;
            tabelacluster.Cellpadding = 5;
            documento.Add(tabelacluster);
            documento.NewPage();

            //Adicionando o histograma

            gif = iTextSharp.text.Image.GetInstance(strHistograma);
            gif.ScalePercent(80);
            gif.Alignment = Element.ALIGN_CENTER;
            documento.Add(gif);

            //Tabela Monte Carlo
            documento.Add(new Paragraph("Monte Carlo"));

            Table montecarlo = new Table(2);

            titulo = new Chunk("Simulações", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell tr2 = new Cell();
            tr2.Add(titulo);
            montecarlo.AddCell(tr2);
            montecarlo.AddCell(strSimulacoes);

            titulo = new Chunk("Pontos Grid", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell tr3 = new Cell();
            tr3.Add(titulo);
            montecarlo.AddCell(tr3);
            montecarlo.AddCell(strPontosGrid);

            titulo = new Chunk("Raio Máximo", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell tr4 = new Cell();
            tr4.Add(titulo);
            montecarlo.AddCell(tr4);
            montecarlo.AddCell(strRaioMax);

            titulo = new Chunk("Raio Mínimo", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell tr5 = new Cell();
            tr5.Add(titulo);
            montecarlo.AddCell(tr5);
            montecarlo.AddCell(strRarioMin);

            titulo = new Chunk("Proporção Máxima", FontFactory.GetFont(FontFactory.DefaultEncoding, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK));
            Cell tr6 = new Cell();
            tr6.Add(titulo);
            montecarlo.AddCell(tr6);
            montecarlo.AddCell(strProp);

            montecarlo.Cellspacing = 5;
            montecarlo.Cellpadding = 5;
            documento.Add(montecarlo);

            documento.Add(new Paragraph("Obs: Utilizou-se a distribuição generalizada de valores extremos."));

            documento.NewPage();

            documento.Close();
        }
    }
}
