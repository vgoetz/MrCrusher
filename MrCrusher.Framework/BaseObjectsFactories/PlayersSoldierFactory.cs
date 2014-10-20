using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class PlayersSoldierFactory : IPlayersSoldierFactory {

        /// <summary>
        /// Create a player soldier at a random position 
        /// </summary>
        /// <returns></returns>
        public override ISoldier CreatePlayersSoldier() {

            var centerStartRectangle = GameEnv.CenterStartLimitationRectangleInMap;
            var randomizer = new Random((int)DateTime.Now.Ticks + GameEnv.ExistingAndRegisteredGameObjects.Count);

            const PlayersSoldierType playersSoldierType = PlayersSoldierType.PlayersDefaultSoldier;
            IGameObjectTypeConstructionPlan constructionPlan = GameEnv.ConstructionPlanCollection.GetConstructionPlan(playersSoldierType);
            Size soldiersCollisionSize = GameEnv.ConstructionPlanCollection.GetConstructionPlan(playersSoldierType).CollisionSize;

            Point? soldiersPosition = MapHelper.GetRandomColisionFreePointWithinRectangle(soldiersCollisionSize, centerStartRectangle, randomizer);
            if (soldiersPosition.HasValue) {
                var playersSoldierFactory = new PlayersSoldierFactory();
                return playersSoldierFactory.CreateSoldier(playersSoldierType, constructionPlan.HealthPoints, soldiersPosition.Value);
            }

            return null;
        }

        public override Soldier CreateSoldier(PlayersSoldierType type, int health, Point positionCenter) {
            return CreateSoldier(type, health, positionCenter, Guid.NewGuid());
        }

        public override Soldier CreateSoldier(int type, int health, Point positionCenter,Guid guid) {
            return CreateSoldier((PlayersSoldierType)type, health, positionCenter, guid);
        }

        public override Soldier CreateSoldier(int type, int health, Point positionCenter) {
            return CreateSoldier((PlayersSoldierType) type, health, positionCenter, Guid.NewGuid());
        }

        public override Soldier CreateSoldier(PlayersSoldierType type, int health, Point positionCenter, Guid guid) {
            Soldier soldier;

            switch (type) {

                case PlayersSoldierType.PlayersDefaultSoldier:
                    soldier = new Soldier(true, null, "Soldat4_NormaleWaffe_Run.png", health, positionCenter, 5, new Rectangle(0, 0, 8, 8), 70, 100, 0.95, 25);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            soldier.Guid = guid;
            soldier.PlayersType = type;
            // Registration
            GameEnv.RegisterGameObjectForAdding(soldier);

            return soldier;
        }

        protected override Soldier CreateSoldier(string imageFileName, string videoFileName, int health, Point positionCenter, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int weaponRange, int viewRange, double accuracy, int fireRate) {
                return new Soldier(true, imageFileName, videoFileName, health, positionCenter, primaryWeaponPower, rectangleForCollisionDetection, weaponRange, viewRange, accuracy, fireRate);
        }
    }
}