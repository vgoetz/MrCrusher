using System;
using XSockets.Core.Common.Interceptor;

namespace MrCrusher.XSocketsServer {

    public class MyErrorInterceptor : IErrorInterceptor {

        public void OnError(Exception exception) {
            Console.WriteLine("ERROR-INTERCEPTOR: {0}", exception.Message);
        }
    }
}