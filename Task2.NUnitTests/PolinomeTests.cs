using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task2.NUnitTests
{
    [TestFixture]
    public class PolinomeTests
    {
        [TestCase(new double[] {10, 10}, new double[] {20, 20}, Result = new double[] {30, 30})]
        [TestCase(new double[] { 10, 10 }, new double[] { }, Result = new double[] { 10, 10 })]
        [TestCase(new double[] { }, new double[] { 20, 20 }, Result = new double[] { 20, 20 })]
        [TestCase(new double[] { 10, 10, 100 }, new double[] { 20, 20 }, Result = new double[] { 30, 30, 100 })]
        public double[] Sum_returnedValue(double[] arrayDoubles1, double[] arrayDoubles2)
        {
            var polinome = new Polinome(arrayDoubles1);
            Polinome result = polinome + arrayDoubles2;
            return result.Elements;
        }


        [TestCase(new double[] { 10, 10 }, new double[] { 20, 20 }, Result = new double[] { -10, -10 })]
        [TestCase(new double[] { 10, 10 }, new double[] { }, Result = new double[] { 10, 10 })]
        [TestCase(new double[] { }, new double[] { 20, 20 }, Result = new double[] { 20, 20 })]
        [TestCase(new double[] { 10, 10, 100 }, new double[] { 20, 20 }, Result = new double[] { -10, -10, 100 })]
        public double[] Sub_returnedValue(double[] arrayDoubles1, double[] arrayDoubles2)
        {
            var polinome = new Polinome(arrayDoubles1);
            Polinome result = polinome - arrayDoubles2;
            return result.Elements;
        }


        [TestCase(new double[] { 3, 7, 9 }, new double[] { 2, 1 }, Result = new double[] { 6, 17, 25, 9 })]
        [TestCase(new double[] { 3, 7, 9 }, new double[] { 1 }, Result = new double[] { 3, 7, 9 })]
        [TestCase(new double[] {2 }, new double[] { 2, 4, 8 }, Result = new double[] { 4, 8, 16 })]
        [TestCase(new double[] { }, new double[] { 2, 4, 8 }, Result = new double[] { 2, 4, 8 })]
        public double[] Mul_returnedValue(double[] arrayDoubles1, double[] arrayDoubles2)
        {
            var polinome = new Polinome(arrayDoubles1);
            Polinome result = polinome * arrayDoubles2;
            return result.Elements;
        }
    }
}
