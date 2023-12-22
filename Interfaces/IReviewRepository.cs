using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfPokemonId(int pokeId);
        bool IsReviewExist(int reviewId);
        bool CreateReview(Review review);
        bool IsSaved();
    }
}