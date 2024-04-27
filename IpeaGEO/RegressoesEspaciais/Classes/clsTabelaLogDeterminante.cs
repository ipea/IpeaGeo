using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsTabelaLogDeterminante
    {
        public clsTabelaLogDeterminante(clsMatrizEsparsa W, double lmin, double lmax)
        {
            this.tabela_lndetLU(W, lmin, lmax);
        }

        private Spline_interp m_spline;
        public Spline_interp Spline
        {
            get
            {
                return m_spline;
            }
            set
            {
            	m_spline = value;
            }
        }

        #region gerando tabela de log-determinantes da matriz esparsa (I-rho x W)

        private void tabela_lndetLU(clsMatrizEsparsa W, double lmin, double lmax)
        {
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsMatrizEsparsa eye = fme.Identity(W.m);
            clsMatrizEsparsa B;
            int numl = Convert.ToInt32(Math.Ceiling((lmax - lmin) / 0.01));
            numl = Math.Min(250, numl);
            double delta = (lmax - lmin) / ((double)numl);
            double[] vetor_rho = new double[numl + 1];
            double[] vetor_ldet = new double[numl + 1];
            for (int i = 0; i < numl + 1; i++)
            {
                vetor_rho[i] = ((double)i) * delta + lmin;
                B = fme.MatrizSoma(eye, W, 1.0, -vetor_rho[i]);
                vetor_ldet[i] = fme.LogDet(B);
            }

            m_spline = new Spline_interp(vetor_rho, vetor_ldet);
        }

        #endregion
    }
}
