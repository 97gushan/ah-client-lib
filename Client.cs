using System;
using ArrowHead.Core;
using Newtonsoft.Json;

namespace ArrowHead
{
    public class Client
    {
        private Service service;

        public Client()
        {
            this.service = new Service("https://localhost:8443/serviceregistry");
        }
        public void GetServices()
        {
            this.service.GetServices();
        }
        public void GetSystems()
        {
            this.service.GetSystems();
        }
    }
}
