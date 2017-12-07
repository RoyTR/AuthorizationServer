using System;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using RTR.IT.AS.Api.Providers;
using RTR.IT.AS.Api.Jobs.Scheduler;

[assembly: OwinStartup(typeof(RTR.IT.AS.Api.Startup))]

namespace RTR.IT.AS.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuthTokenGeneration(app);
            JobScheduler.Start();
        }

        public void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new OAuthAuthorizarionServerProvider(),
                RefreshTokenProvider = new OAuthRefreshTokenProvider(),
                AccessTokenFormat = new AppJwtFormat(System.Web.HttpContext.Current.Request.Url.AbsoluteUri)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
