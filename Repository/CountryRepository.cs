using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class CountryRepository : ICountryRepository
    {
        public readonly AppDbContext _context;
        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }
        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int countryId)
        {
            var country = _context.Countries
            .Where(x => x.Id == countryId)
            .FirstOrDefault();
            return country;
        }

        public Country GetCountry(string countryName)
        {
            var country = _context.Countries
             .Where(x => x.Name == countryName)
             .FirstOrDefault();
            return country;
        }

        public Country GetCountryByOwnerId(int ownerId)
        {
            var country = _context.Owners
           .Where(x => x.Id == ownerId)
           .Select(x => x.Country)
           .FirstOrDefault();

            return country;
        }

        public ICollection<Owner> GetOwnersByCountryId(int countryId)
        {
            var owners = _context.Owners
            .Where(x => x.Country.Id == countryId)
            .ToList();

            return owners;
        }

        public bool IsCountryExist(int countryId)
        {
            return _context.Countries.Any(x => x.Id == countryId);
        }
    }
}