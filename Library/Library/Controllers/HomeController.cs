using Library.Data;
using Library.Entities;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Identity/Account");
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).First().Id;
            var books = _context.Books
               .Where(x => !x.IsDeleted && x.UserId != userId &&
               !x.BookLendings.Any(l => !l.DateTo.HasValue))
               .Select(x => new BookDto
               {
                   BookId = x.BookId,
                   Author = x.Author,
                   Description = x.Description,
                   Name = x.Name
               }).ToList();

            return View(books);
        }

        public IActionResult Borrow(int id)
        {
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).First().Id;
            var newLending = new BookLending
            {
                BookId = id,
                DateFrom = DateTime.Now,
                DateTo = null,
                UserId = userId
            };
            _context.BookLendings.Add(newLending);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
