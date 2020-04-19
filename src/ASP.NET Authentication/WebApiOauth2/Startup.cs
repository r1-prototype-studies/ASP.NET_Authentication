using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiOauth2.Startup))]

namespace WebApiOauth2
{
    public class Startup
    {

        #region Public /Protected Properties.  

        /// <summary>  
        /// OAUTH options property.  
        /// </summary>  
        public static Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>  
        /// Public client ID property.  
        /// </summary>  
        public static string PublicClientId { get; private set; }

        #endregion

        public void Configuration(IAppBuilder app)
        {

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            // Enable the application to use a cookie to store information for the signed in user  
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider  
            // Configure the sign in cookie  
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5.0),
            });

            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            //https://docs.microsoft.com/en-us/previous-versions/aspnet/dn308223(v=vs.113)?redirectedfrom=MSDN
            // Configure the application for OAuth based flow  
            PublicClientId = "self";
            OAuthOptions = new Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"), // This is the path which will be called in order to authorize the user credentials and in return it will return the generated access token.
                Provider = new helper.AppOAuthProvider(PublicClientId), // This Class should be implemented and it will verify the user credential and create identity claims in order to return the generated access token.
                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"), // This path can be updated to external logins to get user consent that is required to generate access token.
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(4), // This is the time period during which the access token is accessible. The shorter time span is recommended for sensitive API(s).
                AllowInsecureHttp = true //Don't do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!  
            };

            // Enable the application to use bearer tokens to authenticate users  
            app.UseOAuthBearerTokens(OAuthOptions);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.  
            app.UseTwoFactorSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Uncomment the following lines to enable logging in with third party login providers  
            //app.UseMicrosoftAccountAuthentication(  
            //    clientId: "",  
            //    clientSecret: "");  

            //app.UseTwitterAuthentication(  
            //   consumerKey: "",  
            //   consumerSecret: "");  

            //app.UseFacebookAuthentication(  
            //   appId: "",  
            //   appSecret: "");  

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()  
            //{  
            //    ClientId = "",  
            //    ClientSecret = ""  
            //}); 
        }
    }
}
