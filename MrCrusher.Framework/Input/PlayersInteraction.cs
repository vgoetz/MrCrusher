using System;

namespace MrCrusher.Framework.Input {

    [Serializable]
    public abstract class PlayersInteraction {
        private readonly Player.Player _playerReference;

        protected PlayersInteraction(Player.Player playerReference) {
            _playerReference = playerReference;
        }

        public Player.Player PlayerReference {
            get { return _playerReference; }
        }
    }
}