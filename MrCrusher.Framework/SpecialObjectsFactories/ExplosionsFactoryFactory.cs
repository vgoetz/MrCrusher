using System;
using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using MrCrusher.Framework.SpecialObjects;

namespace MrCrusher.Framework.SpecialObjectsFactories
{
    public class ExplosionCascadeFactory : IExplosionCascadeFactory {

        private const string Eplosion464X64Sprite = "vidExplosion4_16x16.png";

        public ExplosionCascade CreateExplosionCascade(Point startPositionCenter, int radius, int numberOfExplosions, int framesBetweenExplosions, IGameObject victim) {

            if (victim == null) {
                throw new ArgumentNullException("victim");
            }

            var videoFileName = Eplosion464X64Sprite;

            if (victim is ITank) {
                videoFileName = Eplosion464X64Sprite;
            }

            var explosionCascade = new ExplosionCascade(null, new VideoStartParameter(videoFileName, false, true, true), startPositionCenter, radius, numberOfExplosions, framesBetweenExplosions);

            // Registration 
            GameEnv.RegisterGameObjectForAdding(explosionCascade);

            return explosionCascade;
        }
    }
}