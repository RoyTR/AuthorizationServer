using RTR.IT.AS.Core.Entities.App.Base;
using System.Collections.Generic;

namespace RTR.IT.AS.Core.Entities.Api.Result
{
    public class BaseResult<T> where T : BaseDomain
    {
        public int Number { get; set; }
        public List<T> Data { get; set; }
    }
}
