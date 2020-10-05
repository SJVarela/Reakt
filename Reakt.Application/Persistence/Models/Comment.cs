namespace Reakt.Application.Persistence.Models
{
    public class Comment : AuditableEntity
    {
        public int Likes { get; set; }
        public string Message { get; set; }
        public virtual Comment Parent { get; set; }
        public long? ParentId { get; set; }
        public long PostId { get; set; }
    }
}