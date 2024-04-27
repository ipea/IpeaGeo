using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IpeaGEO.RegressoesEspaciais
{
    class BLBayesianEstimation
    {

        #region Variaveis

        private string[] VariaveisDependentes;
        private string[] VariaveisIndependentes;
        private string[] VariaveisInstrumentais;
        public string[] _VariaveisDependentes
        {
            get { return VariaveisDependentes; }
            set { VariaveisDependentes = value; }
        }
        public string[] _VariaveisIndependentes
        {
            get { return VariaveisIndependentes; }
            set { VariaveisIndependentes = value; }
        }
        public string[] _VariaveisInstrumentais
        {
            get { return VariaveisInstrumentais; }
            set { VariaveisInstrumentais = value; }
        }
        private DataTable dtable;
        public DataTable _dtable
        {
            get { return dtable; }
            set { dtable = value; }
        }
        private clsIpeaShape shapeAlex;
        public clsIpeaShape Shape
        {
            get { return shapeAlex; }
            set { shapeAlex = value; }
        }

        #endregion

        private double[,] SimularNormal(double[,] vetor_medias, double[,] matriz_var)
        {
            //Gerando vetor de uma normal(0,1)
            double[,] vetor_inicial = new double[vetor_medias.GetLength(0),1];
            Random aleat = new Random();
            for (int i = 0; i < vetor_inicial.Length; i++)
            {
                vetor_inicial[i,0] = aleat.NextDouble();
            }

            //Decomposição de Cholesky
            clsUtilTools clt = new clsUtilTools();
            this.Cholesky(ref matriz_var);
            
            //matriz_var agora é apenas a matriz p tal que p'p=matriz_var
            double[,] vetor_final = clt.MatrizSoma(vetor_medias, clt.MatrizMult(matriz_var, vetor_inicial));
            
            return (vetor_final);
        }

        private int n=1;
        private double[,] el;
        public void Cholesky(ref double[,] a)
        {
            clsUtilTools clt = new clsUtilTools();
            //TODO: cholesky failed
            n = a.GetLength(0);
            el = clt.ArrayDoubleClone(a); 

            int i, j, k;
            double sum;
            if (el.GetLength(1) != n) throw new Exception("need square matrix");
            for (i = 0; i < n; i++)
            {
                for (j = i; j < n; j++)
                {
                    for (sum = el[i, j], k = i - 1; k >= 0; k--) sum -= el[i, k] * el[j, k];
                    if (i == j)
                    {
                        if (sum <= 0.0)
                            throw new Exception("Cholesky failed");
                        el[i, i] = Math.Sqrt(sum);
                    }
                    else el[j, i] = sum / el[i, i];
                }
            }
            for (i = 0; i < n; i++) for (j = 0; j < i; j++) el[j, i] = 0.0;
        }

        double nmk;
        int nrho;
        double[,] iota;
        double[,] z;
        double[,] den;
        double[,] bprior;
        double adj;
        double isum;
        double rnd;
        double[,] ind;
        double idraw;

        private double[,] m_detval = new double[0, 0];

        private void MontaDetVal()
        {
            double[,] id = clt.Identity(W.GetLength(0));

            int n = 100;
            m_detval = new double[n, 2];
            double rho = 0.0;
            double detrho = 0.0;
            double rho_max = 0.99;
            double rho_min = -0.99;
            double delta_rho = (rho_max - rho_min) / ((double)n - 1.0);
            for (int i = 0; i < n; i++)
            {
                rho = rho_min + ((double)i) * delta_rho;
                detrho = clt.LogDet(clt.MatrizSubtracao(id, clt.MatrizMult(rho, W)));

                m_detval[i, 0] = rho;
                m_detval[i, 1] = detrho;
            }
        }

        private double[,] beta_prior(double[,] rvec, double a1, double a2)
        {
            IpeaGEO.RegressoesEspaciais.MathBeta clb = new MathBeta();
            double B = clb.betai(a1, a2, 1.0);
            double[,] num = clt.MatrizDotPower(clt.MatrizSubtracao(1.0, rvec), a1 - 1.0);
            num = clt.MatrizDotMult(num, clt.MatrizDotPower(clt.MatrizSubtracao(1.0, rvec), a2 - 1.0));

            double den = Math.Pow(2.0, a1 + a2 - 1.0);
            double[,] res = clt.MatrizMult((1.0 / B)/den, num);
            res[0,0] = 0.001;
            res[res.GetLength(0)-1,0] = 0.001;
            return res;
        }

        public double draw_rho(double[,] epe0, double[,] eped, double[,] epe0d, int n, int k, double rho, double a1, double a2)
        {
            double[,] detval = m_detval;

            nmk = (n - k)/2;
            nrho = detval.GetLength(0);

            clsUtilTools clt = new clsUtilTools();

            Troschuetz.Random.BetaDistribution beta_prior = new Troschuetz.Random.BetaDistribution();

            iota = clt.Ones(nrho,1);

            double[,] detval_0=clt.SubColumnArrayDouble(detval,0);

            z = clt.MatrizSoma(clt.MatrizSubtracao(clt.MatrizMult(epe0,iota),clt.MatrizMult(clt.MatrizMult(detval,2),epe0)),clt.MatrizMult(detval,clt.MatrizMult(detval,eped)));
            z = clt.MatrizMult(clt.LogMatriz(z),-nmk);
            
            double[,] detval_1=clt.SubColumnArrayDouble(detval,1);

            den = clt.MatrizSoma(detval,z);
            
            beta_prior.Alpha = Convert.ToInt32(a1);
            beta_prior.Beta = Convert.ToInt32(a2);
            bprior = this.beta_prior(detval,a1,a2);
            den = clt.MatrizSoma(den,clt.LogMatriz(bprior));
            n = den.GetLength(0);
            Y = detval_0;
            adj = clt.Max(den);
            den = clt.MatrizSubtracao(den,adj);
            X = clt.ExpMatriz(den);

            for(int i=1;i<X.GetLength(0);i++)
            {
                isum += Y[i,0];
                isum += Y[i-1,0]*X[i,0];
                isum -= X[i-1,0]/2;
 
            }

            for (int i = 0; i < X.GetLength(0);i++)
            {
                for(int j=0;j<X.GetLength(1);j++)
                {
                    z[i, j] = Math.Abs(X[i, j] / isum);
                }
                
            }

            den = clt.Cumsum(z);

            Troschuetz.Random.ContinuousUniformDistribution uniform_prior = new Troschuetz.Random.ContinuousUniformDistribution();
            uniform_prior.Alpha = Convert.ToInt32(0);
            uniform_prior.Beta = Convert.ToInt32(1);
            rnd = uniform_prior.NextDouble()*clt.Sum(z);

            for (int i = 0; i < den.GetLength(0);i++)
            {
                for (int j = 0; j < den.GetLength(0); j++)
                {
                    if (den[i, j] <= rnd)
                    { 
                        ind[i,j]=den[i,j];
                    }
                }
            }

            if (idraw > 0 && idraw < nrho)
            {
                rho = detval[Convert.ToInt32(idraw), 0];
            }

            idraw = clt.Max(ind);

            return (rho);
        }



        public double InvGammaDistRandom(double a, double b)
        {
            return InvGammaDistRandom(a, b, 1, 1)[0, 0];
        }

        public double[,] InvGammaDistRandom(double a, double b, int n, int m)
        {
            double[,] r = GammaDistRandom(a, b, n, m);
            double[,] amostra = (double[,])r.Clone();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    amostra[i, j] = 1.0 / r[i, j];

            return amostra;
        }

        /// <summary>
        /// Gera uma amostra aleatória de uma distribuição gamma, com média 'a/b' e variância 'a/b^2'.
        /// </summary>
        public double[,] GammaDistRandom(double a, double b, int n, int m)
        {
            Troschuetz.Random.GammaDistribution gam = new Troschuetz.Random.GammaDistribution();
            gam.Alpha = a;
            gam.Theta = 1.0 / b;
          
            double[,] amostra = new double[n,m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    amostra[i, j] = (double)gam.NextDouble();
                }
            }
            return amostra;
        }
        
        clsUtilTools clt = new clsUtilTools();

        private double[,] Y; 
        private double[,] X;
        private double[,] T;
        private double[,] c;
        private double[,] W;
        private double[,] betas;
        private double[,] betas_antigo;
        private double rho;
        private double sigma2;
        private double sigma2_antigo;

        public void ExecutarModeloBayesiano()
        {

            Y = clt.GetMatrizFromDataTable(dtable, VariaveisDependentes); 
            X = clt.GetMatrizFromDataTable(dtable, VariaveisIndependentes);
            X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            T = clt.MatrizMult(10000000000.0, clt.Identity(X.GetLength(1)));

            //W deverá ser a matriz de vizinhança.
            W = clt.Identity(X.GetLength(0));

            clsIpeaShape cps = new clsIpeaShape();
            int tipo_vizinhanca = 1;
            shapeAlex.TipoVizinhanca = "Queen";
            cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca);

            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(0); j++)
                {
                    if (shapeAlex[i].EstaNaListaVizinhos(j))
                    {
                        W[i, j] = 1.0 / (double)shapeAlex[i].NumeroVizinhos;
                    }
                    else
                    {
                        W[i, j] = 0.0;
                    }
                }
            }

           

            

            c = clt.Zeros(X.GetLength(1), 1);
            rho = 0.0;
            sigma2 = 10000000000;
            double[,] T_star;
            double[,] c_star;
            double[,] A;
            double a_star;
            double[,] b_star;            
            double int_c = 0.00001;
            //for (int p = 0; p < c.Length; p++) c[p,0] = int_c;
            Random rd=new Random();

            //Numero de simulações
            int simulacoes = 1000;

            bool first = true;

            //Salvar os betas de cada simulaçao
            double[,] tabela_betas = new double[simulacoes, X.GetLength(1)];
            double[,] tabela_rho = new double[simulacoes, 1];
            double[,] tabela_sigma = new double[simulacoes, 1];

            for (int i = 0; i < simulacoes; i++)
            {   
                T_star = clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(X),X), clt.MatrizInversa(T)));
                c_star = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(X), X), clt.MatrizInversa(T))), clt.MatrizSoma(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(X), clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult(rho, W))), Y), clt.MatrizMult(clt.MatrizInversa(T), c)));
                
                //Gerando betas
                betas = this.SimularNormal(c_star, clt.MatrizMult(Math.Sqrt(sigma2), T_star));

                //Após simular beta, simulamos sigma 2.
                a_star = (double)X.GetLength(0) / 2.0;
                A = clt.MatrizSubtracao(clt.Identity(X.GetLength(0)),clt.MatrizMult(rho,W));
                b_star= clt.MatrizMult((0.5),(clt.MatrizMult(clt.MatrizTransp((clt.MatrizSubtracao(clt.MatrizMult(A,Y),clt.MatrizMult(X,betas)))),(clt.MatrizSubtracao(clt.MatrizMult(A,Y),clt.MatrizMult(X,betas))))));
                
                sigma2 = this.InvGammaDistRandom(a_star, b_star[0, 0]);
                

                //Após simular sigma2, simulamos rho
                double normal = int_c * rd.NextDouble();
                rho = rho + normal;

                A = clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult(rho, W));
                double expoente = (clt.MatrizMult((-((0.5) / sigma2)), clt.MatrizMult(clt.MatrizTransp(clt.MatrizSubtracao(clt.MatrizMult(A, Y), clt.MatrizMult(X, betas))), clt.MatrizSubtracao(clt.MatrizMult(A, Y), clt.MatrizMult(X, betas)))))[0,0];
                //expoente=Math.Exp(expoente);
                
                //Comentar estas linhas para que utilizemos os betas antigos e os novos para rho_star e rho_c
                //betas_antigo = betas;
                //sigma2_antigo = sigma2;

                if (i == 0)
                { 
                    betas_antigo = betas; 
                    sigma2_antigo = sigma2; 
                }
                A = clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult((rho - normal), W));
                double expoente2 = (clt.MatrizMult((-((0.5) / sigma2_antigo)), clt.MatrizMult(clt.MatrizTransp(clt.MatrizSubtracao(clt.MatrizMult(A, Y), clt.MatrizMult(X, betas_antigo))), clt.MatrizSubtracao(clt.MatrizMult(A, Y), clt.MatrizMult(X, betas_antigo)))))[0, 0];
                //expoente2 = Math.Exp(expoente2);

                //Guarda os betas e sigma2 do passo anterior
                betas_antigo = betas;
                sigma2_antigo = sigma2;

                //Calcular det via eigenvalues
                double expoente_star = 1.0;
                double expoente_c = 1.0;
                double[,] autovetores1 = new double[X.GetLength(0),X.GetLength(0)];
                double[] autovalores1 = new double[X.GetLength(0)];
                //clt.AutovaloresMatrizSimetrica(clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult(rho, W)), ref autovetores1, ref autovalores1);
                //for (int h = 0; h < autovalores1.Length; h++)
                //{
                //    expoente_star *= autovalores1[h];
                //}
                //expoente_star *= expoente;


                //clt.AutovaloresMatrizSimetrica(clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult((rho - normal), W)), ref autovetores1, ref autovalores1);
                //for (int h = 0; h < autovalores1.Length; h++)
                //{
                //    expoente_c *= autovalores1[h];
                //}
                //expoente_c *= expoente2;

                expoente_star = (expoente) + (clt.LogDet(clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult(rho, W))));                
                expoente_c = (expoente2) + (clt.LogDet(clt.MatrizSubtracao(clt.Identity(X.GetLength(0)), clt.MatrizMult((rho - normal), W))));

                double divisao = expoente_star / expoente_c;
                if (double.IsNaN(divisao) || double.IsInfinity(divisao))
                {
                    divisao = 1.0;
                }
                if (divisao < 0.0) divisao = 0.0;

                double prob = Math.Min(0, (expoente_star - expoente_c));
                prob = Math.Exp(prob);
                //double acceptance_rate = Math.Exp(prob);
                //if (prob >= 0.4 && prob <= 0.6) first = false;
                if (prob > 0.6 && first) int_c = int_c * (1.1);
                if (prob < 0.4 && first) int_c = int_c / (1.1);
                
                //Com esta probabilidade, sortear entre rho_c e rho_star
                Random rnd = new Random();
                double logu = rnd.NextDouble();
                if (logu < prob)
                {
                    rho = rho;
                }
                else
                {
                    rho = rho - normal;
                }

                /*
                Troschuetz.Random.BernoulliDistribution ber = new Troschuetz.Random.BernoulliDistribution();
                ber.Alpha = prob;
                int accept = ber.Next();
                if (accept == 0) rho = rho - normal;
                */
                //T = T_star;
                //c = c_star;

                //Guardando os resultados para depois tirar a média
                //Fazer um if(i>1000) como burn-in
                tabela_rho[i,0] = rho;
                tabela_sigma[i,0] = sigma2;
                for (int h = 0; h < betas.GetLength(0); h++)
                {
                    tabela_betas[i, h] = betas[h, 0];
                }

            }

            double media_rho = clt.Mean(tabela_rho);
            double media_sigma = clt.Mean(tabela_sigma);
            double[,] media_betas = clt.Meanc(tabela_betas);
            

            int gilberto = 7;

        }



        #region Modelos MatLab

        public void ExecutarModeloBayesianoMatlab()
        {
            Y = clt.GetMatrizFromDataTable(dtable, VariaveisDependentes);
            X = clt.GetMatrizFromDataTable(dtable, VariaveisIndependentes);
            X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            W = clt.Identity(X.GetLength(0));

            //W deverá ser a matriz de vizinhança.
            W = clt.Identity(X.GetLength(0));          

            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(0); j++)
                {
                    if (shapeAlex[i].EstaNaListaVizinhos(j))
                    {
                        W[i, j] = 1.0 / (double)shapeAlex[i].NumeroVizinhos;
                    }
                    else
                    {
                        W[i, j] = 0.0;
                    }
                }
            }

            IpeaGEO.RegressoesEspaciais.Classes.clsPrior prior = new IpeaGEO.RegressoesEspaciais.Classes.clsPrior();

            int k = 3;

            double[,] yss;
            double[,] AI;
            double[,] Wys;
            double[,] ys;
            double[,] xs;
            int ndraw = 1000;
            int nomit = 500;
            double[,] IN = new double[X.GetLength(0), 1];
            double[,] V = IN;
            double[,] Vi = IN;
            double[,] Wy = clt.MatrizMult(W, Y);
            double[,] TI = clt.MatrizInversa(prior.T);
            double [,] xpy;
            double[,] b;
            double[,] TIc = clt.MatrizMult(TI,prior.C);
            double[,] b0;
            double[,] bhat;
            double[,] xb;
            double nu1;
            double d1;
            double[,] e;
            double chi;
            double[,] ev;
            double[,] chiv = new double[X.GetLength(0),1];
            double[,] vi;
            double mm;
            double[,] bd;
            double[,] e0;
            double[,] ed;
            double[,] epe0;
            double[,] eped;
            double[,] epe0d;
            double rho;
            double[,] psave = new double[Convert.ToInt32(prior.iter)-nomit,1];
            double[,] ssave = new double[Convert.ToInt32(prior.iter)-nomit,1];
            double[,] bsave = new double[Convert.ToInt32(prior.iter)-nomit,k];
            double[,] vmean = new double[X.GetLength(0),1];
            double[,] xpx;
            double[,] xpWy;
            

            for (int i = 0; i < ndraw; i++)
            {
                xs = clt.MatrizMult(X,clt.MatrizDotPower(V,0.5));
                ys = clt.MatrizMult(clt.MatrizDotPower(V, 0.5), Y);
                Wys = clt.MatrizMult(clt.MatrizDotPower(V, 0.5), Wy);
                AI = clt.MatrizMult(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),xs),clt.MatrizMult(prior.sige,TI)),clt.MatrizInversa(clt.Identity(k)));
                yss = clt.MatrizSubtracao(ys, clt.MatrizMult(prior.rho, Wys));
                xpy = clt.MatrizMult(clt.MatrizTransp(xs),yss);
                b = clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),yss),clt.MatrizMult(prior.sige,TIc));
                b0=clt.MatrizMult(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),xs),clt.MatrizMult(prior.sige,TI)),clt.MatrizInversa(b));

                Random rnd = new Random();

                bhat=SimularNormal(b0,clt.MatrizMult(prior.sige,AI));
                xb = clt.MatrizMult(xs, bhat);
                nu1 = xs.GetLength(0) + 2 * prior.nu;
                e = clt.MatrizSubtracao(yss,xb);
                d1=2*prior.d0 + clt.MatrizMult(clt.MatrizTransp(e),e)[0,0];

                Troschuetz.Random.ChiSquareDistribution chis = new Troschuetz.Random.ChiSquareDistribution();

                chis.Alpha = Convert.ToInt32(nu1);
                chi = chis.NextDouble();
                prior.sige = d1 / chi;
                ev = clt.MatrizSubtracao(Y,clt.MatrizSoma(clt.MatrizMult(Wy,rho),clt.MatrizMult(X,bhat)));
                
              
                chis.Alpha = Convert.ToInt32(prior.rval+1);
                for(int n=0;n < X.GetLength(0);n++)
                {
                    chiv[n,1] = chis.NextDouble();
                }

                vi = clt.MatrizSoma(clt.MatrizDiv(clt.MatrizDotMult(ev,ev),prior.sige),clt.MatrizMult(IN,prior.rval));

                for(int m=0;m<ev.GetLength(0);m++)
                {
                    for(int j=0;j<ev.GetLength(1);j++)
                    {
                        vi[m,j]=vi[m,j]/chiv[m,j];
                    }
                }

                for(int m=0;m<vi.GetLength(0);m++)
                {
                    for(int j=0;j<vi.GetLength(1);j++)
                    {
                        V[m,j]=V[m,j]/vi[m,j];
                    }
                }

                Troschuetz.Random.GammaDistribution gamm = new Troschuetz.Random.GammaDistribution();

                gamm.Alpha = Convert.ToInt32(mm);
                gamm.Theta = Convert.ToInt32(prior.kk);
                
                if(mm != 0)
                {
                  prior.rval = gamm.NextDouble();   
                }

                b0 = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),xs),clt.MatrizMult(TI,prior.sige))),(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),ys),clt.MatrizMult(prior.sige,TIc))));
                bd = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),xs),clt.MatrizMult(TI,prior.sige))),(clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(xs),Wys),clt.MatrizMult(prior.sige,TIc))));
                e0 = clt.MatrizSubtracao(ys,clt.MatrizMult(xs,b0));
                ed = clt.MatrizSubtracao(Wys,clt.MatrizMult(xs,bd));
                epe0 = clt.MatrizMult(clt.MatrizTransp(e0),e0);
                eped = clt.MatrizMult(clt.MatrizTransp(ed),ed);
                epe0d = clt.MatrizMult(clt.MatrizTransp(ed),e0);

                this.MontaDetVal();
                rho = draw_rho(epe0,eped,epe0d,X.GetLength(0),k,rho,prior.a1,prior.a2);

                if( prior.iter > nomit)
                {
                    psave[Convert.ToInt32(prior.iter)-nomit,0] = rho; 
                    ssave[Convert.ToInt32(prior.iter)-nomit,0] = prior.sige;
                    bsave[Convert.ToInt32(prior.iter)-nomit,k] = bhat[ndraw-nomit,k];
                    vmean = clt.MatrizSoma(vmean,vi);
                    
                }
                
                double[,] rvals = new double[Convert.ToInt32(prior.iter)-nomit,1];

                if(mm != 0)
                {
                    rvals[ndraw-nomit,0] = prior.rval;
                }

                prior.iter = prior.iter + 1;

                prior.iter = 1;
                xpx = clt.MatrizMult(clt.MatrizTransp(X),X);
                xpy = clt.MatrizMult(clt.MatrizTransp(X),Y);
                Wy = clt.MatrizMult(W,Y);
                xpWy = clt.MatrizMult(clt.MatrizTransp(X),Wy);

            }


            double[,] bdraws;
            double[] ree;

            for(int i = 0; i < ndraw; i++)
            {
                    Troschuetz.Random.ChiSquareDistribution chis = new Troschuetz.Random.ChiSquareDistribution();

                    AI = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(xpx,clt.MatrizMult(prior.sige,TI))),clt.Identity(k));
                    ys = clt.MatrizSubtracao(Y,clt.MatrizMult(rho,Wy));
                    b = clt.MatrizSoma(clt.MatrizMult(clt.MatrizTransp(X),ys),clt.MatrizMult(prior.sige,TIc));
                    b0 = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(xpx,clt.MatrizMult(prior.sige,TI))),b);
                    bhat=SimularNormal(b0,clt.MatrizMult(prior.sige,AI));
                    xb = clt.MatrizDotMult(X,bhat);
                    nu1 = xs.GetLength(0) + 2 * prior.nu;
                    e = clt.MatrizSubtracao(ys,xb);
                    d1=2*prior.d0 + clt.MatrizMult(clt.MatrizTransp(e),e)[0,0];
                    chis.Alpha = Convert.ToInt32(nu1);
                    chi = chis.NextDouble();
                    prior.sige = d1 / chi;
                    AI = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(xpx,clt.MatrizMult(prior.sige,TI))),clt.Identity(k));
                    b0 = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(prior.sige,TI),xpx)),clt.MatrizSoma(xpy,clt.MatrizMult(prior.sige,TIc)));
                    bd = clt.MatrizMult(clt.MatrizInversa(clt.MatrizSoma(clt.MatrizMult(prior.sige,TI),xpx)),clt.MatrizSoma(xpWy,clt.MatrizMult(prior.sige,TIc)));
                    e0 = clt.MatrizSubtracao(Y,clt.MatrizMult(X,b0));
                    ed = clt.MatrizSubtracao(Wy,clt.MatrizMult(X,bd));
                    epe0 = clt.MatrizMult(clt.MatrizTransp(e0),e0);
                    eped = clt.MatrizMult(clt.MatrizTransp(ed),ed);
                    epe0d = clt.MatrizMult(clt.MatrizTransp(ed),e0);

                    this.MontaDetVal();
                    rho = draw_rho(epe0,eped,epe0d,X.GetLength(0),k,rho,prior.a1,prior.a2);

          

                if(prior.iter > nomit)
                {

                    psave[Convert.ToInt32(prior.iter)-nomit,0] = rho; 
                    ssave[Convert.ToInt32(prior.iter)-nomit,0] = prior.sige;
                    bsave[Convert.ToInt32(prior.iter)-nomit,k] = bhat[ndraw-nomit,k];
                    vmean = clt.MatrizSoma(vmean,vi);
                    
                }
           }

                prior.iter = prior.iter + 1;

                double uiter = 50;
                double maxoderu=100;
                double[,] tracew;
                double nobs = X.GetLength(0);
                double[,] rv;
                double[,] pdraws;
                double[,] wjjju = rv;
                double[,] rmat;

                Troschuetz.Random.NormalDistribution normal = new Troschuetz.Random.NormalDistribution();

                normal.Mu = Convert.ToInt32(nobs);
                normal.Sigma = Convert.ToInt32(uiter);

                for(int j=0;j<X.GetLength(0);j++)
                {
                    rv[j,1] = normal.NextDouble();
                }

                tracew = clt.Zeros(Convert.ToInt32(maxoderu),1);
                

                for(int j=0;j<maxoderu;j++)
                {
                    wjjju=clt.MatrizMult(W,wjjju);
                    tracew[j,0] = clt.Mean(clt.MatrizDotMult(rv,wjjju));
                }
            double[,] traces=tracew;
            traces[0,0]=0;
            traces[1,0]=clt.Sum(clt.MatrizDotMult(clt.MatrizTransp(W),W))/nobs;
            double[,] trs=clt.Concatev(clt.Ones(1,1),traces);
            double ntrs = trs.GetLength(0);
            double[,] trbig=clt.MatrizTransp(trs);
            double p = X.GetLength(1);
            double[,,] total;
            double[,,] direct;
            double[,,] indirect;
            
            //Se há intercepto
            int cflag=1;

            if(cflag==1)
            {
                for(int j=1;j<bsave.GetLength(0);j++)
                {
                    bdraws[j-1,0] = bsave[j,0];
                    
                }
            }

            else if(cflag ==0)
            {
                bdraws = bsave;
            }
            
            pdraws = psave;
            
            for(int j=0;j<ntrs;j++)
            {
                ree[j]= j;
            
            }

            rmat = clt.Zeros(1,Convert.ToInt32(ntrs));
            
            for(int j=0;j<(ndraw-nomit);j++)
            {
                for(int m=0;m<p;m++)
                {
                    for(int n=0;n<ntrs;n++)
                    {
                        total[j,m,n]=0; 
                        direct[j,m,n]=0; 
                        indirect[j,m,n]=0; 
                    }
                }
                
            }

            for(int j=0;j<(ndraw-nomit);j++)
            {
                rmat[j,0] = Math.Pow(pdraws[j,0],ree[j]);
       
                for(int m=0;m<(ndraw-nomit);m++)
                {
                    rmat[m,0] = bdraws[j,m];
                    total[j,m,]
                }
            }


        
for i=1:ndraw-nomit;
    rmat = pdraws(i,1).^ree;
    for j=1:p;
            beta = [bdraws(i,j)];
            total(i,j,:) = beta(1,1)*rmat;
    direct(i,j,:) = (beta*trbig).*rmat;
    indirect(i,j,:) = total(i,j,:) - direct(i,j,:);
    end;

end;
       






           
                    
            }




        }
               

        #endregion










    }
}
