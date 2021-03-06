﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reakt.Application.Persistence.Models;

namespace Reakt.Persistance.Configurations
{
    public class PostsConfiguration : IEntityTypeConfiguration<Post>
    {
        void IEntityTypeConfiguration<Post>.Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(e => e.Id)
                .UseIdentityColumn();
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(600);

            builder.HasQueryFilter(x => x.Active);
        }
    }
}