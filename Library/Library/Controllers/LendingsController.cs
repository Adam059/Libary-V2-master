using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Library.Controllers
{
    public class LendingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LendingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Identity/Account");
            var now = DateTime.Now;
            var userId = _context.Users.Where(x => x.Email == User.Identity.Name).First().Id;
            var model = _context.BookLendings
                .Where(x => x.UserId == userId && x.DateTo == null)
                .Select(x => new LendingDto
                {
                    BookId = x.BookId,
                    BookName = $"{x.Book.Name}, {x.Book.Author}",
                    Days = $"{now.Subtract(x.DateFrom).Days} dni temu",
                    LendingDate = x.DateFrom.ToShortDateString()
                }).ToList();
            return View(model);
        }

    }
}