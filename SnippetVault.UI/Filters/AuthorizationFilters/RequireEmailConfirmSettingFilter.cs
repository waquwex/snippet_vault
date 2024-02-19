using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace SnippetVault.UI.Filters.AuthorizationFilters
{
    public class RequireEmailConfirmSettingFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public RequireEmailConfirmSettingFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_configuration.GetValue<bool>("EmailConfirmRequired"))
            {
                context.Result = new LocalRedirectResult("/");
            }
        }
    }
}
