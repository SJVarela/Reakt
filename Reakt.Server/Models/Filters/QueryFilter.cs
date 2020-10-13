using System.ComponentModel;

namespace Reakt.Server.Models.Filters
{
    public class QueryFilter
    {
        [DefaultValue(50)]
        public int EndRange { get; set; } = 50;

        [DefaultValue("id")]
        public string OrderBy { get; set; } = "Id";

        [DefaultValue(0)]
        public int StartRange { get; set; } = 0;
    }
}