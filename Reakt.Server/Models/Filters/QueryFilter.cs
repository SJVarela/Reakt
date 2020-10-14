using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Reakt.Server.Models.Filters
{
    /// <summary>
    /// Common filter for list queries
    /// </summary>
    public class QueryFilter : IValidatableObject
    {
        /// <summary>
        /// Ordered by ascending or descending
        /// </summary>
        [DefaultValue(false)]
        public bool Ascending { get; set; } = false;

        /// <summary>
        /// Ending item position
        /// </summary>
        [DefaultValue(50)]
        public int EndRange { get; set; } = 50;

        /// <summary>
        /// Field to order by
        /// </summary>
        [DefaultValue(null)]
        public string OrderBy { get; set; } = null;

        /// <summary>
        /// Starting item position
        /// </summary>
        [DefaultValue(0)]
        public int StartRange { get; set; } = 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (StartRange > EndRange)
            {
                results.Add(new ValidationResult($"{nameof(StartRange)} cannot be bigger than {nameof(EndRange)}"));
            }
            return results;
        }
    }
}