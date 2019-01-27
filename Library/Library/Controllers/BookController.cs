using Library.Data;
using Library.Entities;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Identity/Account");
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).Select(x => x.Id).FirstOrDefault();
            if (userId == null)
                return RedirectToAction("Login", "Identity/Account");
            var books = _context.Books
                .Where(x => !x.IsDeleted && x.UserId == userId)
                .Select(x => new BookDto
                {
                    BookId = x.BookId,
                    Author = x.Author,
                    Description = x.Description,
                    Name = x.Name,
                    Status = x.BookLendings.Any(l => !l.DateTo.HasValue) ? "Wypożyczona" : "Nie wypożyczona",
                    AvailableToDelete = !x.BookLendings.Any(l => !l.DateTo.HasValue)
                }).ToList();
            return View(books);
        }
        public IActionResult Create()
        {
            return View(new BookDto());
        }

        [HttpPost]
        public IActionResult Create(BookDto model)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).First().Id;
            var newBook = new Book
            {
                Author = model.Author,
                Description = model.Description,
                Name = model.Name,
                UserId = userId
            };
            _context.Books.Add(newBook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _context.Books.Where(x => x.BookId == id)
                .Select(x => new BookDto
                {
                    Author = x.Author,
                    BookId = x.BookId,
                    Description = x.Description,
                    Name = x.Name
                }).First();
            return View("Create", model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _context.Books.Where(x => x.BookId == id).First();
            model.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(BookDto model)
        {
            var editedBook = _context.Books.Where(x => x.BookId == model.BookId).First();
            editedBook.Author = model.Author;
            editedBook.Description = model.Description;
            editedBook.Name = model.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Release(int id)
        {
            var lending = _context.BookLendings.Where(x => x.BookId == id).First();
            lending.DateTo = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}