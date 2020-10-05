namespace Reakt.Application.Persistence
{
    public abstract class BaseEntity
    {
        public bool Active { get; set; }
        public long Id { get; set; }
    }
}