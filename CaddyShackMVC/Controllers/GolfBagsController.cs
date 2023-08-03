using CaddyShackMVC.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CaddyShackMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CaddyShackMVC.Controllers
{
    public class GolfBagsController : Controller
    {
        private readonly CaddyShackContext _context;
        public GolfBagsController(CaddyShackContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var bags = _context.GolfBags.Include(b => b.Clubs).ToList();
            return View(bags);
        }

        [Route("golfbags/{id:int}")]
        public IActionResult Show(int id)
        {
            var bag = _context.GolfBags.Include(b => b.Clubs).Where(b => b.Id == id).First();
            return View(bag);
        }

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var bag = _context.GolfBags.Find(id);

            if (bag != null)
            {
			    _context.GolfBags.Remove(bag);
			    _context.SaveChanges();
            }

			return RedirectToAction("Index");
		}
	}
}
