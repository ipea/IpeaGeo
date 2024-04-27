using System;
using System.Collections.Generic;
using System.Text;
using SharpMap.Styles;
using System.Drawing;
using System.Collections;

namespace IpeaGeo
{
    public class mTheme
    {
        private static Brush[] _iCores;
        private static int[] _intClasses;
        private static string _strIDmapa;
        private ArrayList _vetorID;
        private int[] _vetorIndice;
        
        public mTheme(int[] intClasses, Brush[] iCores, string strIDmapa, ArrayList vetorID, int[] vetorIndice)
        {
            _iCores = iCores;
            _intClasses = intClasses;
            _strIDmapa = strIDmapa;
            _vetorID = vetorID;
            _vetorIndice = vetorIndice;
        }

        private int BuscaIndice(ArrayList entrada, string valor)
        {
            for (int i = 0; i < entrada.Count ; i++)
            {
                if (entrada[i].ToString() == valor) return (i);
            }
            return (-1);
        }

        private int GetIndex(ArrayList vetorID, int[] vetorIndice, string strID1)
        {
            int iPosicao = vetorID.BinarySearch(strID1);
            if (iPosicao < 0)
            {
                iPosicao=BuscaIndice(vetorID, strID1);
                return (vetorIndice[iPosicao]);
            }
            else
            {
                return (vetorIndice[iPosicao]);
            }   
        }        

        public IStyle GetStyle(SharpMap.Data.FeatureDataRow dr)
        {
            //Estilo do vetor
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();

            //Guarda a posição do FeatureDataRow
            int iPosicao = GetIndex(_vetorID, _vetorIndice, dr[_strIDmapa].ToString());

            //Aplica o estilo
            style.Fill = _iCores[_intClasses[iPosicao]];

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
}
