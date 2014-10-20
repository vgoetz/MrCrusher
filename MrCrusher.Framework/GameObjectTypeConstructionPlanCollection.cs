using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;

namespace MrCrusher.Framework {
    public class GameObjectTypeConstructionPlanCollection {

        private readonly List<IGameObjectTypeConstructionPlan> _planList;

        public GameObjectTypeConstructionPlanCollection() {
            _planList = new List<IGameObjectTypeConstructionPlan>();

            //true, null, png..., health, positionCenter, 5, new Rectangle(0, 0, 8, 8), 70, 100, 0.95, 25);
            //Soldier(bool isControledByHumanPlayer, string imageFileName, string videoFileName, int health, Point positionCenter, int primaryWeaponPower, Rectangle rectangleForCollisionDetection, int weaponRange, int viewRange, double accuracy, int fireRate)
            var plan = new GameObjectTypeConstructionPlan(PlayersSoldierType.PlayersDefaultSoldier, 50, new Size(8, 8));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysSoldierType.Soldat5, 35, new Size(8, 8));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysSoldierType.Soldat6, 40, new Size(8, 8));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysSoldierType.Soldat7, 45, new Size(8, 8));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysSoldierType.Soldat8, 50, new Size(8, 8));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(PlayersTankType.PlayersDefaultTank, 100, new Size(28, 16));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysTankType.EnemyTankT1000, 100, new Size(28, 16));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(EnemysTankType.EnemyTankT2000, 130, new Size(28, 16));
            _planList.Add(plan);

            plan = new GameObjectTypeConstructionPlan(PlayersBunkerType.Bunker1, 200, new Size(22, 28));
            _planList.Add(plan);
        }

        public List<IGameObjectTypeConstructionPlan> GetConstructionPlans() {
            return _planList;
        }

        public IGameObjectTypeConstructionPlan GetConstructionPlan(object type) {
            return _planList.FirstOrDefault(plan => plan.Type.GetType() == type.GetType());
        }
    }
}
