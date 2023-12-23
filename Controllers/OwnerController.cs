using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonPreview.Dto;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<IEnumerable<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpGet("ownerId")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owner);
        }

        [HttpGet("/Pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public IActionResult GetOwnerByPokemonId(int pokeId)
        {
            var owner = _mapper.Map<IEnumerable<OwnerDto>>(_ownerRepository.GetOwnerOfPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owner);
        }

        [HttpGet("/Pokemons/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        public IActionResult GetPokemonsByOwnerId(int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            var pokemons = _mapper.Map<IEnumerable<PokemonDto>>(_ownerRepository.GetPokemonsByOwnerId(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int ownerId, [FromBody] OwnerDto updateOwner)
        {
            if (updateOwner is null)
                return BadRequest(ModelState);

            if (ownerId != updateOwner.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();


            var ownerMap = _mapper.Map<Owner>(updateOwner);
            if (!_ownerRepository.IsUpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Update Successfully");
        }


        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            var owner = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_ownerRepository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("delete Successfully");
        }
    }
}