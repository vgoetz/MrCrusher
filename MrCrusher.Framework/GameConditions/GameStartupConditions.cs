using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.SDL;

namespace MrCrusher {

    public class GameStartupConditions {

        public static void SetManyPlayersAndEnemysObjects() {

            var playersTankFactory = new PlayersTankFactory();
            var playersBunkerFactory = new PlayersBunkerFactory();
            var enemysSoldierFactory = new EnemysSoldierFactory();
            var enemysTankFactory = new EnemysTankFactory();

            if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Server) {

                
                playersTankFactory.CreateTank(PlayersTankType.PlayersDefaultTank, 100, new Point(30, 100));
                playersBunkerFactory.CreateBunker(PlayersBunkerType.Bunker1, 400, new Point(230, 330));

                enemysTankFactory.CreateTank(EnemysTankType.EnemyTankT1000, 100, new Point(440, 200));
                enemysTankFactory.CreateTank(EnemysTankType.EnemyTankT2000, 100, new Point(430, 230));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat5, 25, new Point(150, 150));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat5, 30, new Point(450, 250));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat5, 35, new Point(470, 260));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat6, 40, new Point(350, 260));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat7, 45, new Point(360, 280));
                enemysSoldierFactory.CreateSoldier(EnemysSoldierType.Soldat8, 50, new Point(375, 270));
            }

        }

        public static void SetPlayersTankAndBunkerAtRandomLocaltionAtTheCenter() {
            
            if (GameEnv.RunningAspect != PublicFrameworkEnums.RunningAspect.Server) {
                return;
            }

            var centerStartRectangle = GameEnv.CenterStartLimitationRectangleInMap;
            var randomizer = new Random((int)DateTime.Now.Ticks);

            const PlayersTankType playersTankType = PlayersTankType.PlayersDefaultTank;
            Size tanksCollisionSize = GameEnv.ConstructionPlanCollection.GetConstructionPlan(playersTankType).CollisionSize;

            const PlayersBunkerType playersBunkerType = PlayersBunkerType.Bunker1;
            Size bunkerCollisionSize = GameEnv.ConstructionPlanCollection.GetConstructionPlan(playersBunkerType).CollisionSize;

            Point? tankPosition = MapHelper.GetRandomColisionFreePointWithinRectangle(tanksCollisionSize, centerStartRectangle, randomizer);
            if (tankPosition.HasValue) {
                var playersTankFactory = new PlayersTankFactory();
                playersTankFactory.CreateTank(playersTankType, 100, tankPosition.Value);
            }

            Point? bunkerPosition = MapHelper.GetRandomColisionFreePointWithinRectangle(bunkerCollisionSize, centerStartRectangle, randomizer);
            if (bunkerPosition.HasValue) {
                var playersBunkerFactory = new PlayersBunkerFactory();
                playersBunkerFactory.CreateBunker(playersBunkerType, 400, bunkerPosition.Value);
            }
        }

    }
}
