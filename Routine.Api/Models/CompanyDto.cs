namespace Routine.Api.Models
{
    /// <summary>
    /// this is used for display or response about a request on company
    /// this is an output dto
    /// </summary>
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
    }
}
