using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonPreview.Dto;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;


        public ReviewerController(IReviewerRepository reviewerRepo, IMapper mapper)
        {
            _reviewerRepository = reviewerRepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviews()
        {
            var pokemons = _mapper.Map<IEnumerable<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("reviewerId")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetReview(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var review = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(review);
        }


        [HttpGet("reviews/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]

        public IActionResult GetReviewsByReviewerId(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerdto)
        {
            if (reviewerdto is null)
                return BadRequest();

            //TODO: CHKECK model map and if' there is exist return modelstate add erorr
            return Ok("Reviewer Created Successfully");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updateReviewer)
        {
            if (updateReviewer is null)
                return BadRequest(ModelState);
            if (reviewerId != updateReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updateReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully");
        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }
    }
}