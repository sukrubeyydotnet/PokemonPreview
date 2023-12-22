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
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IpokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepo, IReviewerRepository reviewerRepo, IpokemonRepository pokemonRepo, IMapper mapper)
        {
            _reviewRepository = reviewRepo;
            _mapper = mapper;
            _reviewerRepository = reviewerRepo;
            _pokemonRepository = pokemonRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var pokemons = _reviewRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("reviewId")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.IsReviewExist(reviewId))
                return NotFound();

            var review = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(review);
        }


        [HttpGet("reviews/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviewByPokemonId(int pokeId)
        {
            var review = _reviewRepository.GetReviewsOfPokemonId(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId,
        [FromQuery] int pokeId,
        [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate is null)
                return BadRequest();

            var review = _reviewRepository.GetReviews()
            .Where(x => x.Title == reviewCreate.Title).FirstOrDefault();
            if (review is not null)
            {
                ModelState.AddModelError("", "Review Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokeman = _pokemonRepository.GetPokeman(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Review Created Successfully");
        }
    }
}