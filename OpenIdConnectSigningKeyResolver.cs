using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;

namespace LegacyWebApi
{
    public class OpenIdConnectSigningKeyResolver
    {
        private readonly OpenIdConnectConfiguration _openIdConfig;

        public OpenIdConnectSigningKeyResolver(string authority)
        {
            var cm = new ConfigurationManager<OpenIdConnectConfiguration>($"{authority.TrimEnd('/')}/.well-known/openid-configuration");
            _openIdConfig = AsyncHelper.RunSync(async () => await cm.GetConfigurationAsync());
        }

        public SecurityKey GetSigningKey(SecurityKeyIdentifier kid)
        {
            // Find the security token which matches the identifier
            foreach(SecurityToken securityToken in _openIdConfig.JsonWebKeySet.GetSigningTokens())
            {
                foreach(SecurityKeyIdentifierClause keyIdClause in kid)
                {
                    if (securityToken.MatchesKeyIdentifierClause(keyIdClause))
                    {
                        return securityToken.ResolveKeyIdentifierClause(keyIdClause);
                    }
                }
            }
            return null;
        }
    }
}