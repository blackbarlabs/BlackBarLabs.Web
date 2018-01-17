﻿
namespace EastFive.Web
{
    [Config]
    public class AppSettings
    {
        [ConfigKeyAttribute("Forces the token to invalidated and a 401 returned if it was generated before a certain time", DeploymentOverrides.Suggested,
                Location = "This is should be date time of the WebUI client publish",
                DeploymentSecurityConcern = false,
                PrivateRepositoryOnly = false,
            MoreInfo = "This may be necessary if the server is trying to force the client to refresh their browser.")]
        public const string TokenForceRefreshTime = "EastFive.Web.TokenForceRefreshTime";

        public const string TokenForceRefreshMessage = "EastFive.Web.TokenForceRefreshMessage";

        [Config]
        public static class KeyVault
        {
            [ConfigKeyAttribute("Location of the KeyVault", DeploymentOverrides.Suggested,
                Location ="Azure Portal -> KeyValue -> Settings -> (I'm making this up)",
                DeploymentSecurityConcern = true)]
            public const string Url = "KeyVault.Url";
            [ConfigKeyAttribute("The KeyValue at the url is multitennant so the client id specifies the tennant",
                DeploymentOverrides.Suggested,
                DeploymentSecurityConcern = true)]
            public const string ClientId = "KeyVault.ClientId";
            [ConfigKeyAttribute("Used by the application to authenticate to the KeyVault",
                DeploymentOverrides.Suggested,
                DeploymentSecurityConcern = true)]
            public const string ClientSecret = "KeyVault.ClientSecret";
        }
    }
}