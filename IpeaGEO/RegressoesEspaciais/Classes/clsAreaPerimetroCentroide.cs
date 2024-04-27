using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    [Obsolete("Verificar a necessidade desta classe.")]
    class clsAreaPerimetroCentroide
    {        
        public double distancia(double lat1, double long1, double lat2, double long2)
        {
            double a = Math.Sin(lat1) * Math.Sin(lat2);
            double b = Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(long1 - long2);
            double distancia = Math.Acos(a + b) * 6371.0072;

            return (distancia);
        } // distancia()

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
            double distancia = Math.Acos(a*rad + b*rad) * 6371.0072;

            return (distancia);
        } // distancia
    } // class
} // namespace
