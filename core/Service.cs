using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class Service
    {
        public string serviceDefinition { get; set; }
        public string[] interfaces { get; set; }
        public System providerSystem;

        public Service(System system, string serviceDefinition, string[] interfaces)
        {
            this.providerSystem = system;
            this.serviceDefinition = serviceDefinition;
            this.interfaces = interfaces;
        }

        public override string ToString()
        {
            return this.serviceDefinition;
        }
    }
}
