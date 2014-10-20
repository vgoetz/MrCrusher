using System.Drawing;

namespace MrCrusher.Framework.SDL
{
    struct SdmRectangle {
        public Rectangle Rectangle;
        public byte Alpha;

        public SdmRectangle(Rectangle rectangle, byte alpha = 255) {
            Rectangle = rectangle;
            Alpha = alpha;
        }
    }
}
