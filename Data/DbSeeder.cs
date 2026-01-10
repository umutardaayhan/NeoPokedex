using Newtonsoft.Json.Linq;
using PokedexMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace PokedexMvc.Data
{
    public static class DbSeeder
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PokedexContext>();
                context.Database.EnsureCreated();

                if (context.Pokemons.Any()) return;

                Console.WriteLine("--> DATA PULLING...");

                var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

                var listUrl = "https://pokeapi.co/api/v2/pokemon?limit=2000";
                var listResponse = await httpClient.GetStringAsync(listUrl);
                var allResults = JObject.Parse(listResponse)["results"].ToList();

                int total = allResults.Count;
                int processed = 0;

                foreach (var chunk in allResults.Chunk(50))
                {
                    var tasks = new List<Task<PokemonEntity>>();

                    foreach (var item in chunk)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                string detailUrl = item["url"].ToString();
                                var response = await httpClient.GetStringAsync(detailUrl);
                                var data = JObject.Parse(response);
                                int id = (int)data["id"];
                                string remoteImgUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png";

                                var typeList = data["types"].Select(t => t["type"]["name"].ToString()).ToList();
                                var abilityList = data["abilities"].Select(a => a["ability"]["name"].ToString()).ToList();

                                return new PokemonEntity
                                {
                                    Id = id,
                                    Name = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data["name"].ToString()),
                                    ImageUrl = remoteImgUrl,
                                    Types = string.Join(", ", typeList),
                                    Abilities = string.Join(", ", abilityList),
                                    Height = (int)data["height"],
                                    Weight = (int)data["weight"],
                                    Hp = (int)data["stats"][0]["base_stat"],
                                    Attack = (int)data["stats"][1]["base_stat"],
                                    Defense = (int)data["stats"][2]["base_stat"],
                                    Speed = (int)data["stats"][5]["base_stat"],
                                    BaseExperience = data["base_experience"] != null ? (int)data["base_experience"] : 0
                                };
                            }
                            catch
                            {
                                return null;
                            }
                        }));
                    }

                    var results = await Task.WhenAll(tasks);

                    foreach (var p in results.Where(x => x != null)) context.Pokemons.Add(p);

                    await context.SaveChangesAsync();
                    context.ChangeTracker.Clear();

                    processed += chunk.Length;
                    Console.WriteLine($"--> [Updating] {processed} / {total}");
                }

                Console.WriteLine("--> COMPLETED! Database is up to date.");
            }
        }
    }
}