using Newtonsoft.Json.Linq;

namespace Arrowhead.Models
{
    public class System
    {
        public string systemName, address, port;
        public string authenticationInfo;

        public System(string systemName, string address, string port)
        {
            this.systemName = systemName;
            this.address = address;
            this.port = port;
            this.authenticationInfo = "";
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="cloudName"></param>
        /// <returns></returns>
        public string ArrowheadCommonName(string op, string cloudName)
        {
            return this.systemName + "." + cloudName + "." + op + ".Arrowhead.eu";
        }

        /// <summary>
        /// Build a System object based on a json fetched from the service registry
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static System Build(JToken json)
        {
            string systemName = json.SelectToken("systemName").ToString();
            string address = json.SelectToken("address").ToString();
            string port = json.SelectToken("port").ToString();

            return new System(systemName, address, port);
        }
    }
}
