using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Classes
{
    public class clsManipulaItensNoLayer
    {
        public clsManipulaItensNoLayer()
        {
        }

        public void HachurearPoligonos(ref SharpMap.Map map, ref SharpMap.Forms.MapImage map_image,
            string nome_layer, ref Hashtable[] ht_caracteristicas, ref DataTable dt_shape, string var_chave_in,
            ref int[] rows_hachureados)
        {
            string var_chave = var_chave_in;
            if (var_chave == "")
            {
                clsUtilTools clt = new clsUtilTools();
                string[] uvars = clt.RetornaUniqueColunas(dt_shape);
                if (uvars.GetLength(0) > 0) var_chave = uvars[0];
                else var_chave = dt_shape.Columns[0].ColumnName;
            }

            Color[] colors = new Color[rows_hachureados.GetLength(0)];
            Pen aux;
            Brush auxb;
            Brush auxb1;
            Type tipo;
            string[] elementos_chave = new string[dt_shape.Rows.Count];
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                elementos_chave[i] = dt_shape.Rows[i][var_chave].ToString();

                auxb1 = (Brush)(ht_caracteristicas[i])["Fill"];
                tipo = auxb1.GetType();

                if (tipo == typeof(SolidBrush))
                {
                    aux = new Pen(auxb1);
                    colors[i] = aux.Color;
                }
                else
                {
                    colors[i] = ((System.Drawing.Drawing2D.HatchBrush)((ht_caracteristicas[i])["Fill"])).BackgroundColor;
                }

                if (rows_hachureados[i] == 1)
                {
                    auxb = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ZigZag, SystemColors.Highlight, colors[i]);
                    (ht_caracteristicas[i])["Fill"] = (Brush)auxb;
                }
                else
                {
                    auxb = new SolidBrush(colors[i]);
                    (ht_caracteristicas[i])["Fill"] = (Brush)auxb;
                }
            }

            ArrayList col_chave = new ArrayList();
            for (int j = 0; j < dt_shape.Columns.Count; j++)
            {
                if (dt_shape.Columns[j].ColumnName == var_chave)
                {
                    col_chave.Add(j);
                    break;
                }
            }

            SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)map.Layers[nome_layer];

            IpeaGeo.Classes.clsThemeAuxiliaryPropertiesLayers meuTema;

            meuTema = new Classes.clsThemeAuxiliaryPropertiesLayers(ref elementos_chave, ref col_chave, ref ht_caracteristicas);

            layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyleOriginal);
            
            //Refresh o mapa
            map_image.Refresh();

            Application.DoEvents();
        }
    }
}
