using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class EnemysTankFactory : IEnemysTankFactory {

        public override Tank CreateTank(int type, int health, Point positionCenter) {
            return CreateTank((EnemysTankType) type, health, positionCenter, Guid.NewGuid());
        }

        public override Tank CreateTank(int type, int health, Point positionCenter, Guid guid) {
            return CreateTank((EnemysTankType) type, health, positionCenter, guid);
        }

        public override Tank CreateTank(EnemysTankType type, int health, Point positionCenter) {
            return CreateTank(type, health, positionCenter, Guid.NewGuid());
        }

        public override Tank CreateTank(EnemysTankType type, int health, Point positionCenter, Guid guid) {
            Tank tank;

            switch (type) {

                case EnemysTankType.EnemyTankT1000:
                    tank = new Tank(false, "Panzer1_Plattform.png", "Panzer1_Turm.png", health, positionCenter, 10, 10, new Rectangle(0, 0, 28, 16), 130, 150, 0.90, 1);
                    break;
                case EnemysTankType.EnemyTankT2000:
                    tank = new Tank(false, "Panzer3_Plattform.png", "Panzer3_Turm.png", health, positionCenter, 10, 10, new Rectangle(0, 0, 28, 16), 180, 200, 0.90, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
             
            tank.Guid = guid;
            tank.EnemysType = type;
            tank.IsManned = true;
            
            GameEnv.RegisterGameObjectForAdding(tank);

            return tank;
        }



        protected override Tank CreateTank(string imageFileName, string tankTowerImageFileName, int health, Point positionCenter, int cannonLength, int primaryWeaponPower,
            Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate) {
            return new Tank(false, imageFileName, tankTowerImageFileName, health, positionCenter,
                cannonLength, primaryWeaponPower, rectangleForCollisionDetection, range, viewRange, accuracy, fireRate);
        }

        public override IEnumerable<Tank> CreateManyTanks(List<EnemysTankType> types, int numberOfNewObjects, List<Rectangle> limitationRectanglesInMap) {
            if (types.Any() == false || limitationRectanglesInMap.Any() == false) {
                yield break;
            }
            
            var random = new Random((int)DateTime.Now.Ticks);

            while (numberOfNewObjects-- > 0) {
                var randomType = types[random.Next(0, types.Count - 1)];

                var plan = GameEnv.ConstructionPlanCollection.GetConstructionPlan(randomType);

                if (plan != null && limitationRectanglesInMap.Any()) {
                    Rectangle limitationRectangleInMap = limitationRectanglesInMap[random.Next(0, limitationRectanglesInMap.Count)];

                    Point? newPosition = MapHelper.GetRandomColisionFreePointWithinRectangle(plan.CollisionSize, limitationRectangleInMap, random);

                    if (newPosition.HasValue) {
                        yield return CreateTank(randomType, plan.HealthPoints, newPosition.Value);
                    }
                }
            }
            
        }
    }
}