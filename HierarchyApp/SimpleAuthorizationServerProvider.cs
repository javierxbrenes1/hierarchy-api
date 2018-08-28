using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using HierarchyApp.Shared;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;

namespace HierarchyApp
{
    [EnableCors(origins: "*", headers:"*", methods: "*")]
    internal class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var strConnection = ConfigurationManager.ConnectionStrings["DBConection"].ConnectionString;
            var ad = new DataAccess.DataAccess(strConnection);

            var usuario = ad.execSP<UserInfo>("Get_User_Data", new { @UserName=context.UserName, @Password=context.Password});

            if (usuario != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Name));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                     
                });


                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

            }
            else
            {
                context.SetError("Invalid_grant", "provided username and password is incorrect");
                context.Rejected();
            }
           
        }

    }
}