namespace BookStore.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Author Author { get; set; }
    }
}
