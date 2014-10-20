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
    public class EnemysSoldierFactory : IEnemysSoldierFactory {
        public override Soldier CreateSoldier(EnemysSoldierType type, int health, Point positionCenter) {
            return CreateSoldier(type, health, positionCenter, Guid.NewGuid());
        }

        public override Soldier CreateSoldier(int type, int health, Point positionCenter, Guid guid) {
            return CreateSoldier((EnemysSoldierType)type, health, positionCenter, guid);
        }

        public override Soldier CreateSoldier(int type, int health, Point positionCenter) {
            return CreateSoldier((EnemysSoldierType) type, health, positionCenter, Guid.NewGuid());
        }

        public override Soldier CreateSoldier(EnemysSoldierType type, int health, Point positionCenter, Guid guid) {
            Soldier soldier;

            switch (type) {
                case EnemysSoldierType.EnemyDefaultSoldier:
                    soldier = new Soldier(false, null, "Soldat4_NormaleWaffe_Run.png", health, positionCenter, 4, new Rectangle(0, 0, 8, 8), 60, 80, 0.95, 20);
                    break;
                case EnemysSoldierType.Soldat5:
                    soldier = new Soldier(false, null, "Soldat5_NormaleWaffe_Run.png", health, positionCenter, 5, new Rectangle(0, 0, 8, 8), 70, 90, 0.96, 21);
                    break;
                case EnemysSoldierType.Soldat6:
                    soldier = new Soldier(false, null, "Soldat6_NormaleWaffe_Run.png", health, positionCenter, 6, new Rectangle(0, 0, 8, 8), 80, 100, 0.96, 22);
                    break;
                case EnemysSoldierType.Soldat7:
                    soldier = new Soldier(false, null, "Soldat7_NormaleWaffe_Run.png", health, positionCenter, 7, new Rectangle(0, 0, 8, 8), 90, 110, 0.96, 23);
                    break;
                case EnemysSoldierType.Soldat8:
                    soldier = new Soldier(false, null, "Soldat8_NormaleWaffe_Run.png", health, positionCenter, 8, new Rectangle(0, 0, 8, 8), 100, 120, 0.97, 25);
                    break;
                default:
                case EnemysSoldierType.None:
                    throw new ArgumentOutOfRangeException("type");
            }

            soldier.EnemysType = type;
            soldier.Guid = guid;
            
            GameEnv.RegisterGameObjectForAdding(soldier);

            return soldier;
        }

        protected override Soldier CreateSoldier(string imageFileName, string videoFileName, int health, Point positionCenter, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int weaponRange, int viewRange, double accuracy, int fireRate) {
                return new Soldier(false, imageFileName, videoFileName, health, positionCenter, primaryWeaponPower, rectangleForCollisionDetection, weaponRange, viewRange, accuracy, fireRate);
        }

        public override IEnumerable<Soldier> CreateManySoldiers(List<EnemysSoldierType> types, int numberOfNewObjects, List<Rectangle> limitationRectanglesInMap) {
            if (types.Any() == false || limitationRectanglesInMap.Any() == false) {
                yield break;
            }

            var random = new Random((int)DateTime.Now.Ticks);

            while (numberOfNewObjects-- > 0) {
                var randomType = types[random.Next(0, types.Count - 1)];

                var plan = GameEnv.ConstructionPlanCollection.GetConstructionPlan(randomType);

                if (plan != null) {
                    Rectangle limitationRectangleInMap = limitationRectanglesInMap[random.Next(0, limitationRectanglesInMap.Count - 1)];

                    Point? newPosition = MapHelper.GetRandomColisionFreePointWithinRectangle(plan.CollisionSize, limitationRectangleInMap, random);

                    if (newPosition.HasValue) {
                        yield return CreateSoldier(randomType, plan.HealthPoints, newPosition.Value);
                    }
                }
            }
        }

    }
}