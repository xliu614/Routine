namespace Routine.Api.Services
{
    public class PropertyMappingValue
    {
        //FirstName + LastName
        public IEnumerable<string> DestinationProperties { get; set; }
        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false) {
            Revert = revert;
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
        }

    }
}
