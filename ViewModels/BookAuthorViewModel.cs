using BookStore.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class BookAuthorViewModel
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(150, MinimumLength =3)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int AuthorId { get; set; }

        public List<Author> Authors { get; set; }

        public IFormFile FormFile { get; set; }
    }
}
