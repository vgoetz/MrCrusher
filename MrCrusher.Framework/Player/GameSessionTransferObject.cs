using System;
using System.Collections.Generic;
using System.Linq;
using MrCrusher.Framework.Drawable;

namespace MrCrusher.Framework.Player {

    [Serializable]
    public class GameSessionTransferObject : IGameSessionTransferObject {

        public GameSessionTransferObject(IEnumerable<PlayerTo> playerTos, IEnumerable<ImageTransferObject> imageTransferObjects, bool gameOver) {
            ImageTransferObjects = imageTransferObjects.ToArray();
            PlayerTos = playerTos.ToArray();
            GameOver = gameOver;
        }

        public PlayerTo[] PlayerTos { get; private set; }
        public ImageTransferObject[] ImageTransferObjects { get; private set; }
        public bool GameOver { get; private set; }
    }
}