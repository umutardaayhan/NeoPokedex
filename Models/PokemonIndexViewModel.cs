namespace PokedexMvc.Models
{
    public class PokemonIndexViewModel
    {
        public List<PokemonListViewModel>? Pokemons { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public string? SearchTerm { get; set; }
        public string? TypeFilter { get; set; }
        public string? SortOrder { get; set; }
    }
}