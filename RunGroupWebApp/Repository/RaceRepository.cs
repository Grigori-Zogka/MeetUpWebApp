﻿using System;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
	public class RaceRepository : IRaceRepository
	{
        private readonly ApplicationDbContext _context;

		public RaceRepository(ApplicationDbContext context)
		{
            _context = context;
		}

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public Task<Race> GetByIdAsync(int id)
        {
            return _context.Races.Include(ad=>ad.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return _context.Races.Include(ad => ad.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}

