using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static string Aggregate(this IEnumerable<int> items)
        {
            if (items != null && items.Any())
            {
                return items.Aggregate("*", (current, item) => current + $"{item}*");
            }

            return "";
        }
    }
}
