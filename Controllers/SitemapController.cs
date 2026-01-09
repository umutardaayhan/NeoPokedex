using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokedexMvc.Data;
using System.Text;

namespace PokedexMvc.Controllers
{
    public class SitemapController : Controller
    {
        private readonly PokedexContext _context;

        public SitemapController(PokedexContext context)
        {
            _context = context;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            string baseUrl = "https://neopokedex.runasp.net";

            var pokemonIds = await _context.Pokemons
                                           .AsNoTracking()
                                           .OrderBy(x => x.Id)
                                           .Select(x => x.Id)
                                           .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            sb.AppendLine("<url>");
            sb.AppendLine($"<loc>{baseUrl}/</loc>");
            sb.AppendLine("<changefreq>daily</changefreq>");
            sb.AppendLine("<priority>1.0</priority>");
            sb.AppendLine("</url>");

            foreach (var id in pokemonIds)
            {
                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{baseUrl}/Pokemon/Detail/{id}</loc>");
                sb.AppendLine("<changefreq>weekly</changefreq>"); 
                sb.AppendLine("<priority>0.8</priority>");
                sb.AppendLine("</url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}