using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class cs_dmperm_results
    {
        public cs_dmperm_results()
        {
        }

        public cs_dmperm_results(int m, int n)
        {
            this.p = new int[m];
            this.r = new int[m + 6];
            this.q = new int[n];
            this.s = new int[n + 6];
        }

        public int[] p = new int[0];
        public int[] q = new int[0];
        public int[] r = new int[0];
        public int[] s = new int[0];
        public int nb = 0;
        public int[] rr = new int[5];
        public int[] cc = new int[5];
    }
}
