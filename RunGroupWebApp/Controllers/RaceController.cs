using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _raceRepository;

        public RaceController(IRaceRepository raceRepository)
        {
            //_context = context;
            _raceRepository = raceRepository;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //List<Race> races = _context.Races.ToList();
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }


        //[HttpGet("/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            //Club club = _context.Clubs.FirstOrDefault(c => c.Id ==id);
            //In order to include the address  (joined in the  club with Address ID)
            //Race race = _context.Races.Include(ad => ad.Address).FirstOrDefault(c => c.Id == id);
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }


        public IActionResult Create()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }
}

