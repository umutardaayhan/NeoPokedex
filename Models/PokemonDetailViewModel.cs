namespace PokedexMvc.Models
{
    public class PokemonDetailViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        public List<string>? Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int BaseExperience { get; set; }
        public string? Abilities { get; set; }
        public List<string>? Moves { get; set; }

        public string? NextId { get; set; }
        public string? PrevId { get; set; }

        public List<string> Weaknesses { get; set; } = new List<string>();
        public List<string> Resistances { get; set; } = new List<string>();

        public int ReturnPage { get; set; }
        public string? ReturnSearch { get; set; }
        public string? ReturnType { get; set; }
        public string? ReturnSort { get; set; }
        public string? ReturnFavs { get; set; }
    }
}