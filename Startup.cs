//#define IdentityServer3
#if IdentityServer3
using IdentityServer3.AccessTokenValidation;
#endif
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(LegacyWebApi.Startup))]
namespace LegacyWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string authority = ConfigurationManager.AppSettings["Authority"];

#if IdentityServer3
            // XXX: Not working with IdentityServer4 because it validates the audience claim for 
            //  the URI ~/resources (that was removed in IdentityServer4 version >= 3.0)
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = authority,
                RequiredScopes = new[] { "api" },
            });
#endif

            #region IdentityServer4
            var keyResolver = new OpenIdConnectSigningKeyResolver(authority);
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authority,
                    ValidateAudience = false, // Don't validate the audience. Validate the scopes instead.
                    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => keyResolver.GetSigningKey(kid)
                }
            });
            #endregion
        }
    }
}
