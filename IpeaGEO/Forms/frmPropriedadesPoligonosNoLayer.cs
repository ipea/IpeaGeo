using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Forms
{
    public partial class frmPropriedadesPoligonosNoLayer : Form
    {
        public frmPropriedadesPoligonosNoLayer()
        {
            InitializeComponent();
        }

        #region variáveis internas

        private string m_tipo_geometria_do_layer = "Polígonos";
        public string TipoGeometriaLayer
        {
            get { return m_tipo_geometria_do_layer; }
            set { m_tipo_geometria_do_layer = value; }
        }

        private double m_zoom_value_extended = 10.0;
        public double ZoomValueExtended
        {
            get { return m_zoom_value_extended; }
            set { m_zoom_value_extended = value; }
        }

        private int m_num_elementos_layer = 0;

        private ArrayList m_elementos_selecionados = new ArrayList();
        private ArrayList m_colunas_chave = new ArrayList();
        private string[] m_chaves_rows = new string[0];

        private double m_zoom_original_min = 0.0;
        private double m_zoom_original_max = 0.0;

        private SharpMap.Map m_mapa = new SharpMap.Map();
        public SharpMap.Map Mapa
        {
            get { return m_mapa; }
            set 
            { 
                m_mapa = value;
                if (m_layer_selecionado != "")
                {
                    int ind_selected_label = -1;
                    string nome_layer = "";
                    for (int i = 0; i < m_mapa.Layers.Count; i++)
                    {
                        nome_layer = ("Labels_aux_" + m_layer_selecionado);
                        if (m_mapa.Layers[i].LayerName == nome_layer)
                        {
                            ind_selected_label = i;
                            break;
                        }
                    }

                    SharpMap.Layers.VectorLayer l1 = (SharpMap.Layers.VectorLayer)m_mapa.Layers[m_layer_selecionado];
                    m_num_elementos_layer = l1.DataSource.GetFeatureCount();

                    m_zoom_original_max = l1.MaxVisible;
                    m_zoom_original_min = l1.MinVisible;

                    clsUtilTools clt = new clsUtilTools();
                    m_colunas_chave = clt.RetornaIndicesColunasChavesPrimarias(m_variaveis_dados_shape);
                    if (m_colunas_chave.Count <= 0)
                    {
                        m_colunas_chave.Add(0);
                    }

                    dataGridView1.DataSource = m_variaveis_dados_shape;
                    dataGridView1.Refresh();

                    m_chaves_rows = new string[m_variaveis_dados_shape.Rows.Count];
                    StringBuilder v = new StringBuilder();
                    for (int j = 0; j < m_chaves_rows.GetLength(0); j++)
                    {
                        v.Clear();
                        for (int i = 0; i < m_colunas_chave.Count; i++)
                        {
                            v.Append(m_variaveis_dados_shape.Rows[j][(int)m_colunas_chave[i]]);
                        }
                        m_chaves_rows[j] = v.ToString();
                    }

                    if (ind_selected_label < 0)
                    {
                        #region opções para os labels do mapa

                        m_layer_com_labels = false;
                        
                        double vmax = m_zoom_value_extended / 2.0;
                        double vmin = 0.0;

                        txtValueZoomMap.Text = clt.Double2Texto(m_zoom_value_extended, 4);
                        nudMaxValueZoomLabels.Value = Convert.ToDecimal(vmax);
                        nudMinValueZoomLabels.Value = Convert.ToDecimal(vmin);
                        
                        this.txtValorZoomdoMapaTotal.Text = clt.Double2Texto(m_zoom_value_extended, 4);
                        this.numericUpDown2.Value = 0;
                        this.numericUpDown1.Value = numericUpDown2.Maximum;

                        this.grbFonteLabels.Enabled = false;

                        #endregion 
                    }
                    else
                    {
                        #region opções para os labels do mapa

                        m_layer_com_labels = true;

                        nudMaxValueZoomLabels.Value = Convert.ToDecimal(this.m_mapa.Layers["Labels_aux_" + m_layer_selecionado].MaxVisible);
                        nudMinValueZoomLabels.Value = Convert.ToDecimal(this.m_mapa.Layers["Labels_aux_" + m_layer_selecionado].MinVisible);
                        txtValueZoomMap.Text = clt.Double2Texto(m_zoom_value_extended, 4);
                        
                        this.txtValorZoomdoMapaTotal.Text = clt.Double2Texto(m_zoom_value_extended, 4);
                        this.numericUpDown2.Value = 0;
                        this.numericUpDown1.Value = numericUpDown2.Maximum;

                        string cmb_variavel = ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + m_layer_selecionado]).LabelColumn;
                        if (cmbVariavelLabels.Items.Contains(cmb_variavel))
                        {
                            cmbVariavelLabels.SelectedItem = cmb_variavel;
                        }

                        ckbUsarLabelsNosPoligonos.Checked = true;

                        Font font = ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + m_layer_selecionado]).Style.Font;

                        this.txtNomeFonte.Text = font.Name;
                        this.txtEstiloFonte.Text = font.Style.ToString();
                        this.txtTamanhoFonte.Text = font.Size.ToString();

                        this.grbFonteLabels.Enabled = true;

                        #endregion
                    }
                }
            }
        }

        private SharpMap.Forms.MapImage m_map_image;
        public SharpMap.Forms.MapImage MapImage
        {
            get { return m_map_image; }
            set { m_map_image = value; }
        }

        private Hashtable[] m_ht_caracteristicas_originais = new Hashtable[0];
        private Hashtable[] m_ht_caracteristicas = new Hashtable[0];
        public Hashtable[] HashTableCaracteristicasItens
        {
            get { return m_ht_caracteristicas; }
            set 
            { 
                m_ht_caracteristicas = value;
                m_ht_caracteristicas_originais = new Hashtable[value.GetLength(0)];
                for (int i = 0; i < m_ht_caracteristicas.GetLength(0); i++)
                {
                    m_ht_caracteristicas_originais[i] = new Hashtable();
                    (m_ht_caracteristicas_originais[i]).Add("Fill", ((Brush)((m_ht_caracteristicas[i])["Fill"])).Clone());
                    (m_ht_caracteristicas_originais[i]).Add("Line", ((Pen)((m_ht_caracteristicas[i])["Line"])).Clone());
                    (m_ht_caracteristicas_originais[i]).Add("Outline", ((Pen)((m_ht_caracteristicas[i])["Outline"])).Clone());
                    (m_ht_caracteristicas_originais[i]).Add("Symbol", ((Bitmap)((m_ht_caracteristicas[i])["Symbol"])).Clone());
                }
            } 
        }

        private DataTable m_variaveis_dados_shape = new DataTable();
        public DataTable VariaveisDadosShape
        {
            get { return m_variaveis_dados_shape; }
            set 
            {
                clsUtilTools clt = new clsUtilTools();

                m_variaveis_dados_shape = value.Copy();
                cmbVariavelLabels.Items.Clear();
                cmbVariavelLabels.Items.AddRange(clt.RetornaTodasColunas(m_variaveis_dados_shape));
                cmbVariavelLabels.SelectedIndex = 0;
            }
        }

        public ArrayList ObservacoesSelecionadas
        {
            set
            {
                m_elementos_selecionados = new ArrayList();
                DataTable seldt = m_variaveis_dados_shape.Clone();
                for (int i = 0; i < value.Count; i++)
                {
                    m_elementos_selecionados.Add((string)m_chaves_rows[(int)value[i]]);
                    seldt.Rows.Add(m_variaveis_dados_shape.Rows[(int)value[i]].ItemArray);
                }

                dataGridView1.DataSource = seldt;
                dataGridView1.Refresh();

                int n_selecionado = seldt.Rows.Count;

                lblResultadoQuery.Text = "\n" + "Elementos pré-selecionados: " + n_selecionado.ToString() + " elementos filtrados";
            }
        }

        public bool SelecionaTodosElementos
        {
            set
            {
                if (value)
                {
                    m_elementos_selecionados = new ArrayList();
                    for (int i = 0; i < m_num_elementos_layer; i++)
                    {
                        m_elementos_selecionados.Add((string)m_chaves_rows[i]); 
                    }
                }
            }
        }

        private string m_layer_selecionado = "";
        public string NomeLayerSelecionado
        {
            set
            {
                m_layer_selecionado = value;
                this.Text = "Propriedades dos elementos no Layer Selecionado - [" + value + "]";
            }
        }

        private string m_appPath = "";

        #endregion

        private void frmPropriedadesPoligonosNoLayer_Load(object sender, EventArgs e)
        {
            try
            {
                m_appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                cmbEstiloLinhaContorno.Items.Clear();
                cmbEstiloLinhaContorno.Items.AddRange(IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers.ListaEstilosLinha);
                cmbEstiloLinhaContorno.SelectedIndex = 0;

                this.cmbEstiloPreenchimento.Items.Clear();
                this.cmbEstiloPreenchimento.Items.AddRange(IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers.ListaEstilosFill);
                this.cmbEstiloPreenchimento.SelectedIndex = 0;

                this.cmbEstiloMarcador.Items.Clear();
                this.cmbEstiloMarcador.Items.AddRange(IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers.ListaEstilosMarcadores);
                this.cmbEstiloMarcador.SelectedItem = "Círculo e X";

                if (this.m_tipo_geometria_do_layer == "Polígonos")
                {
                    if (this.tabControl1.TabPages.Contains(tabPage7)) this.tabControl1.TabPages.Remove(tabPage7);
                    if (!this.tabControl1.TabPages.Contains(tabPage2)) this.tabControl1.TabPages.Add(tabPage2);
                    if (!this.tabControl1.TabPages.Contains(tabPage6)) this.tabControl1.TabPages.Add(tabPage6);
                    tabPage6.Text = "Contorno";
                }

                if (m_tipo_geometria_do_layer == "Multi-Polígonos")
                {
                    if (this.tabControl1.TabPages.Contains(tabPage7)) this.tabControl1.TabPages.Remove(tabPage7);
                    if (!this.tabControl1.TabPages.Contains(tabPage2)) this.tabControl1.TabPages.Add(tabPage2);
                    if (!this.tabControl1.TabPages.Contains(tabPage6)) this.tabControl1.TabPages.Add(tabPage6);
                    tabPage6.Text = "Contorno";
                }

                if (m_tipo_geometria_do_layer == "Multi-Curvas" || m_tipo_geometria_do_layer == "LineString" || m_tipo_geometria_do_layer == "LinearRing")
                {
                    if (this.tabControl1.TabPages.Contains(tabPage7)) this.tabControl1.TabPages.Remove(tabPage7);
                    if (this.tabControl1.TabPages.Contains(tabPage2)) this.tabControl1.TabPages.Remove(tabPage2);
                    if (!this.tabControl1.TabPages.Contains(tabPage6)) this.tabControl1.TabPages.Add(tabPage6);
                    tabPage6.Text = "Linha";
                }

                if (m_tipo_geometria_do_layer == "Pontos" || m_tipo_geometria_do_layer == "Multi-Pontos")
                {
                    if (!this.tabControl1.TabPages.Contains(tabPage7)) this.tabControl1.TabPages.Add(tabPage7);
                    if (this.tabControl1.TabPages.Contains(tabPage2)) this.tabControl1.TabPages.Remove(tabPage2);
                    if (this.tabControl1.TabPages.Contains(tabPage6)) this.tabControl1.TabPages.Remove(tabPage6);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region fechando formulário 

        private void btnCancela_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)this.m_mapa.Layers[m_layer_selecionado];

                IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers meuTema;

                meuTema = new Classes.clsThemeAuxiliaryPropertiesLayers(ref this.m_chaves_rows, ref m_colunas_chave, ref this.m_ht_caracteristicas_originais);

                layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyleOriginal);

                //m_mapa.ZoomToExtents();

                //Refresh o mapa
                this.m_map_image.Refresh();

                Application.DoEvents();

                m_ht_caracteristicas = new Hashtable[m_ht_caracteristicas_originais.GetLength(0)];
                for (int i = 0; i < m_ht_caracteristicas_originais.GetLength(0); i++)
                {
                    m_ht_caracteristicas[i] = new Hashtable();
                    (m_ht_caracteristicas[i]).Add("Fill", ((Brush)((m_ht_caracteristicas_originais[i])["Fill"])).Clone());
                    (m_ht_caracteristicas[i]).Add("Line", ((Pen)((m_ht_caracteristicas_originais[i])["Line"])).Clone());
                    (m_ht_caracteristicas[i]).Add("Outline", ((Pen)((m_ht_caracteristicas_originais[i])["Outline"])).Clone());
                    (m_ht_caracteristicas[i]).Add("Symbol", ((Bitmap)((m_ht_caracteristicas_originais[i])["Symbol"])).Clone());
                }

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

                layMapa.MaxVisible = m_zoom_original_max;
                layMapa.MinVisible = m_zoom_original_min;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOK2_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
        
        #region alterando o preenchimento e contorno dos elementos do layer

        private void lblCorContorno_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                AlterarCorLinha();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void AlterarCorLinha()
        {
            ColorDialog MyDialog = new ColorDialog();

            // Keeps the user from selecting a custom color.
            MyDialog = new ColorDialog();

            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;

            // so that if the user cancels out, the original color is restored.
            MyDialog.Color = this.lblCorContorno.BackColor;

            // Open color selection dialog box
            if (MyDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.lblCorContorno.BackColor = MyDialog.Color;
            }
        }

        private void AlterarCorPreechimento()
        {
            ColorDialog MyDialog = new ColorDialog();

            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = true;

            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;

            // so that if the user cancels out, the original color is restored.
            MyDialog.Color = this.lblColorFill.BackColor;

            // Open color selection dialog box
            if (MyDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.lblColorFill.BackColor = MyDialog.Color;
            }            
        }

        private void lblColorFill_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                AlterarCorPreechimento();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PreenchimentoElementosLayer(string propriedade)
        {
            //Captura o layer
            SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)this.m_mapa.Layers[m_layer_selecionado];

            Color fill_color = lblColorFill.BackColor;
            Color line_color = lblCorContorno.BackColor;
            Color marker_color = lblCorMarcadores.BackColor;

            double altura_marcador = Convert.ToDouble(nudAlturaMarcadores.Value);
            double largura_marcador = Convert.ToDouble(nudLarguraMarcadores.Value);
            
            IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers meuTema;
            if (ckbSemPreenchimento.Checked) { fill_color = Color.Transparent; }
            if (ckbSemContorno.Checked) { line_color = Color.Transparent; }

            double valor_cor_alpha_fill = ((double)this.trackBarOpacidade.Value)/((double)100)*255;
            double valor_cor_alpha_contour = ((double)this.trackBarContorno.Value)/((double)100)*255;
            double valor_cor_alpha_marker = ((double)this.trackBar1.Value) / ((double)100) * 255;
            
            System.Drawing.Bitmap marcador = new Bitmap(1, 1);            
            if (propriedade == "Círculo e X")
            {
                string arquivo = m_appPath + "\\images\\Icones_PNG\\DefaultSymbol.png";
                marcador = new Bitmap(arquivo);
            }

            meuTema = new Classes.clsThemeAuxiliaryPropertiesLayers(fill_color, line_color, 
                Convert.ToDouble(nudLarguraLinhaContorno.Value), cmbEstiloLinhaContorno.SelectedItem.ToString(), cmbEstiloPreenchimento.SelectedItem.ToString(),
                ref this.m_chaves_rows, ref this.m_elementos_selecionados, ref m_colunas_chave, ref m_ht_caracteristicas, valor_cor_alpha_fill, valor_cor_alpha_contour, propriedade,
                m_appPath, marcador, marker_color, valor_cor_alpha_marker);

            layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

            m_mapa.ZoomToExtents();

            //Refresh o mapa
            this.m_map_image.Refresh();

            Application.DoEvents();
        }

        private void ckbSemPreenchimento_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbSemPreenchimento.Checked)
                {
                    this.lblColorFill.Enabled =
                        this.cmbEstiloPreenchimento.Enabled = 
                        this.trackBarOpacidade.Enabled = false;
                }
                else
                {
                    this.lblColorFill.Enabled =
                        this.cmbEstiloPreenchimento.Enabled =
                        this.trackBarOpacidade.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFillAplicar_Click(object sender, EventArgs e)
        {
            PreenchimentoElementosLayer("Fill");

            btnOK2.Enabled = true;
        }
        
        private void ckbSemContorno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbSemContorno.Checked)
                {
                    this.lblCorContorno.Enabled =
                        this.nudLarguraLinhaContorno.Enabled =
                        this.trackBarContorno.Enabled =
                        this.cmbEstiloLinhaContorno.Enabled = false;
                }
                else
                {
                    this.lblCorContorno.Enabled =
                        this.nudLarguraLinhaContorno.Enabled =
                        this.trackBarContorno.Enabled = 
                        this.cmbEstiloLinhaContorno.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region apresentando labels do layer selecionado

        private void ckbUsarLabelsNosPoligonos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbUsarLabelsNosPoligonos.Checked)
                {
                   if (m_layer_com_labels) grbFonteLabels.Enabled = true;

                   grbZoomLabels.Enabled =
                        cmbVariavelLabels.Enabled = true;
                }
                else
                {
                   if (m_layer_com_labels) grbFonteLabels.Enabled = true;

                   grbZoomLabels.Enabled =
                        cmbVariavelLabels.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {                
                System.Windows.Forms.FontDialog fontDialog1 = new FontDialog();
                fontDialog1.ShowColor = true;
                fontDialog1.Font = ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + this.m_layer_selecionado]).Style.Font;
                fontDialog1.Color = ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + this.m_layer_selecionado]).Style.ForeColor;

                if (fontDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + this.m_layer_selecionado]).Style.Font = fontDialog1.Font;
                    ((SharpMap.Layers.LabelLayer)this.m_mapa.Layers["Labels_aux_" + this.m_layer_selecionado]).Style.ForeColor = fontDialog1.Color;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_layer_com_labels = false;

        private void AplicarModificacoes()
        {
            SharpMap.Layers.VectorLayer l1 = (SharpMap.Layers.VectorLayer)this.m_mapa.Layers[m_layer_selecionado];

            if (ckbAlterarZoomVisualizacaoLayer.Checked)
            {
                if (Convert.ToDouble(numericUpDown2.Value) > 0.0)
                {
                    l1.MinVisible = Convert.ToDouble(numericUpDown2.Value);
                }

                if (numericUpDown1.Value < numericUpDown1.Maximum)
                {
                    l1.MaxVisible = Convert.ToDouble(this.numericUpDown1.Value);
                }
            }

            string variavel_label = "";
            if (this.cmbVariavelLabels.SelectedIndex >= 0)
            {
                variavel_label = this.cmbVariavelLabels.SelectedItem.ToString();
            }

            if (ckbUsarLabelsNosPoligonos.Checked)
            {
                //preparar um layer de labels
                for (int i = 0; i < m_mapa.Layers.Count; i++)
                {
                    if (m_mapa.Layers[i].LayerName == "Labels_aux_" + this.m_layer_selecionado)
                    {
                        m_mapa.Layers.RemoveAt(i);
                        break;
                    }
                }

                SharpMap.Layers.LabelLayer laylabel = new SharpMap.Layers.LabelLayer("Labels_aux_" + this.m_layer_selecionado);
                laylabel.DataSource = l1.DataSource;

                double vmax = l1.MaxVisible;
                double vmin = l1.MinVisible;

                double vz = this.m_mapa.Zoom;

                laylabel.Enabled = true;
                laylabel.LabelColumn = variavel_label;
                laylabel.Style = new SharpMap.Styles.LabelStyle();
                laylabel.Style.ForeColor = Color.Black;
                laylabel.Style.Font = new Font(FontFamily.GenericSerif, 11);

                laylabel.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Left;
                laylabel.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Bottom;
                laylabel.Style.Offset = new PointF(3, 3);
                laylabel.Style.Halo = new Pen(Color.Transparent, 2);
                laylabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                laylabel.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                laylabel.SRID = 4326;

                laylabel.MaxVisible = Convert.ToDouble(nudMaxValueZoomLabels.Value);
                laylabel.MinVisible = Convert.ToDouble(nudMinValueZoomLabels.Value);

                this.m_mapa.Layers.Add(laylabel);

                //Coloca o mapa no mapImage
                this.m_map_image.Map = m_mapa;

                //Zoom todo o mapa.
                //this.m_map_image.Map.ZoomToExtents();

                //Refresh o mapa no mapImage
                this.m_map_image.Refresh();

                m_layer_com_labels = true;
            }
            else
            {
                for (int i = 0; i < m_mapa.Layers.Count; i++)
                {
                    if (m_mapa.Layers[i].LayerName == "Labels_aux_" + this.m_layer_selecionado)
                    {
                        m_mapa.Layers.RemoveAt(i);
                        break;
                    }
                }

                //Coloca o mapa no mapImage
                this.m_map_image.Map = m_mapa;

                //Zoom todo o mapa.
                //this.m_map_image.Map.ZoomToExtents();

                //Refresh o mapa no mapImage
                this.m_map_image.Refresh();

                this.cmbVariavelLabels.Items.Clear();
                this.txtEstiloFonte.Text = "";
                this.txtNomeFonte.Text = "";
                this.txtTamanhoFonte.Text = "";
                this.nudMaxValueZoomLabels.Value = 20;
                this.nudMinValueZoomLabels.Value = 10;

                m_layer_com_labels = false;
            }
        }

        private void btnAplicarLayers_Click(object sender, EventArgs e)
        {
            try
            {
                AplicarModificacoes();

                btnOK2.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region filtro das observações 

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow[] rowsq = m_variaveis_dados_shape.Select(this.userControlRichTextOutput2.Texto);

                DataTable seldt = m_variaveis_dados_shape.Clone();
                m_elementos_selecionados = new ArrayList();
                StringBuilder v = new StringBuilder();

                for (int i = 0; i < rowsq.GetLength(0); i++)
                {
                    v.Clear();
                    seldt.Rows.Add(rowsq[i].ItemArray);
                    for (int j = 0; j < m_colunas_chave.Count; j++)
                    {
                        v.Append(rowsq[i].ItemArray[(int)m_colunas_chave[j]].ToString());
                    }
                    m_elementos_selecionados.Add(v.ToString()); 
                }

                dataGridView1.DataSource = seldt;
                dataGridView1.Refresh();

                int n_selecionado = rowsq.GetLength(0);

                lblResultadoQuery.Text = "\n" + "Foram selecionados " + n_selecionado.ToString() + " elementos no filtro: " + userControlRichTextOutput2.Texto;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void btnDadosOriginais_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = m_variaveis_dados_shape;
                dataGridView1.Refresh();

                lblResultadoQuery.Text = "\n" + "Seleção de todos os dados na tabela original";

                this.SelecionaTodosElementos = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void btnContourAplicar_Click(object sender, EventArgs e)
        {
            PreenchimentoElementosLayer("Contour");

            btnOK2.Enabled = true;
        }

        private void label22_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                AlterarCorMarcadores();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void AlterarCorMarcadores()
        {
            ColorDialog MyDialog = new ColorDialog();

            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = true;

            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;

            // so that if the user cancels out, the original color is restored.
            MyDialog.Color = this.lblCorMarcadores.BackColor;

            // Open color selection dialog box
            if (MyDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.lblCorMarcadores.BackColor = MyDialog.Color;
            }
        }

        private void btnAplicarMarcadores_Click(object sender, EventArgs e)
        {
            string marcador = cmbEstiloMarcador.SelectedItem.ToString();

            PreenchimentoElementosLayer(marcador);

            btnOK2.Enabled = true;
        }

        #region alteração dos detalhes de visualização do layer

        private void btnAplicarZoomLayer_Click(object sender, EventArgs e)
        {
            try
            {
                AplicarModificacoes();

                btnOK2.Enabled = true;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void ckbAlterarZoomVisualizacaoLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAlterarZoomVisualizacaoLayer.Checked) this.groupBox7.Enabled = true;
            else this.groupBox7.Enabled = false;
        }

        private void lblColorFill_Click(object sender, EventArgs e)
        {
            try
            {
                AlterarCorPreechimento();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblCorContorno_Click(object sender, EventArgs e)
        {
            try
            {
                AlterarCorLinha();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void lblColorFill_MouseEnter(object sender, EventArgs e)
        //{
        //    toolTip1.ShowAlways = true;
        //    toolTip1.SetToolTip(lblColorFill, "Duplo clique para selecionar a cor");
        //    toolTip1.Show("Duplo clique para selecionar a cor", lblColorFill);
        //}
    }
}
