using System;
using System.Collections.Generic;
using System.Text;

namespace IpeaGeo
{
    #region Base class Gauleg18
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class Gauleg18
    {
        protected int ngau = 18;
        protected double[] y = new double[18]{0.0021695375159141994,
                                                0.011413521097787704,0.027972308950302116,0.051727015600492421,
                                                0.082502225484340941, 0.12007019910960293,0.16415283300752470,
                                                0.21442376986779355, 0.27051082840644336, 0.33199876341447887,
                                                0.39843234186401943, 0.46931971407375483, 0.54413605556657973,
                                                0.62232745288031077, 0.70331500465597174, 0.78649910768313447,
                                                0.87126389619061517, 0.95698180152629142};

        protected double[] w = new double[18]{0.0055657196642445571,
                                                0.012915947284065419,0.020181515297735382,0.027298621498568734,
                                                0.034213810770299537,0.040875750923643261,0.047235083490265582,
                                                0.053244713977759692,0.058860144245324798,0.064039797355015485,
                                                0.068745323835736408,0.072941885005653087,0.076598410645870640,
                                                0.079687828912071670,0.082187266704339706,0.084078218979661945,
                                                0.085346685739338721,0.085983275670394821};
    }
    #endregion

    #region Special functions
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathSpecialFunctions
    {
        public MathSpecialFunctions()
        {
        }

        public static double invxlogx(double y)
        {
            const double ooe = 0.367879441171442322;
            double t, u, to = 0.0;
            if (y >= 0.0 || y <= -ooe) throw new Exception("no such inverse value");
            if (y < -0.2) u = Math.Log(ooe - Math.Sqrt(2 * ooe * (y + ooe)));
            else u = -10.0;
            do
            {
                u += (t = (Math.Log(y / u) - u) * (u / (1.0 + u)));
                if (t < 1.0e-8 && Math.Abs(t + to) < 0.01 * Math.Abs(t)) break;
                to = t;
            } while (Math.Abs(t / u) > 1.0e-15);
            return Math.Exp(u);
        }

        public static double gammln(double xx)
        {
            int j;
            double x, tmp, y, ser;
            double[] cof = new double[14]{57.1562356658629235,-59.5979603554754912,
	        14.1360979747417471,-0.491913816097620199,.339946499848118887e-4,
	        .465236289270485756e-4,-.983744753048795646e-4,.158088703224912494e-3,
	        -.210264441724104883e-3,.217439618115212643e-3,-.164318106536763890e-3,
	        .844182239838527433e-4,-.261908384015814087e-4,.368991826595316234e-5};
            if (xx <= 0) throw new Exception("bad arg in gammln");
            y = x = xx;
            tmp = x + 5.24218750000000000;
            tmp = (x + 0.5) * Math.Log(tmp) - tmp;
            ser = 0.999999999999997092;
            for (j = 0; j < 14; j++) ser += cof[j] / ++y;
            return tmp + Math.Log(2.5066282746310005 * ser / x);
        }

        public static double factrl(int n)
        {
            double[] a = new double[171];
            bool init = true;
            if (init)
            {
                init = false;
                a[0] = 1.0;
                for (int i = 1; i < 171; i++) a[i] = i * a[i - 1];
            }
            if (n < 0 || n > 170) throw new Exception("factrl out of range");
            return a[n];
        }

        public static double factln(int n)
        {
            int NTOP = 2000;
            double[] a = new double[NTOP];
            bool init = true;
            if (init)
            {
                init = false;
                for (int i = 0; i < NTOP; i++) a[i] = gammln(i + 1.0);
            }
            if (n < 0) throw new Exception("negative arg in factln");
            if (n < NTOP) return a[n];
            return gammln(n + 1.0);
        }

        public static double bico(int n, int k)
        {
            if (n < 0 || k < 0 || k > n) throw new Exception("bad args in bico");
            if (n < 171) return Math.Floor(0.5 + factrl(n) / (factrl(k) * factrl(n - k)));
            return Math.Floor(0.5 + Math.Exp(factln(n) - factln(k) - factln(n - k)));
        }

        public static double beta(double z, double w)
        {
            return Math.Exp(gammln(z) + gammln(w) - gammln(z + w));
        }
    }
    #endregion

    #region Classe MathBeta
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathBeta : Gauleg18
    {
        protected int SWITCH = 3000;
        protected double EPS = 1.0e-6, FPMIN = 1.0e-8;

        public double betai(double a, double b, double x)
        {
            double bt;
            if (a <= 0.0 || b <= 0.0) throw new Exception("Bad a or b in routine betai");
            if (x < 0.0 || x > 1.0) throw new Exception("Bad x in routine betai");
            if (x == 0.0 || x == 1.0) return x;
            if (a > SWITCH && b > SWITCH) return betaiapprox(a, b, x);
            bt = Math.Exp(MathSpecialFunctions.gammln(a + b) - MathSpecialFunctions.gammln(a) - MathSpecialFunctions.gammln(b) + a * Math.Log(x) + b * Math.Log(1.0 - x));
            if (x < (a + 1.0) / (a + b + 2.0)) return bt * betacf(a, b, x) / a;
            else return 1.0 - bt * betacf(b, a, 1.0 - x) / b;
        }

        public double betacf(double a, double b, double x)
        {
            int m, m2;
            double aa, c, d, del, h, qab, qam, qap;
            qab = a + b;
            qap = a + 1.0;
            qam = a - 1.0;
            c = 1.0;
            d = 1.0 - qab * x / qap;
            if (Math.Abs(d) < FPMIN) d = FPMIN;
            d = 1.0 / d;
            h = d;
            for (m = 1; m < 10000; m++)
            {
                m2 = 2 * m;
                aa = m * (b - m) * x / ((qam + m2) * (a + m2));
                d = 1.0 + aa * d;
                if (Math.Abs(d) < FPMIN) d = FPMIN;
                c = 1.0 + aa / c;
                if (Math.Abs(c) < FPMIN) c = FPMIN;
                d = 1.0 / d;
                h *= d * c;
                aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
                d = 1.0 + aa * d;
                if (Math.Abs(d) < FPMIN) d = FPMIN;
                c = 1.0 + aa / c;
                if (Math.Abs(c) < FPMIN) c = FPMIN;
                d = 1.0 / d;
                del = d * c;
                h *= del;
                if (Math.Abs(del - 1.0) <= EPS) break;
            }
            return h;
        }

        public double betaiapprox(double a, double b, double x)
        {
            int j;
            double xu, t, sum, ans;
            double a1 = a - 1.0, b1 = b - 1.0, mu = a / (a + b);
            double lnmu = Math.Log(mu), lnmuc = Math.Log(1.0 - mu);
            t = Math.Sqrt(a * b / (Math.Pow(a + b, 2.0) * (a + b + 1.0)));
            if (x > a / (a + b))
            {
                if (x >= 1.0) return 1.0;
                xu = Math.Min(1.0, Math.Max(mu + 10.0 * t, x + 5.0 * t));
            }
            else
            {
                if (x <= 0.0) return 0.0;
                xu = Math.Max(0.0, Math.Min(mu - 10.0 * t, x - 5.0 * t));
            }
            sum = 0;
            for (j = 0; j < 18; j++)
            {
                t = x + (xu - x) * y[j];
                sum += w[j] * Math.Exp(a1 * (Math.Log(t) - lnmu) + b1 * (Math.Log(1 - t) - lnmuc));
            }
            ans = sum * (xu - x) * Math.Exp(a1 * lnmu - MathSpecialFunctions.gammln(a) + b1 * lnmuc - MathSpecialFunctions.gammln(b) + MathSpecialFunctions.gammln(a + b));
            return ans > 0.0 ? 1.0 - ans : -ans;
        }

        public double invbetai(double p, double a, double b)
        {
            const double EPS = 1.0e-8;
            double pp, t, u, err, x, al, h, w, afac, a1 = a - 1.0, b1 = b - 1.0;
            int j;
            if (p <= 0.0) return 0.0;
            else if (p >= 1.0) return 1.0;
            else if (a >= 1.0 && b >= 1.0)
            {
                pp = (p < 0.5) ? p : 1.0 - p;
                t = Math.Sqrt(-2.0 * Math.Log(pp));
                x = (2.30753 + t * 0.27061) / (1.0 + t * (0.99229 + t * 0.04481)) - t;
                if (p < 0.5) x = -x;
                al = ((x * x) - 3.0) / 6.0;
                h = 2.0 / (1.0 / (2.0 * a - 1.0) + 1.0 / (2.0 * b - 1.0));
                w = (x * Math.Sqrt(al + h) / h) - (1.0 / (2.0 * b - 1) - 1.0 / (2.0 * a - 1.0)) * (al + 5.0 / 6.0 - 2.0 / (3.0 * h));
                x = a / (a + b * Math.Exp(2.0 * w));
            }
            else
            {
                double lna = Math.Log(a / (a + b)), lnb = Math.Log(b / (a + b));
                t = Math.Exp(a * lna) / a;
                u = Math.Exp(b * lnb) / b;
                w = t + u;
                if (p < t / w) x = Math.Pow(a * w * p, 1.0 / a);
                else x = 1.0 - Math.Pow(b * w * (1.0 - p), 1.0 / b);
            }
            afac = -MathSpecialFunctions.gammln(a) - MathSpecialFunctions.gammln(b) + MathSpecialFunctions.gammln(a + b);
            for (j = 0; j < 10; j++)
            {
                if (x == 0.0 || x == 1.0) return x;
                err = betai(a, b, x) - p;
                t = Math.Exp(a1 * Math.Log(x) + b1 * Math.Log(1.0 - x) + afac);
                u = err / t;
                x -= (t = u / (1.0 - 0.5 * Math.Min(1.0, u * (a1 / x - b1 / (1.0 - x)))));
                if (x <= 0.0) x = 0.5 * (x + t);
                if (x >= 1.0) x = 0.5 * (x + t + 1.0);
                if (Math.Abs(t) < EPS * x && j > 0) break;
            }
            return x;
        }
    }
    #endregion

    #region Classe MathGamma
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathGamma : Gauleg18
    {
        public MathGamma()
        {
        }

        protected int ASWITCH = 100;
        protected double EPS = 1.0e-8;
        protected double FPMIN = 1.0e-10;
        protected double gln;

        public double trigamma(double x)
        {
            double[] coef = {46945.803361843835163, -156060.52077844455628, 206504.95680141058236,
								-138893.47750953880545, 50317.964150857072160, -9601.5923291827750158,
								878.58559308952456257, -31.551539060986091211, .29081434211622271742,
								-.23198276304949725592e-3, .12516396700509326789e-9};

            double xx = x - 1.0;
            double tmp = 1.0 / (xx + 11.5) + ((11.0) / Math.Pow(xx + 11.5, 2));
            double y = xx;
            double aux = 1.0;
            double aux1 = 0.0;
            double aux2 = 0.0;
            for (int i = 0; i < 11; i++)
            {
                y++;
                aux += coef[i] / y;
                aux1 -= coef[i] / Math.Pow(y, 2);
                aux2 += 2 * coef[i] / Math.Pow(y, 3);
            }
            return tmp + (aux2 / aux) - Math.Pow(aux1 / aux, 2);
        }

        public double digamma(double x)
        {
            double[] coef = {46945.803361843835163, -156060.52077844455628, 206504.95680141058236,
				             -138893.47750953880545, 50317.964150857072160, -9601.5923291827750158,
							 878.58559308952456257, -31.551539060986091211, .29081434211622271742,
							 -.23198276304949725592e-3, .12516396700509326789e-9};

            double xx = x - 1.0;
            double tmp = Math.Log(xx + 11.5) - ((11.0) / (xx + 11.5));
            double y = xx;
            double aux = 0.0;
            double aux1 = 1.0;
            for (int i = 0; i < 11; i++)
            {
                y++;
                aux -= coef[i] / Math.Pow(y, 2);
                aux1 += coef[i] / y;
            }
            return tmp + (aux / aux1);
        }

        public double gammp(double a, double x)
        {
            if (x < 0.0 || a <= 0.0) throw new Exception("bad args in gammp");
            if (x == 0.0) return 0.0;
            else if ((int)a >= ASWITCH) return gammpapprox(a, x, 1);
            else if (x < a + 1.0) return gser(a, x);
            else return 1.0 - gcf(a, x);
        }

        public double gammq(double a, double x)
        {
            if (x < 0.0 || a <= 0.0) throw new Exception("bad args in gammq");
            if (x == 0.0) return 1.0;
            else if ((int)a >= ASWITCH) return gammpapprox(a, x, 0);
            else if (x < a + 1.0) return 1.0 - gser(a, x);
            else return gcf(a, x);
        }

        public double gser(double a, double x)
        {
            double sum, del, ap;
            gln = MathSpecialFunctions.gammln(a);
            ap = a;
            del = sum = 1.0 / a;
            for (; ; )
            {
                ++ap;
                del *= x / ap;
                sum += del;
                if (Math.Abs(del) < Math.Abs(sum) * EPS)
                {
                    return sum * Math.Exp(-x + a * Math.Log(x) - gln);
                }
            }
        }

        public double gcf(double a, double x)
        {
            int i;
            double an, b, c, d, del, h;
            gln = MathSpecialFunctions.gammln(a);
            b = x + 1.0 - a;
            c = 1.0 / FPMIN;
            d = 1.0 / b;
            h = d;
            for (i = 1; ; i++)
            {
                an = -i * (i - a);
                b += 2.0;
                d = an * d + b;
                if (Math.Abs(d) < FPMIN) d = FPMIN;
                c = b + an / c;
                if (Math.Abs(c) < FPMIN) c = FPMIN;
                d = 1.0 / d;
                del = d * c;
                h *= del;
                if (Math.Abs(del - 1.0) <= EPS) break;
            }
            return Math.Exp(-x + a * Math.Log(x) - gln) * h;
        }

        public double gammpapprox(double a, double x, int psig)
        {
            int j;
            double xu, t, sum, ans;
            double a1 = a - 1.0, lna1 = Math.Log(a1), sqrta1 = Math.Sqrt(a1);
            gln = MathSpecialFunctions.gammln(a);
            if (x > a1) xu = Math.Max(a1 + 11.5 * sqrta1, x + 6.0 * sqrta1);
            else xu = Math.Max(0.0, Math.Min(a1 - 7.5 * sqrta1, x - 5.0 * sqrta1));
            sum = 0;
            for (j = 0; j < ngau; j++)
            {
                t = x + (xu - x) * y[j];
                sum += w[j] * Math.Exp(-(t - a1) + a1 * (Math.Log(t) - lna1));
            }
            ans = sum * (xu - x) * Math.Exp(a1 * (lna1 - 1.0) - gln);
            //return (psig?(ans>0.0? 1.0-ans:-ans):(ans>=0.0? ans:1.0+ans));
            return (psig > 0 ? (ans > 0.0 ? 1.0 - ans : -ans) : (ans >= 0.0 ? ans : 1.0 + ans));
        }

        public double invgammp(double p, double a)
        {
            int j;
            double x, err, t, u, pp, lna1 = 0.0, afac = 0.0, a1 = a - 1;
            double EPS = 1.0e-8;
            gln = MathSpecialFunctions.gammln(a);
            if (a <= 0.0) throw new Exception("a must be pos in invgammap");
            if (p >= 1.0) return Math.Max(100.0, a + 100.0 * Math.Sqrt(a));
            if (p <= 0.0) return 0.0;
            if (a > 1.0)
            {
                lna1 = Math.Log(a1);
                afac = Math.Exp(a1 * (lna1 - 1.0) - gln);
                pp = (p < 0.5) ? p : 1.0 - p;
                t = Math.Sqrt(-2.0 * Math.Log(pp));
                x = (2.30753 + t * 0.27061) / (1.0 + t * (0.99229 + t * 0.04481)) - t;
                if (p < 0.5) x = -x;
                x = Math.Max(1.0e-3, a * Math.Pow(1.0 - 1.0 / (9.0 * a) - x / (3.0 * Math.Sqrt(a)), 3));
            }
            else
            {
                t = 1.0 - a * (0.253 + a * 0.12);
                if (p < t) x = Math.Pow(p / t, 1.0 / a);
                else x = 1.0 - Math.Log(1.0 - (p - t) / (1.0 - t));
            }
            for (j = 0; j < 12; j++)
            {
                if (x <= 0.0) return 0.0;
                err = gammp(a, x) - p;
                if (a > 1.0) t = afac * Math.Exp(-(x - a1) + a1 * (Math.Log(x) - lna1));
                else t = Math.Exp(-x + a1 * Math.Log(x) - gln);
                u = err / t;
                x -= (t = u / (1.0 - 0.5 * Math.Min(1.0, u * ((a - 1.0) / x - 1))));
                if (x <= 0.0) x = 0.5 * (x + t);
                if (Math.Abs(t) < EPS * x) break;
            }
            return x;
        }
    }
    #endregion

    #region Classe MathGammadist
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathGammadist : MathGamma
    {
        private double alph, bet, fac;

        public MathGammadist(double aalph)
        {
            alph = aalph;
            bet = 1.0;
            if (alph <= 0.0 || bet <= 0.0) throw new Exception("bad alph,bet in Gammadist");
            fac = alph * Math.Log(bet) - MathSpecialFunctions.gammln(alph);
        }

        public MathGammadist(double aalph, double bbet)
        {
            alph = aalph;
            bet = bbet;
            if (alph <= 0.0 || bet <= 0.0) throw new Exception("bad alph,bet in Gammadist");
            fac = alph * Math.Log(bet) - MathSpecialFunctions.gammln(alph);
        }

        public double p(double x)
        {
            if (x <= 0.0) throw new Exception("bad x in Gammadist");
            return Math.Exp(-bet * x + (alph - 1.0) * Math.Log(x) + fac);
        }
        public double cdf(double x)
        {
            if (x < 0.0) throw new Exception("bad x in Gammadist");
            return gammp(alph, bet * x);
        }

        public double invcdf(double p)
        {
            if (p < 0.0 || p >= 1.0) throw new Exception("bad p in Gammadist");
            return invgammp(p, alph) / bet;
        }
    }
    #endregion

    #region Classe MathStudenttdist
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathStudenttdist : MathBeta
    {
        double nu, mu, sig, np, fac;

        public MathStudenttdist(double nnu)
        {
            double mmu = 0.0; double ssig = 1.0;
            nu = nnu;
            mu = mmu;
            sig = ssig;
            if (sig <= 0.0 || nu <= 0.0) throw new Exception("bad sig,nu in Studentdist");
            np = 0.5 * (nu + 1.0);
            fac = MathSpecialFunctions.gammln(np) - MathSpecialFunctions.gammln(0.5 * nu);
        }

        public MathStudenttdist(double nnu, double mmu, double ssig)
        {
            nu = nnu;
            mu = mmu;
            sig = ssig;
            if (sig <= 0.0 || nu <= 0.0) throw new Exception("bad sig,nu in Studentdist");
            np = 0.5 * (nu + 1.0);
            fac = MathSpecialFunctions.gammln(np) - MathSpecialFunctions.gammln(0.5 * nu);
        }

        public double p(double t)
        {
            return Math.Exp(-np * Math.Log(1.0 + Math.Pow((t - mu) / sig, 2.0) / nu) + fac)
                / (Math.Sqrt(3.14159265358979324 * nu) * sig);
        }

        public double cdf(double t)
        {
            double p = 0.5 * betai(0.5 * nu, 0.5, nu / (nu + Math.Pow((t - mu) / sig, 2.0)));
            if (t >= mu) return 1.0 - p;
            else return p;
        }

        public double invcdf(double p)
        {
            if (p <= 0.0 || p >= 1.0) throw new Exception("bad p in Studentdist");
            double x = invbetai(2.0 * Math.Min(p, 1.0 - p), 0.5 * nu, 0.5);
            x = sig * Math.Sqrt(nu * (1.0 - x) / x);
            return (p >= 0.5 ? mu + x : mu - x);
        }
    }
    #endregion

    #region Classe MathPoissondist
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathPoissondist : MathGamma
    {
        private double lam;

        public MathPoissondist(double llam)
        {
            lam = llam;
            if (lam <= 0.0) throw new Exception("bad lam in Poissondist");
        }

        public double p(int n)
        {
            if (n < 0) throw new Exception("bad n in Poissondist");
            return Math.Exp(-lam + n * Math.Log(lam) - MathSpecialFunctions.gammln(n + 1.0));
        }

        public double cdf(int n)
        {
            if (n < 0) throw new Exception("bad n in Poissondist");
            if (n == 0) return 0.0;
            return gammq((double)n, lam);
        }

        public double cdf(double n)
        {
            if (n < 0) throw new Exception("bad n in Poissondist");
            if (n == 0) return 0.0;
            return gammq((double)n, lam);
        }

        public int invcdf(double p)
        {
            int n, nl, nu, inc = 1;
            if (p <= 0.0 || p >= 1.0) throw new Exception("bad p in Poissondist");
            if (p < Math.Exp(-lam)) return 0;
            n = (int)Math.Max(Math.Sqrt(lam), 5.0);
            if (p < cdf(n))
            {
                do
                {
                    n = Math.Max(n - inc, 0);
                    inc *= 2;
                } while (p < cdf(n));
                nl = n; nu = n + inc / 2;
            }
            else
            {
                do
                {
                    n += inc;
                    inc *= 2;
                } while (p > cdf(n));
                nu = n; nl = n - inc / 2;
            }
            while (nu - nl > 1)
            {
                n = (nl + nu) / 2;
                if (p < cdf(n)) nu = n;
                else nl = n;
            }
            return nl;
        }
    }
    #endregion

    #region Classe MathChisqdist
    //[Obsolete("Use MathNet.Numerics.Distributions instead.")]
    //public class MathChisqdist : MathGamma
    //{
    //    private double nu, fac;

    //    public MathChisqdist(double nnu)
    //    {
    //        nu = nnu;
    //        if (nu <= 0.0) throw new Exception("bad nu in Chisqdist");
    //        fac = 0.693147180559945309 * (0.5 * nu) + MathSpecialFunctions.gammln(0.5 * nu);
    //    }

    //    public double p(double x2)
    //    {
    //        if (x2 <= 0.0) throw new Exception("bad x2 in Chisqdist");
    //        return Math.Exp(-0.5 * (x2 - (nu - 2.0) * Math.Log(x2)) - fac);
    //    }

    //    public double cdf(double x2)
    //    {
    //        if (x2 < 0.0) throw new Exception("bad x2 in Chisqdist");
    //        return gammp(0.5 * nu, 0.5 * x2);
    //    }

    //    public double invcdf(double p)
    //    {
    //        if (p < 0.0 || p >= 1.0) throw new Exception("bad p in Chisqdist");
    //        return 2.0 * invgammp(p, 0.5 * nu);
    //    }
    //}
    #endregion

    #region Classe MathErf
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathErf
    {
        public MathErf()
        {
        }

        protected int ncof = 28;
        protected double[] cof = new double[28]{-1.3026537197817094, 6.4196979235649026e-1,
	                                            1.9476473204185836e-2,-9.561514786808631e-3,-9.46595344482036e-4,
	                                            3.66839497852761e-4,4.2523324806907e-5,-2.0278578112534e-5,
	                                            -1.624290004647e-6,1.303655835580e-6,1.5626441722e-8,-8.5238095915e-8,
	                                            6.529054439e-9,5.059343495e-9,-9.91364156e-10,-2.27365122e-10,
	                                            9.6467911e-11, 2.394038e-12,-6.886027e-12,8.94487e-13, 3.13092e-13,
	                                            -1.12708e-13,3.81e-16,7.106e-15,-1.523e-15,-9.4e-17,1.21e-16,-2.8e-17};

        public double erf(double x)
        {
            if (x >= 0.0) return 1.0 - erfccheb(x);
            else return erfccheb(-x) - 1.0;
        }

        public double erfc(double x)
        {
            if (x >= 0.0) return erfccheb(x);
            else return 2.0 - erfccheb(-x);
        }

        public double erfccheb(double z)
        {
            int j;
            double t, ty, tmp, d = 0.0, dd = 0.0;
            if (z < 0.0) throw new Exception("erfccheb requires nonnegative argument");
            t = 2.0 / (2.0 + z);
            ty = 4.0 * t - 2.0;
            for (j = ncof - 1; j > 0; j--)
            {
                tmp = d;
                d = ty * d - dd + cof[j];
                dd = tmp;
            }
            return t * Math.Exp(-z * z + 0.5 * (cof[0] + ty * d) - dd);
        }

        public double inverfc(double p)
        {
            double x, err, t, pp;
            if (p >= 2.0) return -100.0;
            if (p <= 0.0) return 100.0;
            pp = (p < 1.0) ? p : 2.0 - p;
            t = Math.Sqrt(-2.0 * Math.Log(pp / 2.0));
            x = -0.70711 * ((2.30753 + t * 0.27061) / (1.0 + t * (0.99229 + t * 0.04481)) - t);
            for (int j = 0; j < 2; j++)
            {
                err = erfc(x) - pp;
                x += err / (1.12837916709551257 * Math.Exp(-Math.Pow(x, 2.0)) - x * err);
            }
            return (p < 1.0 ? x : -x);
        }

        public double inverf(double p) { return inverfc(1.0 - p); }

        public double erfcc(double x)
        {
            double t, z = Math.Abs(x), ans;
            t = 2.0 / (2.0 + z);
            ans = t * Math.Exp(-z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196 + t * (0.09678418 +
                t * (-0.18628806 + t * (0.27886807 + t * (-1.13520398 + t * (1.48851587 +
                t * (-0.82215223 + t * 0.17087277)))))))));
            return (x >= 0.0 ? ans : 2.0 - ans);
        }
    }
    #endregion

    #region Classe MathNormaldist
    //[Obsolete("Use MathNet.Numerics.Distributions instead.")]
    //public class MathNormaldist : MathErf
    //{
    //    double mu, sig;
    //    public MathNormaldist()
    //    {
    //        double mmu = 0.0;
    //        double ssig = 1.0;
    //        mu = mmu;
    //        sig = ssig;
    //        if (sig <= 0.0) throw new Exception("Variância menor que 0");
    //    }

    //    public MathNormaldist(double mmu, double ssig)
    //    {
    //        mu = mmu;
    //        sig = ssig;
    //        if (sig <= 0.0) throw new Exception("Variância menor que 0");
    //    }

    //    public double p(double x)
    //    {
    //        return (0.398942280401432678 / sig) * Math.Exp(-0.5 * Math.Pow((x - mu) / sig, 2.0));
    //    }

    //    public double cdf(double x)
    //    {
    //        return 0.5 * erfc(-0.707106781186547524 * (x - mu) / sig);
    //    }

    //    public double invcdf(double p)
    //    {
    //        if (p <= 0.0 || p >= 1.0) throw new Exception("Limites de probabilidade maior, igual a 1 ou menor, igual a 0");
    //        return -1.41421356237309505 * sig * inverfc(2.0 * p) + mu;
    //    }
    //}
    #endregion

    #region Classe MathExpondist
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathExpondist
    {
        private double bet;
        public MathExpondist(double bbet)
        {
            bet = bbet;
            if (bet <= 0.0) throw new Exception("bad bet in Expondist");
        }
        public double p(double x)
        {
            if (x < 0.0) throw new Exception("bad x in Expondist");
            return bet * Math.Exp(-bet * x);
        }
        public double cdf(double x)
        {
            if (x < 0.0) throw new Exception("bad x in Expondist");
            return 1.0 - Math.Exp(-bet * x);
        }
        public double invcdf(double p)
        {
            if (p < 0.0 || p >= 1.0) throw new Exception("bad p in Expondist");
            return -Math.Log(1.0 - p) / bet;
        }
    }
    #endregion

    #region Classe MathKSdist
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathKSdist
    {
        public double pks(double z)
        {
            if (z < 0.0) throw new Exception("bad z in KSdist");
            if (z == 0.0) return 0.0;
            if (z < 1.18)
            {
                double y = Math.Exp(-1.23370055013616983 / Math.Pow(z, 2.0));
                return 2.25675833419102515 * Math.Sqrt(-Math.Log(y))
                    * (y + Math.Pow(y, 9) + Math.Pow(y, 25) + Math.Pow(y, 49));
            }
            else
            {
                double x = Math.Exp(-2.0 * Math.Pow(z, 2.0));
                return 1.0 - 2.0 * (x - Math.Pow(x, 4) + Math.Pow(x, 9));
            }
        }

        public double qks(double z)
        {
            if (z < 0.0) throw new Exception("bad z in KSdist");
            if (z == 0.0) return 1.0;
            if (z < 1.18) return 1.0 - pks(z);
            double x = Math.Exp(-2.0 * Math.Pow(z, 2.0));
            return 2.0 * (x - Math.Pow(x, 4) + Math.Pow(x, 9));
        }

        public double invqks(double q)
        {
            double y, logy, yp, x, xp, f, ff, u, t;
            if (q <= 0.0 || q > 1.0) throw new Exception("bad q in KSdist");
            if (q == 1.0) return 0.0;
            if (q > 0.3)
            {
                f = -0.392699081698724155 * Math.Pow(1.0 - q, 2.0);
                y = MathSpecialFunctions.invxlogx(f);
                do
                {
                    yp = y;
                    logy = Math.Log(y);
                    ff = f / Math.Pow(1.0 + Math.Pow(y, 4) + Math.Pow(y, 12), 2.0);
                    u = (y * logy - ff) / (1.0 + logy);
                    y = y - (t = u / Math.Max(0.5, 1.0 - 0.5 * u / (y * (1.0 + logy))));
                } while (Math.Abs(t / y) > 1.0e-15);
                return 1.57079632679489662 / Math.Sqrt(-Math.Log(y));
            }
            else
            {
                x = 0.03;
                do
                {
                    xp = x;
                    x = 0.5 * q + Math.Pow(x, 4) - Math.Pow(x, 9);
                    if (x > 0.06) x += Math.Pow(x, 16) - Math.Pow(x, 25);
                } while (Math.Abs((xp - x) / x) > 1.0e-15);
                return Math.Sqrt(-0.5 * Math.Log(x));
            }
        }

        public double invpks(double p) { return invqks(1.0 - p); }
    }
    #endregion

    #region Classe MathKSdist_N
    [Obsolete("Use MathNet.Numerics.Distributions instead.")]
    public class MathKSdist_N
        {

            public void ks(double d, int n, out double pvalor)
            {
                double x = d * Math.Sqrt(n);
                double crit=1000000;
                double acum = 0.0;
                for(int i = 1;i<crit;i++)
                {
                    acum += Math.Exp((-Math.Pow((2.0 * (double)i - 1.0), 2.0) * Math.Pow(Math.PI, 2.0)) / (8.0 * Math.Pow(x, 2.0)));
                }
                pvalor = 1-(Math.Sqrt(2.0 * Math.PI) / x) * acum;
            }
        }
    #endregion

}

