using System;
using SdlDotNet.Core;

namespace MrCrusher.Framework.Input {

    public class LocalInputEventHandler {

        private readonly LocalKeyboardEventHandler _localKeyboardEventHandler;
        private readonly LocalMouseEventHandler _localMouseEventHandler;

        public LocalInputEventHandler() {
            _localKeyboardEventHandler = new LocalKeyboardEventHandler();
            _localMouseEventHandler = new LocalMouseEventHandler();
        }
        
        public void BindApplicationEvents(EventHandler<TickEventArgs> bigLoopMethod) {
            Events.Quit += ApplicationQuitEventHandler;
            Events.Tick += bigLoopMethod;
        }

        public void BindKeyboardAndMouseEvents() {
            Events.KeyboardDown    += _localKeyboardEventHandler.KeyboardDownEventHandler;
            Events.KeyboardUp      += _localKeyboardEventHandler.KeyboardUpEventHandler;
            Events.MouseMotion     += _localMouseEventHandler.ApplicationMouseMotionEventHandler;
            Events.MouseButtonDown += _localMouseEventHandler.ApplicationMouseButtonDownEventHandler;
            Events.MouseButtonUp   += _localMouseEventHandler.ApplicationMouseButtonUpEventHandler;
        }

        private void ApplicationQuitEventHandler(object sender, QuitEventArgs args) {
            Events.QuitApplication();
        }

    }
}
