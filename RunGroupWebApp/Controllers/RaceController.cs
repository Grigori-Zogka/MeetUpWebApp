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


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raveVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raveVM);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", raceVM);
            }

            var userCRace= await _raceRepository.GetByIdAsyncNoTracking(id);

            if (userCRace != null)
            {
                try
                {

                    await _photoService.DeletePhotoAsync(userCRace.Image);
                }
                catch
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(raceVM);
                }


                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);//IFormFile Image { get; set; }

                var club = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,
                };

                _raceRepository.Update(club);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }

        }//Edit post



    }
}

