using System.Collections.Generic;
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
    class TestClassCollisionDetection
    {
        private Rectangle _collisionRectangleForAllTanks;
        private readonly PlayersTankFactory _playersTankFactory = new PlayersTankFactory();

        [SetUp]
        public void SetUp(){
            GameEnv.RunningAspect = PublicFrameworkEnums.RunningAspect.Server;
            new MainProgram();
            _collisionRectangleForAllTanks = new Rectangle(0, 0, 20, 20);
        }

        [TearDown]
        public void TearDown() {
            GameEnv.ClearGameObjects();
        }

        [Test, Sequential]
        public void BasicCollisionDetection_NoCollision(
            [Values(8, 6, 3, 3, 0, 3)] int rect2X,
            [Values(8, 6, 6, 0, 3, 6)] int rect2Y)
        {
            var rect1 = new Rectangle(3, 3, 3, 3);
            var rect2 = new Rectangle(rect2X, rect2Y, 3, 3);

            Assert.IsFalse(rect1.IntersectsWith(rect2));
        }

        [Test, Sequential]
        public void BasicCollisionDetection_Collision(
            [Values(5, 5, 3, 3, 1, 1, 5)] int rect2X,
            [Values(5, 3, 3, 5, 3, 5, 1)] int rect2Y)
        {
            var rect1 = new Rectangle(3, 3, 3, 3);
            var rect2 = new Rectangle(rect2X, rect2Y, 3, 3);

            Assert.That(rect1.IntersectsWith(rect2));
        }
        
        [Test]
        public void SetRectangleForCollisionDetection_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            var expectedCollisionRectangle = new Rectangle(-10, -10, 20, 20);

            Assert.AreEqual(expectedCollisionRectangle, tank1.RectangleForCollisionDetection, "RectangleForCollisionDetection");
        }

        [Test]
        public void SetRectangleForCollisionDetection_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(45, 45));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            var expectedCollisionRectangle = new Rectangle(35, 35, 20, 20);

            Assert.AreEqual(expectedCollisionRectangle, tank1.RectangleForCollisionDetection, "RectangleForCollisionDetection");
        }

        [Test]
        public void WithNoObjects()
        {
            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(null, null), "null and null");
        }

        [Test]
        public void SameObject()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point());
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;


            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank1), "The same object");
        }

        [Test]
        public void Objects_2Objects_NoCollision_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, no collision");
        }

        [Test]
        public void Objects_2Objects_NoCollision_2_ViceVersa()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank1), "tank1 and tank2, no collision");
        }

        [Test]
        public void Objects_2Objects_Collision_SamePoint_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_SamePoint_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(10, 10));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(10, 10));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_SamePoint_3()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_InTheNear_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(90, 90));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_InTheNear_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(85, 85));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_InTheNear_3()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(81, 81));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_Collision_InTheNear_4()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(1, 1));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
        }

        [Test]
        public void Objects_2Objects_NoCollision_InTheNear_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 1));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, no collision");
        }

        [Test]
        public void Objects_2Objects_NoCollision_InTheNear_3()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(1, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(20, 20));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, no collision");
        }

        [Test]
        public void Objects_2Objects_NoCollision_InTheNear_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(80, 80));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank1), "tank1 and tank2, no collision");
        }

        [Test]
        public void Objects_3Objects_NoCollision_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(200, 200));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, no collision");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, no collision");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, no collision");
        }

        [Test]
        public void Objects_3Objects_Collision_SamePosition_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(0, 0));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(3, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void Objects_3Objects_Collision_SamePosition_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(10, 10));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(10, 10));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(10, 10));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(3, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void Objects_3Objects_Collision_SamePosition_3()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(3, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void Objects_3Objects_Collision_SamePosition_4_NegPos()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(-100, -100));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(-100, -100));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(-100, -100));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(3, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void Objects_3Objects_1Collision_1()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(200, 200));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void Objects_3Objects_1Collision_2()
        {
            var tank1 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(100, 100));
            var tank2 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(200, 200));
            var tank3 = _playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 1, new Point(200, 200));
            tank1.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank2.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            tank3.RectangleForCollisionDetection = _collisionRectangleForAllTanks;
            GameEnv.AddRegisteredGameObjects();

            Assert.AreEqual(1, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank2), "tank1 and tank2, collision");
            Assert.IsTrue(CollisionDetection.CollisionDetectionFor2Objects(tank2, tank3), "tank2 and tank3, collision");
            Assert.IsFalse(CollisionDetection.CollisionDetectionFor2Objects(tank1, tank3), "tank1 and tank3, collision");
        }

        [Test]
        public void ProgramWithDefaultInit_NoCollision()
        {
            SetPlayerAnd3EnemysObjects();
            GameEnv.AddRegisteredGameObjects();

            Assert.That(GameEnv.GameObjectsToDrawAndMove.Count, Is.EqualTo(4), "# GameObjects");

            Assert.AreEqual(0, CollisionDetection.CollisionDetectionAndHandlingForAllDrawingObjects(), "# Collisions");
        }

        public static void SetPlayerAnd3EnemysObjects() {

            var localPlayer = new Player("TestPlayer", true, true);
            localPlayer.CreateNewSoldierAtRandomPosition();
            GameEnv.Players = new List<Player> { localPlayer };

            var enemysSoldierFactory = new EnemysSoldierFactory();
            var enemysTankFactory = new EnemysTankFactory();

            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Server) {

                enemysTankFactory.CreateTank(EnemysTankType.EnemyTankT2000, 100, new Point(430, 230));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat5, 25, new Point(150, 150));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat5, 30, new Point(450, 250));
            }

        }
        
    }
}
