using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PokemonPreview.EF;
using PokemonPreview.Interfaces;
using PokemonPreview.Models;

namespace PokemonPreview.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool DeleteCategory(Category category)
        {
           _context.Remove(category);
           return IsSave();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            var category = _context.Categories.Where(x => x.Id == categoryId).FirstOrDefault();
            return category;
        }

        public Category GetCategory(string categoryName)
        {
            var category = _context.Categories.Where(x => x.Name == categoryName).FirstOrDefault();
            return category;
        }

        public ICollection<Pokemon> GetPokemensByCategoryId(int categoryId)
        {
            var pokemons = _context.PokemanCategories
            .Where(x => x.CategoryId == categoryId)
            .Select(x => x.Pokeman).ToList();
            return pokemons;
        }

        public bool IsCategoryExist(int categoryId)
        {
            return _context.Categories.Any(x => x.Id == categoryId);
        }

        public bool IsCategoryUpdate(Category category)
        {
            _context.Update(category);
            return IsSave();
        }

        public bool IsSave()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }
    }
}