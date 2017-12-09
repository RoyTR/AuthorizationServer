using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using RTR.IT.AS.ApplicationServer.Implementations;
using RTR.IT.AS.Core.Contracts.Service;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Constants;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RTR.IT.AS.Api.Providers
{
    public class OAuthAuthorizarionServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Responsible for validating the Client (clientId/secret)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Invalidate context to force sending clientId/secret once obtain access tokens. 
                context.SetError("invalid_client", "client_id should be sent");
                return Task.FromResult<object>(null);
            }

            var empresaIdAux = context.Parameters.FirstOrDefault(x => x.Key == "empresa_id").Value;
            int empresaId;
            if (empresaIdAux == null || empresaIdAux.Length == 0 || !int.TryParse(empresaIdAux.First(), out empresaId))
            {
                context.SetError("invalid_empresa", "empresa_id should be sent");
                return Task.FromResult<object>(null);
            }

            var aplicacionIdAux = context.Parameters.FirstOrDefault(x => x.Key == "aplicacion_id").Value;
            int aplicacionId;
            if (aplicacionIdAux == null || aplicacionIdAux.Length == 0 || !int.TryParse(aplicacionIdAux.First(), out aplicacionId))
            {
                context.SetError("invalid_aplicacion", "aplicacion_id should be sent");
                return Task.FromResult<object>(null);
            }

            using (IAuthService authService = new AuthService(new AuthInfrastructure(new SqlServerAccess<RefreshToken>("ASConnectionString"))))
            {
                client = authService.GetClient(new Core.Entities.Filter.Filter { ClientId = context.ClientId });
            }

            if (client == null)
            {
                context.SetError("invalid_client", string.Format("Client '{0}' is not registered in the system", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == Core.Entities.Enums.ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_secret", "Client secret should be sent");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    //Should compare encrypted clientSecrets
                    if (client.Secret != clientSecret)
                    {
                        context.SetError("invalid_scret", "invalid client secret");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "inactive client");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<int>("as:clientAplicacionId", aplicacionId);
            context.OwinContext.Set<int>("as:clientEmpresaId", empresaId);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Responsible for validating the username and password sent to the authorization server endpoint token(authorization server’s token endpoint)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var aplicacionId = context.OwinContext.Get<int>("as:clientAplicacionId");
            var empresaId = context.OwinContext.Get<int>("as:clientEmpresaId");

            Usuario user;

            using (IAccountService accountService = new AccountService(new AccountInfrastructure(new SqlServerAccess<Usuario>("ASConnectionString"))))
            {
                #region User Authentication
                // Should send encrypted password
                user = accountService.GetUserAccount(new Core.Entities.Filter.Filter { NombreUsuario = context.UserName, Password = context.Password });
                if (user == null || user.Id == 0)
                {
                    context.SetError("invalid_access", "The user has not access to the application.");
                    return;
                }
                #endregion

                #region Get User Tasks
                var tasks = accountService.GetUserTasks(new Core.Entities.Filter.Filter { UsuarioId = user.Id, EmpresaId = empresaId, AplicacionId = aplicacionId });
                if (tasks == null)
                {
                    context.SetError("invalid_access", "The user has not permissions in the application.");
                    return;
                }
                user.Tareas = tasks;
                #endregion
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.NombreUsuario));
            identity.AddClaim(new Claim("sub", user.NombreUsuario));
            //User Info
            identity.AddClaim(new Claim(Constant.ClaimTypes.Usuario.UsuarioId, user.Id.ToString()));
            identity.AddClaim(new Claim(Constant.ClaimTypes.AplicacionId, aplicacionId.ToString()));
            identity.AddClaim(new Claim(Constant.ClaimTypes.EmpresaId, empresaId.ToString()));
            //Tareas
            foreach (var tarea in user.Tareas)
            {
                identity.AddClaim(new Claim(Constant.ClaimTypes.Tarea, tarea.Codigo));
            }

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                },
                {
                    "userName", user.NombreUsuario
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var usuarioClaim = newIdentity.FindFirst(Constant.ClaimTypes.Usuario.UsuarioId);
            var aplicacionClaim = newIdentity.FindFirst(Constant.ClaimTypes.AplicacionId);
            var empresaClaim = newIdentity.FindFirst(Constant.ClaimTypes.EmpresaId);

            if (usuarioClaim == null || aplicacionClaim == null || empresaClaim == null)
            {
                context.SetError("invalid_access", "Invalid token.");
                return Task.FromResult<object>(null);
            }

            #region Update User Tasks
            List<Tarea> tasks;
            //Retrieve tasks
            using (IAccountService accountService = new AccountService(new AccountInfrastructure(new SqlServerAccess<Usuario>("ASConnectionString"))))
            {
                tasks = accountService.GetUserTasks(new Core.Entities.Filter.Filter { UsuarioId = int.Parse(usuarioClaim.Value), EmpresaId = int.Parse(empresaClaim.Value), AplicacionId = int.Parse(aplicacionClaim.Value) });
                if (tasks == null)
                {
                    context.SetError("invalid_access", "The user has not access to the application.");
                    return Task.FromResult<object>(null);
                }
            }
            //Remove previous tasks
            var claimsTareas = newIdentity.Claims;
            foreach (var claim in claimsTareas)
            {
                if (claim.Type == Constant.ClaimTypes.Tarea)
                {
                    newIdentity.RemoveClaim(claim);
                }
            }
            //Add new tasks
            foreach (var task in tasks)
            {
                newIdentity.AddClaim(new Claim(Constant.ClaimTypes.Tarea, task.Codigo));
            }
            #endregion

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}