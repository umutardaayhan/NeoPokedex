using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public async Task<IActionResult> Index(string search = "", string type = "all", string sort = "id", int page = 1)
        {
            if (!_cache.TryGetValue("CompletePokemonList", out List<PokemonListViewModel> allPokemons))
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

            var query = allPokemons.AsEnumerable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search) || p.Id.ToString() == search);
            }

            if (!string.IsNullOrEmpty(type) && type != "all")
            {
                query = query.Where(p => p.Types.Contains(type));
            }

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

            int pageSize = 36;
            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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

            return View(model);
        }
        public async Task<IActionResult> Detail(int id, int page = 1, string search = "", string type = "all", string sort = "id")
        {
            var p = await _context.Pokemons
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return RedirectToAction("Index");

            var model = new PokemonDetailViewModel
            {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Types = p?.Types?.Split(',').ToList(),
                Hp = p.Hp,
                Attack = p.Attack,
                Defense = p.Defense,
                Speed = p.Speed,
                BaseExperience = p.BaseExperience,
                Height = p.Height,
                Weight = p.Weight,
                ReturnPage = page,
                ReturnSearch = search,
                ReturnType = type,
                ReturnSort = sort
            };

            ViewData["Title"] = model?.Name?.ToUpper();

            string types = string.Join("/", model.Types.Select(t => char.ToUpper(t[0]) + t.Substring(1)));
            ViewData["Description"] = $"{char.ToUpper(model.Name[0]) + model.Name.Substring(1)} analysis: {types} type. " +
                                      $"Stats: HP {model.Hp}, Attack {model.Attack}, Defense {model.Defense}. " +
                                      $"View full combat data on NeoPokedex.";

            ViewData["OgImage"] = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";
            return View(model);
        }

    }
}