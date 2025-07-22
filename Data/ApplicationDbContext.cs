using BookReviewer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace BookReviewer.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Review> Reviews => Set<Review>();

    //public DbSet<Review> Reviews => Set<Review>();
    //  public DbSet<ReviewVote> ReviewVotes => Set<ReviewVote>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call base to ensure Identity configuration is applied
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
                entity.Property(b => b.PublishedYear).IsRequired(false);
                //entity.Property(b => b.Description).HasMaxLength(2000);
                //entity.Property(b => b.PublicationDate).IsRequired();

                // Index for better query performance
                entity.HasIndex(b => b.Title);
                entity.HasIndex(b => b.Author);
            });

            // Configure Review entity and relationships
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.Comment).HasMaxLength(1000);
                entity.Property(r => r.CreatedAt).IsRequired();
                
                // Configure foreign key to Book
                entity.HasOne(r => r.Book)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete reviews when book is deleted

                // Configure foreign key to User
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Delete reviews when user is deleted

                // Composite index for better performance on book reviews
                entity.HasIndex(r => new { r.BookId, r.CreatedAt });
                
                // Ensure a user can only review a book once
                entity.HasIndex(r => new { r.BookId, r.UserId }).IsUnique();
            });
    }
    
}
