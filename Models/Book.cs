using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewer.Models;

public class Book
{    
    public required int Id { get; set; }

    [Required(ErrorMessage = "Provide book title")]
    [MaxLength(255,ErrorMessage = "Max 255 characters")]
    public required string Title { get; set; } = default!;    

    [MaxLength(255, ErrorMessage = "Max 255 characters")]
    public string? Author { get; set; } = default!;
    
    [Range(1000, 9999)]
    public int? PublishedYear { get; set; }

    [MaxLength(20, ErrorMessage = "Max 20 characters")]
    public string? Genre { get; set; } = default!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}