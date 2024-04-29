using System;
using System.Collections.Generic;
using System.Linq;

namespace IpeaGeo
{
    public class Integral
    {
        public Integral(double leftEndpoint, double size)
        {
            LeftEndpoint = leftEndpoint;
            RightEndpoint = leftEndpoint + size;
        }

        public double LeftEndpoint
        {
            get;
            set;
        }

        public double RightEndpoint
        {
            get;
            set;
        }

        public double Size
        {
            get
            {
                return RightEndpoint - LeftEndpoint;
            }
        }

        public double Center
        {
            get
            {
                return (LeftEndpoint + RightEndpoint) / 2;
            }
        }

        public IEnumerable<Integral> Subdivide(int subintervalCount)
        {
            double subintervalSize = Size / subintervalCount;
            return Enumerable.Range(0, subintervalCount).Select(index => new Integral(LeftEndpoint + index * subintervalSize, subintervalSize));
        }
    }

    public enum ApproximationMethod
    {
        RectangleLeft = 0,
        RectangleMidpoint,
        RectangleRight,
        Trapezium,
        Simpson
    }

    public class DefiniteIntegral
    {
        public DefiniteIntegral(Func<double, double> integrand, Integral domain)
        {
            Integrand = integrand;
            Domain = domain;
        }

        public Func<double, double> Integrand
        {
            get;
            set;
        }

        public Integral Domain
        {
            get;
            set;
        }

        public double SampleIntegrand(ApproximationMethod approximationMethod, Integral subdomain)
        {
            switch (approximationMethod)
            {
                case ApproximationMethod.RectangleLeft:
                    return Integrand(subdomain.LeftEndpoint);
                case ApproximationMethod.RectangleMidpoint:
                    return Integrand(subdomain.Center);
                case ApproximationMethod.RectangleRight:
                    return Integrand(subdomain.RightEndpoint);
                case ApproximationMethod.Trapezium:
                    return (Integrand(subdomain.LeftEndpoint) + Integrand(subdomain.RightEndpoint)) / 2;
                case ApproximationMethod.Simpson:
                    return (Integrand(subdomain.LeftEndpoint) + 4 * Integrand(subdomain.Center) + Integrand(subdomain.RightEndpoint)) / 6;
                default:
                    throw new NotImplementedException();
            }
        }

        public double Approximate(ApproximationMethod approximationMethod, int subdomainCount)
        {
            return Domain.Size * Domain.Subdivide(subdomainCount).Sum(subdomain => SampleIntegrand(approximationMethod, subdomain)) / subdomainCount;
        }
    }
}

