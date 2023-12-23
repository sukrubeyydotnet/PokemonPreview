using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonPreview.Dto;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IpokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IpokemonRepository pokeRepo,
        IReviewRepository reviewRepository,
        IMapper mapper)
        {
            _pokemonRepository = pokeRepo;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        #region GET

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExist(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokeman(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExist(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(rating);
        }

        #endregion


        #region POST

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int categoryId, [FromQuery] int ownerId, [FromBody] PokemonDto pokemonDto)
        {
            if (pokemonDto is null)
                return BadRequest();

            var pokemon = _pokemonRepository.GetPokemons()
                          .Where(x => x.Name == pokemonDto.Name)
                          .FirstOrDefault();

            if (pokemon is not null)
            {
                ModelState.AddModelError("", "Pokemon Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);

            if (!_pokemonRepository.PokemonCreate(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Pokemon Created Successfully");
        }

        #endregion

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePokemon(int pokeId,
        [FromQuery] int ownerId,
        [FromQuery] int categoryId,
        [FromBody] PokemonDto updatePoke)
        {

            if (updatePoke is null)
                return BadRequest(ModelState);

            if (pokeId != updatePoke.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.IsPokemonExist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();


            var pokeMap = _mapper.Map<Pokemon>(updatePoke);

            if (!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokeMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }



            return Ok("Successfully");
        }



        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExist(pokeId))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewsOfPokemonId(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokeman(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return Ok("delete Successfully");
        }
    }







}