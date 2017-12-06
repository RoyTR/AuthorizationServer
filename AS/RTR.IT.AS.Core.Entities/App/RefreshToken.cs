using RTR.IT.AS.Core.Entities.App.Base;
using System;

namespace RTR.IT.AS.Core.Entities.App
{
    public class RefreshToken : BaseDomain
    {
        public string RefreshTokenId { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
