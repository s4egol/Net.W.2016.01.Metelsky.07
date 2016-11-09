using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Task2
{
    public sealed class Polinome : IEquatable<Polinome>, ICloneable
    {
        private readonly double[] elements;
        private int degree;
        public static double Epsilon { get; set; }

        static Polinome()
        {
            try
            {
                Epsilon = double.Parse(ConfigurationManager.AppSettings["epsilon"]);
            }
            catch (ConfigurationErrorsException exp)
            {
                throw new ConfigurationErrorsException("Impossible to obtain epsilon", exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Invalid value of epsilon", exp);
            }
        }

        public Polinome(params double[] elements)
        {
            if (elements == null)
                throw new ArgumentNullException($"{nameof(elements)} can't be NULL");
            if (elements.Length == 0)
                throw new ArgumentException();

            this.elements = new double[elements.Length];
            Array.Copy(elements, this.elements, elements.Length);
            degree = this.elements.Length - 1;
        }

        public int Degree
        {
            get 
            {
                return degree;
            }
        }

        public double this[int index]
        {
            get
            {
                if (index > elements.Length || index < 0)
                    throw new ArgumentOutOfRangeException();

                return elements[index];
            }
            private set
            {
                if (index > elements.Length || index < 0)
                    throw new ArgumentOutOfRangeException();
                else
                {
                    this.elements[index] = value;
                }
            }
        }

        public double Calculate(double number)
        {
            double result = 0.0;
            for (int i = degree; i >= 0; i--)
            {
                if (Math.Abs(this[i]) > Epsilon)
                {
                    result += Math.Pow(number, i)*this[i];
                }
            }
            return result;
        }

        #region Methods Object and implement IEquatable<Polinome>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is Polinome))
                return false;

            if (ReferenceEquals(obj, this))
                return true;

            return MakeEquals(this, (Polinome)obj);
        }

        public bool Equals(Polinome polinome)
        {
            if (ReferenceEquals(polinome, null))
                return false;

            if (ReferenceEquals(polinome, this))
                return true;

            return MakeEquals(this, polinome);
        }

        public override int GetHashCode()
        {
            return elements.GetHashCode();
        }

        public override string ToString()
        {
            if (elements == null)
                return String.Empty;

            String returnString = String.Empty;
            if (elements[degree] > Epsilon)
                returnString += $"{elements[degree]}*x^{degree}";
            else
                returnString += "0";
            if (degree > 0)
            {
                for (int i = elements.Length - 2; i >= 0; i--)
                {
                    if (elements[i] < Epsilon)
                        continue;
                    else
                        returnString += elements[i] > 0 ? $"+{elements[i]}*x^{i}" : $"-{elements[i]}*x^{i}";
                }
            }

            return returnString;
        }

        private bool MakeEquals(Polinome polinome1, Polinome polinome2)
        {
            if (polinome1.elements.Length != polinome2.elements.Length)
                return false;

            for (int i = 0; i < polinome1.elements.Length; i++)
            {
                if (polinome1.elements[i] != polinome2.elements[i])
                    return false;
            }

            return true;
        }
        #endregion

        #region +
        public static Polinome operator + (Polinome firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Sum(firstPolinom.elements, secondPolinom.elements));
        }

        public static Polinome operator + (double[] firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Sum(firstPolinom, secondPolinom.elements));
        }

        public static Polinome operator + (Polinome firstPolinom, double[] secondPolinom)
        {
            return new Polinome(Sum(firstPolinom.elements, secondPolinom));
        }
        #endregion

        #region -
        public static Polinome operator - (Polinome firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Subtract(firstPolinom.elements, secondPolinom.elements));
        }

        public static Polinome operator - (double[] firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Subtract(firstPolinom, secondPolinom.elements));
        }

        public static Polinome operator - (Polinome firstPolinom, double[] secondPolinom)
        {
            return new Polinome(Subtract(firstPolinom.elements, secondPolinom));
        }
        #endregion

        #region *
        public static Polinome operator * (Polinome firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Multiply(firstPolinom.elements, secondPolinom.elements));
        }

        public static Polinome operator * (double[] firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Multiply(firstPolinom, secondPolinom.elements));
        }

        public static Polinome operator * (Polinome firstPolinom, double[] secondPolinom)
        {
            return new Polinome(Multiply(firstPolinom.elements, secondPolinom));
        }
        #endregion

        #region == and !=
        public static bool operator ==(Polinome polinome1, Polinome polinome2)
        {
            return polinome1.Equals(polinome2);
        }

        public static bool operator != (Polinome polinome1, Polinome polinome2)
        {
            return !(polinome1 == polinome2);
        }
        #endregion

        private static double[] Sum(double[] first, double[] second)
        {
            return MakeOperation(first, second, (coefficient1, coefficient2) => coefficient1 + coefficient2);
        }

        private static double[] Subtract(double[] first, double[] second)
        {
            return MakeOperation(first, second, (coefficient1, coefficient2) => coefficient1 - coefficient2);
        }

        private static double[] Multiply(double[] first, double[] second)
        {
            if (first == null || second == null)
                throw new ArgumentNullException();

            if (first.Length == 0) 
                return second;

            if (second.Length == 0)
                return first;

            double[] result = new double[first.Length + second.Length - 1];

            for (int i = 0; i < first.Length; i++)
            {
                for (int j = 0; j < second.Length; j++)
                {
                    result[i + j] += first[i]*second[j];
                }
            }

            return result;
        }

        private static double[] MakeOperation(double[] firstPolinome, double[] secondPolinome, Func<double, double, double> operation )
        {
            if (firstPolinome == null || secondPolinome == null)
                throw new ArgumentNullException();

            if (firstPolinome.Length == 0)
                return secondPolinome;

            if (secondPolinome.Length == 0)
                return firstPolinome;

            if ((firstPolinome.Length == secondPolinome.Length) || (firstPolinome.Length > secondPolinome.Length))
                return ResultOperation(firstPolinome, secondPolinome, operation);
            else
                return ResultOperation(secondPolinome, firstPolinome, operation);
        }

        private static double[] ResultOperation(double[] polinomeLonger, double[] polinomeSmaller,
            Func<double, double, double> operation)
        {
            for (int i = 0; i < polinomeSmaller.Length; i++)
            {
                polinomeLonger[i] = operation(polinomeLonger[i], polinomeSmaller[i]);
            }

            return polinomeLonger;
        }

        public Polinome Clone()
        {
            double[] copyOfArray = new double[this.elements.Length];
            Array.Copy(this.elements, copyOfArray, this.elements.Length);

            return new Polinome(copyOfArray);
        }
        object  ICloneable.Clone()
        {
            return Clone();
        }
    }
}
