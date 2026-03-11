using ClassWork_WEBAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookEntity>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Title).IsRequired().HasMaxLength(200);
                e.Property(b => b.Image).HasMaxLength(250);
                e.Property(b => b.Pages).HasDefaultValue(0);
                e.Property(b => b.Description).HasColumnType("text");
                e.Property(b => b.Rating).HasDefaultValue(0f);

            });
            modelBuilder.Entity<AuthorEntity>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Name).IsRequired().HasMaxLength(200);
                e.Property(a => a.Image).HasMaxLength(250);
                e.Property(a => a.Country).HasMaxLength(50);
            });
            modelBuilder.Entity<GenreEntity>(e =>
            {
                e.HasKey(g => g.Id);
                e.HasIndex(g => g.Name).IsUnique();
                e.Property(g => g.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<BookEntity>(e =>
            {
                e.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId).OnDelete(DeleteBehavior.SetNull);

                e.HasMany(b => b.Genres).WithMany(g => g.Books).UsingEntity("BookGenres");
            });
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
