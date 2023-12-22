using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonPreview.Models;

namespace PokemonPreview.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemanOwners { get; set; }
        public DbSet<PokemonCategory> PokemanCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>().
            HasKey(x => new { x.PokemonId, x.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
            .HasOne(x => x.Pokeman)
            .WithMany(x => x.PokemanCategories)
            .HasForeignKey(x => x.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(x => x.Category)
                .WithMany(x => x.PokemonCategories)
                .HasForeignKey(x => x.CategoryId);



            modelBuilder.Entity<PokemonOwner>().
          HasKey(x => new { x.PokemanId, x.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
            .HasOne(x => x.Pokeman)
            .WithMany(x => x.PokemanOwners)
            .HasForeignKey(x => x.PokemanId);

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.PokemanOwners)
                .HasForeignKey(x => x.OwnerId);


        }
    }
}