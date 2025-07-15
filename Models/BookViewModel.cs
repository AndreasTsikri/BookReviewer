using System.ComponentModel.DataAnnotations;

namespace BookReviewer.Models;

public class BookViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Provide book title")]
    [MaxLength(255,ErrorMessage = "Max 255 characters")]
    public string Title { get; set; } = default!;
    
    [MaxLength(255, ErrorMessage = "Max 255 characters")]
    public string Author { get; set; } = default!;

    [Range(1000, 9999)]
    public int PublishedYear { get; set; }

    [MaxLength(20, ErrorMessage = "Max 20 characters")]
    public string Genre { get; set; } = default!;
    //public ICollection<Review> Reviews { get; set; } = new List<Review>();


}