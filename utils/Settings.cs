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
        public string SystemName, Ip, Port, ConsumerSystemId;
        public string ServiceDefinition, ApiUri;
        public string[] Interfaces;
        public string CloudName, CloudOperator;

        /// <summary>
        /// Initializes a settings object with default settings
        /// </summary>
        public Settings()
        {
            this.Interfaces = new string[] { "HTTPS-SECURE-JSON" };

            this.SystemName = "";
            this.Ip = "127.0.0.1";
            this.Port = "8080";

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

        public void SetServiceSettings(string serviceDefinition, string[] interfaces, string apiUri)
        {
            this.ServiceDefinition = serviceDefinition;
            this.Interfaces = interfaces;
            this.ApiUri = apiUri;
        }

        public void SetSystemSettings(string systemName, string ip, string port)
        {
            this.SystemName = systemName;
            this.Ip = ip;
            this.Port = port;
        }

        public void SetSystemSettings(string systemName, string ip, string port, string id)
        {
            this.SystemName = systemName;
            this.Ip = ip;
            this.Port = port;
            this.ConsumerSystemId = id;
        }

        public void SetCloudSettings(string cloudName, string cloudOperator)
        {
            this.CloudOperator = cloudOperator;
            this.CloudName = cloudName;
        }

        public void SetCertPath(string certPath)
        {
            this.CertificatePath = certPath;
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

        public bool getSSL()
        {
            return ssl;
        }
    }
}
