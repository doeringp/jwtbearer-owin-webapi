using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace LegacyWebApi.Controllers
{
    [ScopeAuthorize("api")]
    public class IdentityController : ApiController
    {
        public IHttpActionResult Get()
        {
            var cp = User as ClaimsPrincipal;
            var claims = cp.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}