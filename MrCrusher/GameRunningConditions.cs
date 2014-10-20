using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher {

    public class GameRunningConditions {
        
        public static void AddNewEnemySoldiersAtRandomPosition(int maxNumberOfSoldiers) {
            
            if (GameEnv.RunningAspect != PublicFrameworkEnums.RunningAspect.Server) {
                return;
            }

            int numberOfExistingEnemySoldiers =
                GameEnv.ExistingAndRegisteredGameObjects.Count(
                    obj => obj is ISoldier && ((Soldier) obj).EnemysType != EnemysSoldierType.None && obj.Dead == false);
            
            if (numberOfExistingEnemySoldiers >= maxNumberOfSoldiers) {
                return;
            }


            var topStartRect    = GameEnv.TopStartLimitationRectangleInMap;
            var bottomStartRect = GameEnv.BottomStartLimitationRectangleInMap;
            var leftStartRect   = GameEnv.LeftStartLimitationRectangleInMap;
            var rightStartRect  = GameEnv.RightStartLimitationRectangleInMap;
            var listOfLimitationRectanglseInMap = new List<Rectangle> { topStartRect, bottomStartRect, leftStartRect, rightStartRect };

            var listOfSoldierTypes = new List<EnemysSoldierType> { EnemysSoldierType.EnemyDefaultSoldier, EnemysSoldierType.Soldat5, EnemysSoldierType.Soldat6, EnemysSoldierType.Soldat7, EnemysSoldierType.Soldat8 };
            var newNumberOfSoldiers = maxNumberOfSoldiers - numberOfExistingEnemySoldiers;

            var enemysSoldierFactory = new EnemysSoldierFactory();
            var newEnemies = new List<IGameObject>();

            List<Soldier> newSoldiers = enemysSoldierFactory.CreateManySoldiers(listOfSoldierTypes, newNumberOfSoldiers, listOfLimitationRectanglseInMap).ToList();
            newEnemies.AddRange(newSoldiers);
            
            // Initialen Auftrag erteilen -> gehe zur Mitte 
            var randomizer = new Random((int)DateTime.Now.Ticks);
            foreach (var newEnemy in newEnemies.OfType<RotatingObject>()) {
                newEnemy.SetMovingDestination(GetRandomPointWithinRectangle(GameEnv.CenterStartLimitationRectangleInMap, randomizer));
                newEnemy.RotateInstantly(GameEnv.CurrentObjectControledByUser.PositionCenter);
            }
        }

        public static void AddNewEnemyTanksAtRandomPosition(int maxNumberOfTanks) {

            if (GameEnv.RunningAspect != PublicFrameworkEnums.RunningAspect.Server) {
                return;
            }

            int numberOfExistingEnemyTanks =
                GameEnv.GameObjectsToDrawAndMove.Count(
                    obj => obj is ITank && ((Tank)obj).EnemysType != EnemysTankType.None && obj.Dead == false);

            if (numberOfExistingEnemyTanks >= maxNumberOfTanks) {
                return;
            }

            var topStartRect    = GameEnv.TopStartLimitationRectangleInMap;
            var bottomStartRect = GameEnv.BottomStartLimitationRectangleInMap;
            var leftStartRect   = GameEnv.LeftStartLimitationRectangleInMap;
            var rightStartRect  = GameEnv.RightStartLimitationRectangleInMap;
            var listOfLimitationRectanglseInMap = new List<Rectangle> { topStartRect, bottomStartRect, leftStartRect, rightStartRect };

            var listOfTankTypes = new List<EnemysTankType> { EnemysTankType.EnemyTankT1000, EnemysTankType.EnemyTankT2000 };
            var newNumberOfTanks = maxNumberOfTanks - numberOfExistingEnemyTanks;

            var newEnemies = new List<IGameObject>();
            var enemysTankFactory = new EnemysTankFactory();
            
            List<Tank> newTanks = enemysTankFactory.CreateManyTanks(listOfTankTypes, newNumberOfTanks, listOfLimitationRectanglseInMap).ToList();
            newEnemies.AddRange(newTanks);

            // Initialen Auftrag erteilen -> gehe zur Mitte
            var randomizer = new Random((int)DateTime.Now.Ticks);
            foreach (var newEnemy in newEnemies.OfType<Tank>()) {
                newEnemy.SetMovingDestination(GetRandomPointWithinRectangle(GameEnv.CenterStartLimitationRectangleInMap, randomizer));
                newEnemy.SetTowerRotationDestination(GameEnv.CurrentObjectControledByUser.PositionCenter);
            }
        }

        public static Point GetRandomPointWithinRectangle(Rectangle limationRectangle, Random randomizer) {

            return new Point(randomizer.Next(limationRectangle.X, limationRectangle.X + limationRectangle.Width),
                             randomizer.Next(limationRectangle.Y, limationRectangle.Y + limationRectangle.Height));
        }
    }
}
