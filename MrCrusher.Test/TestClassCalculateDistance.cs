using System;
using System.Drawing;
using MrCrusher.Framework.Core;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    public class TestClassCalculateDistance
    {
        [Test]
        public void CalculateDistance_X() {
            var pointA = new Point(0, 0);
            var pointB = new Point(10, 0);
            const double expected = 10;
            
            var distance = Calculator.CalculateDistance(pointA, pointB);

            Console.WriteLine("Distance: {0}", distance);
            Assert.AreEqual(expected, distance);
        }

        [Test]
        public void CalculateDistance_Y() {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 10);
            const double expected = 10;

            var distance = Calculator.CalculateDistance(pointA, pointB);

            Console.WriteLine("Distance: {0}", distance);
            Assert.AreEqual(expected, distance);
        }

        [Test]
        public void CalculateDistance_0a()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 0);
            const double expected = 0;

            var distance = Calculator.CalculateDistance(pointA, pointB);

            Console.WriteLine("Distance: {0}", distance);
            Assert.AreEqual(expected, distance);
        }

        [Test]
        public void CalculateDistance_0b()
        {
            var pointA = new Point(11, 11);
            var pointB = new Point(11, 11);
            const double expected = 0;

            var distance = Calculator.CalculateDistance(pointA, pointB);

            Console.WriteLine("Distance: {0}", distance);
            Assert.AreEqual(expected, distance);
        }

        [Test]
        public void CalculateDistance_XYa()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(7, 5);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDistance_XYb()
        {
            var pointA = new Point(7, 5);
            var pointB = new Point(0, 0);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDistance_XYc()
        {
            var pointA = new Point(14, 10);
            var pointB = new Point(7, 5);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDistance_XYd()
        {
            var pointA = new Point(7, 5);
            var pointB = new Point(14, 10);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDistance_XYe()
        {
            var pointA = new Point(-7, -5);
            var pointB = new Point(-14, -10);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }

        [Test]
        public void CalculateDistance_XYf()
        {
            var pointA = new Point(3, 3);
            var pointB = new Point(-4, -2);
            const double expected = 8.6;

            var distance = Calculator.CalculateDistance(pointA, pointB);
            var diff = Math.Abs(distance - expected);

            Console.WriteLine("Distance: {0}", distance);
            Assert.That(diff, Is.LessThanOrEqualTo(0.1));
        }
    }
}
