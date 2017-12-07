using Quartz;
using RTR.IT.AS.ApplicationServer.Implementations;
using RTR.IT.AS.Core.Contracts.Service;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Implementations;
using System.Data.SqlClient;

namespace RTR.IT.AS.Api.Jobs
{
    public class ExpiredRefreshTokensJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                using (IAuthService authService = new AuthService(new AuthInfrastructure(new SqlServerAccess<RefreshToken>("ASConnectionString"))))
                    authService.RemoveExpiredRefreshTokens();
            }
            catch (SqlException)
            {
                // Avoid exceptions from database
            }
        }
    }
}