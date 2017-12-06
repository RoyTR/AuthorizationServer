using RTR.IT.AS.Core.Entities.App.Base;
using RTR.IT.AS.Core.Entities.Enums;

namespace RTR.IT.AS.Core.Entities.App
{
    public class Client : BaseDomain
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
