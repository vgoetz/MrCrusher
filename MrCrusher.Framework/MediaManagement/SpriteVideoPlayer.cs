using System.Drawing;
using MrCrusher.Framework.Drawable;
using SdlDotNet.Graphics;

namespace MrCrusher.Framework.MediaManagement {

    public class SpriteVideoPlayer {

        private readonly SpriteVideoData _spriteVideoData;
        public readonly VideoStartParameter VideoParameter;

        public int CurrentIndex { get; private set; }
        public bool Freezed { get; private set; }
        private int _tempFramesToSlowDown;


        public SpriteVideoPlayer(VideoStartParameter videoParameter) {
            VideoParameter = videoParameter;
            _spriteVideoData = VideoContainer.GetVideo(videoParameter.VideoFileName);
            
            if (VideoParameter.StartPlaying) {
                PlayFromBeginning();
            }
        }

        private void IncIndexIfNotSlowDown() {

            if (_tempFramesToSlowDown > 0) {
                // Slow down
                _tempFramesToSlowDown--;
            } else {
                // Play
                CurrentIndex++;
                _tempFramesToSlowDown = VideoParameter.FramesToSlowDown;
            }
        }

        public Rectangle GetCurrentVideoSurfaceRectangle() {
            var currentSurface = GetCurrentVideoSurface();
            if (currentSurface != null) {
                return currentSurface.Rectangle;
            }
            return new Rectangle();
        }

        public Surface GetCurrentVideoSurface() {
            return _spriteVideoData.GetVideoSurface(CurrentIndex);
        }

        public string GetCurrentVideoName() {
            return VideoParameter.VideoFileName;
        }

        // PLAY
        public Sprite GetAndPlayVideoSprite() {
            
            // get current image
            var currentSurface = GetCurrentVideoSurface();
            int tempIndex = CurrentIndex;

            // and play ...
            //if not freezed
            if (Freezed == false) {
                // Set index for next turn if not slow down
                IncIndexIfNotSlowDown();

                // Video wieder von Anfang an laufen lassen?
                if (IsAtEnd()) {
                    CurrentIndex = _spriteVideoData.VideoLength - 1;

                    if (VideoParameter.Loop) {
                        CurrentIndex = 0;
                    } else if (VideoParameter.FreezeAtEnd) {
                        Freeze();
                    }
                }
            }

            return new Sprite(currentSurface, new SpriteInfo { SpriteIndex = tempIndex, Name = GetCurrentVideoName(), SpriteType = SpriteType.Video });
        }

        public void PlayFromBeginning() {
            CurrentIndex = 0;
            Continue();
        }

        public void Freeze() {
            Freezed = true;
        }

        public void Stop() {
            PlayFromBeginning();
            Freeze();
        }

        public void Continue() {
            Freezed = false;
        }

        public bool IsAtEnd() {
            return CurrentIndex >= _spriteVideoData.VideoLength - 1;
        }
    }
}
