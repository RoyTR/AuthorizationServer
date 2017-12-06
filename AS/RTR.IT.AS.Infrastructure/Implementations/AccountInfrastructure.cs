using RTR.IT.AS.Core.Contracts.Infrastructure;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Mappers;
using System;
using System.Linq;

namespace RTR.IT.AS.Infrastructure.Implementations
{
    public class AccountInfrastructure : IAccountInfrastructure
    {
        private readonly SqlServerAccess<User> dataAccess;

        public AccountInfrastructure(SqlServerAccess<User> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public User GetUserAccount(Filter filter)
        {
            User result;

            try
            {
                dataAccess.AddParameter("user", filter.User);
                dataAccess.AddParameter("password", filter.Password);
                result = dataAccess.ExcecuteProcedure("usp_sel_userlogin", UserMapper.GetUsers).Data.FirstOrDefault();
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
