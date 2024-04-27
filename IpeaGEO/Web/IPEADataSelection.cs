using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace IpeaGeo.Web
{
    public struct DataResource
    {
        public string label, url, dataFile, shapeFile, metadata;
        public string[] shapeExt;
        public DataResourceItem[] items;
    };

    public struct DataResourceItem
    {
        public string label, dataFile, shapeFile, metadata;
        public string[] shapeExt;
    };

    public static class IpeaDataSelection
    {
        /// <summary>
        /// Create objects of type "resource" in the configuration XML.
        /// </summary>
        /// <param name="writer">XmlWriter object <see cref="System.Xml.XmlWriter"/></param>
        /// <param name="label">the text to identify the resource in ListView.</param>
        /// <param name="url">the base URL to find the files.</param>
        /// <param name="data">.XLS file name</param>
        /// <param name="shape">shape file name without extension.</param>
        /// <param name="exts">a list with shape files extensions.</param>
        private static void createXmlDataResource(XmlWriter writer, string label, string url, string data, string shape, string exts, string metadata)
        {
            writer.WriteStartElement("resource");

            writer.WriteStartAttribute("label");
            writer.WriteString(label);
            writer.WriteEndAttribute();

            writer.WriteStartElement("base-url");
            writer.WriteStartAttribute("link");
            writer.WriteString(url);
            writer.WriteEndAttribute();
            writer.WriteEndElement();

            writer.WriteStartElement("data-file");
            writer.WriteStartAttribute("name");
            writer.WriteString(data);
            writer.WriteEndAttribute();
            writer.WriteEndElement();

            writer.WriteStartElement("shape-file");
            writer.WriteStartAttribute("name");
            writer.WriteString(shape);
            writer.WriteEndAttribute();

            writer.WriteStartAttribute("exts");
            writer.WriteString(exts);
            writer.WriteEndAttribute();
            writer.WriteEndElement();

            if (metadata != null)
            {
                writer.WriteStartElement("metadata");
                writer.WriteString(metadata);
                writer.WriteEndElement();
            } // if

            writer.WriteEndElement();
        } // createXmlDataResource()

        /// <summary>
        /// Creates an item of an existing resource in the XML config file.
        /// </summary>
        /// <param name="writer">XmlWriter object <see cref="System.Xml.XmlWriter"/></param>
        /// <param name="rurl">the base URL to find the object.</param>
        /// <param name="rlabel">the resource label</param>
        /// <param name="labels">the items labels.</param>
        /// <param name="itens">the items identificator in file names.</param>
        /// <param name="data">the template to construct the data file name. <see cref="String.Format"/></param>
        /// <param name="shape">the template to construct the shape file name.</param>
        /// <param name="exts">the shape file extensions array.</param>
        private static void createXmlDataItems(XmlWriter writer, string rlabel, string rurl, string[] labels, string[] itens, string data, string shape, string exts, string metadata)
        {
            writer.WriteStartElement("resource");
            writer.WriteStartAttribute("label");
            writer.WriteString(rlabel);
            writer.WriteEndAttribute();

            writer.WriteStartElement("base-url");
            writer.WriteStartAttribute("link");
            writer.WriteString(rurl);
            writer.WriteEndAttribute();
            writer.WriteEndElement();

            for (int k = 0; k < Math.Min(labels.Length, itens.Length); k++)
            {
                writer.WriteStartElement("item");
                writer.WriteStartAttribute("label");
                writer.WriteString(labels[k]);
                writer.WriteEndAttribute();

                writer.WriteStartElement("data-file");
                writer.WriteStartAttribute("name");
                writer.WriteString(String.Format(data, itens[k]));
                writer.WriteEndAttribute();
                writer.WriteEndElement();

                writer.WriteStartElement("shape-file");
                writer.WriteStartAttribute("name");
                writer.WriteString(String.Format(shape, itens[k]));
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("exts");
                writer.WriteString(exts);
                writer.WriteEndAttribute();
                writer.WriteEndElement();

                if (metadata != null)
                {
                    writer.WriteStartElement("metadata");
                    writer.WriteString(String.Format(metadata, labels[k], itens[k]));
                    writer.WriteEndElement();
                } // if
                writer.WriteEndElement();
            } // for
            writer.WriteEndElement();
        } // createXmlDataItems()

        /// <summary>
        /// Creates the XML data configuration file with the stantard set of data files.
        /// </summary>
        public static void createXmlDataConfig()
        {
            string site = "http://www.ipea.gov.br/ipeageo/docs";
            string meta = null;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter writer = XmlWriter.Create("IPEAGEODataConfig.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("root");

            // Mesoregião
            meta = "DEFINIÇÃO: subidivisão dos estados brasileiros agregando municípios de uma determinada"
                + " área geográfica de acordo com suas características econômicas e sociais em comum.\n\n"
                //
                + "Dados sócio-econômicos referentes às mesorregiões brasileiras:\n\n"
                //
                + "Arquivos: mesoregiao.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                //
                + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                //
                + " - Cadastro Central de Empresa (2006 a 2011)\n"
                + " - Censo 2010 (Característica da População)\n"
                + " - Censo 2010 (Características da População Domiciliar)\n"
                + " - Censo 2010 (Características do Entorno)\n"
                + " - Censo 2010 (CNEFE)\n"
                + " - Censo 2010 (Deslocamento)\n"
                + " - Censo 2010 (Domicílios)\n"
                + " - Censo 2010 (Educação)\n"
                + " - Censo 2010 (Famílias)\n"
                + " - Censo 2010 (Fecundidade)\n"
                + " - Censo 2010 (Indicadores Sociais Municipais)\n"
                + " - Censo 2010 (Migração)\n"
                + " - Censo 2010 (Nupcialidade)\n"
                + " - Censo 2010 (Pessoas com Deficiência)\n"
                + " - Censo 2010 (Religião)\n"
                + " - Censo 2010 (Rendimento)\n"
                + " - Censo 2010 (Resultado Geral da Amostra)\n"
                + " - Censo 2010 (Sinopse)\n"
                + " - Censo 2010 (Trabalho Infantil)\n"
                + " - Censo 2010 (Trabalho)\n"
                + " - Censo Agropecuário 2006\n"
                + " - Estimativa da População (2011 a 2013)\n"
                + " - Extrativismo Vegetal e Silvicultura (2004 a 2011)\n"
                + " - Finanças Públicas (2005, 2006, 2008 e (2009)\n"
                + " - Frota (2005 a 2007, 2009 a 2012)\n"
                + " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
                + " - IDH Municipal (IDHM)\n"
                + " - Instituições Financeiras (2008 a 2012)\n"
                + " - Lavoura Permanente (2004 a 2012)\n"
                + " - Lavoura Temporária (2004 a 2012)\n"
                + " - Matricula Docentes e Rede Escolar (2005, 2007, 2009 e 2012)\n"
                + " - Morbidades Hospitalares (2005 a 2007, 2009 a 2012)\n"
                + " - Pecuária (2004 a 2012)\n"
                + " - PIB municipal (1999 a 2010)\n"
                + " - Pobreza e Desigualdade - Município (2003)\n"
                + " - Produção Agrícola Municipal (2007)\n"
                + " - Registro Civil (2004 a 2011)\n"
                + " - Representação Política (2004 e 2006)\n"
                + " - Saneamento Básico (2008)\n"
                + " - Serviço de Saúde (2005 e 2009)\n"
                + " - Síntese das Informações\n";
               
            createXmlDataResource(writer, "Mesoregião", site,
                "IBGE_CIDADES_MESOREGIAO.XLS", "mesoregiao", ".prj, .sbn, .sbx, .dbf, .shx, .shp", meta);

            // Microregião
              meta = "DEFINIÇÃO: subdivisão dos estados brasileiros agregando municípios limítrofes.\n\n"
			    //
				+ "Dados sócio-econômicos referentes às microrregiões brasileiras:\n\n"
				//
				+ "Arquivos: microregiao.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
				//
				+ "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
				//
				+ " - Cadastro Central de Empresa (2006 a 2011)\n"
				+ " - Censo 2010 (Característica da População)\n"
				+ " - Censo 2010 (Características da População Domiciliar)\n"
				+ " - Censo 2010 (Características do Entorno)\n"
				+ " - Censo 2010 (CNEFE)\n"
				+ " - Censo 2010 (Deslocamento)\n"
				+ " - Censo 2010 (Domicílios)\n"
				+ " - Censo 2010 (Educação)\n"
				+ " - Censo 2010 (Famílias)\n"
				+ " - Censo 2010 (Fecundidade)\n"
				+ " - Censo 2010 (Indicadores Sociais Municipais)\n"
				+ " - Censo 2010 (Migração)\n"
				+ " - Censo 2010 (Nupcialidade)\n"
				+ " - Censo 2010 (Pessoas com Deficiência)\n"
				+ " - Censo 2010 (Religião)\n"
				+ " - Censo 2010 (Rendimento)\n"
				+ " - Censo 2010 (Resultado Geral da Amostra)\n"
				+ " - Censo 2010 (Sinopse)\n"
				+ " - Censo 2010 (Trabalho Infantil)\n"
				+ " - Censo 2010 (Trabalho)\n"
				+ " - Censo Agropecuário 2006\n"
				+ " - Estimativa da População (2011 a 2013)\n"
				+ " - Extrativismo Vegetal e Silvicultura (2004 a 2011)\n"
				+ " - Finanças Públicas (2005, 2006, 2008 e 2009)\n"
				+ " - Frota (2005 a 2007, 2009 a 2012)\n"
				+ " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
				+ " - IDH Municipal (IDHM)\n"
				+ " - Instituições Financeiras (2008 a 2012)\n"
				+ " - Lavoura Permanente (2004 a 2012)\n"
				+ " - Lavoura Temporária (2004 a 2012)\n"
				+ " - Matricula Docentes e Rede Escolar (2005, 2007, 2009 e 2012)\n"
				+ " - Morbidades Hospitalares (2005 a 2007, 2009 a 2012)\n"
				+ " - Pecuária (2004 a 2012)\n"
				+ " - PIB municipal (1999 a 2010)\n"
				+ " - Pobreza e Desigualdade - Município (2003)\n"
				+ " - Produção Agrícola Municipal (2007)\n"
				+ " - Registro Civil (2004 a 2011)\n"
				+ " - Representação Política (2004 e 2006)\n"
				+ " - Saneamento Básico (2008)\n"
				+ " - Serviço de Saúde (2005 e 2009)\n"
				+ " - Síntese das Informações\n";

           createXmlDataResource(writer, "Microregião", site,
                "IBGE_CIDADES_MICROREGIAO.XLS", "microregiao", ".prj, .shx, .dbf, .shp", meta);

            // São Francisco
              meta = "DEFINIÇÃO: região da bacia hidrográfica do rio São Francisco, que compreende os estados de:"
                  + "Minas Gerais, Distrito Federal, Goiás, Bahia, Pernambuco, Sergipe, Alagoas.\n\n"
                  //
                  + "Dados sócio-econômicos referentes aos municípios da região do São Francisco:\n\n"
                  //
                  + "Arquivos: sao_francisco.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                  //
                  + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                  //
                  + " - Cadastro Central de Empresa (2007, 2009, 2010 e 2011)\n"
                  + " - Censo 2010 (Característica da População)\n"
                  + " - Censo 2010 (Pessoas com Deficiência)\n"
                  + " - Censo 2010 (Deslocamento)\n"
                  + " - Censo 2010 (Domicílios)\n"
                  + " - Censo 2010 (Famílias)\n"
                  + " - Censo 2010 (Fecundidade)\n"
                  + " - Censo 2010 (Migração)\n"
                  + " - Censo 2010 (Nupcialidade)\n"
                  + " - Censo 2010 (Religião)\n"
                  + " - Censo 2010 (Trabalho Infantil)\n"
                  + " - Censo 2010 (Indicadores Sociais Municipais)\n"
                  + " - Censo 2010 (Resultado Geral da Amostra)\n"
                  + " - Censo 2010 (Sinopse)\n"
                  + " - Censo Agropecuário 2006\n"
                  + " - Censo Demográfico 2010 (CNEFE)\n"
                  + " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
                  + " - Finanças Públicas (2005 e 2008)\n"
                  + " - Frota (2007, 2009 e 2010)\n"
                  + " - Instituições Financeiras (2006, 2009 a 2012)\n"
                  + " - Lavoura Permanente (2004 a 2011)\n"
                  + " - Lavoura Temporária (2004 a 2011)\n"
                  + " - Matricula Docentes e Rede Escolar (2005, 2007, 2009 e 2012)\n"
                  + " - Morbidades Hospitalares (2005 a 2007, 2009 a 2012)\n"
                  + " - Pobreza e Desigualdade - Município (2003)\n"
                  + " - Representação Política (2004 e 2006)\n"
                  + " - Serviço de Saúde (2005 e 2009)\n";

            createXmlDataResource(writer, "São Francisco", site,
                "CIDADES_SAO_FRANCISCO.XLS", "sao_francisco", ".prj, .sbn, .sbx, .dbf, .shx, .shp", meta);

            // Municípios (5507)
            meta = "Dados sócio-econômicos referentes ao Brasil divido em 5507 municípios:\n\n"
                //
                + "Arquivos: municipio.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                //
                + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                //
                + " - Cadastro Central de Empresas (2008)\n"
                + " - Censo Agropecuário 2006\n"
                + " - Ensino (2009)\n"
                + " - Extrativismo Vegetal e Silvicultura (2008)\n"
                + " - Finanças Públicas (2008)\n"
                + " - Frota (2009)\n"
                + " - Instituições Financeiras (2009)\n"
                + " - Lavoura Permanente (2008)\n"
                + " - Lavoura Temporária (2008)\n"
                + " - Morbidades Hospitalares (2009)\n"
                + " - Pecuária (2008)\n"
                + " - Pobreza e Desigualdade (2003)\n"
                + " - População Domicílios Censo (2000)\n"
                + " - Produção Agrícola (2007)\n"
                + " - Produto Interno Bruto (2007)\n"
                + " - Registro Civil (2008)\n"
                + " - Representação Política (2006)\n"
                + " - Saneamento Básico (2008)\n"
                + " - Serviços de Saúde (2005)\n"
                + " - SIM - óbitos e homicídios\n";

            createXmlDataResource(writer, "Municípios (5507)", site,
                "CIDADES_IPEAGEO.XLS", "municipio", ".prj, .shx, .dbf, .shp", meta);

            // Municipio (5564)
            meta = "Dados sócio-econômicos referentes ao Brasil divido em 5564 municípios:\n\n"
                //
                + "Arquivos: municipio5564.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                //
                + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                //
                + " - Cadastro Central de Empresa (2011)\n"
                + " - Censo 2010 (Característica da População)\n"
                + " - Censo 2010 (CNEFE)\n"
                + " - Censo 2010 (Pessoas com Deficiência)\n"
                + " - Censo 2010 (Deslocamento)\n"
                + " - Censo 2010 (Domicílios)\n"
                + " - Censo 2010 (Famílias)\n"
                + " - Censo 2010 (Fecundidade)\n"
                + " - Censo 2010 (Migração)\n"
                + " - Censo 2010 (Nupcialidade)\n"
                + " - Censo 2010 (Religião)\n"
                + " - Censo 2010 (Trabalho Infantil)\n"
                + " - Censo 2010 (Indicadores Sociais Municipais)\n"
                + " - Censo 2010 (Resultado Geral da Amostra)\n"
                + " - Censo 2010 (Sinopse)\n"
                + " - Censo 2010 (Característica do Entorno)\n"
                + " - Censo 2010 (Educação)\n"
                + " - Censo 2010 (Rendimento)\n"
                + " - Censo 2010 (Trabalho)\n"
                + " - Censo 2010 (Característica da População Domicilio)\n"
                + " - Censo Agropecuário 2006\n"
                + " - Estimação da população (2013)\n"
                + " - Extração Vegetal e Silvicultura (2011)\n"
                + " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
                + " - Finanças Públicas (2009)\n"
                + " - Frota (2012)\n"
                + " - IDH Municipal\n"
                + " - Instituições Financeiras (2012)\n"
                + " - Lavoura Permanente (2012)\n"
                + " - Lavoura Temporária (2012)\n"
                + " - Matricula Docentes e Rede Escolar (2012)\n"
                + " - Morbidades Hospitalares (2012)\n"
                + " - PIB Municipal (2010)\n"
                + " - Produção Agrícola Municipal (2007)\n"
                + " - Pecuária (2012)\n"
                + " - Pobreza e Desigualdade - Município (2003)\n"
                + " - Representação Política (2006)\n"
                + " - Registro Civil (2011)\n"
                + " - Saneamento Básico (2008)\n"
                + " - Serviço de Saúde 2009\n";

            createXmlDataResource(writer, "Municípios (5564)", site, 
                "IBGE_CIDADES_5564.XLS", "municipio5564", ".prj, .sbn, .sbx, .dbf, .shx, .shp", meta);

            // Regiões
              meta = "Dados sócio-econômicos referentes à {0} do Brasil:\n\n"
                  //
                  + "Arquivos: BR_MUN1997_{1}_region.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                  //
                  + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                  //
                  + " - Cadastro Central de Empresa (2006 a 2011)\n"
                  + " - Censo 2010 (Característica da População)\n"
                  + " - Censo 2010 (Características da População Domiciliar)\n"
                  + " - Censo 2010 (Características do Entorno)\n"
                  + " - Censo 2010 (CNEFE)\n"
                  + " - Censo 2010 (Deslocamento)\n"
                  + " - Censo 2010 (Domicílios)\n"
                  + " - Censo 2010 (Educação)\n"
                  + " - Censo 2010 (Famílias)\n"
                  + " - Censo 2010 (Fecundidade)\n"
                  + " - Censo 2010 (Indicadores Sociais Municipais)\n"
                  + " - Censo 2010 (Migração)\n"
                  + " - Censo 2010 (Nupcialidade)\n"
                  + " - Censo 2010 (Pessoas com Deficiência)\n"
                  + " - Censo 2010 (Religião)\n"
                  + " - Censo 2010 (Rendimento)\n"
                  + " - Censo 2010 (Resultado Geral da Amostra)\n"
                  + " - Censo 2010 (Sinopse)\n"
                  + " - Censo 2010 (Trabalho Infantil)\n"
                  + " - Censo 2010 (Trabalho)\n"
                  + " - Censo Agropecuário 2006\n"
                  + " - Estimativa da População (2011 a 2013)\n"
                  + " - Extrativismo Vegetal e Silvicultura (2004 a 2011)\n"
                  + " - Finanças Públicas (2005, 2006, 2008 e 2009)\n"
                  + " - Frota (2005 a 2007, 2009 a 2012)\n"
                  + " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
                  + " - IDH Municipal (IDHM)\n"
                  + " - Instituições Financeiras (2008 a 2012)\n"
                  + " - Lavoura Permanente (2004 a 2012)\n"
                  + " - Lavoura Temporária (2004 a 2012)\n"
                  + " - Matricula Docentes e Rede Escolar (2005, 2007, 2009 e 2012)\n"
                  + " - Morbidades Hospitalares (2005 a 2007, 2009 a 2012)\n"
                  + " - Pecuária (2004 a 2012)\n"
                  + " - PIB municipal (1999 a 2010)\n"
                  + " - Pobreza e Desigualdade - Município (2003)\n"
                  + " - Produção Agrícola Municipal (2007)\n"
                  + " - Registro Civil (2004 a 2011)\n"
                  + " - Representação Política (2004 e 2006)\n"
                  + " - Saneamento Básico (2008)\n"
                  + " - Serviço de Saúde (2005 e 2009)\n"
                  + " - Síntese das Informações\n";

            String[] regs = { "CO", "N", "NE", "S", "SE" };
            String[] rlabs = { "Região Centro-Oeste", "Região Norte", "Região Nordeste", "Região Sul", "Região Sudeste" };
            createXmlDataItems(writer, "Regiões", site, rlabs, regs, 
                "BR_MUN1997_{0}_region.xls", "BR_MUN1997_{0}_region", ".prj, .dbf, .shx, .shp", meta);

            // UFs
              meta = "Dados sócio-econômicos referentes aos municípios do(a) {0}\n\n"
                  //
                  + "Arquivos: {1}_Mun97_region.shp e arquivo em formato Excel com informações sócio-econômicas\n\n"
                  //
                  + "Tabelas no arquivo em Excel contém os seguintes tópicos:\n\n"
                  //
                  + " - Cadastro Central de Empresa (2006 a 2011)\n"
                  + " - Censo 2010 (Característica da População)\n"
                  + " - Censo 2010 (Características da População Domiciliar)\n"
                  + " - Censo 2010 (Características do Entorno)\n"
                  + " - Censo 2010 (CNEFE)\n"
                  + " - Censo 2010 (Deslocamento)\n"
                  + " - Censo 2010 (Domicílios)\n"
                  + " - Censo 2010 (Educação)\n"
                  + " - Censo 2010 (Famílias)\n"
                  + " - Censo 2010 (Fecundidade)\n"
                  + " - Censo 2010 (Indicadores Sociais Municipais)\n"
                  + " - Censo 2010 (Migração)\n"
                  + " - Censo 2010 (Nupcialidade)\n"
                  + " - Censo 2010 (Pessoas com Deficiência)\n"
                  + " - Censo 2010 (Religião)\n"
                  + " - Censo 2010 (Rendimento)\n"
                  + " - Censo 2010 (Resultado Geral da Amostra)\n"
                  + " - Censo 2010 (Sinopse)\n"
                  + " - Censo 2010 (Trabalho Infantil)\n"
                  + " - Censo 2010 (Trabalho)\n"
                  + " - Censo Agropecuário 2006\n"
                  + " - Estimativa da População (2011 a 2013)\n"
                  + " - Extrativismo Vegetal e Silvicultura (2004 a 2011)\n"
                  + " - Finanças Públicas (2005, 2006, 2008 e 2009)\n"
                  + " - Frota (2005 a 2007, 2009 a 2012)\n"
                  + " - Fundações Privadas e Associações sem Fins Lucrativos (2010)\n"
                  + " - IDH Municipal (IDHM)\n"
                  + " - Instituições Financeiras (2008 a 2012)\n"
                  + " - Lavoura Permanente (2004 a 2012)\n"
                  + " - Lavoura Temporária (2004 a 2012)\n"
                  + " - Matricula Docentes e Rede Escolar (2005, 2007, 2009 e 2012)\n"
                  + " - Morbidades Hospitalares (2005 a 2007, 2009 a 2012)\n"
                  + " - Pecuária (2004 a 2012)\n"
                  + " - PIB municipal (1999 a 2010)\n"
                  + " - Pobreza e Desigualdade - Município (2003)\n"
                  + " - Produção Agrícola Municipal (2007)\n"
                  + " - Registro Civil (2004 a 2011)\n"
                  + " - Representação Política (2004 e 2006)\n"
                  + " - Saneamento Básico (2008)\n"
                  + " - Serviço de Saúde (2005 e 2009)\n"
                  + " - Síntese das Informações\n";

            String[] ufs = { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            String[] rufs = { "Acre", "Alagoas "   , "Amapá",
                              "Amazonas", "Bahia " , "Ceará",
                              "Distrito Federal"   , "Espírito Santo", 
                              "Goiás", "Maranhão"  , "Mato Grosso", 
                              "Mato Grosso do Sul" , "Minas Gerais", 
                              "Pará", "Paraíba"    , "Paraná", 
                              "Pernambuco", "Piauí"    , "Rio de Janeiro", 
                              "Rio Grande do Norte", "Rio Grande do Sul", 
                              "Rondônia"            , "Roraima", 
                              "Santa Catarina"     , "São Paulo", 
                              "Sergipe"          , "Tocantins" };
            createXmlDataItems(writer, "Unidades da Federação", site, rufs, ufs, 
                "{0}_Mun97_region.xls", "{0}_Mun97_region", ".prj, .dbf, .shx, .shp", meta);

            // IDH
              meta = "Dados referentes ao índice de Desenvolvimento Humano (IDH) do ano {0}.\n\n"
                  //
                  + "O Índice de Desenvolvimento Humano (IDH) é uma mensuração criada com o "
                  + "intuito de analisar o impacto de fatores considerados essenciais no " 
                  + "nível de desenvolvimento e progresso de uma região.\n"
                  + "O cálculo aborda três questões principais: educação, saúde e renda. "
                  + "O IDH apresenta-se em contraste às informações expostas por outros indicadores, "
                  + "como o Produto Interno Bruto (PIB).\n\n"
                  //
                  + "Mapa subdividido em 5564 municípios.\n\n"
                  //
                  + "Para usar os dados de IDH por Unidade da Federação, utilize a busca por estados.\n\n"
                  //
                  + "Fonte: http://www.pnud.org.br/IDH/DH.aspx";
            
            string[] idhs = { "1991", "2000", "2010" };
            string[] ridhs = { "IDH-1991", "IDH-2000", "IDH-2010" };
            createXmlDataItems(writer, "IDH", site, ridhs, idhs, 
                "IDH_{0}.xls", "IDH_{0}", ".prj, .dbf, .shx, .shp", meta);/*".prj, .sbn, .sbx, .dbf, .shx, .shp", meta);*/

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        } // createXmlDataConfig()

        /// <summary>
        /// Parses the XML config file to a List.
        /// </summary>
        /// <returns>the list with all resources.</returns>
        public static List<DataResource> parseXmlDataConfig()
        {
            string xml = null;
            List<DataResource> resources = new List<DataResource>();
            List<DataResourceItem> items = new List<DataResourceItem>();
            bool isResource = false, isItem = false, isMetadata = false;

            using (StreamReader sr = new StreamReader("IPEAGEODataConfig.xml", Encoding.GetEncoding("utf-8")))
            {
                xml = sr.ReadToEnd();
                
                 
            } // using

            // Create a XML reader.
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                DataResource resource = new DataResource();
                DataResourceItem item = new DataResourceItem();

                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "resource":
                                    isResource = true;
                                    resource.label = resource.url = resource.dataFile = resource.shapeFile = null;
                                    resource.shapeExt = null;

                                    if (reader.HasAttributes)
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.Equals("label"))
                                                resource.label = reader.Value;
                                            else if (reader.Name.Equals("link"))
                                                resource.url = reader.Value;
                                        } // while
                                        reader.MoveToElement();
                                    } // if
                                    break;
                                case "base-url":
                                    if (isResource)
                                    {
                                        if (reader.HasAttributes)
                                        {
                                            while (reader.MoveToNextAttribute())
                                            {
                                                if (reader.Name.Equals("link"))
                                                    resource.url = reader.Value;
                                            } // while
                                            reader.MoveToElement();
                                        } // if
                                    } // if
                                    break;
                                case "item":
                                    isItem = true;
                                    item.label = item.dataFile = item.shapeFile = null;
                                    item.shapeExt = null;

                                    if (reader.HasAttributes)
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.Equals("label"))
                                                item.label = reader.Value;
                                        } // while
                                        reader.MoveToElement();
                                    } // if
                                    break;
                                case "data-file":
                                    if (reader.HasAttributes)
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.Equals("name"))
                                                if (isResource)
                                                    if (isItem)
                                                        item.dataFile = reader.Value;
                                                    else
                                                        resource.dataFile = reader.Value;
                                        } // while
                                        reader.MoveToElement();
                                    } // if
                                    break;
                                case "metadata":
                                    if (isResource || isItem) isMetadata = true;
                                    break;
                                case "shape-file":
                                    if (reader.HasAttributes)
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name.Equals("name"))
                                            {
                                                if (isResource)
                                                    if (isItem)
                                                        item.shapeFile = reader.Value;
                                                    else
                                                        resource.shapeFile = reader.Value;
                                            } // if
                                            else if (reader.Name.Equals("exts"))
                                            {
                                                char[] seps = { ',' };
                                                if (isResource)
                                                    if (isItem)
                                                    {
                                                        item.shapeExt = reader.Value.Split(seps);
                                                        for (int k = 0; k < item.shapeExt.Length; k++)
                                                            item.shapeExt[k] = item.shapeExt[k].Trim();
                                                    } // if
                                                    else
                                                    {
                                                        resource.shapeExt = reader.Value.Split(seps);
                                                        for (int k = 0; k < resource.shapeExt.Length; k++)
                                                            resource.shapeExt[k] = resource.shapeExt[k].Trim();
                                                    } // else
                                            } // else if
                                        } // while
                                        reader.MoveToElement();
                                    } // if
                                    break;
                            } // switch
                            break;
                        case XmlNodeType.Text:
                            if (isResource && isMetadata)
                                if (isItem)
                                    item.metadata = reader.Value;
                                else
                                    resource.metadata = reader.Value;
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name.Equals("item"))
                            {
                                isItem = false;
                                items.Add(item);
                                item = new DataResourceItem();
                            } // if
                            else if (reader.Name.Equals("resource"))
                            {
                                isResource = false;
                                if (items.Count > 0)
                                    resource.items = items.ToArray();
                                else
                                    resource.items = null;

                                items.Clear();
                                resources.Add(resource);
                            } // else if
                            else if (reader.Name.Equals("metadata"))
                                isMetadata = false;
                            break;
                    } // switch
                } // while
            } // using
            return resources;
        } // parseXmlDataConfig()

        /// <summary>
        /// Open a dataset and shape specified in a DataResource object.
        /// </summary>
        /// <param name="resource">the data/shape specification.</param>
        public static bool openData(DataResource resource)
        {
            return openData(resource.url, resource.dataFile, resource.shapeFile, resource.shapeExt);
        } // openData()

        /// <summary>
        /// Open a dataset and shape specified in a DataResourceItem object.
        /// </summary>
        /// <param name="item">the data/shape specification.</param>
        /// <param name="url">the base url</param>
        public static bool openData(DataResourceItem item, string url)
        {
            return openData(url, item.dataFile, item.shapeFile, item.shapeExt);
        } // openData()

        /// <summary>
        /// Open a dataset and shape specified using the Downloader class.
        /// </summary>
        /// <param name="url">the base url</param>
        /// <param name="dataFile">the data file</param>
        /// <param name="shapeFile">the shapefile without extension</param>
        /// <param name="exts">the list of extensiont to the shapefile</param>
        private static bool openData(string url, string dataFile, string shapeFile, string[] exts)
        {
            bool loaded = false;

            string localData = String.Format(@"{0}\bases\{1}",
                Path.GetDirectoryName(Application.ExecutablePath), dataFile);
            string remoteData = String.Format(@"{0}/{1}", url, dataFile);

            string[] localShape = new string[exts.Length];
            string[] remoteShape = new string[exts.Length];
            for (int k = 0; k < exts.Length; k++)
            {
                localShape[k] = String.Format(@"{0}\malhas\{1}{2}",
                    Path.GetDirectoryName(Application.ExecutablePath), shapeFile, exts[k]);
                remoteShape[k] = String.Format(@"{0}/{1}{2}", url, shapeFile, exts[k]);
            } // for

            // Verifica a existência dos arquivos locais.
            List<Downloader> list = new List<Downloader>();

            bool hasDataFolder = Directory.Exists(Path.GetDirectoryName(localData));
            bool hasData = File.Exists(localData);
            if (!hasData || !hasDataFolder) list.Add(new Downloader(localData, remoteData));
            if (!hasDataFolder) Directory.CreateDirectory(Path.GetDirectoryName(localData));

            for (int k = 0; k < localShape.Length; k++)
            {
                bool hasShapeFolder = Directory.Exists(Path.GetDirectoryName(localShape[k]));
                bool hasShape = File.Exists(localShape[k]);
                if (!hasShape || !hasShapeFolder) list.Add(new Downloader(localShape[k], remoteShape[k]));
                if (!hasShapeFolder) Directory.CreateDirectory(Path.GetDirectoryName(localShape[k]));
            } // for

            if (list.Count == 0) return true;

            try
            {
                // Solicita autorização para o download.
                DialogResult result =
                    MessageBox.Show("Alguns arquivos de dados ou malha digital não existem em seu computador. Deseja fazer o download do site do Ipea?",
                        "Baixar Arquivo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Inicia um dowload em cada Thread.
                    List<Thread> threads = new List<Thread>();

                    foreach (Downloader d in list) threads.Add(new Thread(d.download));

                    Cursor.Current = Cursors.WaitCursor;
                    while (threads.Count > 0)
                    {
                        for (int k = 0; k < threads.Count; k++)
                        {
                            switch (threads[k].ThreadState)
                            {
                                case ThreadState.Unstarted:
                                    threads[k].Start();
                                    break;
                                case ThreadState.Stopped:
                                    if (list[k].error)
                                        MessageBox.Show(String.Format("{0}\n\n{1}",
                                            list[k].errorMessage, list[k].remote),
                                            "Falha de download", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    list.RemoveAt(k);
                                    threads.RemoveAt(k--);
                                    break;
                            } // switch
                        } // for
                    } // while
                    Cursor.Current = Cursors.Default;
                } // if
            } // try
            finally { loaded = true; }

            return loaded;
        } // openData() 
    } // IpeaDataSelection

    public class Downloader
    {
        public string local, remote;
        public bool error = false;
        public string errorMessage = null;

        public Downloader(string local, string remote)
        {
            this.local = local; this.remote = remote;
        }

        public void download()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Headers.Add("User-Agent", "Other");

            try { client.DownloadFile(remote, local); }
            catch (Exception ex)
            {
                error = true;
                errorMessage = ex.Message;
            } // catch
        } // download()
    } // class
} // namespace
