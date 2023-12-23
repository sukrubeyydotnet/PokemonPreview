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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var pokemons = _mapper.Map<IEnumerable<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("reviewId")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.IsReviewExist(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(review);
        }


        [HttpGet("reviews/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewByPokemonId(int pokeId)
        {
            var review = _mapper.Map<IEnumerable<ReviewDto>>(_reviewRepository.GetReviewsOfPokemonId(pokeId));

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

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview is null)
                return BadRequest(ModelState);

            if (reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.IsReviewExist(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updateReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.IsReviewExist(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}