using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL
{
    public class AppDbContext : IdentityDbContext<AppUserEntity, AppRoleEntity,
        string, AppUserClaimEntity, AppUserRoleEntity, AppUserLoginEntity,
        AppRoleClaimEntity, AppUserTokenEntity>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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


            modelBuilder.Entity<AppUserEntity>(e =>
            {
                e.Property(u=>u.FirstName).HasMaxLength(100);
                e.Property(u=>u.LastName).HasMaxLength(100);
                e.Property(u => u.Image).HasMaxLength(150);
            });

            modelBuilder.Entity<AppUserEntity>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<AppRoleEntity>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

        }
    }
}
