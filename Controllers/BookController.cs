using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BookStore.Data.Specifications;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IGenericRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IGenericRepository<Book> bookRepository, IGenericRepository<Author> authorRepository, IWebHostEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var spec = new BooksWithAuthorsSpecification();

            var books = bookRepository.List(spec);

            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var spec = new BooksWithAuthorsSpecification(id);

            var book = bookRepository.GetEntityWithSpec(spec);

            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadFile(model.FormFile) ?? string.Empty;

                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please Select an author";

                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all the required field");
            return View();
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var spec = new BooksWithAuthorsSpecification(id);

                var book = bookRepository.GetEntityWithSpec(spec);

                var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

                var model = new BookAuthorViewModel
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    AuthorId = authorId,
                    Authors = authorRepository.List().ToList(),
                    ImageUrl = book.ImageUrl
                };

                return View(model);
            }
            catch (Exception)
            {
                return View();
            }
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = EditUploadedFile(model.FormFile, model.ImageUrl);

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new()
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName
                    };
                    bookRepository.Update(model.BookId, book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(model);
                }
            }
            ModelState.AddModelError("", "You have to fill all the required field");
            return View();
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var book = bookRepository.Find(id);
                return View(book);
            }
            catch
            {
                return View();
            }
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: BookController/Search
        public ActionResult Search(string term)
        {
            try
            {
                var spec = new BooksWithAuthorsSpecification(term);

                var books = bookRepository.List(spec);

                return View("Index", books);
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            try
            {
                var authors = authorRepository.List().ToList();

                authors.Insert(0, new Author { Id = -1, FullName = "Please select an author" });

                return authors;
            }
            catch
            {
                return null;
            }
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var viewModel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return viewModel;
        }

        string UploadFile(IFormFile formFile)
        {
            if (formFile != null)
            {
                string upload = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(upload, formFile.FileName);
                formFile.CopyTo(new FileStream(fullPath, FileMode.Create));

                return formFile.FileName;
            }
            return null;
        }


        string EditUploadedFile(IFormFile formFile, string imageUrl)
        {

            if (formFile != null)
            {
                string upload = Path.Combine(hosting.WebRootPath, "uploads");
                string newPath = Path.Combine(upload, formFile.FileName);

                string oldPath = Path.Combine(upload, imageUrl);
                if (newPath != oldPath)
                {
                    System.IO.File.Delete(oldPath);
                    formFile.CopyTo(new FileStream(newPath, FileMode.Create));
                }
                return formFile.FileName;
            }
            return imageUrl;
        }
    }
}
