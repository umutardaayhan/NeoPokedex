using Microsoft.EntityFrameworkCore;
using PokedexMvc.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext Configuration
builder.Services.AddDbContext<PokedexContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=pokedex.db"));

builder.Services.AddMemoryCache();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- DATABASE CREATION AND SEEDING (ONE-TIME SETUP) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PokedexContext>(); // Your Context Name

        // 👇 FIX 1: EnsureCreated instead of Migrate
        // This command does not look at Migration files. 
        // If the database doesn't exist, it creates it from scratch based on C# classes.
        context.Database.EnsureCreated();

        // Since the database is created, we can now seed the data
        await DbSeeder.Seed(app);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}
// -------------------------------------------------------

app.UseResponseCompression();

// ❌ REMOVED: await DbSeeder.Seed(app); (Already called above, this was redundant)

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache images for 1 year (Critical for speed)
        const int durationInSeconds = 60 * 60 * 24 * 365;
        ctx.Context.Response.Headers["Cache-Control"] =
            "public,max-age=" + durationInSeconds;
    }
});

app.UseStatusCodePagesWithReExecute("/Pokemon/Index");

app.UseHttpsRedirection();
app.UseRouting();

// app.MapStaticAssets(); // Keep this commented out if not using .NET 9 features; UseStaticFiles is sufficient.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokemon}/{action=Index}/{id?}");

app.Run();