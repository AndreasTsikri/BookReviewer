using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookReviewer.Models;

/// <summary>
/// Represents a review written by a user for a book
/// </summary>
public class Review
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
    public string Comment { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int BookId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    // Navigation properties
    public virtual Book Book { get; set; } = null!;
    public virtual IdentityUser User { get; set; } = null!;
}