using Routine.Api.Services;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace Routine.Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (mappingDictionary == null) throw new ArgumentNullException(nameof(mappingDictionary));
            if (string.IsNullOrWhiteSpace(orderBy)) return source;

            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit.Reverse()) {
                var trimmedOrderBy = orderByClause.Trim();

                var orderDesc = trimmedOrderBy.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderBy.IndexOf(" ", StringComparison.OrdinalIgnoreCase);

                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderBy : trimmedOrderBy.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName)) {
                    throw new ArgumentException($"Cannot find the mapping with the key as {propertyName}");
                }
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null) {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                foreach (var destination in propertyMappingValue.DestinationProperties.Reverse()) {
                    if (propertyMappingValue.Revert) {
                        orderDesc = !orderDesc;
                    }
                    source = source.OrderBy(destination + (orderDesc ? " descending" : " ascending"));
                    
                }
                
            }
            return source;
        }
    }
}
