using RTR.IT.AS.Core.Entities.App.Base;
using System;
using System.Xml.Serialization;

namespace RTR.IT.AS.Core.Entities.App
{
    public class RefreshToken : BaseDomain
    {
        [XmlAttribute]
        public string RefreshTokenId { get; set; }
        [XmlAttribute]
        public string Subject { get; set; }
        [XmlAttribute]
        public string ClientId { get; set; }
        [XmlAttribute]
        public DateTime IssuedUtc { get; set; }
        [XmlAttribute]
        public DateTime ExpiresUtc { get; set; }
        [XmlAttribute]
        public string ProtectedTicket { get; set; }
    }
}
