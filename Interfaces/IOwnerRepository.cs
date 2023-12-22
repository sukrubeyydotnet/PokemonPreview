using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonsByOwnerId(int ownerId);
        bool IsOwnerExist(int ownerId);
    }
}