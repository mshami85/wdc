using System.Reflection;

namespace StudentServicesPortal.Classes
{
    public static class ObjectHelpers
    {
        public static IEnumerable<KeyValuePair<string, object>> ExtractProperties(this object obj)
        {
            var properties = obj.GetType()
                                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                .Where(p => p.PropertyType == typeof(string) || (!p.PropertyType.IsClass && !p.PropertyType.IsInterface));

            return properties.Where(p => p.GetValue(obj) != null).Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(obj)));
        }
    }
}
