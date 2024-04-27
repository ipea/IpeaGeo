using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class Complex
    {
        private double m_real = 0.0;
        private double m_imag = 0.0;

        public Complex()
        {
        }

        public Complex(double r, double im)
        {
            m_real = r;
            m_imag = im;
        }

        public static Complex operator *(Complex a, double b)
        {
            return new Complex(a.Real * b, a.Imag * b);
        }

        public static Complex operator /(Complex a, double b)
        {
            return new Complex(a.Real / b, a.Imag / b);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            Complex r = new Complex();
            double denominador = b.Real * b.Real - b.Imag * b.Imag;
            r = (a * b.Conj) / denominador;
            return r;
        }

        public static Complex operator *(Complex a, Complex b)
        {
            Complex r = new Complex();
            r.Real = a.Real * b.Real - a.Imag * b.Imag;
            r.Imag = a.Real * b.Imag + a.Imag * b.Real;
            return r;
        }

        public static Complex operator -(Complex a, Complex b)
        {
            Complex r = new Complex();
            r.Real = a.Real - b.Real;
            r.Imag = a.Imag - b.Imag;
            return r;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            Complex r = new Complex();
            r.Real = a.Real + b.Real;
            r.Imag = a.Imag + b.Imag;
            return r;
        }

        public Complex Conj
        {
            get
            {
                return new Complex(this.m_real, - this.m_imag);
            }
        }

        public Complex Clone
        {
            get
            {
                return new Complex(this.m_real, this.m_imag);
            }
        }

        public double Imag
        {
            get
            {
                return m_imag;
            }
            set
            {
            	m_imag = value;
            }
        }

        public double Real
        {
            get
            {
            	return m_real;
            }
            set
            {
            	m_real = value;
            }
        }
    }
}
