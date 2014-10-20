using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;

namespace MrCrusher.Framework.MediaManagement {
    public class VideoPlayerCollection {
        private readonly List<SpriteVideoPlayer> _playerCollection = new List<SpriteVideoPlayer>();
        public SpriteVideoPlayer ActiveVideoPlayer { get; private set; }

        public VideoPlayerCollection() {}

        public VideoPlayerCollection(VideoStartParameter videoParameter, bool setAsActive) {
            Add(videoParameter, setAsActive);
        }

        public VideoPlayerCollection(IEnumerable<VideoStartParameter> videoParameters, string activeVideoPlayer = "") {
            Add(videoParameters, activeVideoPlayer);
        }

        public void Add(IEnumerable<VideoStartParameter> videoParameters, string activeVideoPlayer = "") {
            foreach (var videoParameter in videoParameters) {
                Add(videoParameter, !string.IsNullOrEmpty(activeVideoPlayer));
            }
        }

        public void Add(VideoStartParameter videoParameter, bool setAsActive) {
            bool alreadyIncluded = _playerCollection.Any(v => v.VideoParameter.VideoFileName == videoParameter.VideoFileName);
            if(alreadyIncluded == false) {
                _playerCollection.Add(new SpriteVideoPlayer(videoParameter));
            }

            if(setAsActive) {
                ActiveVideoPlayer = _playerCollection.Find(v => v.VideoParameter.VideoFileName == videoParameter.VideoFileName);
            }
        }

        public void SetActiveVideo(string videoFileName) {
            SpriteVideoPlayer video = _playerCollection.FirstOrDefault(v => v.VideoParameter.VideoFileName == videoFileName);
            if (video == null) {
                throw new InstanceNotFoundException("Video wurde nicht gefunden!");
            }

            ActiveVideoPlayer = video;
        }
    }
}
