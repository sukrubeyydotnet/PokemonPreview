using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface IpokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokeman(int id);
        Pokemon GetPokeman(string pokeName);
        decimal GetPokemonRating(int pokeId);
        bool IsPokemonExist(int pokeId);
        bool PokemonCreate(int ownerId, int categoryId, Pokemon pokemon);
        bool IsSaved();
    }
}