using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGEO
{
    class clsAreaPerimetroCentroide
    {
        public double perimetro(double[] lati, double[] longi)
        {
            double perimetro = 0;
            for (int i = 0; i < lati.Length - 1; i++)
            {
                perimetro += Math.Sqrt(Math.Pow((lati[i + 1] - lati[i]), 2) + Math.Pow((longi[i + 1] - longi[i]), 2));
            }
            perimetro += Math.Sqrt(Math.Pow((lati[lati.Length - 1] - lati[0]), 2) + Math.Pow((longi[lati.Length - 1] - longi[0]), 2));

            return (perimetro);
        }

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

        //TODO: CONFERIR SE OS PONTOS NO POLIGINO ESTAO ORDENADOS
        public double[] centroid(double[] lati, double[] longi)
        {
            
            double[] centroid = new double[2];
            double cx = 0;
            double cy = 0;
            for (int i = 0; i < lati.Length - 1; i++)
            {
                cx += (lati[i] + lati[i + 1]) * (lati[i] * longi[i + 1] + lati[i + 1] * longi[i]);
                cy += (longi[i] + longi[i + 1]) * (longi[i] * lati[i + 1] + longi[i + 1] * lati[i]);
            }

            double areapoli = area(lati, longi);
            double razao = areapoli * 6;

            cx = cx / razao;
            cy = cy / razao;

            centroid[0] = cx;
            centroid[1] = cy;

            return (centroid);

        }
        
        public double distancia(double lat1, double long1, double lat2, double long2)
        {
            double a = Math.Sin(lat1) * Math.Sin(lat2);
            double b = Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(long1 - long2);
            double distancia = Math.Acos(a + b) * 6371.0072;

            return (distancia);

        }
    }
}
