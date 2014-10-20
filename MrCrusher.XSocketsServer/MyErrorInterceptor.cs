using System;
using XSockets.Core.Common.Interceptor;
using XSockets.Core.Common.Socket;
using XSockets.Core.Common.Socket.Event.Arguments;

namespace MrCrusher.XSocketsServer {

    public class MyErrorInterceptor : IErrorInterceptor {

        public void OnError(OnErrorArgs errorArgs) {
            Console.WriteLine("ERROR-INTERCEPTOR: {0}, {1}", errorArgs.Exception, errorArgs.Message);
        }

        public void OnError(IXSocketController socket, OnErrorArgs errorArgs) {
            Console.WriteLine("ERROR-INTERCEPTOR: {0}, {1}, {2}", socket.Alias, errorArgs.Exception, errorArgs.Message);
        }
    }
}