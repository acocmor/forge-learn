using System;
using System.Threading.Tasks;
using Autodesk.Forge;
using forgeSample.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace forgeSample.Services
{
    public class OAuthService : IOAuthService
    {
        private static dynamic InternalToken { get; set; }
        private static dynamic PublicToken { get; set; }
        
        private readonly ForgeSetting _forgeConfig;

        public OAuthService(IOptionsMonitor<ForgeSetting> forgeConfig)
        {
            _forgeConfig = forgeConfig.CurrentValue;
        }
        
        public async Task<dynamic> GetPublicAsync()
        {
            if (PublicToken == null || PublicToken.ExpiresAt < DateTime.UtcNow)
            {
                PublicToken = await Get2LeggedTokenAsync(new Scope[] { Scope.ViewablesRead });
                PublicToken.ExpiresAt = DateTime.UtcNow.AddSeconds(PublicToken.expires_in);
            }
            return PublicToken;
        }
        
        public async Task<dynamic> GetInternalAsync()
        {
            if (InternalToken == null || InternalToken.ExpiresAt < DateTime.UtcNow)
            {
                InternalToken = await Get2LeggedTokenAsync(new Scope[] { Scope.BucketCreate, Scope.BucketRead, Scope.BucketDelete, Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.CodeAll });
                InternalToken.ExpiresAt = DateTime.UtcNow.AddSeconds(InternalToken.expires_in);
            }
            return InternalToken;
        }

        public string GetSettings(string setting)
        {
            return setting switch
            {
                "FORGE_CLIENT_ID" => _forgeConfig.ForgeClientId,
                "FORGE_CLIENT_SECRET" => _forgeConfig.ForgeClientSecret,
                "FORGE_CLIENT_CALLBACK" => _forgeConfig.ForgeCallBack,
                _ => null
            };
        }

        private async Task<dynamic> Get2LeggedTokenAsync(Scope[] scopes)
        {
            var oauth = new TwoLeggedApi();
            var grantType = "client_credentials";
            var bearer = await oauth.AuthenticateAsync(
                GetSettings("FORGE_CLIENT_ID"),
                GetSettings("FORGE_CLIENT_SECRET"),
                grantType,
                scopes);
            return bearer;
        }
    }
}