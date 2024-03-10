using API.Data.Migrations;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    //DB uses int id, so every identity type's key must be specified here
    public class ApplicationDbContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserFollow> Follows { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups{ get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //User-Role
            builder.Entity<UserRole>()
                .HasOne(u => u.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<UserRole>()
                .HasOne(u => u.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            //User-User
            builder.Entity<UserFollow>()
                .HasKey(k => new {k.SourceUserId, k.TargetUserId});

            builder.Entity<UserFollow>()
                .HasOne(s => s.SourceUser)
                .WithMany(f => f.Following)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollow>()
                .HasOne(s => s.TargetUser)
                .WithMany(f => f.Followers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            //User-Message
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(f => f.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(f => f.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            // User-Post (Author)
            builder.Entity<Post>()
                .HasOne(u => u.Author)
                .WithMany(f => f.Posts)
                .HasForeignKey(u => u.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // User-Post (Likes)
            builder.Entity<Post>()
                .HasMany(u => u.LikedBy)
                .WithMany(f => f.LikedPosts);

            // User-Post (Dislikes)
            builder.Entity<Post>()
                .HasMany(u => u.DislikedBy)
                .WithMany(f => f.DislikedPosts);
        }
    }
}