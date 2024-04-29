using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMap.Styles;
using System.Drawing;
using System.Collections;

namespace IpeaGeo.Classes
{
    public class clsThemeAuxiliaryPropertiesLayers
    {
        #region variáveis internas

        public static string[] ListaEstilosLinha = new string[] { "Padrão", "Traço", "Traço ponto", "Traço ponto ponto", "Ponto", "Sólido" };
        public static string[] ListaEstilosFill = new string[] { "Sólido", "Hachureado - zigzag", "Hachureado - diagonal", "Hachureado - diagonal invertido" };
        public static string[] ListaEstilosMarcadores = new string[] { "Ponto pequeno", "Ponto médio", "Ponto grande", 
                                                                       "Quadrado pequeno", "Quadrado médio", "Quadrado grande", "X pequeno", "X médio", "X grande", "Círculo e X" };

        private Color _iFillColor;
        private double _iLineWidth;
        private Color _iLineColor;
        private string _iEstiloLinha;
        private string _iEstiloFill;
        private ArrayList _elementosSelecionados = new ArrayList();
        private string[] _chavesElementos = new string[0];
        private ArrayList _colunasChave = new ArrayList();
        private Hashtable[] _ht_caracteristicas = new Hashtable[0];
        private string _tipo_alteracao;
        private Color _aux_cor_fill;
        private Color _aux_cor_contour;

        private Color _iMarkerColor;
        private Color _aux_cor_marker;
        private string _path_marcador;
        private System.Drawing.Bitmap _marcador;

        private int IndiceElemento(string elemento)
        {
            int indice = -1;
            for (int i = 0; i < _chavesElementos.GetLength(0); i++)
            {
                if (_chavesElementos[i] == elemento)
                {
                    return i;
                }
            }
            
            return indice;
        }

        #endregion

        #region construtores

        public clsThemeAuxiliaryPropertiesLayers(Color iFillColor, Color iLineColor, double iLineWidth, string estiloLinha, 
            string estiloFill, ref string[] chavesElementos, ref ArrayList elementosSelecionados, ref ArrayList colunasChave, ref Hashtable[] ht_caracteristicas, 
            double valor_alpha_cor_fill, double valor_alpha_cor_contour, string tipo_alteracao,
            string path_marcador, System.Drawing.Bitmap marcador, Color cor_marcador, double valor_alpha_cor_marker)
        {
            _iMarkerColor = cor_marcador;
            _path_marcador = path_marcador;
            _marcador = marcador;

            _iFillColor = iFillColor;
            _iLineWidth = iLineWidth;
            _iLineColor = iLineColor;
            _iEstiloLinha = estiloLinha;
            _iEstiloFill = estiloFill;
            _elementosSelecionados = elementosSelecionados;
            _chavesElementos = chavesElementos;
            _colunasChave = colunasChave;
            _ht_caracteristicas = ht_caracteristicas;
            _tipo_alteracao = tipo_alteracao;

            _aux_cor_fill = Color.FromArgb((int)valor_alpha_cor_fill, _iFillColor.R, _iFillColor.G, _iFillColor.B);
            _aux_cor_contour = Color.FromArgb((int)valor_alpha_cor_contour, _iLineColor.R, _iLineColor.G, _iLineColor.B);
            _aux_cor_marker = Color.FromArgb((int)valor_alpha_cor_marker, _iMarkerColor.R, _iMarkerColor.G, _iMarkerColor.B);
        }
        
        public clsThemeAuxiliaryPropertiesLayers(ref string[] chavesElementos, ref ArrayList colunasChave, ref Hashtable[] ht_caracteristicas)
        {
            _chavesElementos = chavesElementos;
            _colunasChave = colunasChave;
            _ht_caracteristicas = ht_caracteristicas;
        }

        #endregion

        #region função para mudança nos atributos dos itens do layer

        public SharpMap.Styles.VectorStyle GetStyle(SharpMap.Data.FeatureDataRow dr)
        {
            StringBuilder v = new StringBuilder();
            object[] itens = dr.ItemArray;
            
            for (int i=0; i<_colunasChave.Count; i++)
            {
                v.Append(itens[(int)_colunasChave[i]].ToString());
            }
            int indice_elemento = -1;

            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();

            indice_elemento = IndiceElemento(v.ToString());

            if (_elementosSelecionados.Contains(v.ToString()))
            {
                //Estilo do vetor
                style = new SharpMap.Styles.VectorStyle();

                if (_tipo_alteracao == "Fill")
                {
                    //Aplica o estilo
                    switch (_iEstiloFill)
                    {
                        case "Sólido":
                            style.Fill = new SolidBrush(_aux_cor_fill);
                            break;
                        case "Hachureado - zigzag":
                            style.Fill = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ZigZag, SystemColors.Highlight, _iFillColor);
                            break;
                        case "Hachureado - diagonal invertido":
                            style.Fill = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, SystemColors.Highlight, _iFillColor);
                            break;
                        case "Hachureado - diagonal":
                            style.Fill = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal, SystemColors.Highlight, _iFillColor);
                            break;
                        default:
                            style.Fill = new SolidBrush(_iFillColor);
                            break;
                    }

                    if (indice_elemento >= 0)
                    {
                        (_ht_caracteristicas[indice_elemento])["Fill"] = (Brush)style.Fill.Clone();

                        style.Outline = (Pen)(_ht_caracteristicas[indice_elemento])["Outline"];
                        style.Line = (Pen)(_ht_caracteristicas[indice_elemento])["Line"];

                        style.EnableOutline = true;
                    }
                }

                if (_tipo_alteracao == "Contour")
                {
                    //Habilita o Outline
                    style.Outline = new Pen(_aux_cor_contour, 1);
                    style.Line.Width = (float)_iLineWidth;
                    style.Line.Color = _aux_cor_contour;
                    style.Line.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    style.Line.StartCap = style.Line.EndCap;

                    style.Outline = (System.Drawing.Pen)style.Line.Clone();
                    style.Outline.Width = (float)_iLineWidth;
                    style.Outline.Color = _aux_cor_contour;
                    style.EnableOutline = true;

                    switch (_iEstiloLinha)
                    {
                        case "Traço":
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            break;
                        case "Traço ponto":
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                            break;
                        case "Traço ponto ponto":
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                            break;
                        case "Ponto":
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                            break;
                        case "Sólido":
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            break;
                        default:
                            style.Outline.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                            break;
                    }

                    if (indice_elemento >= 0)
                    {
                        (_ht_caracteristicas[indice_elemento])["Line"] = (Pen)style.Line.Clone();
                        (_ht_caracteristicas[indice_elemento])["Outline"] = (Pen)style.Outline.Clone();

                        style.Fill = (Brush)(_ht_caracteristicas[indice_elemento])["Fill"];
                    }
                }

                System.Drawing.Bitmap flag = new Bitmap(3, 3);

                if (_tipo_alteracao == "Círculo e X")
                {
                    if (indice_elemento >= 0)
                    {
                        try
                        {
                            style.Symbol = _marcador;

                            (_ht_caracteristicas[indice_elemento])["Symbol"] = (Bitmap)style.Symbol.Clone();
                        }
                        catch { }
                    }
                }

                if (_tipo_alteracao == "Ponto pequeno" || _tipo_alteracao == "Ponto médio" || _tipo_alteracao == "Ponto grande")
                {
                    if (_tipo_alteracao == "Ponto pequeno") flag = new Bitmap(3, 3);
                    if (_tipo_alteracao == "Ponto médio") flag = new Bitmap(5, 5);
                    if (_tipo_alteracao == "Ponto grande") flag = new Bitmap(7, 7);

                    for (int i = 0; i < flag.Height; i++)
                    {
                        for (int j = 0; j < flag.Width; j++)
                        {
                            flag.SetPixel(i, j, _aux_cor_marker);
                        }
                    }

                    if (indice_elemento >= 0)
                    {
                        style.Symbol = flag;

                        (_ht_caracteristicas[indice_elemento])["Symbol"] = (Bitmap)style.Symbol.Clone();
                    }
                }

                if (_tipo_alteracao == "Quadrado pequeno" || _tipo_alteracao == "Quadrado médio" || _tipo_alteracao == "Quadrado grande")
                {
                    if (_tipo_alteracao == "Quadrado pequeno") flag = new Bitmap(5, 5);
                    if (_tipo_alteracao == "Quadrado médio") flag = new Bitmap(7, 7);
                    if (_tipo_alteracao == "Quadrado grande") flag = new Bitmap(9, 9);

                    for (int i = 0; i < flag.Height; i++)
                    {
                        flag.SetPixel(i, 0, _aux_cor_marker);
                        flag.SetPixel(i, flag.Width - 1, _aux_cor_marker);
                    }

                    for (int i = 0; i < flag.Width; i++)
                    {
                        flag.SetPixel(0, i, _aux_cor_marker);
                        flag.SetPixel(flag.Height - 1, i, _aux_cor_marker);
                    }

                    if (indice_elemento >= 0)
                    {
                        style.Symbol = flag;

                        (_ht_caracteristicas[indice_elemento])["Symbol"] = (Bitmap)style.Symbol.Clone();
                    }
                }
                
                if (_tipo_alteracao == "X pequeno" || _tipo_alteracao == "X médio" || _tipo_alteracao == "X grande")
                {
                    if (_tipo_alteracao == "X pequeno") flag = new Bitmap(5, 5);
                    if (_tipo_alteracao == "X médio") flag = new Bitmap(7, 7);
                    if (_tipo_alteracao == "X grande") flag = new Bitmap(9, 9);

                    for (int i = 0; i < flag.Height; i++)
                    {
                        flag.SetPixel(i, i, _aux_cor_marker);
                        flag.SetPixel(i, flag.Height - 1 - i, _aux_cor_marker);
                    }

                    if (_tipo_alteracao == "X grande")
                    {
                        for (int i = 0; i < flag.Height-1; i++)
                        {
                            flag.SetPixel(i, i + 1, _aux_cor_marker);
                            flag.SetPixel(i, flag.Height - 2 - i, _aux_cor_marker);
                        }
                        flag.SetPixel(flag.Height - 1, 0, Color.Transparent);
                        flag.SetPixel(flag.Height - 1, flag.Height - 1, Color.Transparent);
                    }

                    if (indice_elemento >= 0)
                    {
                        style.Symbol = flag;

                        (_ht_caracteristicas[indice_elemento])["Symbol"] = (Bitmap)style.Symbol.Clone();
                    }      
                }

                //Retorna o estilo
                return style;           
            }
            else
            {
                if (indice_elemento >= 0)
                {
                    //Estilo do vetor
                    style = new SharpMap.Styles.VectorStyle();

                    //Guarda a posição do FeatureDataRow
                    //int iPosicao = GetIndex(_vetorID, _vetorIndice, dr[_strIDmapa].ToString());

                    //Aplica o estilo
                    style.Fill = (Brush)(_ht_caracteristicas[indice_elemento])["Fill"];
                    style.Outline = (Pen)(_ht_caracteristicas[indice_elemento])["Outline"];
                    style.Line = (Pen)(_ht_caracteristicas[indice_elemento])["Line"];
                    style.Symbol = (Bitmap)(_ht_caracteristicas[indice_elemento])["Symbol"];

                    //Habilita o Outline
                    style.EnableOutline = true;

                    //Retorna o estilo
                    return style;
                }
                
                //Estilo do vetor
                style = new SharpMap.Styles.VectorStyle();

                //Guarda a posição do FeatureDataRow
                //int iPosicao = GetIndex(_vetorID, _vetorIndice, dr[_strIDmapa].ToString());

                //Aplica o estilo
                style.Fill = new SolidBrush(Color.Transparent);

                //style.Fi

                //Habilita o Outline
                style.Outline = new Pen(Color.Black, 1);
                style.Line.Width = 4;
                style.Line.Color = Color.Black;
                style.Line.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                style.Line.StartCap = style.Line.EndCap;
                style.Outline = (System.Drawing.Pen)style.Line.Clone();
                style.Outline.Width = 1.5f;
                style.Outline.Color = Color.Black;
                style.EnableOutline = true;

                //Retorna o estilo
                return style;
            }
        }

        #endregion

        #region função para recuperação dos atributos originais

        public SharpMap.Styles.VectorStyle GetStyleOriginal(SharpMap.Data.FeatureDataRow dr)
        {
            StringBuilder v = new StringBuilder();
            object[] itens = dr.ItemArray;
            for (int i = 0; i < _colunasChave.Count; i++)
            {
                v.Append(itens[(int)_colunasChave[i]].ToString());
            }
            int indice_elemento = -1;

            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();

            indice_elemento = IndiceElemento(v.ToString());

            if (indice_elemento >= 0)
            {
                //Estilo do vetor
                style = new SharpMap.Styles.VectorStyle();

                //Guarda a posição do FeatureDataRow
                //int iPosicao = GetIndex(_vetorID, _vetorIndice, dr[_strIDmapa].ToString());

                //Aplica o estilo
                style.Fill = (Brush)(_ht_caracteristicas[indice_elemento])["Fill"];
                style.Outline = (Pen)(_ht_caracteristicas[indice_elemento])["Outline"];
                style.Line = (Pen)(_ht_caracteristicas[indice_elemento])["Line"];
                style.Symbol = (Bitmap)(_ht_caracteristicas[indice_elemento])["Symbol"];

                //Habilita o Outline
                style.EnableOutline = true;

                //Retorna o estilo
                return style;
            }

            //Estilo do vetor
            style = new SharpMap.Styles.VectorStyle();

            //Guarda a posição do FeatureDataRow
            //int iPosicao = GetIndex(_vetorID, _vetorIndice, dr[_strIDmapa].ToString());

            //Aplica o estilo
            style.Fill = new SolidBrush(Color.Transparent);

            //Habilita o Outline
            style.Outline = new Pen(Color.Black, 1);
            style.Line.Width = 4;
            style.Line.Color = Color.Black;
            style.Line.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            style.Line.StartCap = style.Line.EndCap;
            style.Outline = (System.Drawing.Pen)style.Line.Clone();
            style.Outline.Width = 1.5f;
            style.Outline.Color = Color.Black;
            style.EnableOutline = true;

            //Retorna o estilo
            return style;
        }

        #endregion
    }
}
