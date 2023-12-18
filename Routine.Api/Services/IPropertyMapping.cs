namespace Routine.Api.Services
{
    public interface IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
