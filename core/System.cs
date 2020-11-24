namespace Arrowhead.Core
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
    }
}
