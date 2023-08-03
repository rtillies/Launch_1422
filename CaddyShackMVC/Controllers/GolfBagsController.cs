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

        public IActionResult New()
        {
			return View();
		}

        [HttpPost]
        public IActionResult Index(GolfBag bag)
        {
            _context.GolfBags.Add(bag);
            _context.SaveChanges();

            var newBagId = bag.Id;

            return RedirectToAction("Show", new { id = newBagId });
        }

        [Route("GolfBags/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var bag = _context.GolfBags.Include(g => g.Clubs).Where(g => g.Id == id).FirstOrDefault();
            return View(bag);
        }

        [HttpPost]
        [Route("GolfBags/{bagId:int}")]
        public IActionResult Update(int bagId, Club club)
        {
			var bag = _context.GolfBags.Include(b => b.Clubs).Where(b => b.Id == bagId).FirstOrDefault();
            if (bag != null)
            {
                bag.Clubs.Add(club);
                _context.GolfBags.Update(bag);
                _context.SaveChanges();
            }

            return Redirect($"/golfbags/{bagId}");
		}
	}
}
