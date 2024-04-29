using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    internal class SingularValueDecomposition
    {
	private int m,n;
	private Matrix u,v;
	private Matrix w;
	private double eps, tsh;
        private double thresh = 1.0e-6;
	    
        //public double Threshold
        //{
        //    get { return this.thresh; }
        //    set 
        //    {
        //        if (value > 0) this.thresh = value;
        //        else this.thresh = 1.0e-6;
        //    }
        //}

        public SingularValueDecomposition(ref Matrix a) 
        {
            m = a.nrows();
            n = a.ncols();
            u = a.Clone();
            v = new Matrix(n,n);
            w = new Matrix(n,1);

	    eps = 1.0e-8;

	    decompose();
	    reorder();
	    tsh = 0.5*Math.Sqrt((double)(m+n+1.0))*w[0]*eps;
	}
       
        private void solve(ref Matrix b, ref Matrix x) 
        {
	        int i,j,jj;
	        double s;
		
        	if (b.size() != m || x.size() != n) throw new Exception("SVD::solve bad sizes");
	        Matrix tmp = new Matrix(n);
	        tsh = (thresh >= 0.0 ? thresh : 0.5*Math.Sqrt((double)(m+n+1.0))*w[0]*eps);
		
	        for (j=0;j<n;j++) {
		        s=0.0;
		        if (w[j] > tsh) {
			        for (i=0;i<m;i++) s += u[i,j]*b[i];
			        s /= w[j];
		        }
		        tmp[j]=s;
	        }
	        for (j=0;j<n;j++) {
		        s=0.0;
		        for (jj=0;jj<n;jj++) s += v[j,jj]*tmp[jj];
		        x[j]=s;
	        }
        }

        private void solvemat(ref Matrix b, ref Matrix x)
        {
	        int i,j,m=b.ncols();
	        if (b.nrows() != n || x.nrows() != n || b.ncols() != x.ncols())
		        throw new Exception("SVD::solve bad sizes");
	        Matrix xx = new Matrix(n);
	        for (j=0;j<m;j++) {
		        for (i=0;i<n;i++) xx[i] = b[i,j];
		        solve(ref xx, ref xx);
		        for (i=0;i<n;i++) x[i,j] = xx[i];
	        }
        }

        private int rank() 
        {
	        int j,nr=0;
	        tsh = (thresh >= 0.0 ? thresh : 0.5*Math.Sqrt((double)(m+n+1.0))*w[0]*eps);
	        for (j=0;j<n;j++) if (w[j] > tsh) nr++;
	        return nr;
        }

        private int nullity() 
        {
	        int j,nn=0;
	        tsh = (thresh >= 0.0 ? thresh : 0.5*Math.Sqrt((double)(m+n+1.0))*w[0]*eps);
	        for (j=0;j<n;j++) if (w[j] <= tsh) nn++;
	        return nn;
        }

        private Matrix range()
        {
	        int i,j,nr=0;
	        Matrix rnge = new Matrix(m,rank());
	        for (j=0;j<n;j++) {
		        if (w[j] > tsh) {
			        for (i=0;i<m;i++) rnge[i,nr] = u[i,j];
			        nr++;
		        }
	        }
	        return rnge;
        }

        private Matrix nullspace()
        {
	        int j,jj,nn=0;
	        Matrix nullsp = new Matrix(n,nullity());
	        for (j=0;j<n;j++) {
		        if (w[j] <= tsh) {
			        for (jj=0;jj<n;jj++) nullsp[jj,nn] = v[jj,j];
			        nn++;
		        }
	        }
	        return nullsp;
        }

        public void decompose() 
        {
	        bool flag;
	        int i,its,j,jj,k,l=0,nm=0;
	        double anorm,c,f,g,h,s,scale,x,y,z;
	        Matrix rv1 = new Matrix(n);
	        g = scale = anorm = 0.0;
	        for (i=0;i<n;i++) {
		        l=i+2;
		        rv1[i]=scale*g;
		        g=s=scale=0.0;
		        if (i < m) {
			        for (k=i;k<m;k++) scale += Math.Abs(u[k,i]);
			        if (scale != 0.0) {
				        for (k=i;k<m;k++) {
					        u[k,i] /= scale;
					        s += u[k,i]*u[k,i];
				        }
				        f=u[i,i];
                        g = ((f < 0) ? -1 : 1) * Math.Sqrt(s);
				        h=f*g-s;
				        u[i,i]=f-g;
				        for (j=l-1;j<n;j++) {
					        for (s=0.0,k=i;k<m;k++) s += u[k,i]*u[k,j];
					        f=s/h;
					        for (k=i;k<m;k++) u[k,j] += f*u[k,i];
				        }
				        for (k=i;k<m;k++) u[k,i] *= scale;
			        }
		        }
		        w[i]=scale *g;
		        g=s=scale=0.0;
		        if (i+1 <= m && i+1 != n) {
			        for (k=l-1;k<n;k++) scale += Math.Abs(u[i,k]);
			        if (scale != 0.0) {
				        for (k=l-1;k<n;k++) {
					        u[i,k] /= scale;
					        s += u[i,k]*u[i,k];
				        }
				        f=u[i,l-1];
                        g = ((f < 0) ? -1 : 1) * Math.Sqrt(s);
				        h=f*g-s;
				        u[i,l-1]=f-g;
				        for (k=l-1;k<n;k++) rv1[k]=u[i,k]/h;
				        for (j=l-1;j<m;j++) {
					        for (s=0.0,k=l-1;k<n;k++) s += u[j,k]*u[i,k];
					        for (k=l-1;k<n;k++) u[j,k] += s*rv1[k];
				        }
				        for (k=l-1;k<n;k++) u[i,k] *= scale;
			        }
		        }
		        anorm=Math.Max(anorm,(Math.Abs(w[i])+Math.Abs(rv1[i])));
	        }
	        for (i=n-1;i>=0;i--) {
		        if (i < n-1) {
			        if (g != 0.0) {
				        for (j=l;j<n;j++)
					        v[j,i]=(u[i,j]/u[i,l])/g;
				        for (j=l;j<n;j++) {
					        for (s=0.0,k=l;k<n;k++) s += u[i,k]*v[k,j];
					        for (k=l;k<n;k++) v[k,j] += s*v[k,i];
				        }
			        }
			        for (j=l;j<n;j++) v[i,j]=v[j,i]=0.0;
		        }
		        v[i,i]=1.0;
		        g=rv1[i];
		        l=i;
	        }
	        for (i=Math.Min(m,n)-1;i>=0;i--) {
		        l=i+1;
		        g=w[i];
		        for (j=l;j<n;j++) u[i,j]=0.0;
		        if (g != 0.0) {
			        g=1.0/g;
			        for (j=l;j<n;j++) {
				        for (s=0.0,k=l;k<m;k++) s += u[k,i]*u[k,j];
				        f=(s/u[i,i])*g;
				        for (k=i;k<m;k++) u[k,j] += f*u[k,i];
			        }
			        for (j=i;j<m;j++) u[j,i] *= g;
		        } else for (j=i;j<m;j++) u[j,i]=0.0;
		        ++u[i,i];
	        }
	        for (k=n-1;k>=0;k--) {
		        for (its=0;its<30;its++) {
			        flag=true;
			        for (l=k;l>=0;l--) {
				        nm=l-1;
				        if (l == 0 || Math.Abs(rv1[l]) <= eps*anorm) {
					        flag=false;
					        break;
				        }
				        if (Math.Abs(w[nm]) <= eps*anorm) break;
			        }
			        if (flag) {
				        c=0.0;
				        s=1.0;
				        for (i=l;i<k+1;i++) {
					        f=s*rv1[i];
					        rv1[i]=c*rv1[i];
					        if (Math.Abs(f) <= eps*anorm) break;
					        g=w[i];
					        h=pythag(f,g);
					        w[i]=h;
					        h=1.0/h;
					        c=g*h;
					        s = -f*h;
					        for (j=0;j<m;j++) {
						        y=u[j,nm];
						        z=u[j,i];
						        u[j,nm]=y*c+z*s;
						        u[j,i]=z*c-y*s;
					        }
				        }
			        }
			        z=w[k];
			        if (l == k) {
				        if (z < 0.0) {
					        w[k] = -z;
					        for (j=0;j<n;j++) v[j,k] = -v[j,k];
				        }
				        break;
			        }
			        if (its == 29) throw new Exception("no convergence in 30 svdcmp iterations");
			        x=w[l];
			        nm=k-1;
			        y=w[nm];
			        g=rv1[nm];
			        h=rv1[k];
			        f=((y-z)*(y+z)+(g-h)*(g+h))/(2.0*h*y);
			        g=pythag(f,1.0);
                    f = ((x - z) * (x + z) + h * ((y / (f + ((f < 0) ? -1 : 1) * g)) - h)) / x;
			        c=s=1.0;
			        for (j=l;j<=nm;j++) {
				        i=j+1;
				        g=rv1[i];
				        y=w[i];
				        h=s*g;
				        g=c*g;
				        z=pythag(f,h);
				        rv1[j]=z;
				        c=f/z;
				        s=h/z;
				        f=x*c+g*s;
				        g=g*c-x*s;
				        h=y*s;
				        y *= c;
				        for (jj=0;jj<n;jj++) {
					        x=v[jj,j];
					        z=v[jj,i];
					        v[jj,j]=x*c+z*s;
					        v[jj,i]=z*c-x*s;
				        }
				        z=pythag(f,h);
				        w[j]=z;
                        //if (z) {
				        if (z!=0.0) {
					        z=1.0/z;
					        c=f*z;
					        s=h*z;
				        }
				        f=c*g+s*y;
				        x=c*y-s*g;
				        for (jj=0;jj<m;jj++) {
					        y=u[jj,j];
					        z=u[jj,i];
					        u[jj,j]=y*c+z*s;
					        u[jj,i]=z*c-y*s;
				        }
			        }
			        rv1[l]=0.0;
			        rv1[k]=f;
			        w[k]=x;
		        }
	        }
        }

        private void reorder() 
        {
	        int i,j,k,s,inc=1;
	        double sw;
	        Matrix su = new Matrix(m);
                Matrix sv = new Matrix(n);
	        do { inc *= 3; inc++; } while (inc <= n);
	        do {
		        inc /= 3;
		        for (i=inc;i<n;i++) {
			        sw = w[i];
			        for (k=0;k<m;k++) su[k] = u[k,i];
			        for (k=0;k<n;k++) sv[k] = v[k,i];
			        j = i;
			        while (w[j-inc] < sw) {
				        w[j] = w[j-inc];
				        for (k=0;k<m;k++) u[k,j] = u[k,j-inc];
				        for (k=0;k<n;k++) v[k,j] = v[k,j-inc];
				        j -= inc;
				        if (j < inc) break;
			        }
			        w[j] = sw;
			        for (k=0;k<m;k++) u[k,j] = su[k];
			        for (k=0;k<n;k++) v[k,j] = sv[k];

		        }
	        } while (inc > 1);

	        for (k=0;k<n;k++) {
		        s=0;
		        for (i=0;i<m;i++) if (u[i,k] < 0.0) s++;
		        for (j=0;j<n;j++) if (v[j,k] < 0.0) s++;
		        if (s > (m+n)/2) {
			        for (i=0;i<m;i++) u[i,k] = -u[i,k];
			        for (j=0;j<n;j++) v[j,k] = -v[j,k];
		        }
	        }
        }
 
        private double pythag(double a, double b) 
        {
	        double absa=Math.Abs(a), absb=Math.Abs(b);
	        return (absa > absb ? absa*Math.Sqrt(1.0+SQR(absb/absa)) :
		        (absb == 0.0 ? 0.0 : absb*Math.Sqrt(1.0+SQR(absa/absb))));
        }

        private double SQR(double a)
        {
            return a * a;
        }
    }
}
