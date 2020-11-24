using System;

namespace Arrowhead.Utils
{
    public class Settings
    {

        public Boolean ssl, VerifyCertificate;
        private string ServiceRegistryAddress, ServiceRegistryPort;
        private string OrchestratorAddress, OrchestratorPort;
        private string AuthorizationAddress, AuthorizationPort;
        public string CertificatePath;

        /// <summary>
        /// Initializes a settings object with default settings
        /// </summary>
        public Settings()
        {
            this.ssl = true;
            this.VerifyCertificate = false;
            this.ServiceRegistryAddress = "127.0.0.1";
            this.ServiceRegistryPort = "8443";
            this.OrchestratorAddress = "127.0.0.1";
            this.OrchestratorPort = "8441";
            this.AuthorizationAddress = "127.0.0.1";
            this.AuthorizationPort = "8445";

            this.CertificatePath = "/home/user/Projects/arrowhead/core-java-spring/certificates/testcloud2/sysop.p12";
        }

        public string getServiceRegistryUrl()
        {
            string scheme = this.ssl ? "https://" : "http://";
            return scheme + this.ServiceRegistryAddress + ":" + this.ServiceRegistryPort;
        }
        public string getOrchestratorUrl()
        {
            string scheme = this.ssl ? "https://" : "http://";
            return scheme + this.OrchestratorAddress + ":" + this.OrchestratorPort;
        }

        public string getAuthorizationUrl()
        {
            string scheme = this.ssl ? "https://" : "http://";
            return scheme + this.AuthorizationAddress + ":" + this.AuthorizationPort;
        }
    }
}
