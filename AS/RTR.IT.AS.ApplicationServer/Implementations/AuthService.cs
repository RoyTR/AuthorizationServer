using RTR.IT.AS.ApplicationServer.Extensions;
using RTR.IT.AS.Core.Contracts.Infrastructure;
using RTR.IT.AS.Core.Contracts.Service;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;
using System.Collections.Generic;

namespace RTR.IT.AS.ApplicationServer.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthInfrastructure authInfrastructure;

        public AuthService(IAuthInfrastructure authInfrastructure)
        {
            this.authInfrastructure = authInfrastructure;
        }

        public Client GetClient(Filter filter)
        {
            Client result = null;

            try
            {
                result = authInfrastructure.GetClient(filter);
            }
            finally
            {
                authInfrastructure.Dispose();
            }

            return result;
        }

        public RefreshToken GetRefreshToken(Filter filter)
        {
            RefreshToken result = null;

            try
            {
                result = authInfrastructure.GetRefreshToken(filter);
            }
            finally
            {
                authInfrastructure.Dispose();
            }

            return result;
        }

        public bool AddRefreshTokens(List<RefreshToken> entities)
        {
            var result = false;

            try
            {
                String xml = entities.SerializarListToXml();
                result = authInfrastructure.AddRefreshTokens(xml);
            }
            finally
            {
                authInfrastructure.Dispose();
            }

            return result;
        }

        public bool RemoveRefreshToken(Filter filter)
        {
            var result = false;

            try
            {
                result = authInfrastructure.RemoveRefreshToken(filter);
            }
            finally
            {
                authInfrastructure.Dispose();
            }

            return result;
        }

        public void Dispose()
        {
            if (authInfrastructure != null)
                authInfrastructure.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
