using BookReviewer.Data;
using BookReviewer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context/*, UserManager<ApplicationUser> userManager*/)
    {
        // Apply migrations
        if (!context.Database.EnsureCreated())
            await context.Database.MigrateAsync();

        // Seed users
        // if (!userManager.Users.Any())
        // {
        //     var user1 = new ApplicationUser { Id = "user-1", UserName = "alice", Email = "alice@example.com", EmailConfirmed = true };
        //     var user2 = new ApplicationUser { Id = "user-2", UserName = "bob", Email = "bob@example.com", EmailConfirmed = true };

        //     await userManager.CreateAsync(user1, "Password123!");
        //     await userManager.CreateAsync(user2, "Password123!");
        // }

        // Seed Books
        if (!context.Books.Any())
        {
            context.Books.AddRange(
                new Book { Id = 1, Title = "1984", Author = "George Orwell", PublishedYear = 1949, Genre = "Dystopian" },
                new Book { Id = 2, Title = "The Hobbit", Author = "J.R.R. Tolkien", PublishedYear = 1937, Genre = "Fantasy" }
            );
        }

        // Seed Reviews
        // if (!context.Reviews.Any())
        // {
        //     context.Reviews.AddRange(
        //         new Review { Id = 1, Content = "Great book!", Rating = 5, DateCreated = DateTime.UtcNow, BookId = 1, UserId = "user-1" },
        //         new Review { Id = 2, Content = "Not bad.", Rating = 3, DateCreated = DateTime.UtcNow, BookId = 2, UserId = "user-2" }
        //     );
        // }

        // // Seed ReviewVotes
        // if (!context.ReviewVotes.Any())
        // {
        //     context.ReviewVotes.AddRange(
        //         new ReviewVote { Id = 1, ReviewId = 1, UserId = "user-2", IsUpvote = true },
        //         new ReviewVote { Id = 2, ReviewId = 2, UserId = "user-1", IsUpvote = false }
        //     );
        // }

        await context.SaveChangesAsync();
    }
}
