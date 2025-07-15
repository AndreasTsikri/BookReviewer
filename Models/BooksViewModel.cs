namespace BookReviewer.Models;

public class BooksViewModel
{
    public AppUser? User{ get; set; }
    public List<Book> Books { get; set; } = new List<Book>();
    //public List<Review> Reviews { get; set; } = new List<Book>();
}