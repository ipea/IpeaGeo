using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class linbcg
    {
        public virtual void asolve(ref double[] b, ref double[] x, int itrnsp)
        {
        }

        public virtual void atimes(ref double[] x, ref double[] r, int itrnsp)
        {
        }

        public void solve(ref double[] b, ref double[] x, int itol, double tol,
	        int itmax, ref int iter, ref double err)
        {
	        double ak,akden,bk,bkden=1.0,bknum,bnrm,dxnrm,xnrm,zm1nrm,znrm=0;
	        double EPS=1.0e-14;
	        int j,n=b.GetLength(0);
	        double[] p = new double[n], pp = new double[n],r = new double[n], 
                rr = new double[n],
                z = new double[n], zz = new double[n];
	        iter=0;

	        atimes(ref x, ref r,0);
	        for (j=0;j<n;j++) {
		        r[j]=b[j]-r[j];
		        rr[j]=r[j];
	        }
	        //atimes(r,rr,0);
	        if (itol == 1) {
		        bnrm=snrm(ref b,itol);
		        asolve(ref r, ref z,0);
	        }
	        else if (itol == 2) {
		        asolve(ref b,ref z,0);
		        bnrm=snrm(ref z,itol);
		        asolve(ref r,ref z,0);
	        }
	        else if (itol == 3 || itol == 4) {
		        asolve(ref b,ref z,0);
		        bnrm=snrm(ref z,itol);
		        asolve(ref r,ref z,0);
		        znrm=snrm(ref z,itol);
	        } else throw new Exception("illegal itol in linbcg");
	        while (iter < itmax) {
		        ++iter;
		        asolve(ref rr, ref zz,1);
		        for (bknum=0.0,j=0;j<n;j++) bknum += z[j]*rr[j];
		        if (iter == 1) {
			        for (j=0;j<n;j++) {
				        p[j]=z[j];
				        pp[j]=zz[j];
			        }
		        } else {
			        bk=bknum/bkden;
			        for (j=0;j<n;j++) {
				        p[j]=bk*p[j]+z[j];
				        pp[j]=bk*pp[j]+zz[j];
			        }
		        }
		        bkden=bknum;
		        atimes(ref p,ref z,0);
		        for (akden=0.0,j=0;j<n;j++) akden += z[j]*pp[j];
		        ak=bknum/akden;
		        atimes(ref pp,ref zz,1);
		        for (j=0;j<n;j++) {
			        x[j] += ak*p[j];
			        r[j] -= ak*z[j];
			        rr[j] -= ak*zz[j];
		        }
		        asolve(ref r,ref z,0);
		        if (itol == 1)
			        err=snrm(ref r,itol)/bnrm;
		        else if (itol == 2)
			        err=snrm(ref z,itol)/bnrm;
		        else if (itol == 3 || itol == 4) {
			        zm1nrm=znrm;
			        znrm=snrm(ref z,itol);
			        if (Math.Abs(zm1nrm-znrm) > EPS*znrm) {
				        dxnrm= Math.Abs(ak)*snrm(ref p,itol);
				        err=znrm/ Math.Abs(zm1nrm-znrm)*dxnrm;
			        } else {
				        err=znrm/bnrm;
				        continue;
			        }
			        xnrm=snrm(ref x,itol);
			        if (err <= 0.5*xnrm) err /= xnrm;
			        else {
				        err=znrm/bnrm;
				        continue;
			        }
		        }
		        if (err <= tol) break;
	        }
        }

        public double snrm(ref double[] sx, int itol)
        {
	        int i,isamax,n=sx.GetLength(0);
	        double ans;
	        if (itol <= 3) 
            {
		        ans = 0.0;
		        for (i=0;i<n;i++) ans += Math.Pow(sx[i], 2.0);
		        return Math.Sqrt(ans);
	        } 
            else {
		        isamax=0;
		        for (i=0;i<n;i++) {
			        if (Math.Abs(sx[i]) > Math.Abs(sx[isamax])) isamax=i;
		        }
		        return Math.Abs(sx[isamax]);
	        }
        }
    }
}
