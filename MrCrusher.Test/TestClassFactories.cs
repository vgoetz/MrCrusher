using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    class TestClassFactories {

        [SetUp]
        public void SetUp() {
            GameEnv.RunningAspect = PublicFrameworkEnums.RunningAspect.TestsOnly;
        }

        [Test]
        public void RifleShotFactory_CreateShot_1() {
            
            var shotFactory = new RifleShotFactory();
            var startPoint = new Point(0, 0);
            var endPoint = new Point(0, 0);
            var shot = (RifleShot)shotFactory.CreateProjectile(startPoint, endPoint, 1, null);
            
            var expectedCenterPosition = new Point(0, 0);
            var expectedDestinationPosition = new Point(0, 0);


            Console.WriteLine("Current Position Center: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedCenterPosition, shot.PositionCenter);

            Console.WriteLine("Destination Position: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedDestinationPosition, shot.DestinationPoint);
        }

        [Test]
        public void RifleShotFactory_CreateShot_2()
        {
            var shotFactory = new RifleShotFactory();
            var startPoint = new Point(0, 0);
            var endPoint = new Point(10, 15);
            var shot = (RifleShot)shotFactory.CreateProjectile(startPoint, endPoint, 1, null);

            var expectedCenterPosition = new Point(0, 0);
            var expectedDestinationPosition = new Point(10, 15);


            Console.WriteLine("Current Position Center: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedCenterPosition, shot.PositionCenter);

            Console.WriteLine("Destination Position: ({0},{1})", shot.DestinationPoint.X, shot.DestinationPoint.Y);
            Assert.AreEqual(expectedDestinationPosition, shot.DestinationPoint);
        }

        [Test]
        public void RifleShotFactory_CreateShot_AndFly_1x()
        {
            var shotFactory = new RifleShotFactory();
            var startPoint = new Point(0, 0);
            var endPoint = new Point(10, 15);
            var shot = (RifleShot)shotFactory.CreateProjectile(startPoint, endPoint, 1, null);

            var expectedCenterPosition = new Point(2, 2);
            var expectedDestinationPosition = new Point(10, 15);

            Assert.That(shot.PendingMove(), "Movement done");

            Console.WriteLine("PositionCenter after movement: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedCenterPosition, shot.PositionCenter);

            Console.WriteLine("Destination Position: ({0},{1})", shot.DestinationPoint.X, shot.DestinationPoint.Y);
            Assert.AreEqual(expectedDestinationPosition, shot.DestinationPoint);
        }

        [Test]
        public void RifleShotFactory_CreateShot_AndFly_2x()
        {
            var shotFactory = new RifleShotFactory();
            var startPoint = new Point(0, 0);
            var endPoint = new Point(10, 15);
            var shot = (RifleShot)shotFactory.CreateProjectile(startPoint, endPoint, 1, null);

            var expectedCenterPosition = new Point(3, 5);
            var expectedDestinationPosition = new Point(10, 15);

            Assert.That(shot.PendingMove(), "Movement done 1x");
            shot.MakeReadyForMotion();
            Assert.That(shot.PendingMove(), "Movement done 2x");

            Console.WriteLine("PositionCenter after movement: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedCenterPosition, shot.PositionCenter);

            Console.WriteLine("Destination Position: ({0},{1})", shot.DestinationPoint.X, shot.DestinationPoint.Y);
            Assert.AreEqual(expectedDestinationPosition, shot.DestinationPoint);
        }

        [Test]
        public void RifleShotFactory_CreateShot_AndFly_10x()
        {
            var shotFactory = new RifleShotFactory();
            var startPoint = new Point(0, 0);
            var endPoint = new Point(30, 30);
            var shot = (RifleShot)shotFactory.CreateProjectile(startPoint, endPoint, 1, null);

            var expectedCenterPosition = new Point(30, 30);
            var expectedDestinationPosition = new Point(30, 30);

            for (int i = 0; i < 10; i++)
            {
                Assert.That(shot.PendingMove(), "Movement done {0}x", i);
                shot.MakeReadyForMotion();
            }

            Console.WriteLine("PositionCenter after movement: ({0},{1})", shot.PositionCenter.X, shot.PositionCenter.Y);
            Assert.AreEqual(expectedCenterPosition, shot.PositionCenter);

            Console.WriteLine("Destination Position: ({0},{1})", shot.DestinationPoint.X, shot.DestinationPoint.Y);
            Assert.AreEqual(expectedDestinationPosition, shot.DestinationPoint);
        }
    }
}
