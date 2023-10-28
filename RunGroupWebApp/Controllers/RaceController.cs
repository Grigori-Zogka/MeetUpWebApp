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
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _raceRepository;

        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            //_context = context;
            _raceRepository = raceRepository;

            _photoService = photoService;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if(ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    RaceCategory = raceVM.RaceCategory,
                    //AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };

                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }
    }
}

