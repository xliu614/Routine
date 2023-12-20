using System.Reflection;
namespace Routine.Api.Services
{
    public class PropertyCheckerService:IPropertyCheckerService
    {
        public bool TypeHasProperties<T>(string? fields) {
            if (string.IsNullOrWhiteSpace(fields)) return true;
            var properties = fields.Split(',');
            foreach (string prop in properties)
            {
                var trimmedPropName = prop.Trim();                
                var propertyInfo = typeof(T).GetProperty(trimmedPropName,BindingFlags.IgnoreCase|BindingFlags.Public|BindingFlags.Instance);
                if (propertyInfo == null)
                    return false;
            }
            return true;
        }
    }
}
