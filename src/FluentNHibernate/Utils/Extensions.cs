using System.Linq;

namespace FluentNHibernate.Utils
{
    public static class Extensions
    {
        public static bool In<T>(this T actual, params T[] expectations)
        {
            return expectations.Any(x => x.Equals(actual));
        }
    }
}