using System;
using System.Collections.Generic;
using System.Linq;
using SdlDotNet.Audio;

namespace MrCrusher.Framework.Game.Environment {

    public static class SoundHandler {

        private static List<Channel> _soundChannels;

        private static List<Sound> _hahaAndQuotes;
        private static List<Sound> _soldierDiedSounds;
        private static List<Sound> _soldierWasHitSounds;
        private static List<Sound> _rifleShotSounds;
        private static List<Sound> _tankFiringMainGunSounds;
        private static List<Sound> _tankShotHitsGroundSounds;
        private static List<Sound> _tankShotExplodingSounds;
        private static List<Sound> _tankDestroyedSounds;
        private static List<Sound> _rifleShotHitsSoftMaterial;
        private static List<Sound> _rifleShotHitsHardMaterial;
        private static List<Sound> _enemySeeYouSounds;
        private static List<Sound> _granadeIsFlyingSounds;

        public static void LoadSounds() {
            _soundChannels = new List<Channel>();
            for (var i = 0; i < 8; i++) {
                _soundChannels.Add(new Channel(i));
            }

            _hahaAndQuotes             = new List<Sound>();
            _soldierDiedSounds         = new List<Sound>();
            _soldierDiedSounds         = new List<Sound>();
            _soldierWasHitSounds       = new List<Sound>();
            _rifleShotSounds           = new List<Sound>();
            _tankFiringMainGunSounds   = new List<Sound>();
            _tankShotHitsGroundSounds  = new List<Sound>();
            _tankShotExplodingSounds   = new List<Sound>();
            _tankDestroyedSounds       = new List<Sound>();
            _rifleShotHitsSoftMaterial = new List<Sound>();
            _rifleShotHitsHardMaterial = new List<Sound>();
            _rifleShotHitsHardMaterial = new List<Sound>();
            _enemySeeYouSounds         = new List<Sound>();
            _granadeIsFlyingSounds     = new List<Sound>();

            _hahaAndQuotes.Add(new Sound(GameEnv.SoundResourcesSubDir + @"ComeGetSome.ogg"));
            _hahaAndQuotes.Add(new Sound(GameEnv.SoundResourcesSubDir + @"DreckigeLache1.ogg"));
            _hahaAndQuotes.Add(new Sound(GameEnv.SoundResourcesSubDir + @"DreckigeLache2.ogg"));
            _hahaAndQuotes.Add(new Sound(GameEnv.SoundResourcesSubDir + @"Groovy.ogg"));

            _rifleShotSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShot_1.ogg"));
            _tankFiringMainGunSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"PanzerFeuert_0001.ogg"));

            _tankShotHitsGroundSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"TankShotHitsGround1.ogg"));
            _tankShotHitsGroundSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"TankShotHitsGround2.ogg"));

            _tankShotExplodingSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"PanzerFeuert_0003_leise.ogg"));

            _tankDestroyedSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"TankDestroyed1.ogg"));
            _tankDestroyedSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"TankDestroyed2.ogg"));

            _rifleShotHitsSoftMaterial.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShotHitsSoftMaterial1.ogg"));

            _rifleShotHitsHardMaterial.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShotHitsHardMaterial1.ogg"));
            _rifleShotHitsHardMaterial.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShotHitsHardMaterial2.ogg"));
            _rifleShotHitsHardMaterial.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShotHitsHardMaterial3.ogg"));
            _rifleShotHitsHardMaterial.Add(new Sound(GameEnv.SoundResourcesSubDir + @"RifleShotHitsHardMaterial4.ogg"));

            _soldierDiedSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierDiedSound1.ogg"));
            _soldierDiedSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierDiedSound2.ogg"));

            _soldierWasHitSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierWasHitSound1.ogg"));
            _soldierWasHitSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierWasHitSound2.ogg"));
            _soldierWasHitSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierWasHitSound3.ogg"));
            _soldierWasHitSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"SoldierWasHitSound4.ogg"));

            _enemySeeYouSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"EnemySeeYou1.ogg"));
            _enemySeeYouSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"EnemySeeYou2.ogg"));

            // TODO:
            //_granadeIsFlyingSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"GranadeIsFlying1.ogg"));
            //_granadeIsFlyingSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"GranadeIsFlying2.ogg"));
            //_granadeIsFlyingSounds.Add(new Sound(GameEnv.SoundResourcesSubDir + @"GranadeIsFlying3.ogg"));
        }

        private static Sound GetRandomSoundFromList(IList<Sound> soundList) {
            if (soundList == null || soundList.Any() == false) {
                return null;
            }

            var random = new Random((int)DateTime.Now.Ticks);
            var randomNumber = random.Next(0, soundList.Count);

            return soundList[randomNumber];
        }

        private static void PlaySoundOnAvailableChannel(IList<Sound> soundList) {
            //bool freeChannelFound = false;

            if (!soundList.Any()) {
                return;
            }


            for (int i = 0; i < _soundChannels.Count; i++) {
                if (_soundChannels[i] == null || _soundChannels[i].IsPlaying() == false) {
                    _soundChannels[i] = GetRandomSoundFromList(soundList).Play();
                    //freeChannelFound = true;
                    break;
                } 
            }
        }

        public static void PlayRandomHahaAndQuotesSound() {
            PlaySoundOnAvailableChannel(_hahaAndQuotes);
        }
        
        public static void PlayRandomSoldierWasHitSound() {
            PlaySoundOnAvailableChannel(_soldierWasHitSounds);
        }

        public static void PlayRandomSoldierDiedSound() {
            PlaySoundOnAvailableChannel(_soldierDiedSounds);
        }

        public static void PlayRandomRifleShotSound() {
            PlaySoundOnAvailableChannel(_rifleShotSounds);
        }

        public static void PlayRandomTankFiringMainGunSound() {
            PlaySoundOnAvailableChannel(_tankFiringMainGunSounds);
        }

        public static void PlayRandomTankWasHitWithoutDamageSound() {
            PlaySoundOnAvailableChannel(_rifleShotHitsHardMaterial);
        }

        public static void PlayRandomRifleShotHitsSoftMaterialSound() {
            PlaySoundOnAvailableChannel(_rifleShotHitsSoftMaterial);
        }

        public static void PlayRandomTankShotHitsGroundSound() {
            PlaySoundOnAvailableChannel(_tankShotHitsGroundSounds);
        }

        public static void PlayRandomTankDestroyedSound() {
            PlaySoundOnAvailableChannel(_tankDestroyedSounds);
        }

        public static void PlayRandomTankShotExplodingSound() {
            PlaySoundOnAvailableChannel(_tankShotExplodingSounds);
        }

        public static void PlayRandomEnemySeeYouSound() {
            PlaySoundOnAvailableChannel(_enemySeeYouSounds);
        }

        public static void PlayRandomGranadeIsFlyingSound() {
            PlaySoundOnAvailableChannel(_granadeIsFlyingSounds);
        }

        public static void PlayRandomGranadeExplodesSound() {
            PlaySoundOnAvailableChannel(_tankShotExplodingSounds);
        }
    }
}
