using System;
using Newtonsoft.Json.Linq;

namespace Arrowhead.Models
{
    public class Service
    {
        public string serviceDefinition { get; set; }
        public string[] interfaces { get; set; }
        public System providerSystem;
        public int id;

        public string serviceUri;

        public Service(System system, string serviceDefinition, string[] interfaces, string serviceUri)
        {
            this.providerSystem = system;
            this.serviceDefinition = serviceDefinition;
            this.interfaces = interfaces;
            this.serviceUri = serviceUri;
        }

        public override string ToString()
        {
            return this.serviceDefinition;
        }


        /// <summary>
        /// Build a Service object based on the json fetched from the service registry
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Service Build(JToken json)
        {
            JToken systemJson = json.SelectToken("provider");
            System sys = System.Build(systemJson);

            JToken serviceJson = json.SelectToken("serviceDefinition");
            string serviceDef = serviceJson.SelectToken("serviceDefinition").ToString();

            JArray interfacesJson = JArray.Parse(json.SelectToken("interfaces").ToString());

            string[] interfaces = new string[interfacesJson.Count];
            for (int i = 0; i < interfaces.Length; i++)
            {
                interfaces[i] = interfacesJson.Value<JToken>(i).SelectToken("interfaceName").ToString();
            }
            Service service = new Service(sys, serviceDef, interfaces, "");
            service.id = json.SelectToken("id").ToObject<Int32>();
            return service;
        }
    }
}
