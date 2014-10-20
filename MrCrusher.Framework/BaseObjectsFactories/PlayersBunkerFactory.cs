using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.BaseObjectsFactories
{
    public class PlayersBunkerFactory : IPlayersBunkerFactory {
        public override Bunker CreateBunker(PlayersBunkerType type, int health, Point positionCenter) {
            return CreateBunker(type, health, positionCenter, Guid.NewGuid());
        }

        public override Bunker CreateBunker(int type, int health, Point positionCenter, Guid guid) {
            return CreateBunker((PlayersBunkerType) type, health, positionCenter, guid);
        }

        public override Bunker CreateBunker(int type, int health, Point positionCenter) {
            return CreateBunker((PlayersBunkerType)type, health, positionCenter, Guid.NewGuid());
        }

        public override Bunker CreateBunker(PlayersBunkerType type, int health, Point positionCenter, Guid guid) {
            Bunker bunker;

            switch (type) {

                case PlayersBunkerType.Bunker1:
                    bunker = new Bunker(true , "Bunker1_Plattform.png", "Bunker1_Turm.png", health, positionCenter, 10, 25, new Rectangle(0, 0, 22, 28), 150, 200, 0.95, 1);

                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            bunker.PlayersType = type;
            bunker.Guid = guid;
            
            GameEnv.RegisterGameObjectForAdding(bunker);

            return bunker;
        }


        protected override Bunker CreateBunker(string imageFileName, string bunkerTowerImageFileName, int health, Point positionCenter, int cannonLength, int primaryWeaponPower,
                                           Rectangle rectangleForCollisionDetection, int range, int viewRange, double accuracy, int fireRate) {
            return new Bunker(true, imageFileName, bunkerTowerImageFileName, health, positionCenter,
                            cannonLength, primaryWeaponPower, rectangleForCollisionDetection, range, viewRange, accuracy, fireRate);
        }
    }
}