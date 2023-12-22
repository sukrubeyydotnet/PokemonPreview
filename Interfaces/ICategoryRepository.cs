using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        Category GetCategory(string categoryName);
        ICollection<Pokemon> GetPokemensByCategoryId(int categoryId);
        bool IsCategoryExist(int categoryId);

    }
}