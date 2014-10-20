using System.Drawing;
using System.Linq;
using MrCrusher.Framework.AI;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher {
    public class AiHandler {

        public static void AiPerforming() {
            if (!GameEnv.Players.Any() || GameEnv.Players.All(p => p.CurrentControlledGameObject.Dead)) {
                return;
            }

            foreach (IGameObject gameObject in GameEnv.GameObjectsToDrawAndMove) {

                var canTargetGameObjects = gameObject as ICanTargetGameObjects;
                if (canTargetGameObjects == null || gameObject.IsControlledByHumanPlayer || gameObject.ShouldBeDeleted || gameObject.Dead) {
                    continue;
                }

                var canBeEntered = gameObject as ICanBeEntered;
                // Ignore empty objects
                if (canBeEntered != null && !canBeEntered.IsManned) {
                    continue;
                }
                
                // MOVING
                var hasMoveStrategy = gameObject as IHasMoveStrategy;
                IHasAimStrategy hasAimStrategy;

                if (hasMoveStrategy != null) {
                    if (gameObject.CurrentMission != null) {
                        
                        Point destination = gameObject.CurrentMission.GetMissionTargetAsPoint();

                        if (destination == gameObject.PositionCenter) {
                            // Mission reached
                            gameObject.CurrentMission = null;
                            hasMoveStrategy.Stop();
                        } else {
                            // go on 
                            hasMoveStrategy.AI_SetMoveDestination(destination);

                            hasAimStrategy = gameObject as IHasAimStrategy;
                            if (hasAimStrategy != null) {
                                hasAimStrategy.AI_SetAimPosition(destination);
                            }
                        }
                    }
                }

                var target = canTargetGameObjects.TargetedGameObject;

                // AIMING
                hasAimStrategy = gameObject as IHasAimStrategy;
                if (hasAimStrategy != null && target != null && !target.Dead) {
                    hasAimStrategy.AI_SetAimTarget(target);
                }

                // SHOOTING
                var hasShootStrategy = gameObject as IHasShootStrategy;
                if (hasShootStrategy != null && target != null && !target.Dead) {
                    hasShootStrategy.AI_Shoot(target);
                }

                // TRIGGER ATTACK?
                var hasTriggerAttackStrategy = gameObject as IHasTriggerAttackStrategy;
                if (hasTriggerAttackStrategy != null) {

                    if (target != null && target.Dead) {
                        gameObject.CurrentMission = null;
                        hasTriggerAttackStrategy.Stop();
                    }

                    var attackMission = gameObject.CurrentMission as AttackObjectMission;

                    if (attackMission == null) {

                        foreach (var playerAsTarget in GameEnv.Players
                            .Select(player => player.CurrentControlledGameObject)
                            .Where(obj => obj != null && !obj.Dead)) {

                            if (hasTriggerAttackStrategy.AI_TriggerAttackOnTarget(playerAsTarget)) {

                                canTargetGameObjects.TargetedGameObject = playerAsTarget;
                                gameObject.CurrentMission = new AttackObjectMission(playerAsTarget);

                                SoundHandler.PlayRandomEnemySeeYouSound();
                            } 
                        }
                    }
                }
            }
        }


    }
}