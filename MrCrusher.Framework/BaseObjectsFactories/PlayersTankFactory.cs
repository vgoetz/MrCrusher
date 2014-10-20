using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class PlayersTankFactory : IPlayersTankFactory {
        public override Tank CreateTank(PlayersTankType type, int health, Point positionCenter) {
            return CreateTank(type, health, positionCenter, Guid.NewGuid());
        }

        public override Tank CreateTank(int type, int health, Point positionCenter, Guid guid) {
            return CreateTank((PlayersTankType) type, health, positionCenter, guid);
        }

        public override Tank CreateTank(int type, int health, Point positionCenter) {
            return CreateTank((PlayersTankType)type, health, positionCenter, Guid.NewGuid());
        }

        public override Tank CreateTank(PlayersTankType type, int health, Point positionCenter, Guid guid) {
            Tank tank;

            switch (type) {

                case PlayersTankType.PlayersDefaultTank:
                    tank = new Tank(true , "Panzer4_Plattform.png", "Panzer4_Turm.png", health, positionCenter, 18, 18, new Rectangle(0, 0, 28, 16), 150, 200, 0.90, 1);

                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            tank.PlayersType = type;
            tank.Guid = guid;
            // Registration
            GameEnv.RegisterGameObjectForAdding(tank);

            return tank;
        }



        protected override Tank CreateTank(string imageFileName, string tankTowerImageFileName, int health, Point positionCenter, int cannonLength, int primaryWeaponPower,
                                           Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate) {
            return new Tank(true, imageFileName, tankTowerImageFileName, health, positionCenter,
                            cannonLength, primaryWeaponPower, rectangleForCollisionDetection, range, viewRange, accuracy, fireRate);
        }
    }
}