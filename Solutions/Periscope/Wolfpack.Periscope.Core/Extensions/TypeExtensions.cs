using System;

namespace Wolfpack.Periscope.Core.Extensions
{
    public static class TypeExtensions
    {
        public static string BuildTypeName(this Type target)
        {
            return string.Format("{0}, {1}", target.FullName, target.Assembly.GetName().Name);
        }
    }
}