using RTR.IT.AS.Core.Contracts.Infrastructure;
using RTR.IT.AS.Core.Contracts.Service;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;
using System.Collections.Generic;

namespace RTR.IT.AS.ApplicationServer.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountInfrastructure accountInfrastructure;

        public AccountService(IAccountInfrastructure accountInfrastructure)
        {
            this.accountInfrastructure = accountInfrastructure;
        }

        public Usuario GetUserAccount(Filter filter)
        {
            Usuario result;

            try
            {
                result = accountInfrastructure.GetUserAccount(filter);
            }
            finally
            {
                accountInfrastructure.Dispose();
            }

            return result;
        }

        public List<Tarea> GetUserTasks(Filter filter)
        {
            List<Tarea> result;

            try
            {
                result = accountInfrastructure.GetUserTasks(filter);
            }
            finally
            {
                accountInfrastructure.Dispose();
            }

            return result;
        }

        public void Dispose()
        {
            if (accountInfrastructure != null)
                accountInfrastructure.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
