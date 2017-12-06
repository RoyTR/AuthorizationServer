using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace RTR.IT.AS.Infrastructure.Mappers
{
    internal static class RefreshTokenMapper
    {
        internal static BaseResult<RefreshToken> GetRefreshTokens(IDataReader reader)
        {
            List<RefreshToken> result = new List<RefreshToken>();

            while (reader.Read())
            {
                var refreshToken = new RefreshToken
                {
                    RefreshTokenId = reader["Id"] != DBNull.Value ? reader["Id"].ToString() : "",
                    Subject = reader["Subject"] != DBNull.Value ? reader["Subject"].ToString() : "",
                    ClientId = reader["ClientId"] != DBNull.Value ? reader["ClientId"].ToString() : "",
                    IssuedUtc = reader["IssuedUtc"] != DBNull.Value ? (DateTime)reader["IssuedUtc"] : DateTime.MinValue,
                    ExpiresUtc = reader["ExpiresUtc"] != DBNull.Value ? (DateTime)reader["ExpiresUtc"] : DateTime.MinValue,
                    ProtectedTicket = reader["ProtectedTicket"] != DBNull.Value ? reader["ProtectedTicket"].ToString() : ""
                };
                result.Add(refreshToken);
            }

            return result.ConvertirListToResult(result.Count);
        }
    }
}
