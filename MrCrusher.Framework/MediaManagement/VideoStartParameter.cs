using System;

namespace MrCrusher.Framework.MediaManagement {

    public class VideoStartParameter {

        public VideoStartParameter(string videoFileName, bool loop, bool startPlaying = false, bool freezeAtEnd = false, int framesToSlowDown = 0) {
            if (String.IsNullOrEmpty(videoFileName)) {
                throw new ArgumentException("videoFileName");
            }

            VideoFileName = videoFileName;
            Loop = loop;
            StartPlaying = startPlaying;
            FreezeAtEnd = freezeAtEnd;
            FramesToSlowDown = framesToSlowDown > 0 ? framesToSlowDown : 1;
        }

        public string VideoFileName { get; private set; }
        public bool Loop { get; private set; }
        public bool StartPlaying { get; private set; }
        public bool FreezeAtEnd { get; private set; }
        public int FramesToSlowDown { get; private set; }
    }
}