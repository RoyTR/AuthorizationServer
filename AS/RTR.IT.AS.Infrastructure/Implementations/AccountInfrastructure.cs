using RTR.IT.AS.Core.Contracts.Infrastructure;
using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using RTR.IT.AS.Infrastructure.Data;
using RTR.IT.AS.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTR.IT.AS.Infrastructure.Implementations
{
    public class AccountInfrastructure : IAccountInfrastructure
    {
        private readonly SqlServerAccess<Usuario> dataAccess;

        public AccountInfrastructure(SqlServerAccess<Usuario> dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Usuario GetUserAccount(Filter filter)
        {
            Usuario result;

            try
            {
                dataAccess.AddParameter("nombreUsuario", filter.NombreUsuario);
                dataAccess.AddParameter("password", filter.Password);
                result = dataAccess.ExcecuteProcedure("usp_sel_userlogin", UsuarioMapper.GetUsers).Data.FirstOrDefault();
            }
            finally
            {
                dataAccess.Dispose();
            }

            return result;
        }

        public List<Tarea> GetUserTasks(Filter filter)
        {
            List<Tarea> result;

            try
            {
                dataAccess.AddParameter("empresaId", filter.EmpresaId);
                dataAccess.AddParameter("aplicacionId", filter.AplicacionId);
                dataAccess.AddParameter("usuarioId", filter.UsuarioId);
                result = dataAccess.ExcecuteProcedure("usp_sel_usuariotareas", UsuarioMapper.GetUserTasks).Data;
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
