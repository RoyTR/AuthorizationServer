using Microsoft.Owin.Security.Infrastructure;
using RTR.IT.AS.Api.Extensions;
using RTR.IT.AS.ApplicationServer.Implementations;
using RTR.IT.AS.Core.Contracts.Service;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Implementations;
using System;
using System.Threading.Tasks;

namespace RTR.IT.AS.Api.Providers
{
    public class OAuthRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (IAuthService authService = new AuthService(new AuthInfrastructure(new SqlServerAccess<RefreshToken>("ASConnectionString"))))
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    RefreshTokenId = refreshTokenId,
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = authService.AddRefreshTokens(token.ItemToList());

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
        }

        public async void Create(AuthenticationTokenCreateContext context)
        {
            return;
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = context.Token;

            using (IAuthService authService = new AuthService(new AuthInfrastructure(new SqlServerAccess<RefreshToken>("ASConnectionString"))))
            {
                var refreshToken = authService.GetRefreshToken(new Filter { RefreshTokenId = hashedTokenId });

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = authService.RemoveRefreshToken(new Filter { RefreshTokenId = hashedTokenId });
                }
            }
        }

        public async void Receive(AuthenticationTokenReceiveContext context)
        {
            return;
        }
    }
}