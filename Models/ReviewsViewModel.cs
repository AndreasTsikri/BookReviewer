using BookReviewer.Models;

public class ReviewsViewModel
{
    public int BookId {get; set;}
    public string? BookTitle { get; set; }    
    public string? BookAuthor { get; set; }
    public int? BookPublishedYear { get; set; }
    public ICollection<Review> Reviews {get;set;} = new List<Review>(); 
}