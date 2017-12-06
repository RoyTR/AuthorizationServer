using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;

namespace RTR.IT.AS.Core.Contracts.Infrastructure
{
    public interface IAuthInfrastructure : IDisposable
    {
        Client GetClient(Filter filter);
        RefreshToken GetRefreshToken(Filter filter);
        bool AddRefreshTokens(string entities);
        bool RemoveRefreshToken(Filter filter);
    }
}
