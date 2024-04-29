using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;

namespace IpeaGeo
{
    #region Enumerações

    public enum TipoMatrizVizinhanca : int
    {
        Original = 1,
        Normalizada = 2
    };

    public enum TipoEstatisticaAmostra : int
    {
        N = 1,
        Soma = 2,
        Media = 3,
        VarianciaPopulacional = 4,
        DesvioPadraoPopulacional = 5,
        Minimo = 6,
        Maximo = 7
    };

    #endregion
}

