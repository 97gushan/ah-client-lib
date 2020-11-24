using System;


namespace Arrowhead.Models
{
    public class Service
    {
        public string serviceDefinition { get; set; }
        public string[] interfaces { get; set; }
        public System providerSystem;
        public int id;

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


        /// <summary>
        /// Build a Service object based on the json fetched from the service registry
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Service Build(Newtonsoft.Json.Linq.JToken json)
        {
            Newtonsoft.Json.Linq.JToken systemJson = json.SelectToken("provider");
            System sys = System.Build(systemJson);

            Newtonsoft.Json.Linq.JToken serviceJson = json.SelectToken("serviceDefinition");
            string serviceDef = serviceJson.SelectToken("serviceDefinition").ToString();

            Newtonsoft.Json.Linq.JArray interfacesJson = Newtonsoft.Json.Linq.JArray.Parse(json.SelectToken("interfaces").ToString());

            string[] interfaces = new string[interfacesJson.Count];
            for (int i = 0; i < interfaces.Length; i++)
            {
                interfaces[i] = interfacesJson.Value<Newtonsoft.Json.Linq.JToken>(i).SelectToken("interfaceName").ToString();
            }
            Service service = new Service(sys, serviceDef, interfaces);
            service.id = json.SelectToken("id").ToObject<Int32>();
            return service;
        }
    }
}
