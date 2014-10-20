using SdlDotNet.Audio;

namespace MrCrusher.Framework.SDL
{
    public class MusicHelper
    {
        public static void LoadMusic(string dirAndfileName)
        {
            var bgMusic = new Music(dirAndfileName);
            MusicPlayer.Volume = 30;
            MusicPlayer.Load(bgMusic);
        }
    }
}
