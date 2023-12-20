namespace Routine.Api.Services
{
    public interface IPropertyMappingService
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        public bool ValidMappingExistsFor<TSource, TDestination>(string inputFields);
    }
}
