using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool IsReviewerExist(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool IsSaved();
    }
}