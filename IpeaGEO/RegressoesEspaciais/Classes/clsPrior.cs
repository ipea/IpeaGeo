using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGEO.RegressoesEspaciais.Classes
{
    class clsPrior
    {

        public double eflag = 0; 
        public double ldetflag = 1;
        public double mflag = 1; 
        public double order = 50; 
        public double iter = 30;
        public double rmin = -1;
        public double rmax = 1;
        public double[,] detval;
        public double rho = 0.5;
        public double sige = 1.0;
        public double rval = 4;
        public double mm = 0;
        public double kk = 0;
        public double nu = 0;
        public double d0 = 0;
        public double a1 = 1.0;
        public double a2 = 1.0;
        public double prior_beta = 0;
        public double novi_flag  = 0;
        public double inform_flag = 0;
        public double[,] T;
        public double[,] C;

        clsUtilTools clt = new clsUtilTools();       



        private double[] sar_parse(double[] prior, int k)
        {
            C = clt.Zeros(k, 1);
            T = clt.MatrizMult(clt.Identity(k),Math.Pow(10,12));

            
        }

       




    }
}
