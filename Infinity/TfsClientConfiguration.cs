using Infinity.Util;
using System;
using System.Net;

namespace Infinity
{
    public enum AuthenticationType
    {
        Basic = 0,
        OAuth
    }

    /// <summary>
    /// Configuration for a <see cref="TfsClient"/> instance.
    /// </summary>
    public class TfsClientConfiguration
    {

        private readonly AuthenticationType authenticationType;
        private readonly string accessToken;
        private readonly NetworkCredential credentials;

        public AuthenticationType AuthenticationType
        {
            get
            {
                return this.authenticationType;
            }
        }
 
        /// <summary>
        /// Configuration for a <see cref="TfsClient"/> instance.
        /// </summary>
        public TfsClientConfiguration(Uri url, string userAgent, string username, string password)
            : this(url, userAgent, username, password, null)
        {

        }

        public TfsClientConfiguration(Uri url, string userAgent, string username, string password, string domain)
        {
            Assert.NotNullOrEmpty(username, "username");
            Assert.NotNullOrEmpty(password, "password");

            this.Url = url;
            this.credentials = domain == null ? 
                new NetworkCredential(username, password) : new NetworkCredential(username, password, domain);
            this.UserAgent = userAgent;
            this.authenticationType = Infinity.AuthenticationType.Basic;
        }

        public TfsClientConfiguration(Uri url, string userAgent, string accessToken)
        {
            Assert.NotNullOrEmpty(accessToken, "accessToken");

            this.Url = url;
            this.UserAgent = UserAgent;
            this.accessToken = accessToken;
        }

        /// <summary>
        /// The URI of the Project Collection to connect to.
        /// 
        /// For Visual Studio Online, this will look like
        /// <code>https://accountname.visualstudio.com/DefaultCollection</code>.
        /// For on-premises Team Foundation Server instances, this will look
        /// like <code>http://tfsserver:8080/tfs/DefaultCollection</code>.
        /// </summary>
        public Uri Url
        {
            get;
            set;
        }

        
        /// <summary>
        /// The credentials to authenticate with.  For Visual Studio Online,
        /// this should be the username of the "alternate credentials"
        /// that you have configured for your account.  For on-premises
        /// servers, you may specify credentials or use default.
        /// </summary>
        public NetworkCredential Credentials
        {
            get
            {
                if (this.AuthenticationType != Infinity.AuthenticationType.Basic)
                    throw new InvalidOperationException("Not configured to use basic authentication");

                return this.credentials;
            }
        }

        
        public string AccessToken
        {
            get
            {
                if (this.AuthenticationType != Infinity.AuthenticationType.OAuth)
                    throw new InvalidOperationException("Not configured to use OAuth access token");

                return this.accessToken;
            }
        }

        /// <summary>
        /// The user-agent string to include on the HTTP request.
        /// </summary>
        public string UserAgent
        {
            get;
            set;
        }
    }
}