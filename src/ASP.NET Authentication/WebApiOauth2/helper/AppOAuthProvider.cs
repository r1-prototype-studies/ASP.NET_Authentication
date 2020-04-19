using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace WebApiOauth2.helper
{
    /// <summary>
    /// Application OAuth class provider
    /// </summary>
    public class AppOAuthProvider : Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        private databaseEntities.DB_Oauth_APIEntities databaseManager = new databaseEntities.DB_Oauth_APIEntities();

        public AppOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }
            _publicClientId = publicClientId;
        }

        //
        // Summary:
        //     Called when a request to the Token endpoint arrives with a "grant_type" of "password".
        //     This occurs when the user has provided name and password credentials directly
        //     into the client application's user interface, and the client application is using
        //     those to acquire an "access_token" and optional "refresh_token". If the web application
        //     supports the resource owner credentials grant type it must validate the context.Username
        //     and context.Password as appropriate. To issue an access token the context.Validated
        //     must be called with a new ticket containing the claims about the resource owner
        //     which should be associated with the access token. The application should take
        //     appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        //     The default behavior is to reject this grant type. See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        //
        // Parameters:
        //   context:
        //     The context of the event carries information in and results out.
        //
        // Returns:
        //     Task to enable asynchronous execution
        public override async Task GrantResourceOwnerCredentials(Microsoft.Owin.Security.OAuth.OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            var user = databaseManager.LoginByUsernamePassword(userName, password).ToList();

            if (user == null || user.Count() <= 0)
            {
                context.SetError("invalid_grant", "The user name and password is incorrect");
                return;
            }

            var claims = new List<System.Security.Claims.Claim>();
            var userInfo = user.FirstOrDefault();

            claims.Add(new System.Security.Claims.Claim(
                            System.Security.Claims.ClaimTypes.Name, userInfo.username));

            // Setting claim identities for OAuth2
            var oAuthClaimIdentity = new System.Security.Claims.ClaimsIdentity(claims, Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType);
            var cookiesClaimIdentity = new System.Security.Claims.ClaimsIdentity(claims, Microsoft.Owin.Security.Cookies.CookieAuthenticationDefaults.AuthenticationType);

            // Setting user authentication
            var properties = CreateProperties(userInfo.username);
            var ticket = new Microsoft.Owin.Security.AuthenticationTicket(oAuthClaimIdentity, properties);

            // Grant access to user
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesClaimIdentity);

        }

        public static Microsoft.Owin.Security.AuthenticationProperties CreateProperties(string userName)
        {
            var data = new Dictionary<string, string> {
                {"userName", userName }
            };

            return new Microsoft.Owin.Security.AuthenticationProperties(data);
        }

        //
        // Summary:
        //     Called to validate that the context.ClientId is a registered "client_id", and
        //     that the context.RedirectUri a "redirect_uri" registered for that client. This
        //     only occurs when processing the Authorize endpoint. The application MUST implement
        //     this call, and it MUST validate both of those factors before calling context.Validated.
        //     If the context.Validated method is called with a given redirectUri parameter,
        //     then IsValidated will only become true if the incoming redirect URI matches the
        //     given redirect URI. If context.Validated is not called the request will not proceed
        //     further.
        //
        // Parameters:
        //   context:
        //     The context of the event carries information in and results out.
        //
        // Returns:
        //     Task to enable asynchronous execution
        public override Task ValidateClientRedirectUri(Microsoft.Owin.Security.OAuth.OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId.Equals(_publicClientId))
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri.Equals(context.RedirectUri))
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Called to validate that the origin of the request is a registered "client_id",
        //     and that the correct credentials for that client are present on the request.
        //     If the web application accepts Basic authentication credentials, context.TryGetBasicCredentials(out
        //     clientId, out clientSecret) may be called to acquire those values if present
        //     in the request header. If the web application accepts "client_id" and "client_secret"
        //     as form encoded POST parameters, context.TryGetFormCredentials(out clientId,
        //     out clientSecret) may be called to acquire those values if present in the request
        //     body. If context.Validated is not called the request will not proceed further.
        //
        // Parameters:
        //   context:
        //     The context of the event carries information in and results out.
        //
        // Returns:
        //     Task to enable asynchronous execution
        public override Task ValidateClientAuthentication(Microsoft.Owin.Security.OAuth.OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        //
        // Summary:
        //     Called at the final stage of a successful Token endpoint request. An application
        //     may implement this call in order to do any final modification of the claims being
        //     used to issue access or refresh tokens. This call may also be used in order to
        //     add additional response parameters to the Token endpoint's json response body.
        //
        // Parameters:
        //   context:
        //     The context of the event carries information in and results out.
        //
        // Returns:
        //     Task to enable asynchronous execution
        public override Task TokenEndpoint(Microsoft.Owin.Security.OAuth.OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}