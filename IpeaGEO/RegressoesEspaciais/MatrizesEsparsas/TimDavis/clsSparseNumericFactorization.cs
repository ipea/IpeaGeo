using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsSparseNumericFactorization
    {
        public clsSparseNumericFactorization()
        {
        }

        private clsMatrizEsparsa m_L = new clsMatrizEsparsa();
        private clsMatrizEsparsa m_U = new clsMatrizEsparsa();
        private int[] m_pinv = new int[0];
        private double[] m_B = new double[0];

        public clsMatrizEsparsa L
        {
            get { return m_L; }
            set { m_L = value; }
        }

        public clsMatrizEsparsa U
        {
            get { return m_U; }
            set { m_U = value; }
        }

        public int[] pinv
        {
            get { return m_pinv; }
            set { m_pinv = value; }
        }

        public double[] B
        {
            get { return m_B; }
            set { m_B = value; }
        }
    }
}
