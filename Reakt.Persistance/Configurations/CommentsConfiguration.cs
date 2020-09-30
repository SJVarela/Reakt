using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reakt.Application.Persistence.Models;


namespace Reakt.Persistance.Configurations
{
    public class CommentsConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(e => e.Message)
                .IsRequired()                
                .HasMaxLength(4000);
        }
    }
}
