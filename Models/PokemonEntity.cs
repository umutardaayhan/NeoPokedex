using System.ComponentModel.DataAnnotations;

namespace PokedexMvc.Models
{
    public class PokemonEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int BaseExperience { get; set; }
    }
}