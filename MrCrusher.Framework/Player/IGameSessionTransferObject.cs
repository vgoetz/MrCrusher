using MrCrusher.Framework.Drawable;

namespace MrCrusher.Framework.Player {
    
    public interface IGameSessionTransferObject {

        bool GameOver { get; }
        PlayerTo[] PlayerTos { get; }
        ImageTransferObject[] ImageTransferObjects { get; }
    }
}
