using AppCore.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WissAppEF.Contexts;
using WissAppEntities.Entities;
using WissAppWebApi.Configs;

namespace WissAppWebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            using (var db = new WissAppContext())
            {
                using (var userService = new Service<Users>(db))
                {
                    var user = userService.GetEntity(e => e.UserName == context.UserName && e.Password == context.Password && e.IsActive == true);
                    if (user != null)
                    {
                        UserConfig.RemoveLoggedOutUser(user.UserName);
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("user", user.UserName));
                        identity.AddClaim(new Claim("role", user.Roles.Name));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "User name or password is incorrect.");
                    }
                }
            }
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            var accessToken = context.AccessToken;
            return Task.FromResult<object>(null);
        }
    }
}