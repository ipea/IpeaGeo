namespace IpeaGeo.Modelagem
{
    partial class FormRegressaoDadosBinarios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegressaoDadosBinarios));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlRegressaoInstrumentos1 = new IpeaGeo.UserControlRegressaoInstrumentos();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownCT = new System.Windows.Forms.NumericUpDown();
            this.ckbClassificationTable = new System.Windows.Forms.CheckBox();
            this.grbGMM = new System.Windows.Forms.GroupBox();
            this.rdbCompLogLog = new System.Windows.Forms.RadioButton();
            this.rdbLinkLogit = new System.Windows.Forms.RadioButton();
            this.rdbProbit = new System.Windows.Forms.RadioButton();
            this.ckbAnaliseResiduos = new System.Windows.Forms.CheckBox();
            this.ckbObservacoesInfluente = new System.Windows.Forms.CheckBox();
            this.ckbMulticolinearidade = new System.Windows.Forms.CheckBox();
            this.ckbIncluirNovasVariaveisTabelaDados = new System.Windows.Forms.CheckBox();
            this.ckbApresentaCovMatrixBetaHat = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaNovasVariaveis = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaOutput = new System.Windows.Forms.CheckBox();
            this.ckbIncluiIntercepto = new System.Windows.Forms.CheckBox();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput2 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCT)).BeginInit();
            this.grbGMM.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lblProgressBar);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Size = new System.Drawing.Size(1070, 639);
            this.splitContainer1.SplitterDistance = 603;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1070, 603);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1062, 576);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabela de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userControlDataGrid1
            // 
            this.userControlDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataGrid1.ListaVarsNumericas = new string[0];
            this.userControlDataGrid1.ListaVarsTotais = new string[0];
            this.userControlDataGrid1.Location = new System.Drawing.Point(3, 3);
            this.userControlDataGrid1.Name = "userControlDataGrid1";
            clsIpeaShape1.CoordenadasEmRadianos = false;
            clsIpeaShape1.Count = 0;
            clsIpeaShape1.HoraCriacao = new System.DateTime(2013, 3, 5, 16, 55, 47, 445);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(1056, 570);
            this.userControlDataGrid1.TabControl = null;
            this.userControlDataGrid1.TabIndex = 2;
            this.userControlDataGrid1.UserControlPropScoreMatching = null;
            this.userControlDataGrid1.UserControlRegInstrumentos = null;
            this.userControlDataGrid1.UserControlSelecao2BlocosVariaveis = null;
            this.userControlDataGrid1.UserControlSelecaoVariaveis = null;
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.splitContainer2);
            this.tabPage0.ImageIndex = 2;
            this.tabPage0.Location = new System.Drawing.Point(4, 23);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Size = new System.Drawing.Size(1062, 576);
            this.tabPage0.TabIndex = 3;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userControlRegressaoInstrumentos1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(1062, 576);
            this.splitContainer2.SplitterDistance = 759;
            this.splitContainer2.TabIndex = 0;
            // 
            // userControlRegressaoInstrumentos1
            // 
            this.userControlRegressaoInstrumentos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRegressaoInstrumentos1.Location = new System.Drawing.Point(0, 0);
            this.userControlRegressaoInstrumentos1.Name = "userControlRegressaoInstrumentos1";
            this.userControlRegressaoInstrumentos1.Size = new System.Drawing.Size(759, 576);
            this.userControlRegressaoInstrumentos1.TabIndex = 1;
            this.userControlRegressaoInstrumentos1.VariaveisDB = null;
            this.userControlRegressaoInstrumentos1.VariaveisIndependentes = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisInstrumentais = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisList = null;
            this.userControlRegressaoInstrumentos1.VariavelDependente = new string[0];
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDownCT);
            this.groupBox2.Controls.Add(this.ckbClassificationTable);
            this.groupBox2.Controls.Add(this.grbGMM);
            this.groupBox2.Controls.Add(this.ckbAnaliseResiduos);
            this.groupBox2.Controls.Add(this.ckbObservacoesInfluente);
            this.groupBox2.Controls.Add(this.ckbMulticolinearidade);
            this.groupBox2.Controls.Add(this.ckbIncluirNovasVariaveisTabelaDados);
            this.groupBox2.Controls.Add(this.ckbApresentaCovMatrixBetaHat);
            this.groupBox2.Controls.Add(this.ckbLimpaJanelaNovasVariaveis);
            this.groupBox2.Controls.Add(this.ckbLimpaJanelaOutput);
            this.groupBox2.Controls.Add(this.ckbIncluiIntercepto);
            this.groupBox2.Controls.Add(this.btnExecutar);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 576);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 393);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 30;
            this.textBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 328);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Probabilidade de Corte";
            // 
            // numericUpDownCT
            // 
            this.numericUpDownCT.DecimalPlaces = 2;
            this.numericUpDownCT.Enabled = false;
            this.numericUpDownCT.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCT.Location = new System.Drawing.Point(159, 326);
            this.numericUpDownCT.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCT.Name = "numericUpDownCT";
            this.numericUpDownCT.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownCT.TabIndex = 28;
            this.numericUpDownCT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownCT.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownCT.ValueChanged += new System.EventHandler(this.numericUpDownCT_ValueChanged);
            // 
            // ckbClassificationTable
            // 
            this.ckbClassificationTable.AutoSize = true;
            this.ckbClassificationTable.Location = new System.Drawing.Point(21, 303);
            this.ckbClassificationTable.Name = "ckbClassificationTable";
            this.ckbClassificationTable.Size = new System.Drawing.Size(139, 17);
            this.ckbClassificationTable.TabIndex = 27;
            this.ckbClassificationTable.Text = "Tabela de Classificação";
            this.ckbClassificationTable.UseVisualStyleBackColor = true;
            this.ckbClassificationTable.CheckedChanged += new System.EventHandler(this.ckbClassificationTable_CheckedChanged);
            // 
            // grbGMM
            // 
            this.grbGMM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbGMM.Controls.Add(this.rdbCompLogLog);
            this.grbGMM.Controls.Add(this.rdbLinkLogit);
            this.grbGMM.Controls.Add(this.rdbProbit);
            this.grbGMM.Location = new System.Drawing.Point(21, 113);
            this.grbGMM.Name = "grbGMM";
            this.grbGMM.Size = new System.Drawing.Size(252, 106);
            this.grbGMM.TabIndex = 26;
            this.grbGMM.TabStop = false;
            this.grbGMM.Text = "Funções de ligação";
            // 
            // rdbCompLogLog
            // 
            this.rdbCompLogLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbCompLogLog.AutoSize = true;
            this.rdbCompLogLog.Location = new System.Drawing.Point(23, 73);
            this.rdbCompLogLog.Name = "rdbCompLogLog";
            this.rdbCompLogLog.Size = new System.Drawing.Size(131, 17);
            this.rdbCompLogLog.TabIndex = 6;
            this.rdbCompLogLog.Text = "Complementary log-log";
            this.rdbCompLogLog.UseVisualStyleBackColor = true;
            // 
            // rdbLinkLogit
            // 
            this.rdbLinkLogit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbLinkLogit.AutoSize = true;
            this.rdbLinkLogit.Checked = true;
            this.rdbLinkLogit.Location = new System.Drawing.Point(23, 27);
            this.rdbLinkLogit.Name = "rdbLinkLogit";
            this.rdbLinkLogit.Size = new System.Drawing.Size(48, 17);
            this.rdbLinkLogit.TabIndex = 4;
            this.rdbLinkLogit.TabStop = true;
            this.rdbLinkLogit.Text = "Logit";
            this.rdbLinkLogit.UseVisualStyleBackColor = true;
            // 
            // rdbProbit
            // 
            this.rdbProbit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbProbit.AutoSize = true;
            this.rdbProbit.Location = new System.Drawing.Point(23, 50);
            this.rdbProbit.Name = "rdbProbit";
            this.rdbProbit.Size = new System.Drawing.Size(52, 17);
            this.rdbProbit.TabIndex = 3;
            this.rdbProbit.Text = "Probit";
            this.rdbProbit.UseVisualStyleBackColor = true;
            // 
            // ckbAnaliseResiduos
            // 
            this.ckbAnaliseResiduos.AutoSize = true;
            this.ckbAnaliseResiduos.Location = new System.Drawing.Point(21, 257);
            this.ckbAnaliseResiduos.Name = "ckbAnaliseResiduos";
            this.ckbAnaliseResiduos.Size = new System.Drawing.Size(124, 17);
            this.ckbAnaliseResiduos.TabIndex = 24;
            this.ckbAnaliseResiduos.Text = "Análise dos resíduos";
            this.ckbAnaliseResiduos.UseVisualStyleBackColor = true;
            this.ckbAnaliseResiduos.CheckedChanged += new System.EventHandler(this.ckbAnaliseResiduos_CheckedChanged);
            // 
            // ckbObservacoesInfluente
            // 
            this.ckbObservacoesInfluente.AutoSize = true;
            this.ckbObservacoesInfluente.Enabled = false;
            this.ckbObservacoesInfluente.Location = new System.Drawing.Point(21, 280);
            this.ckbObservacoesInfluente.Name = "ckbObservacoesInfluente";
            this.ckbObservacoesInfluente.Size = new System.Drawing.Size(187, 17);
            this.ckbObservacoesInfluente.TabIndex = 23;
            this.ckbObservacoesInfluente.Text = "Análise de observações influentes";
            this.ckbObservacoesInfluente.UseVisualStyleBackColor = true;
            // 
            // ckbMulticolinearidade
            // 
            this.ckbMulticolinearidade.AutoSize = true;
            this.ckbMulticolinearidade.Location = new System.Drawing.Point(21, 78);
            this.ckbMulticolinearidade.Name = "ckbMulticolinearidade";
            this.ckbMulticolinearidade.Size = new System.Drawing.Size(162, 17);
            this.ckbMulticolinearidade.TabIndex = 22;
            this.ckbMulticolinearidade.Text = "Análise de multicolinearidade";
            this.ckbMulticolinearidade.UseVisualStyleBackColor = true;
            // 
            // ckbIncluirNovasVariaveisTabelaDados
            // 
            this.ckbIncluirNovasVariaveisTabelaDados.AutoSize = true;
            this.ckbIncluirNovasVariaveisTabelaDados.Location = new System.Drawing.Point(21, 479);
            this.ckbIncluirNovasVariaveisTabelaDados.Name = "ckbIncluirNovasVariaveisTabelaDados";
            this.ckbIncluirNovasVariaveisTabelaDados.Size = new System.Drawing.Size(232, 17);
            this.ckbIncluirNovasVariaveisTabelaDados.TabIndex = 21;
            this.ckbIncluirNovasVariaveisTabelaDados.Text = "Mostrar novas variáveis na tabela de dados";
            this.ckbIncluirNovasVariaveisTabelaDados.UseVisualStyleBackColor = true;
            // 
            // ckbApresentaCovMatrixBetaHat
            // 
            this.ckbApresentaCovMatrixBetaHat.AutoSize = true;
            this.ckbApresentaCovMatrixBetaHat.Location = new System.Drawing.Point(21, 55);
            this.ckbApresentaCovMatrixBetaHat.Name = "ckbApresentaCovMatrixBetaHat";
            this.ckbApresentaCovMatrixBetaHat.Size = new System.Drawing.Size(257, 17);
            this.ckbApresentaCovMatrixBetaHat.TabIndex = 20;
            this.ckbApresentaCovMatrixBetaHat.Text = "Apresenta matriz de covariância dos coeficientes";
            this.ckbApresentaCovMatrixBetaHat.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaNovasVariaveis
            // 
            this.ckbLimpaJanelaNovasVariaveis.AutoSize = true;
            this.ckbLimpaJanelaNovasVariaveis.Checked = true;
            this.ckbLimpaJanelaNovasVariaveis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaNovasVariaveis.Location = new System.Drawing.Point(21, 456);
            this.ckbLimpaJanelaNovasVariaveis.Name = "ckbLimpaJanelaNovasVariaveis";
            this.ckbLimpaJanelaNovasVariaveis.Size = new System.Drawing.Size(252, 17);
            this.ckbLimpaJanelaNovasVariaveis.TabIndex = 19;
            this.ckbLimpaJanelaNovasVariaveis.Text = "Limpar janela novas variáveis a cada estimação";
            this.ckbLimpaJanelaNovasVariaveis.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaOutput
            // 
            this.ckbLimpaJanelaOutput.AutoSize = true;
            this.ckbLimpaJanelaOutput.Checked = true;
            this.ckbLimpaJanelaOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaOutput.Location = new System.Drawing.Point(21, 433);
            this.ckbLimpaJanelaOutput.Name = "ckbLimpaJanelaOutput";
            this.ckbLimpaJanelaOutput.Size = new System.Drawing.Size(177, 17);
            this.ckbLimpaJanelaOutput.TabIndex = 18;
            this.ckbLimpaJanelaOutput.Text = "Limpar output a cada estimação";
            this.ckbLimpaJanelaOutput.UseVisualStyleBackColor = true;
            // 
            // ckbIncluiIntercepto
            // 
            this.ckbIncluiIntercepto.AutoSize = true;
            this.ckbIncluiIntercepto.Checked = true;
            this.ckbIncluiIntercepto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbIncluiIntercepto.Location = new System.Drawing.Point(21, 32);
            this.ckbIncluiIntercepto.Name = "ckbIncluiIntercepto";
            this.ckbIncluiIntercepto.Size = new System.Drawing.Size(101, 17);
            this.ckbIncluiIntercepto.TabIndex = 17;
            this.ckbIncluiIntercepto.Text = "Inclui intercepto";
            this.ckbIncluiIntercepto.UseVisualStyleBackColor = true;
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(200, 531);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 4;
            this.btnExecutar.Text = "&Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 9;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1062, 576);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1056, 570);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1062, 576);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 7;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1062, 576);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Variáveis Geradas";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(1062, 576);
            this.userControlRichTextOutput2.TabIndex = 1;
            this.userControlRichTextOutput2.Texto = "";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Ipea_GEO.ico");
            this.imageList1.Images.SetKeyName(1, "application_cascade.png");
            this.imageList1.Images.SetKeyName(2, "application_form.png");
            this.imageList1.Images.SetKeyName(3, "chart_curve.png");
            this.imageList1.Images.SetKeyName(4, "chart_bar.png");
            this.imageList1.Images.SetKeyName(5, "photos.png");
            this.imageList1.Images.SetKeyName(6, "map.png");
            this.imageList1.Images.SetKeyName(7, "databases.png");
            this.imageList1.Images.SetKeyName(8, "database.png");
            this.imageList1.Images.SetKeyName(9, "page_refresh.png");
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(484, 10);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(412, 10);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.Black;
            this.lblProgressBar.Location = new System.Drawing.Point(136, 7);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(267, 13);
            this.lblProgressBar.TabIndex = 9;
            this.lblProgressBar.Text = "Rotinas para análise de regressão com resposta binária";
            this.lblProgressBar.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(967, 1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&Atualizar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(28, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Fechar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormRegressaoDadosBinarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 639);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(991, 620);
            this.Name = "FormRegressaoDadosBinarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modelos de Regressão com Resposta Binária";
            this.Load += new System.EventHandler(this.FormBaseModelagem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCT)).EndInit();
            this.grbGMM.ResumeLayout(false);
            this.grbGMM.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressBar;
        private UserControlRegressaoInstrumentos userControlRegressaoInstrumentos1;
        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.TabPage tabPage4;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.CheckBox ckbMulticolinearidade;
        private System.Windows.Forms.CheckBox ckbIncluirNovasVariaveisTabelaDados;
        private System.Windows.Forms.CheckBox ckbApresentaCovMatrixBetaHat;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaNovasVariaveis;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaOutput;
        private System.Windows.Forms.CheckBox ckbIncluiIntercepto;
        private System.Windows.Forms.CheckBox ckbAnaliseResiduos;
        private System.Windows.Forms.CheckBox ckbObservacoesInfluente;
        private System.Windows.Forms.GroupBox grbGMM;
        private System.Windows.Forms.RadioButton rdbCompLogLog;
        private System.Windows.Forms.RadioButton rdbLinkLogit;
        private System.Windows.Forms.RadioButton rdbProbit;
        private System.Windows.Forms.NumericUpDown numericUpDownCT;
        private System.Windows.Forms.CheckBox ckbClassificationTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
    }
}