using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class Polinome
    {
        private readonly double[] elements;

        public double[] Elements { get { return elements; } }

        public Polinome(double[] elements)
        {
            this.elements = elements;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Polinome))
                return false;
            else
                return Enumerable.SequenceEqual(((Polinome)obj).elements, this.elements);
        }

        public bool Equals(Polinome polinome)
        {
            if (polinome == null)
                return false;
            else
                return Enumerable.SequenceEqual(polinome.elements, this.elements);
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
            for (int i = 0; i < elements.Length; i++)
            {
                returnString += $"{elements[i]}*x^{i} ";
            }

            return returnString;
        }

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

        public static Polinome operator - (Polinome firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Sub(firstPolinom.elements, secondPolinom.elements));
        }

        public static Polinome operator - (double[] firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Sub(firstPolinom, secondPolinom.elements));
        }

        public static Polinome operator - (Polinome firstPolinom, double[] secondPolinom)
        {
            return new Polinome(Sub(firstPolinom.elements, secondPolinom));
        }

        public static Polinome operator * (Polinome firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Mul(firstPolinom.elements, secondPolinom.elements));
        }

        public static Polinome operator * (double[] firstPolinom, Polinome secondPolinom)
        {
            return new Polinome(Mul(firstPolinom, secondPolinom.elements));
        }

        public static Polinome operator * (Polinome firstPolinom, double[] secondPolinom)
        {
            return new Polinome(Mul(firstPolinom.elements, secondPolinom));
        }

        private static double[] Sum(double[] first, double[] second)
        {
            return MakeOperation(first, second, (coefficient1, coefficient2) => coefficient1 + coefficient2);
        }

        private static double[] Sub(double[] first, double[] second)
        {
            return MakeOperation(first, second, (coefficient1, coefficient2) => coefficient1 - coefficient2);
        }

        private static double[] Mul(double[] first, double[] second)
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

    }
}
