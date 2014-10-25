using System;
using System.Drawing;
using MrCrusher.Framework.Game.Environment;

namespace MrCrusher.Framework.Drawable {

    [Serializable]
    public class ImageTransferObject : IImageTransferObject {
        private Guid _gameObjectGuid;
        private Guid? _clientGuid;
        private SpriteInfo _infos;
        private int _health;
        private int _maxHealth;
        private bool _dead;
        private bool _isControlledByHumanPlayer;
        private short _idCircleRadius;
        private string _idCircleColorAsString;
        private string _displayname;

        public ImageTransferObject(Guid gameObjectGuid, Player.Player player, SpriteInfo infos, int health, int maxHealth, bool dead, bool isControlledByHumanPlayer, Color? idCircleColor, short idCircleRadius) {
            
            if (player != null) {

                Displayname = player.Name ?? String.Empty;

                if (player.ClientGuid != Guid.Empty) {
                    ClientGuid = player.ClientGuid;
                }
            }
            
            GameObjectGuid = gameObjectGuid;
            Infos = infos;
            Health = health;
            MaxHealth = maxHealth;
            Dead = dead;
            IsControlledByHumanPlayer = isControlledByHumanPlayer;
            IdCircleRadius = idCircleRadius;
            IdCircleColorAsString = GameEnv.ColorConverter.ConvertToInvariantString(idCircleColor ?? Color.Black);
        }

        public Guid GameObjectGuid {
            get { return _gameObjectGuid; }
            private set { _gameObjectGuid = value; }
        }

        public Guid? ClientGuid {
            get { return _clientGuid; }
            private set { _clientGuid = value; }
        }

        public SpriteInfo Infos {
            get { return _infos; }
            private set { _infos = value; }
        }

        public int Health {
            get { return _health; }
            private set { _health = value; }
        }

        public int MaxHealth {
            get { return _maxHealth; }
            private set { _maxHealth = value; }
        }

        public bool Dead {
            get { return _dead; }
            private set { _dead = value; }
        }

        public bool IsControlledByHumanPlayer {
            get { return _isControlledByHumanPlayer; }
            private set { _isControlledByHumanPlayer = value; }
        }

        public string IdCircleColorAsString {
            get { return _idCircleColorAsString; }
            private set { _idCircleColorAsString = value; }
        }

        public short IdCircleRadius {
            get { return _idCircleRadius; }
            private set { _idCircleRadius = value; }
        }

        public string Displayname {
            get { return _displayname; }
            private set { _displayname = value; }
        }
    }
}