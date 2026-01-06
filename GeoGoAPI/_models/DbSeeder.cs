using System.Security.Cryptography;
using System.Text;
using GeoGoAPI._models.entities;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._models;

public static class DbSeeder
{
    public static async Task SeedAsync(GeoGoDbContext db)
    {
        await db.Database.MigrateAsync();

        // Avoid duplicate seeding
        var alreadySeeded = await db.Places.AnyAsync(p => p.Name == "Castelo de Leiria");
        if (alreadySeeded)
            return;

        // ----------------------------
        // 1) Categories
        // ----------------------------
        var catHistory = new Category { Name = "History" };
        var catCulture = new Category { Name = "Culture" };
        var catNature = new Category { Name = "Nature" };

        db.Categories.AddRange(catHistory, catCulture, catNature);
        await db.SaveChangesAsync();

        // ----------------------------
        // 2) Places
        // ----------------------------
        var castelo = new Place
        {
            Name = "Castelo de Leiria",
            Latitude = 39.747443349853704,
            Longitude = -8.809388802130607,
            CategoryId = catHistory.Id,
            Active = true,
        };

        var praca = new Place
        {
            Name = "Praça Rodrigues Lobo",
            Latitude = 39.74479891267496,
            Longitude = -8.807875288636358,
            CategoryId = catCulture.Id,
            Active = true,
        };

        var ponte = new Place
        {
            Name = "Ponte Bairro dos Anjos",
            Latitude = 39.74473232891287,
            Longitude = -8.805205706511261,
            CategoryId = catNature.Id,
            Active = true,
        };

        db.Places.AddRange(castelo, praca, ponte);
        await db.SaveChangesAsync();

        // ----------------------------
        // 3) VirtualPlaces (1 per place)
        // ----------------------------
        var vpCastelo = new VirtualPlace { PlaceId = castelo.Id };
        var vpPraca = new VirtualPlace { PlaceId = praca.Id };
        var vpPonte = new VirtualPlace { PlaceId = ponte.Id };

        db.VirtualPlaces.AddRange(vpCastelo, vpPraca, vpPonte);
        await db.SaveChangesAsync();

        // ----------------------------
        // 4) VirtualObjects (2 per virtual place)
        // ----------------------------
        const string casteloObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/castle.obj";

        const string pracaObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/main/coin.obj";

        const string ponteObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/bridge_mesh.obj";

        const string coinObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/coin.obj";
        const string coinBagObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/coin_bag.obj";
        const string duckObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/duck.obj";
        const string guardObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/guard.obj";
        const string lanternObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/lantern.obj";
        const string scrollObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/scroll.obj";
        const string shieldObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/shield.obj";
        const string swordObj = "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/sword.obj";

        const string castleTexAsset = "models/castle_albedo.png";
        const string coinTexAsset = "models/coin_albedo.png";
        const string bridgeTexAsset = "models/bridge_albedo.png";

        const string coinAlbedo = "models/coin_albedo.png";
        const string coinBagAlbedo = "models/coin_albedo.png";
        const string duckAlbedo = "models/duck_albedo.png";
        const string guardAlbedo = "models/guard_albedo.png";
        const string lanternAlbedo = "models/lantern_albedo.png";
        const string scrollAlbedo = "models/scroll_albedo.png";
        const string shieldAlbedo = "models/shield_albedo.png";
        const string swordAlbedo = "models/sword_albedo.png";

        static string Steps(params (string id, string title, string text)[] steps) =>
            $$"""
    [
      {{string.Join(",\n", steps.Select(s =>
        $$"""
          { "id": "{{s.id}}", "title": "{{s.title}}", "text": "{{s.text}}" }
        """))}}
    ]
    """;

        db.VirtualObjects.AddRange(
            // ---------------- Castelo (2 objects) ----------------
            new VirtualObject
            {
                Name = "Castelo - Object A",
                VirtualPlaceId = vpCastelo.Id,
                ModelUrl = guardObj,
                ModelUrlTexture = guardAlbedo,
                PX = 0f,
                PY = -5f,
                PZ = 0f,
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Welcome to the castle!",
                StepsJson = Steps(
                    (
                        "intro",
                        "Welcome",
                        "Praça Rodrigues Lobo is one of Leiria’s most iconic social spots."
                    ),
                    (
                        "rotate",
                        "Check details",
                        "Rotate the object and look for its engraved edges."
                    ),
                    (
                        "facts",
                        "Culture note",
                        "This square often hosts events, music, and local gatherings."
                    )
                ),
            },
            new VirtualObject
            {
                Name = "Castelo - Object B",
                VirtualPlaceId = vpCastelo.Id,
                ModelUrl = shieldObj, // duplicate mesh for demo
                ModelUrlTexture = shieldAlbedo,
                PX = 0f,
                PY = -5f,
                PZ = -5f, // different placement
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Find the hidden viewpoint!",
                StepsJson = Steps(
                    (
                        "intro",
                        "Welcome",
                        "Praça Rodrigues Lobo is one of Leiria’s most iconic social spots."
                    ),
                    (
                        "rotate",
                        "Check details",
                        "Rotate the object and look for its engraved edges."
                    ),
                    (
                        "facts",
                        "Culture note",
                        "This square often hosts events, music, and local gatherings."
                    )
                ),
            },
            // ---------------- Praça (2 objects) ----------------
            new VirtualObject
            {
                Name = "Praça - Object A",
                VirtualPlaceId = vpPraca.Id,
                ModelUrl = duckObj,
                ModelUrlTexture = duckAlbedo,
                PX = 0f,
                PY = -5f,
                PZ = -5f,
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Central square vibe.",
                StepsJson = Steps(
                    (
                        "intro",
                        "Welcome",
                        "Praça Rodrigues Lobo is one of Leiria’s most iconic social spots."
                    ),
                    (
                        "rotate",
                        "Check details",
                        "Rotate the object and look for its engraved edges."
                    ),
                    (
                        "facts",
                        "Culture note",
                        "This square often hosts events, music, and local gatherings."
                    )
                ),
            },
            new VirtualObject
            {
                Name = "Praça - Object B",
                VirtualPlaceId = vpPraca.Id,
                ModelUrl = lanternObj,
                ModelUrlTexture = lanternAlbedo,
                PX = 0f,
                PY = 0f,
                PZ = -5f,
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Try to align it!",
                StepsJson = Steps(
                    (
                        "intro",
                        "Challenge",
                        "This object is about aligning perspective in the square."
                    ),
                    (
                        "rotate",
                        "Align",
                        "Rotate it until it ‘matches’ the direction of the street."
                    ),
                    (
                        "facts",
                        "Local tip",
                        "Cafés around the praça are a popular stop for students and tourists."
                    )
                ),
            },
            // ---------------- Ponte (2 objects) ----------------
            new VirtualObject
            {
                Name = "Ponte - Object A",
                VirtualPlaceId = vpPonte.Id,
                ModelUrl = scrollObj,
                ModelUrlTexture = scrollAlbedo,
                PX = 0f,
                PY = -5f,
                PZ = -5f,
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Bridge checkpoint.",
                StepsJson = Steps(
                    (
                        "intro",
                        "Welcome",
                        "This spot highlights a crossing point connecting neighborhoods."
                    ),
                    (
                        "rotate",
                        "Explore",
                        "Rotate the model and observe its structure and direction."
                    ),
                    (
                        "facts",
                        "Nature note",
                        "Areas near bridges often become small biodiversity corridors."
                    )
                ),
            },
            new VirtualObject
            {
                Name = "Ponte - Object B",
                VirtualPlaceId = vpPonte.Id,
                ModelUrl = coinBagObj,
                ModelUrlTexture = coinBagAlbedo,
                PX = 0f,
                PY = -5f,
                PZ = 0f,
                RX = 0f,
                RY = 0f,
                RZ = 90f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Look underneath!",
                StepsJson = Steps(
                    (
                        "intro",
                        "Underpass",
                        "This object focuses on what’s beneath and around the bridge."
                    ),
                    (
                        "rotate",
                        "Inspect angle",
                        "Rotate and zoom to get a good ‘under-bridge’ viewpoint."
                    ),
                    (
                        "facts",
                        "Fun fact",
                        "Bridges are common reference points in local navigation stories."
                    )
                ),
            }
        );

        await db.SaveChangesAsync();

        // ----------------------------
        // 5) Demo user + twin (test/test) using SAME hashing routine as AccountController
        // ----------------------------
        using (var hmac = new HMACSHA512())
        {
            var user = new AppUser
            {
                UserName = "test", // controller lowercases; "test" is already fine
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("test")),
            };

            // Create the twin exactly like Register() does: Twin holds User reference
            var twin = new UserTwin { User = user };

            db.Users.Add(user);
            db.UserTwins.Add(twin);

            await db.SaveChangesAsync();

            // ----------------------------
            // 6) Give the user a simple preference profile (so ranking is non-zero)
            // ----------------------------
            db.CategoryWeights.AddRange(
                new CategoryWeights
                {
                    UserTwinId = twin.Id,
                    CategoryId = catHistory.Id,
                    Score = 0.90,
                },
                new CategoryWeights
                {
                    UserTwinId = twin.Id,
                    CategoryId = catCulture.Id,
                    Score = 0.60,
                },
                new CategoryWeights
                {
                    UserTwinId = twin.Id,
                    CategoryId = catNature.Id,
                    Score = 0.30,
                }
            );

            // Optional: seed place likes too
            db.PlaceLikes.AddRange(
                new PlaceLikes
                {
                    UserTwinId = twin.Id,
                    PlaceId = castelo.Id,
                    Score = 0.80,
                },
                new PlaceLikes
                {
                    UserTwinId = twin.Id,
                    PlaceId = praca.Id,
                    Score = 0.40,
                },
                new PlaceLikes
                {
                    UserTwinId = twin.Id,
                    PlaceId = ponte.Id,
                    Score = 0.20,
                }
            );

            await db.SaveChangesAsync();
        }
    }
}
