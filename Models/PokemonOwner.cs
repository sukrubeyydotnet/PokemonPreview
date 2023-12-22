using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonPreview.Models
{
    public class PokemonOwner
    {
        public int PokemanId { get; set; }
        public int OwnerId { get; set; }

        public Pokemon Pokeman{get;set;}
        public Owner Owner{get;set;}
    }
}