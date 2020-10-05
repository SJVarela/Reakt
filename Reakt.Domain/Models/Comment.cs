namespace Reakt.Domain.Models
{
    public class Comment : AuditableEntity
    {
        public int Likes { get; set; }
        public string Message { get; set; }
        public Comment Parent { get; set; }
    }
}