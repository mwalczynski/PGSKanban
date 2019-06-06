using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PgsKanban.DataAccess.Models;

namespace PgsKanban.DataAccess
{
    public class PgsKanbanContext : IdentityDbContext<User>
    {
        public PgsKanbanContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<UserBoard> UserBoards { get; set; }
        public virtual DbSet<List> Lists { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ExternalUser> ExternalUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBoard>()
                .HasKey(x => new { x.UserId, x.BoardId });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new { x.UserId });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId });

            modelBuilder.Entity<User>()
                .HasIndex(x => x.FirstName)
                .HasName("FirstNameIndex");

            modelBuilder.Entity<User>()
                .HasIndex(x => x.LastName)
                .HasName("LastNameIndex");

            modelBuilder.Entity<ExternalUser>()
                .HasIndex(x => new {x.LastName, x.FirstName})
                .HasName("IX_ExternalUser_LastName_FirstName");
        }
    }
}
