using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Enums;
using RTR.IT.AS.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace RTR.IT.AS.Infrastructure.Mappers
{
    internal static class ClientMapper
    {
        internal static BaseResult<Client> GetClients(IDataReader reader)
        {
            List<Client> result = new List<Client>();

            while (reader.Read())
            {
                var client = new Client
                {
                    ClientId = reader["Id"] != DBNull.Value ? reader["Id"].ToString() : "",
                    Secret = reader["Secret"] != DBNull.Value ? reader["Secret"].ToString() : "",
                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : "",
                    ApplicationType = reader["ApplicationType"] != DBNull.Value ?
                        (Int32)reader["ApplicationType"] == 0 ? ApplicationTypes.JavaScript : ApplicationTypes.NativeConfidential :
                        0,
                    Active = reader["Active"] != DBNull.Value ? (bool)reader["Active"] : false,
                    RefreshTokenLifeTime = reader["RefreshTokenLifeTime"] != DBNull.Value ? (Int32)reader["RefreshTokenLifeTime"] : 0,
                    AllowedOrigin = reader["AllowedOrigin"] != DBNull.Value ? reader["AllowedOrigin"].ToString() : ""
                };
                result.Add(client);
            }

            return result.ConvertirListToResult(result.Count);
        }
    }
}
