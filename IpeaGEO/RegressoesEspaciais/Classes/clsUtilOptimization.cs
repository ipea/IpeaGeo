using System;
using System.Collections.Generic;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsUtilOptimization
    {
        public clsUtilOptimization()
        {
        }

        private UnivRootBrent uvb = new UnivRootBrent();
        protected int maxiter = 1000;			    	//maximum number of iterations allowed
        protected double tolerancia_parms = 1.0e-6;		//tolerance for the change in parameters
        protected double tolerancia_grad = 1.0e-4;		//tolerance for the gradient
        protected double tolerancia_func = 1.0e-8;		//tolerance for the norm of nonlinear system of equations
        protected double tolerancia_check = 1.0e-6;		//tolerance for checking the nonlinear system of equatios solution

        #region Functions delegates

        public delegate double FunctionUnivariate(double x);
        public delegate double FunctionSimple(double[,] x);
        public delegate double FunctionGrad(double[,] x, ref double[,] grad);
        public delegate double FunctionHess(double[,] x, ref double[,] grad, ref double[,] hess);
        public delegate double[,] SystemFunctionSimple(double[,] x);
        public delegate double[,] SystemFunctionJac(double[,] x, ref double[,] jacobian);

        #endregion

        #region Partial derivatives

        /// <summary>
        /// Derivada para uma função univariada, utilizandeo o five-point stencil method.
        /// </summary>
        public double DiffFunction(FunctionUnivariate f, double x)
        {
            double h = 1.0e-4;
            return (-f(x+2.0*h) + 8.0*f(x+h) - 8.0*f(x-h) + f(x-2.0*h)) / (12.0*h);
        }

        /// <summary>
        /// Segunda derivada para uma função univariada, utilizandeo o five-point stencil method à primeira derivada.
        /// </summary>
        public double Diff2Function(FunctionUnivariate f, double x)
        {
            double h = 1.0e-4;
            return (-DiffFunction(f, x + 2.0 * h) + 8.0 * DiffFunction(f, x + h) - 8.0 * DiffFunction(f, x - h) + DiffFunction(f, x - 2.0 * h)) / (12.0 * h);
        }

        #endregion

        #region Public functions for univariate optmization (minimization)
        //-----------------------------------------------------------------------------------------------//
        // Functions for univariate optimization (minimization)
        //-----------------------------------------------------------------------------------------------//
        /// <summary>Finds the minimum of a univariate function using Golden rule method.</summary>
        /// <param name="funcao">Delegate to univariate function.</param>
        /// <param name="xmin">Minimum limite for the interval.</param>
        /// <param name="xmax">Maximum limite for the interval.</param>
        /// <param name="fvalopt">Minimum value for the target function.</param>
        /// <returns>Returns the point of minimum function value.</returns>
        public double MinGoldenSearch(FunctionUnivariate funcao, double xmin, double xmax, ref double fvalopt)
        {
            int npassos = 10;
            double passo = (xmax - xmin) / (double)npassos;
            double minimo = Math.Min(funcao(xmax), funcao(xmin));
            double x0 = 0.0;
            for (int i = 1; i < npassos; i++)
            {
                if (funcao(xmin + passo * (double)i) < minimo)
                {
                    x0 = xmin + passo * (double)i;
                    break;
                }
            }
            Golden gm = new Golden();
            fvalopt = gm.golden(xmin, x0, xmax, funcao, this.tolerancia_grad);
            return gm.Xmin;
        }

        /// <summary>Finds the minimum of a univariate function using Brent's method.</summary>
        /// <param name="funcao">Delegate to univariate function.</param>
        /// <param name="xmin">Minimum limite for the interval.</param>
        /// <param name="xmax">Maximum limite for the interval.</param>
        /// <param name="fvalopt">Minimum value for the target function.</param>
        /// <returns>Returns the point of minimum function value.</returns>
        public double MinBrentSearch(FunctionUnivariate funcao, double xmin, double xmax, ref double fvalopt)
        {
            int npassos = 10;
            double passo = (xmax - xmin) / (double)npassos;
            double minimo = Math.Min(funcao(xmax), funcao(xmin));
            double x0 = 0.0;
            for (int i = 1; i < npassos; i++)
            {
                if (funcao(xmin + passo * (double)i) < minimo)
                {
                    x0 = xmin + passo * (double)i;
                    break;
                }
            }
            Brent gm = new Brent();
            fvalopt = gm.brent(xmin, x0, xmax, funcao, this.tolerancia_grad);
            return gm.Minimum;
        }
        #endregion

        #region Public functions for univariate root finding

        /// <summary>Returns the root of a nonlinear equation, using Brent's method. The function sign has
        /// to be different between both extremes.</summary>
        /// <param name="func">Delegate to the nonlinear function.</param>
        /// <param name="xmin">Minimum value of the range.</param>
        /// <param name="xmax">Maximum value of the range.</param>
        /// <param name="tolerancia">Tolerance for solution finding.</param>
        /// <param name="ITMAX">Maximum number of iterations.</param>
        /// <returns>Returns the equation root.</returns>
        public int RootUnivBrent(ref double root, clsUtilOptimization.FunctionUnivariate func, double xmin, double xmax,
            double tolerancia, int ITMAX)
        {
            root = uvb.zbrent(func, xmin, xmax, tolerancia, ITMAX);
            return uvb.GetProblem();
        }

        /// <summary>Returns the root of a nonlinear equation, using Brent's method. The function sign has
        /// to be different between both extremes.</summary>
        /// <param name="func">Delegate to the nonlinear function.</param>
        /// <param name="xmin">Minimum value of the range.</param>
        /// <param name="xmax">Maximum value of the range.</param>
        /// <returns>Returns the equation root.</returns>
        public int RootUnivBrent(ref double root, clsUtilOptimization.FunctionUnivariate func, double xmin, double xmax)
        {
            root = uvb.zbrent(func, xmin, xmax, 1.0e-6, 1000);
            return uvb.GetProblem();
        }

        #endregion
    }

    #region Brent's method for root finding

    /// <summary>
    /// This class implements Brent's method for root finding, for a univariate function. Brent's 
    /// method combines the sureness of bisection with the speed of a higher-order method when 
    /// appropriate. We recommend it as the method of choice for general one-dimensional root
    /// finding where a function's values only (and not its derivative or functional form) are available.
    /// </summary>
    internal class UnivRootBrent
    {
        public UnivRootBrent()
        {
        }

        private int problem = 0;
        private string message = "";

        public int GetProblem() { return this.problem; }

        public double zbrent(clsUtilOptimization.FunctionUnivariate func, double x1, double x2, double tol, int ITMAX)
        {
            this.problem = 0;
            this.message = "Brent's algorithm successful";
            double EPS = 3.0e-8;
            int iter;
            double a = x1, b = x2, c = x2, d = 0.0, e = 0.0, min1, min2;
            double fa = func(a), fb = func(b), fc, p, q, r, s, tol1, xm;

            if ((fa > 0.0 && fb > 0.0) || (fa < 0.0 && fb < 0.0))
            {
                this.problem = 1;
                this.message = "Root must be bracketed in zbrent";
                //nrerror("Root must be bracketed in zbrent");
            }
            fc = fb;
            for (iter = 0; iter < ITMAX; iter++)
            {
                if ((fb > 0.0 && fc > 0.0) || (fb < 0.0 && fc < 0.0))
                {
                    c = a;
                    fc = fa;
                    e = d = b - a;
                }
                if (Math.Abs(fc) < Math.Abs(fb))
                {
                    a = b;
                    b = c;
                    c = a;
                    fa = fb;
                    fb = fc;
                    fc = fa;
                }
                tol1 = 2.0 * EPS * Math.Abs(b) + 0.5 * tol;
                xm = 0.5 * (c - b);
                if (Math.Abs(xm) <= tol1 || fb == 0.0) return b;
                if (Math.Abs(e) >= tol1 && Math.Abs(fa) > Math.Abs(fb))
                {
                    s = fb / fa;
                    if (a == c)
                    {
                        p = 2.0 * xm * s;
                        q = 1.0 - s;
                    }
                    else
                    {
                        q = fa / fc;
                        r = fb / fc;
                        p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
                        q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                    }
                    if (p > 0.0) q = -q;
                    p = Math.Abs(p);
                    min1 = 3.0 * xm * q - Math.Abs(tol1 * q);
                    min2 = Math.Abs(e * q);
                    if (2.0 * p < (min1 < min2 ? min1 : min2))
                    {
                        e = d;
                        d = p / q;
                    }
                    else
                    {
                        d = xm;
                        e = d;
                    }
                }
                else
                {
                    d = xm;
                    e = d;
                }
                a = b;
                fa = fb;
                if (Math.Abs(d) > tol1)
                    b += d;
                else
                    b += Math.Sign(tol1) * xm;
                fb = func(b);
            }
            this.problem = 1;
            this.message = "Maximum number of iterations exceeded in zbrent";
            //nrerror("Maximum number of iterations exceeded in zbrent");
            return 0.0;
        }
    }

    #endregion

    #region Golden rule method

    /// <summary>
    /// Given a function f, and given a bracketing triplet of abscissas ax, bx, cx (such that bx is
    /// between ax and cx, and f(bx) is less than both f(ax) and f(cx)), this routine performs a
    /// golden section search for the minimum, isolating it to a fractional precision of about tol. The
    /// abscissa of the minimum is returned as Xmin, and the minimum function value is returned as
    /// golden, the returned function value.
    /// </summary>
    internal class Golden
    {
        private double R = 0.61803399;   // The golden ratios.
        private double C = 1.0 - 0.61803399;
        private double xmin;
        public double Xmin { get { return xmin; } }
        private string message;

        public Golden()
        {
        }
        public double golden(double ax, double bx, double cx, clsUtilOptimization.FunctionUnivariate f, double tol)
        {
            double f1, f2, x0, x1, x2, x3;
            x0 = ax;            // At any given time we will keep track of four points, x0,x1,x2,x3. 
            x3 = cx;
            if (Math.Abs(cx - bx) > Math.Abs(bx - ax))
            {                 // Make x0 to x1 the smaller segment,
                x1 = bx;
                x2 = bx + C * (cx - bx);        // and fill in the new point to be tried.
            }
            else
            {
                x2 = bx;
                x1 = bx - C * (bx - ax);
            }
            f1 = f(x1);         // The initial function evaluations. Note that
            // we never need to evaluate the function
            // at the original endpoints.
            f2 = f(x2);
            while (Math.Abs(x3 - x0) > tol * (Math.Abs(x1) + Math.Abs(x2)))
            {
                if (f2 < f1)
                {            // One possible outcome,
                    x0 = x1;
                    x1 = x2;
                    x2 = R * x1 + C * x3;  // SHFT3(x0,x1,x2,R*x1+C*x3); // its housekeeping,
                    f1 = f2;
                    f2 = f(x2);      // SHFT2(f1,f2,f(x2)); // and a new function evaluation.
                }
                else
                {           // The other outcome,
                    x3 = x2;
                    x2 = x1;
                    x1 = R * x2 + C * x0;   // SHFT3(x3,x2,x1,R*x2+C*x0);
                    f2 = f1;
                    f1 = f(x1);       // SHFT2(f2,f1,f(x1));          // and its new function evaluation.
                }
            }                                      // Back to see if we are done.
            if (f1 < f2)
            {            // We are done. Output the best of the two current values. 
                xmin = x1;
                this.message = "Golden algorithm successful";
                return f1;
            }
            else
            {
                xmin = x2;
                this.message = "Golden algorithm successful";
                return f2;
            }
        }
    }

    #endregion

    #region Brent's method for optimization

    /// <summary>
    /// Given a function f, and given a bracketing triplet of abscissas ax, bx, cx (such that bx is
    /// between ax and cx, and f(bx) is less than both f(ax) and f(cx)), this routine isolates
    /// the minimum to a fractional precision of about tol using Brent's method. The abscissa of
    /// the minimum is returned as Minimum, and the minimum function value is returned as brent, the
    /// returned function value.
    /// </summary>
    internal class Brent
    {
        public Brent()
        {
        }

        private double xmin;
        public double Minimum { get { return xmin; } }
        private int ITMAX = 1000;
        private double CGOLD = 0.3819660;
        private double ZEPS = 1.0e-10;
        private string message;

        public double brent(double ax, double bx, double cx, clsUtilOptimization.FunctionUnivariate f, double tol)
        {
            int iter;
            double a, b, d = 0.0, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
            double e = 0.0;

            a = (ax < cx ? ax : cx);
            b = (ax > cx ? ax : cx);
            x = w = v = bx;
            fw = fv = fx = f(x);
            for (iter = 1; iter <= ITMAX; iter++)
            {
                xm = 0.5 * (a + b);
                tol2 = 2.0 * (tol1 = tol * Math.Abs(x) + ZEPS);
                if (Math.Abs(x - xm) <= (tol2 - 0.5 * (b - a)))
                {
                    xmin = x;
                    this.message = "Brent's method successfull";
                    return fx;
                }
                if (Math.Abs(e) > tol1)
                {
                    r = (x - w) * (fx - fv);
                    q = (x - v) * (fx - fw);
                    p = (x - v) * q - (x - w) * r;
                    q = 2.0 * (q - r);
                    if (q > 0.0) p = -p;
                    q = Math.Abs(q);
                    etemp = e;
                    e = d;
                    if (Math.Abs(p) >= Math.Abs(0.5 * q * etemp) || p <= q * (a - x) || p >= q * (b - x))
                        d = CGOLD * (e = (x >= xm ? a - x : b - x));
                    else
                    {
                        d = p / q;
                        u = x + d;
                        if (u - a < tol2 || b - u < tol2)
                            d = Math.Sign(xm - x) * tol1;
                    }
                }
                else
                {
                    d = CGOLD * (e = (x >= xm ? a - x : b - x));
                }
                u = (Math.Abs(d) >= tol1 ? x + d : x + Math.Sign(d) * tol1);
                fu = f(u);
                if (fu <= fx)
                {
                    if (u >= x) a = x; else b = x;
                    v = w; w = x; x = u;
                    fv = fw; fw = fx; fx = fu;
                }
                else
                {
                    if (u < x) a = u; else b = u;
                    if (fu <= fw || w == x)
                    {
                        v = w;
                        w = u;
                        fv = fw;
                        fw = fu;
                    }
                    else if (fu <= fv || v == x || v == w)
                    {
                        v = u;
                        fv = fu;
                    }
                }
            }
            this.message = "Too many iterations in brent";
            xmin = x;
            return fx;
        }
    }

    #endregion

    #region Indexing arrays for sorting

    /// <summary>
    /// Indexes an array arr[0..n-1], i.e., outputs the array indx[0..n-1] such that arr[indx[j-1]-1] is
    /// in ascending order for j = 1, 2, . . . ,N. The input quantities n and arr are not changed.
    /// </summary>
    public class Indexx
    {
        public Indexx()
        {
        }
        private int M = 7;
        private int NSTACK = 500;
        //private int NSTACK = 50;

        /// <summary>Indexes an array arr[0..n-1], i.e., outputs the array indx[0..n-1] such that arr[indx[j-1]-1] is
        /// in ascending order for j = 1, 2, . . . ,N. The input quantities n and arr are not changed.</summary>
        /// <param name="n">Ulong variable with size of the vector to be sorted.</param>
        /// <param name="arr">Double vector with data to be sorted.</param>
        /// <param name="indx">Output ulong vector with indexes for sorting.</param>
        public void indexx(ulong n, double[] arr, ulong[] indx)
        {
            ulong i, indxt, ir = n, itemp, j, k, l = 1;
            int jstack = 0;
            double a;
            ulong[] istack = new ulong[NSTACK];
            for (j = 1; j <= n; j++) indx[j - 1] = j;
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        indxt = indx[j - 1];
                        a = arr[indxt - 1];
                        for (i = j - 1; i >= 1; i--)
                        {
                            if (arr[indx[i - 1] - 1] <= a) break;
                            indx[i] = indx[i - 1];
                        }
                        indx[i] = indxt;
                    }
                    if (jstack == 0) break;
                    ir = istack[jstack-- - 1];
                    l = istack[jstack-- - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    itemp = indx[k - 1];
                    indx[k - 1] = indx[l];
                    indx[l] = itemp;
                    if (arr[indx[l] - 1] > arr[indx[ir - 1] - 1])
                    {
                        itemp = indx[l];
                        indx[l] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    if (arr[indx[l - 1] - 1] > arr[indx[ir - 1] - 1])
                    {
                        itemp = indx[l - 1];
                        indx[l - 1] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    if (arr[indx[l] - 1] > arr[indx[l - 1] - 1])
                    {
                        itemp = indx[l];
                        indx[l] = indx[l - 1];
                        indx[l - 1] = itemp;
                    }
                    i = l + 1;
                    j = ir;
                    indxt = indx[l - 1];
                    a = arr[indxt - 1];
                    for (; ; )
                    {
                        do i++; while (arr[indx[i - 1] - 1] < a);
                        do j--; while (arr[indx[j - 1] - 1] > a);
                        if (j < i) break;
                        itemp = indx[i - 1];
                        indx[i - 1] = indx[j - 1];
                        indx[j - 1] = itemp;
                    }
                    indx[l - 1] = indx[j - 1];
                    indx[j - 1] = indxt;
                    jstack += 2;
                    if (jstack > NSTACK) try { throw new Exception(); }
                        catch (Exception)
                        {
                            //MessageBox.Show("NSTACK too small in indexx.",
                            //	 "Invalid method",MessageBoxButtons.OK, MessageBoxIcon.Warning );
                        }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
        }
    }

    #endregion

    #region Nelder-Mead simplex method
    /// <summary>
    /// Nelder-Mead simplex algorithm for function minimization.
    /// </summary>
    internal class Fminsearch
    {
        #region Initialization Functions

        public Fminsearch()
        {
        }

        private int maxfun = -1;
        private int maxiter = -1;
        private double tolx = 1.0e-4;
        private double tolf = 1.0e-4;
        private string message = "";
        private int problem = 0;

        #endregion

        #region Main Algorithm

        public int fminsearch(clsUtilOptimization.FunctionSimple func, double[,] x0, ref double[,] x, ref double fval)
        {
            if (this.maxfun <= 0) this.maxfun = 200 * x0.GetLength(0);
            if (this.maxiter <= 0) this.maxiter = 200 * x0.GetLength(0);

            clsUtilTools clt = new clsUtilTools();
            Indexx ind = new Indexx();

            // Initialize parameters
            double rho = 1.0;
            double chi = 2.0;
            double psi = 0.5;
            double sigma = 0.5;
            int n = x0.GetLength(0);

            // Set up simplex near the initial guess
            double[,] xin = new double[0,0];
            if (x0.GetLength(0) < x0.GetLength(1)) xin = clt.MatrizTransp(x0);
            else xin = clt.ArrayDoubleClone(x0);
            double[,] v = clt.Zeros(n, n + 1);
            double[,] fv = clt.Zeros(1, n + 1);
            for (int i = 0; i < n; i++) v[i, 0] = xin[i,0];
            x = clt.ArrayDoubleClone(xin);
            fv[0, 0] = func(x);
            int func_evals = 1;
            int itercount = 0;
            string how = "";
            // Initial simplex setup continues later

            // Continue setting up the initial simplex
            // Following the improvement suggested by L.Pfeffer at Stanford
            double usual_delta = 0.05;
            double zero_term_delta = 0.00025;
            double[,] y = new double[n, 1];
            for (int j = 0; j < n; j++)
            {
                y = clt.ArrayDoubleClone(xin);
                if (y[j,0] != 0.0) y[j,0] = (1.0 + usual_delta) * y[j,0];
                else y[j,0] = zero_term_delta;
                for (int i = 0; i < n; i++)
                {
                    v[i, j + 1] = y[i,0];
                    x[i,0] = y[i,0];
                }
                fv[0, j + 1] = func(x);
            }

            // sort so v(0,:) has the lowest function value
            double[] arr1 = new double[fv.GetLength(1)];
            for (int i = 0; i < fv.GetLength(1); i++) arr1[i] = fv[0, i];
            ulong[] indx = new ulong[fv.GetLength(1)];
            ulong nindx = (ulong)fv.GetLength(1);
            ind.indexx(nindx, arr1, indx);
            double[,] vaux = clt.ArrayDoubleClone(v);
            double[,] fvaux = clt.ArrayDoubleClone(fv);
            for (int j = 0; j < n + 1; j++)
            {
                fvaux[0, j] = fv[0, (int)indx[j] - 1];
                for (int i = 0; i < n; i++)
                {
                    vaux[i, j] = v[i, (int)indx[j] - 1];
                }
            }
            fv = clt.ArrayDoubleClone(fvaux);
            v = clt.ArrayDoubleClone(vaux);

            how = "initial simplex";
            itercount++;
            func_evals = n + 1;

            // Main algorithm
            // Iterate until the diameter of the simplex is less than tolx
            //   AND the function values differ from the min by less than tolf,
            //   or the max function evaluations are exceeded. (Cannot use OR 
            //   instead of AND.)

            double aux_compare = 0.0;
            double[,] xbar = new double[0,0];
            double[,] xr = new double[0,0];
            double fxr = 0.0;
            double[,] xe = new double[0,0];
            double fxe = 0.0;
            double[,] xc = new double[0,0];
            double fxc = 0.0;
            double[,] aux1 = new double[0,0];
            double[,] xcc = new double[0,0];
            double fxcc = 0.0;

            while (func_evals < this.maxfun && itercount < this.maxiter)
            {
                // Check numbers
                if (double.IsInfinity(fv[0, 0]) || double.IsNaN(fv[0, 0])
                    || double.IsNegativeInfinity(fv[0, 0]) || double.IsPositiveInfinity(fv[0, 0]))
                {
                    this.problem = 1;
                    this.message = "Convergence failed";
                    return this.problem;
                }

                // Check convergence
                aux_compare = this.Maxabs(clt.MatrizSubtracao(clt.SubMatriz(v, 0, n - 1, 0, 0), clt.SubMatriz(v, 0, n - 1, 1, 1)));
                for (int j = 2; j < n + 1; j++)
                {
                    if (this.Maxabs(clt.MatrizSubtracao(clt.SubMatriz(v,0, n - 1, 0, 0), clt.SubMatriz(v,0, n - 1, j, j))) > aux_compare)
                        aux_compare = this.Maxabs(clt.MatrizSubtracao(clt.SubMatriz(v,0, n - 1, 0, 0), clt.SubMatriz(v,0, n - 1, j, j)));
                }
                if ((this.Maxabs(clt.MatrizSubtracao(clt.MatrizMult(fv[0, 0], clt.Ones(1, n)), clt.SubMatriz(fv,0, 0, 1, n))) <= this.tolf)
                    && (aux_compare <= this.tolx)) break;

                // Compute the reflection point

                // xbar = average of the n (NOT n+1) best points
                xbar = clt.MatrizDiv(clt.Sumr(clt.SubMatriz(v,0, n - 1, 0, n - 1)) , (double)n);
                xr = clt.MatrizSubtracao(clt.MatrizMult((1.0 + rho) , xbar), clt.MatrizMult(rho , clt.SubMatriz(v,0, n - 1, n, n)));
                x = clt.ArrayDoubleClone(xr);
                fxr = func(x);
                func_evals++;

                if (fxr < fv[0, 0])
                {
                    // Calculate the expansion point
                    xe = clt.MatrizSubtracao(clt.MatrizMult((1.0 + rho * chi), xbar), clt.MatrizMult(rho * chi, clt.SubMatriz(v,0, n - 1, n, n)));
                    x = clt.ArrayDoubleClone(xe);
                    fxe = func(x);
                    func_evals++;
                    if (fxe < fxr)
                    {
                        for (int i = 0; i < n; i++) v[i, n] = xe[i,0];
                        fv[0, n] = fxe;
                        how = "expand";
                    }
                    else
                    {
                        for (int i = 0; i < n; i++) v[i, n] = xr[i,0];
                        fv[0, n] = fxr;
                        how = "reflect";
                    }
                }
                else
                {
                    if (fxr < fv[0, n - 1])
                    {
                        for (int i = 0; i < n; i++) v[i, n] = xr[i,0];
                        fv[0, n] = fxr;
                        how = "reflect";
                    }
                    else
                    {
                        // Perform contraction
                        if (fxr < fv[0, n])
                        {
                            // Perform an outside contraction
                            xc = clt.MatrizSubtracao(clt.MatrizMult((1.0 + psi * rho) , xbar) , clt.MatrizMult(psi * rho , clt.SubMatriz(v,0, n - 1, n, n)));
                            x = clt.ArrayDoubleClone(xc);
                            fxc = func(x);
                            func_evals++;

                            if (fxc <= fxr)
                            {
                                for (int i = 0; i < n; i++) v[i, n] = xc[i,0];
                                fv[0, n] = fxc;
                                how = "contract outside";
                            }
                            else
                            {
                                // Perform a shrink
                                how = "shrink";
                            }
                        }
                        else
                        {
                            // Perform an inside contraction
                            xcc = clt.MatrizSoma(clt.MatrizMult((1.0 - psi) , xbar), clt.MatrizMult(psi , clt.SubMatriz(v,0, n - 1, n, n)));
                            x = clt.ArrayDoubleClone(xcc);
                            fxcc = func(x);
                            func_evals++;

                            if (fxcc < fv[0, n])
                            {
                                for (int i = 0; i < n; i++) v[i, n] = xcc[i,0];
                                fv[0, n] = fxcc;
                                how = "contract inside";
                            }
                            else
                            {
                                // Perform shrink
                                how = "shrink";
                            }
                        }
                        if (how == "shrink")
                        {
                            for (int j = 1; j <= n; j++)
                            {
                                aux1 = clt.MatrizSoma(clt.SubMatriz(v, 0, n - 1, 0, 0), 
                                        clt.MatrizMult(sigma, (clt.MatrizSubtracao(clt.SubMatriz(v,0, n - 1, j, j), clt.SubMatriz(v,0, n - 1, 0, 0)))));
                                for (int i = 0; i < n; i++) v[i, j] = aux1[i,0];
                                x = clt.SubMatriz(v,0, n - 1, j, j);
                                fv[0, j] = func(x);
                            }
                            func_evals += n;
                        }
                    }
                }
                // sort so v(0,:) has the lowest function value
                for (int i = 0; i < fv.GetLength(1); i++) arr1[i] = fv[0, i];
                ind.indexx(nindx, arr1, indx);
                vaux = clt.ArrayDoubleClone(v);
                fvaux = clt.ArrayDoubleClone(fv);
                for (int j = 0; j < n + 1; j++)
                {
                    fvaux[0, j] = fv[0, (int)indx[j] - 1];
                    for (int i = 0; i < n; i++)
                    {
                        vaux[i, j] = v[i, (int)indx[j] - 1];
                    }
                }
                fv = clt.ArrayDoubleClone(fvaux);
                v = clt.ArrayDoubleClone(vaux);

                itercount++;
            } // end while

            x = clt.SubMatriz(v, 0, n - 1, 0, 0);
            fval = clt.Min(fv);

            if (func_evals >= this.maxfun)
            {
                this.message = "Maximum number of function evaluations has been exceeded";
                this.problem = 1;
            }
            else
            {
                if (itercount >= this.maxiter)
                {
                    this.problem = 1;
                    this.message = "Maximum number of iterations has been exceeded";
                }
                else
                {
                    this.problem = 0;
                    this.message = "Optimization terminated";
                }
            }
            return this.problem;
        }

        #endregion

        #region Auxiliary Funnctions

        private double Maxabs(double[,] a)
        {
            double m = Math.Abs(a[0, 0]);
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (m < Math.Abs(a[i, j])) m = Math.Abs(a[i, j]);
                }
            }
            return m;
        }

        #endregion
    }
    #endregion
}
