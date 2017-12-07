using RTR.IT.AS.Core.Entities.App.Base;
using System.Collections.Generic;

namespace RTR.IT.AS.Api.Extensions
{
    internal static class UtilExtensions
    {
        internal static List<T> ItemToList<T>(this T item) where T : BaseDomain
        {
            return new List<T> { item };
        }
    }
}