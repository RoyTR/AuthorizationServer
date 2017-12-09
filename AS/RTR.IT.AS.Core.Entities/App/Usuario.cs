using RTR.IT.AS.Core.Entities.App.Base;
using System.Collections.Generic;

namespace RTR.IT.AS.Core.Entities.App
{
    public class Usuario : BaseDomain
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreUsuario { get; set; }
        public string UrlFoto { get; set; }
        public string CorreoElectronico { get; set; }

        public List<Tarea> Tareas { get; set; }
    }
}
