using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }



        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfPokemonId(int pokeId)
        {
            var reviews = _context.Reviews
            .Where(x => x.Pokeman.Id == pokeId)
            .ToList();
            return reviews;
        }

        public bool IsReviewExist(int reviewId)
        {
            return _context.Reviews.Any(x => x.Id == reviewId);
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return IsSaved();
        }

        public bool IsSaved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}