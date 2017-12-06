using RTR.IT.AS.Core.Entities.Api.Result;
using RTR.IT.AS.Core.Entities.App.Base;
using System;
using System.Collections.Generic;

namespace RTR.IT.AS.Infrastructure.Extensions
{
    internal static class BaseResultExtension
    {
        internal static BaseResult<T> ConvertirListToResult<T>(this List<T> listaParametro, Int32 number) where T : BaseDomain
        {
            return new BaseResult<T> { Data = listaParametro, Number = number };
        }
    }
}
