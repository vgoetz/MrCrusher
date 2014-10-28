using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.Player;
using NUnit.Framework;

namespace MrCrusher.Test
{
    [TestFixture]
    class TestClassTankAsMovingObject {
        readonly PlayersTankFactory _tankFactory = new PlayersTankFactory();
        private Player _testPlayer;

        [SetUp]
        public void SetUp() {
            GameEnv.TopMenuHeight = 0;
            GameEnv.TopMenuWidth = 0;
            GameEnv.RunningAspect = PublicFrameworkEnums.RunningAspect.TestsOnly;
            _testPlayer = new Player("Test", true, true);
        }

        [Test]
        public void InitialMovingObject() {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            const int expectedHealth = 1;
            var expectedPosition = new Point(0, 0);

            Console.WriteLine("Health: {0}", movObj.Health);
            Assert.AreEqual(expectedHealth, movObj.Health);

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter);
        }

        [Test]
        public void MovingObjectToPoint_SamePosition_ThenObjectDoesntMoved()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            var expectedPosition = new Point(20, 20);

            movObj.SetMovingDestination(expectedPosition);
            movObj.PendingMove();

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter);
        }

        [Test]
        public void MovingObjectToPoint_21x21_ObjectMoved()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            movObj.PlayerAsController = _testPlayer;
            var expectedPosition = new Point(21, 21);

            movObj.SetMovingDestination(expectedPosition);
            var moved = movObj.PendingMove();

            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "Position");
            Assert.That(moved, "moved");
        }

        [Test]
        public void MovingObjectToPoint_22x22_ObjectMovedTwice()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            movObj.PlayerAsController = _testPlayer;
            var expectedPosition = new Point(22, 22);

            movObj.SetMovingDestination(expectedPosition);
            var movedFirst = movObj.PendingMove();

            Console.WriteLine("Position1: {0}", movObj.PositionCenter);
            Assert.That(movedFirst, "first move");

            movObj.MakeReadyForMotion();
            var movedSecond = movObj.PendingMove();

            Console.WriteLine("Position2: {0}", movObj.PositionCenter);
            Assert.That(movedSecond, "second move");
        }

        [Test]
        public void MovingObjectToPoint_23x23_ObjectMoved()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            movObj.PlayerAsController = _testPlayer;
            var expectedPosition = new Point(23, 23);

            movObj.SetMovingDestination(expectedPosition);
            var moved = movObj.PendingMove();

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.That(moved, "moved");
        }

        [Test]
        public void MovingObjectToPoint_21x21_ThenBackTo_20x20()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            movObj.PlayerAsController = _testPlayer;
            var expectedPosition1 = new Point(21, 21);
            var expectedPosition2 = new Point(20, 20);

            movObj.SetMovingDestination(expectedPosition1);
            movObj.PendingMove();

            Console.WriteLine("Position1: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition1, movObj.PositionCenter);

            movObj.MakeReadyForMotion();
            movObj.SetMovingDestination(expectedPosition2);
            movObj.PendingMove();

            Console.WriteLine("Position2: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition2, movObj.PositionCenter, "position after moving");
        }

        [Test]
        public void MovingObjectToPoint_30x25()
        {
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            movObj.PlayerAsController = _testPlayer;
            var expectedPosition = new Point(30, 25);

            movObj.SetMovingDestination(expectedPosition);
            TestHelper.WriteRoute(movObj.Route);

            int i = 15;
            while (movObj.PendingMoveToDo && i-- > 0)
            {
                movObj.PendingMove();
                movObj.MakeReadyForMotion();
            }

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "position after moving");
        }

        [Test]
        public void MovingObjectToHigherPoint__OutOfBoundsIsNotAllowed() {
            //int fieldWidth = GameEnvironment.StdVideoScreen.Width;
            //int fieldHeight = GameEnvironment.StdVideoScreen.Height;
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            movObj.PlayerAsController = _testPlayer;
            int tankWidth = movObj.RectangleForCollisionDetection.Width;
            int tankHeight = movObj.RectangleForCollisionDetection.Height;

            // Place tank to the bottom right of the field --> the players tank is not allowed to leave the field
            movObj.PositionCenter = new Point(tankWidth / 2, tankHeight / 2);
            var expectedPosition = movObj.PositionCenter;

            Console.WriteLine("Tank-Size:   {0}x{1}", tankWidth, tankHeight);
            Console.WriteLine("Tank-Coord.: {0}", movObj.PositionCenter);

            movObj.SetMovingDestination(movObj.PositionCenter.X, movObj.PositionCenter.Y - 1);

            TestHelper.WriteRoute(movObj.Route);
            int i = 15;
            while (movObj.PendingMoveToDo && i-- > 0) {
                movObj.PendingMove();
                movObj.MakeReadyForMotion();
            }

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "position after moving");
        }

        [Test]
        public void MovingObjectToTheLeft__OutOfBoundsIsNotAllowed() {
            //int fieldWidth = GameEnvironment.StdVideoScreen.Width;
            //int fieldHeight = GameEnvironment.StdVideoScreen.Height;
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            movObj.PlayerAsController = _testPlayer;
            int tankWidth = movObj.RectangleForCollisionDetection.Width;
            int tankHeight = movObj.RectangleForCollisionDetection.Height;

            // Place tank to the bottom right of the field --> the players tank is not allowed to leave the field
            movObj.PositionCenter = new Point(tankWidth / 2, tankHeight / 2);
            var expectedPosition = movObj.PositionCenter;

            Console.WriteLine("Tank-Size:   {0}x{1}", tankWidth, tankHeight);
            Console.WriteLine("Tank-Coord.: {0}", movObj.PositionCenter);

            movObj.SetMovingDestination(movObj.PositionCenter.X - 1, movObj.PositionCenter.Y);

            TestHelper.WriteRoute(movObj.Route);
            int i = 15;
            while (movObj.PendingMoveToDo && i-- > 0) {
                movObj.PendingMove();
                movObj.MakeReadyForMotion();
            }

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "position after moving");
        }

        [Test]
        public void MovingObjectToTheBottom_OutOfBoundsIsNotAllowed() {
            int fieldWidth = GameEnv.ScreenWidth;
            int fieldHeight = GameEnv.ScreenHeight;
            var movObj      = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            movObj.PlayerAsController = _testPlayer;
            int tankWidth   = movObj.RectangleForCollisionDetection.Width;
            int tankHeight  = movObj.RectangleForCollisionDetection.Height;

            // Place tank to the bottom right of the field --> the players tank is not allowed to leave the field
            movObj.PositionCenter = new Point(fieldWidth - tankWidth/2, fieldHeight - tankHeight/2);
            var expectedPosition = movObj.PositionCenter;

            Console.WriteLine("Field: {0}x{1}", fieldWidth, fieldHeight);
            Console.WriteLine("Tank: {0}x{1}", tankWidth, tankHeight);
            Console.WriteLine("Tank-Coord.: {0}", movObj.PositionCenter);

            movObj.SetMovingDestination(movObj.PositionCenter.X, movObj.PositionCenter.Y + 1);

            TestHelper.WriteRoute(movObj.Route);
            int i = 15;
            while (movObj.PendingMoveToDo && i-- > 0) {
                movObj.PendingMove();
                movObj.MakeReadyForMotion();
            }

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "position after moving");
        }

        [Test]
        public void MovingObjectToTheRight_OutOfBoundsIsNotAllowed() {
            int fieldWidth = GameEnv.ScreenHeight;
            int fieldHeight = GameEnv.ScreenWidth;
            var movObj = _tankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            movObj.PlayerAsController = _testPlayer;
            int tankWidth = movObj.RectangleForCollisionDetection.Width;
            int tankHeight = movObj.RectangleForCollisionDetection.Height;

            // Place tank to the bottom right of the field --> the players tank is not allowed to leave the field
            movObj.PositionCenter = new Point(fieldWidth - tankWidth / 2, fieldHeight - tankHeight / 2);
            var expectedPosition = movObj.PositionCenter;

            Console.WriteLine("Field: {0}x{1}", fieldWidth, fieldHeight);
            Console.WriteLine("Tank: {0}x{1}", tankWidth, tankHeight);
            Console.WriteLine("Tank-Coord.: {0}", movObj.PositionCenter);

            movObj.SetMovingDestination(movObj.PositionCenter.X + 1, movObj.PositionCenter.Y);

            TestHelper.WriteRoute(movObj.Route);
            int i = 15;
            while (movObj.PendingMoveToDo && i-- > 0) {
                movObj.PendingMove();
                movObj.MakeReadyForMotion();
            }

            Console.WriteLine("Position: {0}", movObj.PositionCenter);
            Assert.AreEqual(expectedPosition, movObj.PositionCenter, "position after moving");
        }
    }
}
