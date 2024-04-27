using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IpeaGeo
{
    class clsAreaPerimetroCentroide
    {
        public double area(double[] lati, double[] longi)
        {
            double area = 0;
            double a = 0;
            double b = 0;
            for (int i = 0; i < lati.Length - 1; i++)
            {
                a += lati[i] * longi[i + 1];
                b += longi[i] * lati[i + 1];
            }
            a += lati[lati.Length - 1] * longi[0];
            b += longi[longi.Length - 1] * lati[0];

            area = Math.Abs((a - b) / 2);

            return (area);
        }
        
        public double distancia(double lat1, double long1, double lat2, double long2)
        {
            double a = Math.Sin(lat1) * Math.Sin(lat2);
            double b = Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(long1 - long2);
            double distancia = Math.Acos(a + b) * 6371.0072;

            return (distancia);

        }

        /// <summary>
        /// Calcula a distancia entre dois pontos
        /// </summary>
        /// <param name="lat1">Coordenada Y</param>
        /// <param name="long1">Coordenada X</param>
        /// <param name="lat2">Coordenada Y</param>
        /// <param name="long2">Coordenada X</param>
        /// <param name="coordenadas_em_radianos">Colocar "true" se os dados já estão em radianos</param>
        /// <returns></returns>
        public double distancia(double lat1, double long1, double lat2, double long2, bool coordenadas_em_radianos)
        {
            double rad=1;
            if (!coordenadas_em_radianos) rad = Math.PI / 180;

            double a = Math.Sin(lat1*rad) * Math.Sin(lat2*rad);
            double b = Math.Cos(lat1*rad) * Math.Cos(lat2*rad) * Math.Cos(long1*rad - long2*rad);
            double distancia = Math.Acos(a + b) * 6371.0072;

            return (distancia);

        }
    }
}
