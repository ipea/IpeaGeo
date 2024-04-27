using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class NRsparseLinbcg : linbcg
    {
        public NRsparseMat mat;
	    public int n;
	
        public NRsparseLinbcg(NRsparseMat matrix)
        { 
            this.mat = matrix;
            n = mat.nrows;
        }

        public override void atimes(ref double[] x, ref double[] r, int itrnsp) 
        {
	        if (itrnsp != 0) r=mat.atx(x);
	        else r=mat.ax(x);
        }

	    public override void asolve(ref double[] b, ref double[] x, int itrnsp) 
        {
		    int i,j;
		    double diag;
		    for (i=0;i<n;i++) {
			    diag=0.0;
			    for (j=mat.col_ptr[i];j<mat.col_ptr[i+1];j++)
				    if (mat.row_ind[j] == i) {
					    diag=mat.val[j];
					    break;
				    }
			    x[i]=(diag != 0.0 ? b[i]/diag : b[i]);
		    }
	    }
    }
}
