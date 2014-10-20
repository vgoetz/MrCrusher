using System;
using MrCrusher.Framework.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace MrCrusher.Framework.SDL
{
    public class VideoDriverSettings
    {
        public static Surface InitializeComponents(int videoWidth, int videoHeight, bool fullscreen, PublicFrameworkEnums.RunningAspect aspect) {
            if(aspect == PublicFrameworkEnums.RunningAspect.DedicatedServer) {
                throw new ApplicationException("Ein Dedicated Server braucht das hier nicht.");
            }

            Keyboard.EnableKeyRepeat(10, 3);
            return SetVideoDriver(videoWidth, videoHeight, fullscreen);
        }

        private static Surface SetVideoDriver(int videoWidth, int videoHeight, bool fullscreen)
        {
            Video.Initialize();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                Environment.SetEnvironmentVariable("SDL_VIDEODRIVER", "directx");
            }

            Console.Out.WriteLine("Using Video Driver: {0}", Video.VideoDriver);
            Console.Out.WriteLine("Hardware surfaces : {0}", VideoInfo.HasHardwareSurfaces);
            Console.Out.WriteLine("Hardware blits    : {0}", VideoInfo.HasHardwareBlits);

            return Video.SetVideoMode(videoWidth, videoHeight, 32, false, false, fullscreen, true);
        }



    }
}
