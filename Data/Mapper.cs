using AutoMapper;
using BookReviewer.Models;
public class BookMappingProfile : Profile
{
  public BookMappingProfile()
  {
    
    CreateMap<Book, BookViewModel>();

    // BookEditViewModel → Book
    // (if you want reverse mapping; be cautious about collections)
    // CreateMap<BookEditViewModel, Book>()
    //   .ForMember(dest => dest.Reviews, opt => opt.Ignore());

    // // Book → BookDetailsViewModel
    // CreateMap<Book, BookDetailsViewModel>();

    // // Review → ReviewViewModel
    // CreateMap<Review, ReviewViewModel>();
  }
}
