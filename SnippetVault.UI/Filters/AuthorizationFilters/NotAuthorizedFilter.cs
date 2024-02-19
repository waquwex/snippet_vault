using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SnippetVault.UI.Filters.AuthorizationFilters
{
    public class NotAuthorizedFilter : IAuthorizationFilter
    {
        private readonly string _redirectUrl;

        public NotAuthorizedFilter(string redirectUrl = "/")
        {
            _redirectUrl = redirectUrl;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                context.Result = new LocalRedirectResult(_redirectUrl);
            }
        }
    }
}
