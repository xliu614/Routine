namespace Routine.Api.DtoParems
{
    public class CompanyDtoParameters
    {
        private const int MaxPageSize = 20;
        public string? CompanyName { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;
        public string OrderBy { get; set; } = "CompanyName";
        public int PageSize { 
            get => _pageSize;
            set {
                //if (value > MaxPageSize)
                //    _pageSize = MaxPageSize;
                //else
                //    _pageSize = value;
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        } 

    }
}
