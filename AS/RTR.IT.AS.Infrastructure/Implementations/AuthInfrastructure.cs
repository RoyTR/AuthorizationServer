using RTR.IT.AS.Core.Contracts.Infrastructure;
using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Mappers;
using System;
using System.Linq;

namespace RTR.IT.AS.Infrastructure.Implementations
{
    public class AuthInfrastructure : IAuthInfrastructure
    {
        private readonly SqlServerAccess<RefreshToken> dataAccess;

        public AuthInfrastructure(SqlServerAccess<RefreshToken> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Client GetClient(Filter filter)
        {
            Client result;

            try
            {
                dataAccess.AddParameter("clientId", filter.ClientId);
                result = dataAccess.ExcecuteProcedure("usp_sel_clients", ClientMapper.GetClients).Data.FirstOrDefault();
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public RefreshToken GetRefreshToken(Filter filter)
        {
            RefreshToken result;

            try
            {
                dataAccess.AddParameter("refreshTokenId", filter.RefreshTokenId);
                result = dataAccess.ExcecuteProcedure("usp_sel_refreshtokens", RefreshTokenMapper.GetRefreshTokens).Data.FirstOrDefault();
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public bool AddRefreshTokens(string entities)
        {
            var result = false;

            try
            {
                dataAccess.AddParameter("xmlRefreshTokens", entities);
                dataAccess.ExcecuteProcedure("usp_ins_refreshtokens");
                result = true;
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public bool RemoveRefreshToken(Filter filter)
        {
            var result = false;

            try
            {
                dataAccess.AddParameter("refreshTokenId", filter.RefreshTokenId);
                dataAccess.ExcecuteProcedure("usp_del_refreshtoken");
                result = true;
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public bool RemoveExpiredRefreshTokens()
        {
            var result = false;

            try
            {
                dataAccess.AddParameter("param", "");
                dataAccess.ExcecuteProcedure("usp_del_expiredrefreshtokens");
                result = true;
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public void Dispose()
        {
            if (dataAccess != null)
                dataAccess.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
