using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LegacyWebApi
{
    public class ScopeAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string scope;

        public ScopeAuthorizeAttribute(string scope)
        {
            this.scope = scope;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            var validIssuer = ConfigurationManager.AppSettings["Authority"];

            ClaimsPrincipal principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            // Get the scope claim. Ensure that the issuer is IdentityServer.
            var scopeClaim = principal?.Claims.FirstOrDefault(c => c.Type == "scope" && c.Issuer == validIssuer);
            if (scopeClaim != null)
            {
                // Split scopes
                var scopes = scopeClaim.Value.Split(' ');

                // Succeed if the scope array contains the required scope
                if (scopes.Any(s => s == scope))
                    return;
            }

            HandleUnauthorizedRequest(actionContext);
        }
    }
}