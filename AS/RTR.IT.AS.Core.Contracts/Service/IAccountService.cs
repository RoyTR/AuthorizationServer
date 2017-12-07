using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;
using System.Collections.Generic;

namespace RTR.IT.AS.Core.Contracts.Service
{
    public interface IAuthService : IDisposable
    {
        Client GetClient(Filter filter);
        RefreshToken GetRefreshToken(Filter filter);
        bool AddRefreshTokens(List<RefreshToken> jsonRefreshTokens);
        bool RemoveRefreshToken(Filter filter);
        bool RemoveExpiredRefreshTokens();
    }
}
