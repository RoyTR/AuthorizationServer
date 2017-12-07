using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace RTR.IT.AS.Infrastructure.Mappers
{
    internal static class UsuarioMapper
    {
        internal static BaseResult<Usuario> GetUsers(IDataReader reader)
        {
            List<Usuario> result = new List<Usuario>();

            while (reader.Read())
            {
                var user = new Usuario
                {
                    Id = (int)reader["Id"],
                    Nombres = reader["Nombres"] != DBNull.Value ? reader["Nombres"].ToString() : null,
                    ApellidoPaterno = reader["ApellidoPaterno"] != DBNull.Value ? reader["ApellidoPaterno"].ToString() : null,
                    ApellidoMaterno = reader["ApellidoMaterno"] != DBNull.Value ? reader["ApellidoMaterno"].ToString() : null,
                    NombreUsuario = reader["NombreUsuario"] != DBNull.Value ? reader["NombreUsuario"].ToString() : null,
                    UrlFoto = reader["UrlFoto"] != DBNull.Value ? reader["UrlFoto"].ToString() : null,
                    CorreoElectronico = reader["CorreoElectronico"] != DBNull.Value ? reader["CorreoElectronico"].ToString() : null
                };
                result.Add(user);
            }

            return result.ConvertirListToResult(result.Count);
        }
    }
}
