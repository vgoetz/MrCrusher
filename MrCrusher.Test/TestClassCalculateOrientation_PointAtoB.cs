using System;
using System.Drawing;
using MrCrusher.Framework.Core;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    public class TestClassCalculateOrientation_PointAtoB
    {
        [Test]
        public void CalculateDegrees_000a_360() {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 0);
            var expected = new Degree(0);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_000b()
        {
            var pointA = new Point(11, 11);
            var pointB = new Point(11, 11);
            var expected = new Degree(0);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_000Xa()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(11, 0);
            var expected = new Degree(0);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_000Xb()
        {
            var pointA = new Point(11, 0);
            var pointB = new Point(42, 0);
            var expected = new Degree(0);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_000Xc()
        {
            var pointA = new Point(11, 7);
            var pointB = new Point(42, 7);
            var expected = new Degree(0);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_045()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(7, -7);
            var expected = new Degree(45);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_090()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, -7);
            var expected = new Degree(90);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_135()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(-7, -7);
            var expected = new Degree(135);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_180()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(-7, 0);
            var expected = new Degree(180);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_225()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(-7, 7);
            var expected = new Degree(225);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_270()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 7);
            var expected = new Degree(270);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_315()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(7, 7);
            var expected = new Degree(315);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            Console.WriteLine("Degrees: {0}", degrees);
            Assert.AreEqual(expected, degrees);
        }

        [Test]
        public void CalculateDegrees_32_9()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(17, -11);
            var expected = new Degree(32.9);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            var diff = Math.Abs(degrees - expected);

            Console.WriteLine("Degrees: {0}", degrees);
            Console.WriteLine("Difference: {0}", diff);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDegrees_61_69()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(7, -13);
            var expected = new Degree(61.69);

            var degrees = Calculator.CalculateDegree(pointA, pointB);
            var diff = Math.Abs(degrees - expected);

            Console.WriteLine("Degrees: {0}", degrees);
            Console.WriteLine("Difference: {0}", diff);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

    }
}
