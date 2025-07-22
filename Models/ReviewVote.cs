using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookReviewer.Models;
public class ReviewVote
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Upvote is required")]
    public bool IsUpvoted { get; set; }
    [Required]
    public int ReviewId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public virtual Review Review { get; set; } = null!;
    public virtual IdentityUser User { get; set; } = null!;
}