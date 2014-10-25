using System;
using System.Collections.Generic;
using System.Drawing;
using MrCrusher.Framework.BaseObjects.Interfaces;
using MrCrusher.Framework.BaseObjectsFactories;
using MrCrusher.Framework.Core;
using MrCrusher.Framework.Drawable;
using MrCrusher.Framework.Game.Environment;
using MrCrusher.Framework.Input;

namespace MrCrusher.Framework.Player {

    public class Player {

        private readonly string _name;
        private MouseInteraction _lastPlayersMouseInteraction;
        private KeyboardInteraction _lastPlayersKeybardInteraction;
        private readonly bool _isLocalPlayer;
        private readonly bool _isHost;
        private Color _playersColor;
        private static readonly List<Color> PlayersPossibleColors = new List<Color>() { Color.Aqua, Color.DarkMagenta, Color.Blue, Color.Yellow, Color.Green};
        
        private readonly PlayersSoldierFactory _playersSoldierFactory;
        
        private Guid _clientGuid;

        public Player(string name, bool isLocalPlayer, bool isHost) {
            _name = name;
            _lastPlayersMouseInteraction = new MouseInteraction(this, new Point(), false, false, false);
            _lastPlayersKeybardInteraction = new KeyboardInteraction(this);
            _isLocalPlayer = isLocalPlayer;
            _isHost = isHost;

            if (isHost) {
                _clientGuid = new Guid();
                _playersColor = Color.FromArgb(80, Color.Red);
            } else if (GameEnv.RunningAspect == PublicFrameworkEnums.RunningAspect.Server) {
                // Set client-player´s user color (actually it´s set by the server, not choosen by client)
                _playersColor = Color.FromArgb(80, PlayersPossibleColors[GameEnv.Random.Next(PlayersPossibleColors.Count - 1)]);
            }

            _playersSoldierFactory = new PlayersSoldierFactory();

            BodyCount = new PlayerBodyCount();
        }

        public PlayerTo ToPlayerTo() {
            return new PlayerTo(Name, ClientGuid, GameEnv.ColorConverter.ConvertToInvariantString(PlayersColor));
        }

        public string Name {
            get { return _name; }
        }

        public Color PlayersColor {
            get { return _playersColor; }
            set { _playersColor = value; }
        }

        public MouseInteraction LastPlayersMouseInteraction {
            get { return _lastPlayersMouseInteraction; }
            set { _lastPlayersMouseInteraction = value; }
        }

        public KeyboardInteraction LastPlayersKeybardInteraction {
            get { return _lastPlayersKeybardInteraction; }
        }

        public Guid ClientGuid {
            get { return _clientGuid; }
            set { _clientGuid = value; }
        }

        public bool IsLocalPlayer {
            get { return _isLocalPlayer; }
        }

        public bool IsHost {
            get { return _isHost; }
        }

        public ISoldier MainControlledSoldier { get; set; }
        public IGameObject AdditionallyControlledGameObject { get; set; }

        public IGameObject CurrentControlledGameObject {
            get {
                if (AdditionallyControlledGameObject != null) {
                    return AdditionallyControlledGameObject;
                } 
                
                if (MainControlledSoldier != null) {
                    return MainControlledSoldier;
                }

                return null;
            }
        }

        public IImageTransferObject CurrentControlledTransferObject { get; set; }
        
        public void CreateNewSoldierAtRandomPosition() {
            MainControlledSoldier = _playersSoldierFactory.CreatePlayersSoldier();
            MainControlledSoldier.PlayerAsController = this;
        }

        public void TakeAdditionallyControllOf(IGameObject gameObject) {
            gameObject.PlayerAsController = this;
            AdditionallyControlledGameObject = gameObject;
        }

        public void ReleaseAdditionallyControllOfAnObject() {
            if (AdditionallyControlledGameObject != null) {
                AdditionallyControlledGameObject.PlayerAsController = null;
                AdditionallyControlledGameObject = null;
            }
        }

        public PlayerBodyCount BodyCount { get; private set; }

        public void Reset() {

            _lastPlayersMouseInteraction = new MouseInteraction(this, new Point(), false, false, false);
            _lastPlayersKeybardInteraction = new KeyboardInteraction(this);

            ReleaseAdditionallyControllOfAnObject();
            BodyCount.Reset();
        }
    }

    public class PlayerBodyCount {

        public int KilledSoldiers { get; set; }
        public int KilledTanks { get; set; }
        public int KilledBunkers { get; set; }
        public int KillsTotal { get { return KilledSoldiers + KilledTanks + KilledBunkers; } }

        public void Reset() {
            KilledSoldiers = 0;
            KilledTanks = 0;
            KilledBunkers = 0;
        }
    }
}