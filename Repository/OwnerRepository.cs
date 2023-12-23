using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;
        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return IsSave();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners
            .Where(x => x.Id == ownerId)
            .FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokeId)
        {
            var owners = _context.PokemanOwners
            .Where(x => x.PokemanId == pokeId)
            .Select(x => x.Owner)
            .ToList();
            return owners;
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwnerId(int ownerId)
        {
            var pokemons = _context.PokemanOwners
            .Where(x => x.OwnerId == ownerId)
            .Select(x => x.Pokeman).ToList();
            return pokemons;
        }

        public bool IsOwnerExist(int ownerId)
        {
            return _context.Owners.Any(x => x.Id == ownerId);
        }

        public bool IsSave()
        {
            var saveCount = _context.SaveChanges();
            return saveCount > 0 ? true : false;
        }

        public bool IsUpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return IsSave();
        }
    }
}