namespace Reakt.Application.Contracts.Common
{
    public class QueryFilter
    {
        public bool Ascending { get; set; }
        public int EndRange { get; set; }
        public string OrderBy { get; set; }
        public int StartRange { get; set; }
    }
}