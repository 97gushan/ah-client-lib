using System;
using Arrowhead.Core;
using Arrowhead.Utils;
using Newtonsoft.Json;

namespace Arrowhead
{
    public class Client
    {
        private Service service;
        private Arrowhead.Core.System system;

        public Client()
        {
            this.system = new Arrowhead.Core.System("test-system", "https://127.0.0.1", "8080");
            this.service = new Service(this.system, "test-consumer", new string[] { "HTTPS-SECURE-JSON" });
            ServiceRegistry.InitServiceRegistry("https://127.0.0.1", "8443");

            Console.WriteLine(ServiceRegistry.RegisterService(this.service));
        }
    }
}
