using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo
{
    public class clsMatrizEsparsa : IpeaGeo.RegressoesEspaciais.clsMatrizEsparsa
    {
        #region Construtores

        public clsMatrizEsparsa(double[,] mat)
            : base(mat)
        {
        }

        public clsMatrizEsparsa(double[,] mat, bool formato_triplet)
            : base(mat, formato_triplet)
        {
        }

        public clsMatrizEsparsa()
            : base()
        {
        }

        public clsMatrizEsparsa(int m, int n, int nzmax, bool formato_triplet)
            : base(m, n, nzmax, formato_triplet)
        {
        }

        public clsMatrizEsparsa(int m, int n, int nzmax, double[] x, bool formato_triplet)
            : base(m, n, nzmax, x, formato_triplet)
        {
        }

        #endregion
    }
}
