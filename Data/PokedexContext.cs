using Microsoft.EntityFrameworkCore;
using PokedexMvc.Models;

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