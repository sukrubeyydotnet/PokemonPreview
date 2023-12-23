using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _context;
        public ReviewerRepository(AppDbContext context)
        {
            _context = context;
        }



        public Reviewer GetReviewer(int reviewerId)
        {
            //DO INCLUDE
            return _context.Reviewers
            .Where(x => x.Id == reviewerId)
            .FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            var reviews = _context.Reviews.Where(x => x.Reviewer.Id == reviewerId).ToList();
            return reviews;
        }

        public bool IsReviewerExist(int reviewerId)
        {
            return _context.Reviewers.Any(x => x.Id == reviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return IsSaved();
        }
        public bool IsSaved()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return IsSaved();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return IsSaved();
        }
    }
}