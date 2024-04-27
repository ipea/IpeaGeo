using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsSparseSymbolicFactorization
    {
        public clsSparseSymbolicFactorization()
        {
        }

        private int[] m_pinv = new int[0];
        private int[] m_q = new int[0];
        private int[] m_parent = new int[0];
        private int[] m_cp = new int[0];
        private int[] m_leftmost = new int[0];
        private int m_m2 = 0;
        private double m_lnz = 0.0;
        private double m_unz = 0.0;

        public double unz
        {
            get { return m_unz; }
            set { m_unz = value; }
        }

        public double lnz
        {
            get { return m_lnz; }
            set { m_lnz = value; }
        }

        public int m2
        {
            get { return m_m2; }
            set { m_m2 = value; }
        }

        public int[] leftmost
        {
            get { return m_leftmost; }
            set { m_leftmost = value; }
        }

        public int[] cp
        {
            get { return m_cp; }
            set { m_cp = value; }
        }

        public int[] parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public int[] pinv
        {
            get { return m_pinv; }
            set { m_pinv = value; }
        }

        public int[] q
        {
            get { return m_q; }
            set { m_q = value; }
        }
    }
}
