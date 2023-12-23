using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class PokemonRepository : IpokemonRepository
    {
        private readonly AppDbContext _context;
        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return IsSaved();
        }

        public Pokemon GetPokeman(int id)
        {
            var pokemon = _context.Pokemon.Where(x => x.Id == id)
            .FirstOrDefault();
            return pokemon;
        }

        public Pokemon GetPokeman(string pokeName)
        {
            var pokemon = _context.Pokemon.Where(x => x.Name == pokeName)
            .FirstOrDefault();
            return pokemon;
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var rewiev = _context.Reviews
            .Where(x => x.Pokeman.Id == pokeId);
            if (rewiev.Count() > 0)
                return 0;

            Console.WriteLine($"Review {rewiev.Count()}");
            return ((decimal)rewiev.Sum(x => x.Rating) / rewiev.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(x => x.Id).ToList();
        }

        public bool IsPokemonExist(int pokeId)
        {
            return _context.Pokemon.Any(x => x.Id == pokeId);
        }

        public bool IsSaved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool PokemonCreate(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner
            {
                Owner = owner,
                Pokeman = pokemon

            };
            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory
            {
                Category = category,
                Pokeman = pokemon
            };
            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return IsSaved();
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return IsSaved();
        }
    }
}