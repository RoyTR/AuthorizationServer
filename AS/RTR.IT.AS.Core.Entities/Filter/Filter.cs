using RTR.IT.AS.Core.Entities.App.Base;

namespace RTR.IT.AS.Core.Entities.Filter
{
    public class Filter : BaseDomain
    {
        public string User { get; set; }
        public string Password { get; set; }

        public string ClientId { get; set; }
        public string RefreshTokenId { get; set; }
    }
}
