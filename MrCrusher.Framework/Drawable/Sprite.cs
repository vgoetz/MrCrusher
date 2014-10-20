using SdlDotNet.Graphics;

namespace MrCrusher.Framework.Drawable {

    public class Sprite {

        public Sprite(Surface surface, SpriteInfo infos) {
            Surface = surface;
            Infos = infos;
        }

        public Surface Surface { get; set; }
        public SpriteInfo Infos { get; set; }
    }
}