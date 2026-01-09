using Microsoft.EntityFrameworkCore;
using PokedexMvc.Models;
using System.Collections.Generic;

namespace PokedexMvc.Data
{
    public class PokedexContext : DbContext
    {
        public PokedexContext(DbContextOptions<PokedexContext> options) : base(options)
        {
        }

        public DbSet<PokemonEntity> Pokemons { get; set; }
    }
}