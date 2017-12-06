using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;

namespace RTR.IT.AS.Core.Contracts.Service
{
    public interface IAccountService : IDisposable
    {
        User GetUserAccount(Filter filter);
    }
}
