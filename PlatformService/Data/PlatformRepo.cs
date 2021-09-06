using PlatformService.Data;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private AppDbContext _context { get; }

       public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            {
              throw new ArgumentNullException(nameof(plat));
            }

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatForms()
        {
            return (IEnumerable<Platform>)_context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }

}