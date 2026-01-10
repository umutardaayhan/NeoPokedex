using PokedexMvc.Models;
using System.Text.Json;

namespace PokedexMvc.Data
{
    public static class DbSeeder
    {
        public static async Task Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PokedexContext>();

                // Check if database is empty
                if (!context.Pokemons.Any())
                {
                    var webHostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                    string imgFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "pokemons");

                    if (!Directory.Exists(imgFolderPath))
                    {
                        Directory.CreateDirectory(imgFolderPath);
                    }

                    using (var client = new HttpClient())
                    {
                        // ⏰ NOTE: Increased timeout to 30 minutes for 1025 images
                        client.Timeout = TimeSpan.FromMinutes(30);

                        // 🔢 NOTE: Limit set to 1025 (Includes Gen 9)
                        var response = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon?limit=1025");
                        var data = JsonSerializer.Deserialize<JsonElement>(response);
                        var results = data.GetProperty("results");

                        var pokemons = new List<PokemonEntity>();
                        int count = 1;

                        Console.WriteLine("🚀 Pokemon download starting... This might take 5-10 minutes depending on internet speed.");

                        foreach (var item in results.EnumerateArray())
                        {
                            string name = item.GetProperty("name").GetString();
                            string url = item.GetProperty("url").GetString();

                            try
                            {
                                var detailResponse = await client.GetStringAsync(url);
                                var detail = JsonSerializer.Deserialize<JsonElement>(detailResponse);

                                // --- IMAGE DOWNLOADING ---
                                string fileName = $"{count}.png";
                                string localFilePath = Path.Combine(imgFolderPath, fileName);

                                // Image source (Official Artwork)
                                string remoteImgUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{count}.png";

                                if (!File.Exists(localFilePath))
                                {
                                    try
                                    {
                                        var imgBytes = await client.GetByteArrayAsync(remoteImgUrl);
                                        await File.WriteAllBytesAsync(localFilePath, imgBytes);
                                    }
                                    catch
                                    {
                                        Console.WriteLine($"⚠️ Image not found: {name} ({count})");
                                    }
                                }
                                // ---------------------

                                // Parse Data
                                var types = detail.GetProperty("types").EnumerateArray()
                                    .Select(t => t.GetProperty("type").GetProperty("name").GetString()).ToList();

                                var abilities = detail.GetProperty("abilities").EnumerateArray()
                                    .Select(a => a.GetProperty("ability").GetProperty("name").GetString()).ToList();

                                var stats = detail.GetProperty("stats").EnumerateArray().ToList();

                                var moves = detail.GetProperty("moves").EnumerateArray()
                                    .Take(6) // Only first 6 moves
                                    .Select(m => m.GetProperty("move").GetProperty("name").GetString()).ToList();

                                var entity = new PokemonEntity
                                {
                                    Id = count, // Manually assigning ID to keep order
                                    Name = Convert.ToString(name),
                                    ImageUrl = $"/images/pokemons/{count}.png",
                                    Types = string.Join(", ", types),
                                    Abilities = string.Join(", ", abilities),
                                    Moves = string.Join(", ", moves),
                                    Hp = stats[0].GetProperty("base_stat").GetInt32(),
                                    Attack = stats[1].GetProperty("base_stat").GetInt32(),
                                    Defense = stats[2].GetProperty("base_stat").GetInt32(),
                                    Speed = stats[5].GetProperty("base_stat").GetInt32(),
                                    BaseExperience = detail.TryGetProperty("base_experience", out var xp) ? xp.GetInt32() : 0,
                                    Height = detail.GetProperty("height").GetInt32(),
                                    Weight = detail.GetProperty("weight").GetInt32()
                                };

                                pokemons.Add(entity);

                                // Log to console to show progress
                                Console.WriteLine($"✅ {count}/1025 Downloaded: {name}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"❌ ERROR: Could not download {name}. Error: {ex.Message}");
                            }

                            count++;
                        }

                        // Bulk Save
                        context.Pokemons.AddRange(pokemons);
                        await context.SaveChangesAsync();
                        Console.WriteLine("🎉 ALL OPERATIONS COMPLETED!");
                    }
                }
            }
        }
    }
}