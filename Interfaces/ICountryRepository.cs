using PokemonPreview.Models;

namespace PokemonPreview.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountry(string countryName);
        Country GetCountryByOwnerId(int ownerId);
        ICollection<Owner> GetOwnersByCountryId(int countryId);
        bool IsCountryExist(int countryId);
    }
}