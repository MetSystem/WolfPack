using System.Collections.Generic;

namespace Wolfpack.Periscope.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static IDictionary<string, object> AddFromAnonymous(this IDictionary<string, object> target,
            object anonymous)
        {
            foreach (var propertyInfo in anonymous.GetType().GetProperties())
                target.Add(propertyInfo.Name, propertyInfo.GetValue(anonymous, null));
            return target;
        }
    }
}