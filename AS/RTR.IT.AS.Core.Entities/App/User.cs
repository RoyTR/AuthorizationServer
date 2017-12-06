using RTR.IT.AS.Core.Entities.App.Base;

namespace RTR.IT.AS.Core.Entities.App
{
    public class User : BaseDomain
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NombreUsuario { get; set; }
        public string UrlFoto { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
