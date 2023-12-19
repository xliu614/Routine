using Routine.Api.Entities;
using Routine.Api.Models;

namespace Routine.Api.Services
{
    public class PropertyMappingService:IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _companyPropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) {
            {"Id", new PropertyMappingValue(new List<string> {"Id"}) },
            {"CompanyName", new PropertyMappingValue(new List<string> {"Name"}) },
            {"Country", new PropertyMappingValue(new List<string> {"Country" }) },
            {"Industry", new PropertyMappingValue(new List<string> {"Industry" }) },
            {"Product", new PropertyMappingValue(new List<string> {"Product"}) },
            {"Introduction", new PropertyMappingValue(new List<string> {"Intruduction" }) }
        };

        private Dictionary<string, PropertyMappingValue> _employeePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) {
            {"Id", new PropertyMappingValue(new List<string> {"Id"}) },
            {"CompanyId", new PropertyMappingValue(new List<string> {"CompanyId"}) },
            {"EmployeeNo", new PropertyMappingValue(new List<string> {"EmployeeNo" }) },
            {"Name", new PropertyMappingValue(new List<string> {"FirstName", "LastName" }) },
            {"GenderDisplay", new PropertyMappingValue(new List<string> {"Gender"}) },
            {"Age", new PropertyMappingValue(new List<string> {"DateOfBirth" }, revert: true) }
        };

        private IList<IPropertyMapping> _propertyMapping = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMapping.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
            _propertyMapping.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>() {
            var matchingMapping = _propertyMapping.OfType<PropertyMapping<TSource, TDestination>>();
            var propertyMappings = matchingMapping.ToList();

            if (propertyMappings.Count() == 1)
                return matchingMapping.First().MappingDictionary;

            throw new Exception($"Cannot find unique mapping: {typeof(TSource)}, {typeof(TDestination)} ");
        }

    }
}
