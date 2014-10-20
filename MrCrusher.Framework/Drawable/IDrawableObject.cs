using System.Collections.Generic;

namespace MrCrusher.Framework.Drawable {
    
    public interface IDrawableObject {

        IEnumerable<ImageTransferObject> ToImageTransferObjects();
    }
}