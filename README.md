# IpeaGEO
<img align="right" src="https://github.com/ipeadata-lab/IpeaGeo/blob/main/IpeaGEO/images/Icones_PNG/Ipea%20geo_principal.png?raw=true" alt="logo" width="150"> 

**IpeaGEO** é um software gratuito para análises de estatística e econometria espacial. O software foi desenvolvido pelo Ipea entre 2008 e 2015, e seu desenvolvimento foi descontinuado. Desenvolvido inicialmente para uso nas pesquisas internas do Instituto, o software foi disponibilizado para o público em geral em agosto de 2010. O objetivo deste repositório é preservar uma versão pública e aberta do código fonte. Veja tutoriais e versão mais recente do arquivo executável no portal do projeto: https://www.ipea.gov.br/ipeageo. O IpeaGEO conta com uma grande variedade de funcionalidades estatísticas, econométricas, e de análise de dados espaciais. 

**Algumas Referências de Metodologias Desenvolvidas no IpeaGEO:**
- Mapeamento de taxas Bayesianas, em https://repositorio.ipea.gov.br/handle/11058/1181
- Índices de segregação espacial, em https://repositorio.ipea.gov.br/handle/11058/1203
- Econometria espacial para dados cross-section, em https://repositorio.ipea.gov.br/handle/11058/1394
- Clusters espaciais e não espaciais para a agricultura brasileira, em https://repositorio.ipea.gov.br/bitstream/11058/7554/1/TD_2279.pdf
- Clusterização hierárquica espacial, em https://repositorio.ipea.gov.br/handle/11058/2594
- Clusterização hierárquica espacial com atributos binários, em https://repositorio.ipea.gov.br/handle/11058/2585

**Alternativas ao IpeaGEO:**
- Dados espaciais do Brasil: pacote de R [**geobr**](https://ipeagit.github.io/geobr/), para download de dados espaciais oficiais do Brasil.
- Estatística e econometria espaciail
    - Python: [PySAL: Python Spatial Analysis Library](https://pysal.org/pysal/)
    - R: [spdep](https://r-spatial.github.io/spdep/) e [spatialreg](https://r-spatial.github.io/spatialreg/)

## Descrição:
O [IpeaGEO](https://www.ipea.gov.br/ipeageo/) é um software gratuito projetado para análise estatística e georreferenciamento. Desenvolvido pela Assessoria de Métodos Quantitativos (ASMEQ) da Diretoria de Estudos Regionais, Urbanos e Ambientais (DIRUR) do Instituto de Pesquisa Econômica Aplicada (Ipea), esta ferramenta simplifica e amplia a aplicação de técnicas econométricas espaciais. É ideal para estudantes, educadores, acadêmicos e empresas envolvidas em estatísticas, econometria e georreferenciamento.

## Versão:
IpeaGEO Versão 2.1.15_06_26, atualizado em 28 de julho de 2015
               
## Recursos:
- Ferramentas abrangentes de estatísticas e econometria
- Capacidades de análise estatística espacial
- Acesso a bases de dados incluindo dados de censos e Índice de Desenvolvimento Humano (IDH)
- Adaptação de dados para diversas regiões nacionais

## Funcionalidades Detalhadas:
- Análise Exploratória dos Dados e Análise Gráfica
- Correlações e Estatísticas Descritivas
- Geração de Variáveis Dummy 
- Matrizes de Vizinhança
- Tabelas de Frequência e Tabulações Cruzadas
- Análise Multivariada, Análise de Clusters, Análise de Componentes Principais, Análise Fatorial
- Cálculo de Taxas Bayesianas 
- Propensity Score Matching
- Regressão com Dados Binários, Regressão Linear, e Modelos Lineares Generalizados
- Análise e Testes para Médias
- Distribuições Contínuas e Discretas
- Métodos Multicritérios de Apoio às Decisões
- Funções Espaciais de Manipulação de Geometrias 
- Áreas Mínimas Comparáveis para Construção de Painéis de Dados Municipais
- Conglomerados Espaciais Hierárquicos 
- Índices Globais e Locais de Dependência Espacial
- Métodos de Econometria Espacial
- Indicadores de Segregação Espacial

## Tutorial: 
A última versão do tutorial, disponibilizado pelo Ipea, está disponível em https://www.ipea.gov.br/ipeageo/arquivos/Tutorial_IpeaGEO_VF.pdf. 

## Licença:
O IpeaGEO é distribuído sob a Licença Pública Geral GNU versão 3, conforme publicada pela Free Software Foundation. É fornecido SEM GARANTIA, sem sequer a garantia implícita de COMERCIALIZAÇÃO ou ADEQUAÇÃO A UM PROPÓSITO ESPECÍFICO.

## Componentes Utilizados:
- Random Project
- NetTopology
- ZedGraph
- XPTabControlCS
- SharpMap
- ITextSharp
- Combinatorial
<img align="right" src="https://github.com/ipeadata-lab/IpeaGeo/blob/main/IpeaGEO/images/Icones_PNG/logo_ipea_GEO_v_2_0.png?raw=true" alt="logo" width="180"> 

## Líderes Técnicos:
- Alexandre Xavier Ywata de Carvalho, Ph.D.
- Pedro Henrique Melo Albuquerque, D.Sc.

## Equipe de Desenvolvimento:
- Alex Rodrigues do Nascimento
- Camilo Rey Laureto
- Cauê de Castro Dobbin
- Demerson André Polli
- Fabio Augusto Scalet Medina
- Gabriela Drummond Marques da Silva
- Gilberto Rezende de Almeida Júnior
- Guilherme Costa Chadud Moreira
- Gustavo Gomes Basso
- Igor Ferreira do Nascimento
- Luis Felipe Biato de Carvalho
- Luiz Felipe Dantas Guimarães
- Marcius Correia Lima Filho
- Mariana Rosa Montenegro
- Marina Garcia Pena
- Rafael Dantas Guimarães

## Mudanças na Versão 2.1.15_06_26 Versão:
- Novo output para o formulário "Cluster Espacial Hierárquico"
- Implementação dos dados Finbra de 2000 a 2012
- Adição de variáveis de cluster na tabela de dados
- Ajustes de parâmetros padrão no formulário de Cluster Espacial Hierárquico
- Diversas correções de bugs

## Dados:
Algumas bases de dados digitais e malhas podem ser baixadas diretamente do site do IpeaGEO ou através do próprio software, cobrindo dados socioeconômicos de todas as regiões brasileiras.

## Aviso Legal:
Copyright (C) 2010-2015 Instituto de Pesquisa Econômica Aplicada
Todos os direitos reservados.

Consulte os arquivos LICENSE.pdf e Tutorial.pdf no site do IpeaGEO para informações detalhadas sobre licenciamento e orientações para os usuários.

Obrigado por escolher o IpeaGEO para suas necessidades de análise espacial!

