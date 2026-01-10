using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PokedexMvc.Data;
using PokedexMvc.Models;

namespace PokedexMvc.Controllers
{
    public class PokemonController : Controller
    {
        private readonly PokedexContext _context;
        private readonly IMemoryCache _cache;

        public PokemonController(PokedexContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // ===================================================================
        // HELPER METHOD: FILTERING LOGIC 
        // (Used by both Index and Detail for context consistency)
        // ===================================================================
        private async Task<List<PokemonListViewModel>> GetFilteredList(string search, string type, string sort, string favs)
        {
            // 1. Retrieve raw data from Cache (or Database if empty)
            if (!_cache.TryGetValue("CompletePokemonList", out List<PokemonListViewModel>? allPokemons))
            {
                var dbData = await _context.Pokemons.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

                allPokemons = dbData.Select(p => new PokemonListViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Types = p.Types.Split(',').ToList(),
                    Hp = p.Hp,
                    Attack = p.Attack,
                    Defense = p.Defense,
                    Speed = p.Speed,
                    BaseExperience = p.BaseExperience
                }).ToList();

                _cache.Set("CompletePokemonList", allPokemons, TimeSpan.FromHours(1));
            }

            // Ensure allPokemons is not null
            var query = (allPokemons ?? new List<PokemonListViewModel>()).AsEnumerable();

            // 2. Favorites Filter
            // If the URL contains ?favs=1,4,25, filter only these IDs.
            if (!string.IsNullOrEmpty(favs))
            {
                try
                {
                    var favIds = favs.Split(',')
                                     .Where(x => int.TryParse(x, out _))
                                     .Select(int.Parse)
                                     .ToList();

                    query = query.Where(p => favIds.Contains(p.Id));
                }
                catch
                {
                    // If parsing fails, ignore the filter
                }
            }

            // 3. Search Filter
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search) || p.Id.ToString() == search);
            }

            // 4. Type Filter
            if (!string.IsNullOrEmpty(type) && type != "all")
            {
                query = query.Where(p => p.Types.Contains(type));
            }

            // 5. Sorting Logic
            query = sort switch
            {
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "hp_desc" => query.OrderByDescending(p => p.Hp),
                "atk_desc" => query.OrderByDescending(p => p.Attack),
                "def_desc" => query.OrderByDescending(p => p.Defense),
                "spd_desc" => query.OrderByDescending(p => p.Speed),
                "xp_desc" => query.OrderByDescending(p => p.BaseExperience),
                "id_desc" => query.OrderByDescending(p => p.Id),
                _ => query.OrderBy(p => p.Id)
            };

            return query.ToList();
        }

        // ===================================================================
        // INDEX ACTION
        // ===================================================================
        public async Task<IActionResult> Index(string search = "", string type = "all", string sort = "id", int page = 1, string favs = "")
        {
            // Get the filtered list using the helper method
            var filteredData = await GetFilteredList(search, type, sort, favs);

            // Pagination Logic
            int pageSize = 36;
            int totalRecords = filteredData.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedData = filteredData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PokemonIndexViewModel
            {
                Pokemons = pagedData,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                SearchTerm = search,
                TypeFilter = type,
                SortOrder = sort
            };

            // Pass flag to View to display "Favorites Filter Active" alert
            ViewData["IsFavFilter"] = !string.IsNullOrEmpty(favs);

            if (string.IsNullOrEmpty(search) && type == "all" && string.IsNullOrEmpty(favs))
            {
                ViewData["Title"] = "Ultimate Pokemon Database";
                ViewData["Description"] = "The fastest and most modern Pokedex. Analyze Pokemon stats, calculate type weaknesses, view move sets, and manage your favorites.";
            }
            else
            {
                ViewData["Title"] = "Search Results";
                ViewData["Description"] = $"Showing results for {(string.IsNullOrEmpty(search) ? "Pokemon" : search)}.";
            }

            return View(model);
        }

        // ===================================================================
        // DETAIL ACTION (Context-Aware)
        // ===================================================================
        public async Task<IActionResult> Detail(string id, int page = 1, string search = "", string type = "all", string sort = "id", string favs = "")
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            PokemonEntity? p = null;

            if (int.TryParse(id, out int numericId))
            {
                p = await _context.Pokemons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);
            }

            else
            {
                p = await _context.Pokemons.AsNoTracking()
                                           .FirstOrDefaultAsync(x => x.Name.ToLower() == id.ToLower());
            }

            if (p == null) return RedirectToAction("Index");

            var contextList = await GetFilteredList(search, type, sort, favs);
            int currentIndex = contextList.FindIndex(x => x.Id == p.Id);

            string? nextId = null;
            string? prevId = null;

            if (currentIndex != -1)
            {

                if (currentIndex > 0)
                    prevId = contextList[currentIndex - 1].Name.ToLower();

                if (currentIndex < contextList.Count - 1)
                    nextId = contextList[currentIndex + 1].Name.ToLower();
            }

            // --- DATA PARSING ---
            var types = p.Types?.Split(',').Select(t => t.Trim().ToLower()).ToList() ?? new List<string>();
            var matchups = CalculateMatchups(types);

            var model = new PokemonDetailViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Types = types,
                Hp = p.Hp,
                Attack = p.Attack,
                Defense = p.Defense,
                Speed = p.Speed,
                BaseExperience = p.BaseExperience,
                Height = p.Height,
                Weight = p.Weight,
                Abilities = p.Abilities,
                Moves = p.Moves?.Split(',').Select(m => m.Trim()).ToList(),

                PrevId = prevId,
                NextId = nextId,

                Weaknesses = matchups.Weaknesses,
                Resistances = matchups.Resistances,

                ReturnPage = page,
                ReturnSearch = search,
                ReturnType = type,
                ReturnSort = sort,
                ReturnFavs = favs
            };

            ViewData["Title"] = model?.Name?.ToUpper();
            return View(model);
        }

        // ==========================================
        // TYPE MATCHUP LOGIC ENGINE (MATRIX)
        // ==========================================
        private (List<string> Weaknesses, List<string> Resistances) CalculateMatchups(List<string> pokemonTypes)
        {
            string[] allTypes = { "normal", "fire", "water", "grass", "electric", "ice", "fighting", "poison", "ground", "flying", "psychic", "bug", "rock", "ghost", "dragon", "steel", "dark", "fairy" };

            var weaknesses = new List<string>();
            var resistances = new List<string>();

            foreach (var attackType in allTypes)
            {
                double multiplier = 1.0;

                // Check effectiveness against each of the Pokemon's types (Dual-type support)
                foreach (var defType in pokemonTypes)
                {
                    multiplier *= GetTypeEffectiveness(attackType, defType);
                }

                if (multiplier > 1.0) weaknesses.Add(attackType);
                else if (multiplier < 1.0) resistances.Add(attackType);
            }

            return (weaknesses, resistances);
        }

        // Returns damage multiplier: Attack Type vs Defense Type
        private double GetTypeEffectiveness(string atk, string def)
        {
            return (atk, def) switch
            {
                ("normal", "rock") => 0.5,
                ("normal", "ghost") => 0,
                ("normal", "steel") => 0.5,
                ("fire", "fire") => 0.5,
                ("fire", "water") => 0.5,
                ("fire", "grass") => 2.0,
                ("fire", "ice") => 2.0,
                ("fire", "bug") => 2.0,
                ("fire", "rock") => 0.5,
                ("fire", "dragon") => 0.5,
                ("fire", "steel") => 2.0,
                ("water", "fire") => 2.0,
                ("water", "water") => 0.5,
                ("water", "grass") => 0.5,
                ("water", "ground") => 2.0,
                ("water", "rock") => 2.0,
                ("water", "dragon") => 0.5,
                ("grass", "fire") => 0.5,
                ("grass", "water") => 2.0,
                ("grass", "grass") => 0.5,
                ("grass", "poison") => 0.5,
                ("grass", "ground") => 2.0,
                ("grass", "flying") => 0.5,
                ("grass", "bug") => 0.5,
                ("grass", "rock") => 2.0,
                ("grass", "dragon") => 0.5,
                ("grass", "steel") => 0.5,
                ("electric", "water") => 2.0,
                ("electric", "grass") => 0.5,
                ("electric", "electric") => 0.5,
                ("electric", "ground") => 0,
                ("electric", "flying") => 2.0,
                ("electric", "dragon") => 0.5,
                ("ice", "fire") => 0.5,
                ("ice", "water") => 0.5,
                ("ice", "grass") => 2.0,
                ("ice", "ice") => 0.5,
                ("ice", "ground") => 2.0,
                ("ice", "flying") => 2.0,
                ("ice", "dragon") => 2.0,
                ("ice", "steel") => 0.5,
                ("fighting", "normal") => 2.0,
                ("fighting", "ice") => 2.0,
                ("fighting", "poison") => 0.5,
                ("fighting", "flying") => 0.5,
                ("fighting", "psychic") => 0.5,
                ("fighting", "bug") => 0.5,
                ("fighting", "rock") => 2.0,
                ("fighting", "ghost") => 0,
                ("fighting", "dark") => 2.0,
                ("fighting", "steel") => 2.0,
                ("fighting", "fairy") => 0.5,
                ("poison", "grass") => 2.0,
                ("poison", "poison") => 0.5,
                ("poison", "ground") => 0.5,
                ("poison", "rock") => 0.5,
                ("poison", "ghost") => 0.5,
                ("poison", "steel") => 0,
                ("poison", "fairy") => 2.0,
                ("ground", "fire") => 2.0,
                ("ground", "grass") => 0.5,
                ("ground", "electric") => 2.0,
                ("ground", "poison") => 2.0,
                ("ground", "flying") => 0,
                ("ground", "bug") => 0.5,
                ("ground", "rock") => 2.0,
                ("ground", "steel") => 2.0,
                ("flying", "grass") => 2.0,
                ("flying", "electric") => 0.5,
                ("flying", "fighting") => 2.0,
                ("flying", "bug") => 2.0,
                ("flying", "rock") => 0.5,
                ("flying", "steel") => 0.5,
                ("psychic", "fighting") => 2.0,
                ("psychic", "poison") => 2.0,
                ("psychic", "psychic") => 0.5,
                ("psychic", "dark") => 0,
                ("psychic", "steel") => 0.5,
                ("bug", "fire") => 0.5,
                ("bug", "grass") => 2.0,
                ("bug", "fighting") => 0.5,
                ("bug", "poison") => 0.5,
                ("bug", "flying") => 0.5,
                ("bug", "psychic") => 2.0,
                ("bug", "ghost") => 0.5,
                ("bug", "dark") => 2.0,
                ("bug", "steel") => 0.5,
                ("bug", "fairy") => 0.5,
                ("rock", "fire") => 2.0,
                ("rock", "ice") => 2.0,
                ("rock", "fighting") => 0.5,
                ("rock", "ground") => 0.5,
                ("rock", "flying") => 2.0,
                ("rock", "bug") => 2.0,
                ("rock", "steel") => 0.5,
                ("ghost", "normal") => 0,
                ("ghost", "psychic") => 2.0,
                ("ghost", "ghost") => 2.0,
                ("ghost", "dark") => 0.5,
                ("dragon", "dragon") => 2.0,
                ("dragon", "steel") => 0.5,
                ("dragon", "fairy") => 0,
                ("steel", "fire") => 0.5,
                ("steel", "water") => 0.5,
                ("steel", "electric") => 0.5,
                ("steel", "ice") => 2.0,
                ("steel", "rock") => 2.0,
                ("steel", "steel") => 0.5,
                ("steel", "fairy") => 2.0,
                ("dark", "fighting") => 0.5,
                ("dark", "psychic") => 2.0,
                ("dark", "ghost") => 2.0,
                ("dark", "dark") => 0.5,
                ("dark", "fairy") => 0.5,
                ("fairy", "fire") => 0.5,
                ("fairy", "fighting") => 2.0,
                ("fairy", "poison") => 0.5,
                ("fairy", "dragon") => 2.0,
                ("fairy", "dark") => 2.0,
                ("fairy", "steel") => 0.5,
                _ => 1.0
            };
        }
    }
}