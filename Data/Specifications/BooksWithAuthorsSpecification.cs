using BookStore.Models;

namespace BookStore.Data.Specifications
{
    /// <summary>
    /// Use specifications to include authors into books
    /// </summary>
    public class BooksWithAuthorsSpecification : BaseSpecification<Book>
    {
        /// <summary>
        /// Apply specification on specific book using id
        /// </summary>
        /// <param name="id"></param>
        public BooksWithAuthorsSpecification(int id) : base(a => a.Id == id)
        {
            AddInclude(x => x.Author);
        }

        /// <summary>
        /// Apply specification on books to use it on search
        /// </summary>
        /// <param name="term"></param>
        public BooksWithAuthorsSpecification(string term) : base(b => b.Title.Contains(term) || b.Description.Contains(term) || b.Author.FullName.Contains(term))
        {
            AddInclude(x => x.Author);
        }

        /// <summary>
        /// Apply specification on books to get all books
        /// </summary>
        public BooksWithAuthorsSpecification()
        {
            AddInclude(x => x.Author);
        }
    }
}
