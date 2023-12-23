using AutoMapper;
using PokemonPreview.Dto;
using PokemonPreview.Models;

namespace PokemonPreview.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();

            CreateMap<OwnerDto,Owner>();
            CreateMap<Owner,OwnerDto>();

            CreateMap<Review,ReviewDto>();
            CreateMap<ReviewDto,Review>();

            CreateMap<ReviewerDto,Reviewer>();
            CreateMap<Reviewer,ReviewerDto>();
        }
    }
}