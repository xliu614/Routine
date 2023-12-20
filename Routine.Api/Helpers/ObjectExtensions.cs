using System.Dynamic;
using System.Reflection;

namespace Routine.Api.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields) {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var extandoObj = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, Object>)extandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            else {
                var fieldsAftersplit = fields.Split(',');
                foreach (var field in fieldsAftersplit) {
                    var propName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null) {
                        throw new Exception($"At {typeof(TSource)} there's no {propName} found");
                    }
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)extandoObj).Add(propertyInfo.Name, propertyValue);
                }            
            }

            return extandoObj;
        }
    }
}
