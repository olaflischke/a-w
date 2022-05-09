using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using icg = Iesi.Collections.Generic;

namespace TradingDayDal.Extensions
{
    public static class ExtensionsMethod
    {
        public static icg.HashedSet<T> ToHashedSet<T>(this IEnumerable<T> elements)
        {
            icg.HashedSet<T> set = new icg.HashedSet<T>();

            foreach (var item in elements)
            {
                set.Add(item);
            }

            return set;
        }
    }
}
