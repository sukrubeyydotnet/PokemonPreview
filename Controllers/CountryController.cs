using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonPreview.Dto;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {

            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(countries);
        }



        [HttpGet("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwnerId(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwnerId(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpGet("/GetOwnersByCountryId/{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwnersByCountryId(int countryId)
        {
            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            var owners = _countryRepository.GetOwnersByCountryId(countryId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }



        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {

            if (updatedCountry is null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.IsUpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Update successfully");
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("delete Successfully");
        }
    }
}