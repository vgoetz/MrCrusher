using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MrCrusher.Framework.BaseObjects;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.MediaManagement;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.SpecialObjects {

    
    public class ExplosionCascade : GameObject {
        private readonly int _radius;
        private readonly int _totalNumberOfExplosions;
        private readonly int _framesBetweenExplosions;
        private int _framesElapsedSinceLastExplosion;
        private readonly VideoStartParameter _tempVideoStartParameter;
        private readonly Point _positionCenter;
        
        private readonly Dictionary<VideoPlayerCollection, Point> _explosionVideos;

        public ExplosionCascade(string surfaceFileName, VideoStartParameter videoParameter, Point positionCenter, int radius, int totalNumberOfExplosions, int framesBetweenExplosions)
            : base(false, surfaceFileName, videoParameter, positionCenter) {

            _tempVideoStartParameter = videoParameter;
            _positionCenter = positionCenter;
            _radius = radius;
            _totalNumberOfExplosions = totalNumberOfExplosions;
            _framesBetweenExplosions = framesBetweenExplosions;
            _framesElapsedSinceLastExplosion = 0;

            SoundHandler.PlayRandomTankDestroyedSound();

            if (totalNumberOfExplosions > 0) {
                _explosionVideos = new Dictionary<VideoPlayerCollection, Point> { { new VideoPlayerCollection(videoParameter, true), _positionCenter } };
            }
        }

        public override IEnumerable<Sprite> GetCurrentSprites() {
            var listOfExplosionsToShow = (from explosionVideo in _explosionVideos
                                          select explosionVideo.Key.ActiveVideoPlayer
                                          into explosionVideoPlayer
                                          where explosionVideoPlayer != null && !explosionVideoPlayer.IsAtEnd()
                                             select explosionVideoPlayer.GetAndPlayVideoSprite()
                                             into surfaceAndInfo
                                             where surfaceAndInfo != null
                                                select surfaceAndInfo).ToList();

            // Nächste Explosion hinzufügen?
            if (_totalNumberOfExplosions > _explosionVideos.Count &&
                _framesElapsedSinceLastExplosion > _framesBetweenExplosions) {

                _framesElapsedSinceLastExplosion = 0;

                var random = new Random((int) DateTime.UtcNow.Ticks);
                var xOffset = random.Next(-_radius, _radius);
                var yOffset = random.Next(-_radius, _radius);

                var newExplosionToAdd = new VideoPlayerCollection(_tempVideoStartParameter, true);
                _explosionVideos.Add(newExplosionToAdd, new Point(_positionCenter.X + xOffset, _positionCenter.Y + yOffset));
            }

            _framesElapsedSinceLastExplosion++;

            return listOfExplosionsToShow;
        }

    }
}
