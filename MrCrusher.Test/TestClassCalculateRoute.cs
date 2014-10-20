using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.Core;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    class TestClassCalculateRoute
    {
        [Test]
        public void CalculateRoute_0x0_dis0_deg0_Count()
        {
            var pointA = new Point(0, 0);
            const int distance = 0;
            const int degrees = 0;
            const int expectedWaypoints = 0;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_dis1_deg0_Count()
        {
            var pointA = new Point(0, 0);
            const int distance = 1;
            const int degrees = 0;
            const int expectedWaypoints = 1;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_dis5_deg0_Count()
        {
            var pointA = new Point(0, 0);
            const int distance = 5;
            const int degrees = 0;
            const int expectedWaypoints = 5;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_dis1_deg45_Count()
        {
            var pointA = new Point(0, 0);
            const int distance = 1;
            const int degrees = 45;
            const int expectedWaypoints = 1;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_3x3_dis1_deg45_Count()
        {
            var pointA = new Point(3, 3);
            const int distance = 1;
            const int degrees = 45;
            const int expectedWaypoints = 1;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_3x3_dis1_deg90_Count()
        {
            var pointA = new Point(3, 3);
            const int distance = 1;
            const int degrees = 90;
            const int expectedWaypoints = 1;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_neg3xneg3_dis1_deg90_Count()
        {
            var pointA = new Point(-3, -3);
            const int distance = 1;
            const int degrees = 90;
            const int expectedWaypoints = 1;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_neg3xneg3_dis5_deg90_Count()
        {
            var pointA = new Point(-3, -3);
            const int distance = 5;
            const int degrees = 90;
            const int expectedWaypoints = 5;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_7x7_dis5_deg90_Count()
        {
            var pointA = new Point(7, 7);
            const int distance = 5;
            const int degrees = 13;
            const int expectedWaypoints = 5;

            var route = Calculator.CalculateRoute(pointA, distance, degrees);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_0x0_Count()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 0);
            const int expectedWaypoints = 0;

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_10x10_10x10_Count()
        {
            var pointA = new Point(10, 10);
            var pointB = new Point(10, 10);
            const int expectedWaypoints = 0;

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_10x10_17x17_Count()
        {
            var pointA = new Point(10, 10);
            var pointB = new Point(17, 17);
            const int expectedWaypoints = 7;

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_10x10_3x3_Count()
        {
            var pointA = new Point(10, 10);
            var pointB = new Point(3, 3);
            const int expectedWaypoints = 7;

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedWaypoints, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_0x1_Count()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 1);
            const int expectedCount = 1;

            var route = Calculator.CalculateRoute(pointA, pointB);

            TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedCount, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_0x5_Count(){
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 5);
            const int expectedCount = 5;

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoute(route);
            Assert.AreEqual(expectedCount, route.Count);
        }

        [Test]
        public void CalculateRoute_0x0_0x5()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(0, 5);
            var expectedRoute = new Queue<Point>();
            expectedRoute.Enqueue(new Point(0,1));
            expectedRoute.Enqueue(new Point(0,2));
            expectedRoute.Enqueue(new Point(0,3));
            expectedRoute.Enqueue(new Point(0,4));
            expectedRoute.Enqueue(new Point(0,5));

            var route = Calculator.CalculateRoute(pointA, pointB);

           TestHelper.WriteRoutes(expectedRoute, route);

            for (int i = 0; i < expectedRoute.Count; i++) {
                Assert.IsTrue(expectedRoute.Dequeue().Equals(route.Dequeue()), "Route");
            }
            
        }

        [Test]
        public void CalculateRoute_0x0_5x5()
        {
            var pointA = new Point(0, 0);
            var pointB = new Point(5, 5);
            var expectedPosition = new Point(1, 1);

            var route = Calculator.CalculateRoute(pointA, pointB);

            TestHelper.WriteRoute(route);
            Assert.IsTrue(route.Dequeue().Equals(expectedPosition), "Route");
        }

        
    }
}
