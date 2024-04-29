using System;
using System.Collections.Generic;
using System.Text;

namespace IpeaGEO
{
    //#region Enumerações

    //public enum TipoModeloEspacial : int
    //{
    //    SAR = 1,
    //    SEM = 2
    //};

    //#endregion

    //public class clsModelosRegressaoEspacial
    //{
    //    public clsModelosRegressaoEspacial()
    //    {
    //    }

    //    protected double m_loglik = 0.0;
    //    protected double m_aic = 0.0;
    //    protected double m_bic = 0.0;

    //    public double LogLik { get { return this.m_loglik; } }
    //    public double AIC { get { return this.m_aic; } }
    //    public double BIC { get { return this.m_bic; } }

    //    protected double[,] m_beta_hat = new double[0,0];
    //    protected double[,] m_beta_stderror = new double[0, 0];
    //    protected double[,] m_beta_tstat = new double[0, 0];
    //    protected double[,] m_beta_pvalor = new double[0, 0];
    //    protected double[,] m_beta_liminf_ci = new double[0, 0];
    //    protected double[,] m_beta_limsup_ci = new double[0, 0];
    //    protected double[,] m_beta_hat_cov = new double[0, 0];

    //    protected double m_sigma2_hat = 0.0;
    //    protected double m_prob_confidence_intervals = 95.0;
    
    //    public double[,] BetaHat { get { return this.m_beta_hat; } }
    //    public double[,] BetaStdError { get { return this.m_beta_stderror; } }
    //    public double[,] BetaTStat { get { return this.m_beta_tstat; } }
    //    public double[,] BetaPValue { get { return this.m_beta_pvalor; } }
    //    public double[,] BetaLimInfCI { get { return this.m_beta_liminf_ci; } }
    //    public double[,] BetaLimSupCI { get { return this.m_beta_limsup_ci; } }
    //    public double Sigma2Hat { get { return this.m_sigma2_hat; } }

    //    protected double m_rho_hat = 0.0;
    //    protected double m_rho_stderror = 0.0;
    //    protected double m_rho_tstat = 0.0;
    //    protected double m_rho_pvalor = 0.0;
    //    protected double m_rho_liminf_ci = 0.0;
    //    protected double m_rho_limsup_ci = 0.0;

    //    public double RhoHat { get { return this.m_rho_hat; } }
    //    public double RhoStdError { get { return this.m_rho_stderror; } }
    //    public double RhoTStat { get { return this.m_rho_tstat; } }
    //    public double RhoPValue { get { return this.m_rho_pvalor; } }
    //    public double RhoLimInfCI { get { return this.m_rho_liminf_ci; } }
    //    public double RhoLimSupCI { get { return this.m_rho_limsup_ci; } }

    //    protected double[,] m_residuos = new double[0, 0];
    //    protected double[,] m_res_versus_w = new double[0, 0];
    //    protected double[,] m_Y = new double[0, 0];
    //    protected double[,] m_X = new double[0, 0];
    //    protected double[,] m_Z = new double[0, 0];
    //    protected int m_nobs = 0;

    //    protected double m_likratio_rho_stat = 0.0;
    //    protected double m_likratio_rho_pvalue = 0.0;

    //    public double[,] ResiduosDefasados { get { return this.m_res_versus_w; } }

    //    protected TipoModeloEspacial m_tipo_modelo = TipoModeloEspacial.SAR;
    //    public TipoModeloEspacial TipoModeloRegressaoEspacial
    //    {
    //        get { return this.m_tipo_modelo; }
    //        set { this.m_tipo_modelo = value; }
    //    }

    //    public double LikelihoodRatioTestStatRho
    //    {
    //        get { return this.m_likratio_rho_stat; }
    //        set { this.m_likratio_rho_stat = value; }
    //    }

    //    public double LikelihoodRatioTestPvalueRho
    //    {
    //        get { return this.m_likratio_rho_pvalue; }
    //        set { this.m_likratio_rho_pvalue = value; }
    //    }

    //    public double[,] Residuos { get { return this.m_residuos; } }
    //    public double[,] Y 
    //    { 
    //        get { return this.m_Y; }
    //        set { this.m_Y = value; }
    //    }
    //    public double[,] X 
    //    { 
    //        get { return this.m_X; }
    //        set { this.m_X = value; }
    //    }
    //    public double[,] Z 
    //    { 
    //        get { return this.m_Z; }
    //        set { this.m_Z = value; }
    //    }

    //    protected clsIpeaShape m_shape = new clsIpeaShape();
    //    public clsIpeaShape Shape
    //    {
    //        get { return this.m_shape.Clone(); }
    //        set { this.m_shape = value.Clone(); }
    //    }
        
    //    protected double[,] m_W = new double[0, 0];
    //    public double[,] Wmatriz
    //    {
    //        get { return this.m_W; }
    //        set { this.m_W = value; }
    //    }

    //    public void MatrizWFromVizinhos()
    //    {
    //        this.m_W = new double[this.m_shape.Count, this.m_shape.Count];
    //        int[] ind_vizinhos = new int[0];

    //        for (int i = 0; i < this.m_shape.Count; i++)
    //        {
    //            ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
    //            for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
    //            {
    //                this.m_W[i, ind_vizinhos[k]] = 1.0;
    //                this.m_W[ind_vizinhos[k], i] = 1.0;
    //            }
    //        }
    //        for (int i = 0; i < this.m_shape.Count; i++)
    //        {
    //            this.m_W[i, i] = 0.0;
    //        }
    //    }

    //    public void MatrizWFromVizinhosNorm()
    //    {
    //        this.m_W = new double[this.m_shape.Count, this.m_shape.Count];
    //        int[] ind_vizinhos = new int[0];

    //        for (int i = 0; i < this.m_shape.Count; i++)
    //        {
    //            ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
    //            for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
    //            {
    //                this.m_W[i, ind_vizinhos[k]] = 1.0 / ((double)ind_vizinhos.GetLength(0));
    //                this.m_W[ind_vizinhos[k], i] = 1.0 / ((double)m_shape[ind_vizinhos[k]].ListaIndicesVizinhos.GetLength(0));
    //            }
    //        }
    //        for (int i = 0; i < this.m_shape.Count; i++)
    //        {
    //            this.m_W[i, i] = 0.0;
    //        }
    //    }

    //    public virtual void EstimateRegression()
    //    {
    //    }

    //    /// <summary>
    //    /// Função para gerar os vetores de significância dos coeficientes da regressão, a partir 
    //    /// da matriz de variância-covariância dos estimadores. Pode-se utilizar essa função para
    //    /// os diversos estimadores de modelos de regressão.
    //    /// </summary>
    //    protected void GeraSignificanciaCoeficientes()
    //    {
    //        MathNormaldist normal = new MathNormaldist();

    //        this.m_beta_stderror = new double[this.m_beta_hat.GetLength(0), 1];
    //        this.m_beta_tstat = new double[this.m_beta_hat.GetLength(0), 1];
    //        this.m_beta_pvalor = new double[this.m_beta_hat.GetLength(0), 1];
    //        this.m_beta_liminf_ci = new double[this.m_beta_hat.GetLength(0), 1];
    //        this.m_beta_limsup_ci = new double[this.m_beta_hat.GetLength(0), 1];

    //        for (int i = 0; i < this.m_beta_hat.GetLength(0); i++)
    //        {
    //            double prob_cauda = this.m_prob_confidence_intervals / 200.0;
    //            double cv = normal.invcdf(1.0 - prob_cauda);

    //            m_beta_stderror[i, 0] = Math.Sqrt(m_beta_hat_cov[i, i]);
    //            m_beta_tstat[i, 0] = m_beta_hat[i, 0] / m_beta_stderror[i, 0];
    //            m_beta_pvalor[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(m_beta_tstat[i, 0])));
    //            m_beta_liminf_ci[i, 0] = m_beta_hat[i, 0] - cv * m_beta_stderror[i, 0];
    //            m_beta_limsup_ci[i, 0] = m_beta_hat[i, 0] + cv * m_beta_stderror[i, 0];
    //        }
    //        clsUtilTools clsut=new clsUtilTools();
    //        double[,] predito = clsut.MatrizMult(m_X, m_beta_hat);

    //        if (this.m_tipo_modelo == TipoModeloEspacial.SAR)
    //        {
    //            predito = clsut.MatrizSoma(predito, clsut.MatrizMult(this.m_rho_hat, clsut.MatrizMult(this.m_W, m_Y)));
    //        }

    //        m_residuos = clsut.MatrizSubtracao(m_Y, predito);
    //        m_res_versus_w = clsut.MatrizMult(m_W, m_residuos);
    //    }
    //}
}

