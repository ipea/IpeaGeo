using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    #region NRsparseCol
    public class NRsparseCol
    {
        public int nrows;
        public int nvals;
        public int[] row_ind;
        public double[] val;

        public NRsparseCol()
        {
            nrows = 0;
            nvals = 0;
            row_ind = new int[0];
            val = new double[0];
        }

        public NRsparseCol(int m, int nnvals)
        {
            nrows = m;
            nvals = nnvals;
            row_ind = new int[nnvals];
            val = new double[nnvals];
        }

        public void resize(int m, int nnvals)
        {
            nrows = m;
            nvals = nnvals;
            row_ind = new int[nnvals];
            val = new double[nnvals];;
        }
    }
    #endregion

    #region NRsparseMat
    public class NRsparseMat
    {
        public int nrows;
        public int ncols;
        public int nvals;
        public int[] col_ptr;
        public int[] row_ind;
        public double[] val;

        public NRsparseMat()
        {
            nrows = 0;
            ncols = 0;
            nvals = 0;
            col_ptr = new int[0];
            row_ind = new int[0];
            val = new double[0];
        }

        public NRsparseMat(int m, int n, int nnvals)
        {
            nrows = m;
            ncols = n;
            nvals = nnvals;
            col_ptr = new int[n+1];
            row_ind = new int[nnvals];
            val = new double[nnvals];
        }

        public double[] ax(double[] x)
        {
	        double[] y= new double[nrows];
	        for (int j=0;j<ncols;j++) {
		        for (int i=col_ptr[j];i<col_ptr[j+1];i++)
			        y[row_ind[i]] += val[i]*x[j];
	        }
	        return y;
        }
        
        public double[] atx(double[] x) 
        {
	        double[] y = new double[ncols];
	        for (int i=0;i<ncols;i++) 
            {
		        y[i]=0.0;
		        for (int j=col_ptr[i];j<col_ptr[i+1];j++)
			        y[i] += val[j]*x[row_ind[j]];
	        }
	        return y;
        }

        public NRsparseMat transpose() 
        {
	        int i,j,k,index,m=nrows,n=ncols;
	        NRsparseMat at = new NRsparseMat(n,m,nvals);
	        int[] count = new int[m];
	        for (i=0;i<n;i++)
		        for (j=col_ptr[i];j<col_ptr[i+1];j++) {
			        k=row_ind[j];
			        count[k]++;
		        }
	        for (j=0;j<m;j++)
		        at.col_ptr[j+1]=at.col_ptr[j]+count[j];
	        for(j=0;j<m;j++)
		        count[j]=0;
	        for (i=0;i<n;i++)
		        for (j=col_ptr[i];j<col_ptr[i+1];j++) {
			        k=row_ind[j];
			        index=at.col_ptr[k]+count[k];
			        at.row_ind[index]=i;
			        at.val[index]=val[j];
			        count[k]++;
		        }
	        return at;
        }
    }
    #endregion

    #region ADAT
    public class ADAT
    {
        public NRsparseMat a, at;
        public NRsparseMat adat;

        public ADAT(NRsparseMat A, NRsparseMat AT)
        {
            a = A;
            at = AT;

            clsUtilTools clt = new clsUtilTools();
            	        
            int h,i,j,k,l,nvals,m=AT.ncols;
	        int[] done = new int[m];
	        for (i=0;i<m;i++)
		        done[i]=-1;
	        nvals=0;
	        for (j=0;j<m;j++) {
		        for (i=AT.col_ptr[j];i<AT.col_ptr[j+1];i++) {
			        k=AT.row_ind[i];
			        for (l=A.col_ptr[k];l<A.col_ptr[k+1];l++) {
				        h=A.row_ind[l];
				        if (done[h] != j) {
					        done[h]=j;
					        nvals++;
				        }
			        }
		        }
	        }
	        adat = new NRsparseMat(m,m,nvals);
	        for (i=0;i<m;i++)
		        done[i]=-1;
	        nvals=0;
	        for (j=0;j<m;j++) {
		        adat.col_ptr[j]=nvals;
		        for (i=AT.col_ptr[j];i<AT.col_ptr[j+1];i++) {
			        k=AT.row_ind[i];
			        for (l=A.col_ptr[k];l<A.col_ptr[k+1];l++) {
				        h=A.row_ind[l];
				        if (done[h] != j) {
					        done[h]=j;
					        adat.row_ind[nvals]=h;
					        nvals++;
				        }
			        }
		        }
	        }
	        adat.col_ptr[m]=nvals;
	        for (j=0;j<m;j++) 
            {
		        i=adat.col_ptr[j];
		        int size=adat.col_ptr[j+1]-i;
		        if (size > 1) 
                {
			        //VecInt col(size,&adat.row_ind[i]);
                    int[] col = new int[size];
                    for (int u = 0; u < size; u++) { col[u] = adat.row_ind[u+i]; }

                    col = clt.sort(col);
			        for (k=0;k<size;k++)
				        adat.row_ind[i+k]=col[k];
		        }
	        }
        }
        
        public void updateD(double[] D) 
        {
	        int h,i,j,k,l,m=a.nrows,n=a.ncols;
	        double[] temp = new double[n], temp2 = new double[m];
	        for (i=0;i<m;i++) {
		        for (j=at.col_ptr[i];j< at.col_ptr[i+1];j++) {
			        k=at.row_ind[j];
			        temp[k]=at.val[j]*D[k];
		        }
		        for (j=at.col_ptr[i];j<at.col_ptr[i+1];j++) {
			        k=at.row_ind[j];
			        for (l=a.col_ptr[k];l<a.col_ptr[k+1];l++) {
				        h=a.row_ind[l];
				        temp2[h] += temp[k]*a.val[l];
			        }
		        }
		        for (j=adat.col_ptr[i];j<adat.col_ptr[i+1];j++) {
			        k=adat.row_ind[j];
			        adat.val[j]=temp2[k];
			        temp2[k]=0.0;
		        }
	        }
        }
        
        public NRsparseMat adat_ref() 
        {
	        return adat;
        }

        public void adat_delete() 
        {
	        adat = null;
        }
    }
    #endregion
}
