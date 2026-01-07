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
            Name = "Ponte Ondolada",
            Latitude = 39.7460861,
            Longitude = -8.8036494,
            CategoryId = catNature.Id,
            Active = true,
        };

        var house = new Place
        {
            Name = "House",
            Latitude = 39.742301,
            Longitude = -8.812726,
            CategoryId = catNature.Id,
            Active = true,
        };

        db.Places.AddRange(castelo, praca, ponte, house);
        await db.SaveChangesAsync();

        // ----------------------------
        // 3) VirtualPlaces (1 per place)
        // ----------------------------
        var vpCastelo = new VirtualPlace { PlaceId = castelo.Id };
        var vpPraca = new VirtualPlace { PlaceId = praca.Id };
        var vpPonte = new VirtualPlace { PlaceId = ponte.Id };
        var vpHouse = new VirtualPlace { PlaceId = house.Id };

        db.VirtualPlaces.AddRange(vpCastelo, vpPraca, vpPonte, vpHouse);
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

        const string coinObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/coin.obj";
        const string coinBagObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/coin_bag.obj";
        const string duckObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/duck.obj";
        const string guardObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/guard.obj";
        const string lanternObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/lantern.obj";
        const string scrollObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/scroll.obj";
        const string shieldObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/shield.obj";
        const string swordObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/exports/sword.obj";

        const string castleTexAsset = "models/castle_albedo.png";
        const string bridgeTexAsset = "models/bridge_albedo.png";

        const string coinAlbedo = "models/coin_albedo.png";
        const string coinBagAlbedo = "models/coin-bag_albedo.png";
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
            // ============================================================
            // CASTELO - Historical figures
            // ============================================================
            new VirtualObject
            {
                Name = "Castelo - Guard",
                VirtualPlaceId = vpCastelo.Id,
                ModelUrl = guardObj,
                ModelUrlTexture = guardAlbedo,
                PX = -2f,
                PY = 1.5f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Castle Guard",
                StepsJson = Steps(
                    ("intro", "Guard", "Medieval castle defender."),
                    ("rotate", "Inspect", "Check the armor details."),
                    ("complete", "Done", "Guard inspected!")
                ),
            },
            new VirtualObject
            {
                Name = "Castelo - Shield",
                VirtualPlaceId = vpCastelo.Id,
                ModelUrl = shieldObj,
                ModelUrlTexture = shieldAlbedo,
                PX = 2f,
                PY = 1.5f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Medieval Shield",
                StepsJson = Steps(
                    ("intro", "Shield", "Defensive armor piece."),
                    ("rotate", "Details", "Look at the heraldic symbols."),
                    ("complete", "Done", "Shield inspected!")
                ),
            },
            // ============================================================
            // PRAÇA - Cultural items
            // ============================================================
            new VirtualObject
            {
                Name = "Praça - Lantern",
                VirtualPlaceId = vpPraca.Id,
                ModelUrl = lanternObj,
                ModelUrlTexture = lanternAlbedo,
                PX = -2f,
                PY = 1.5f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Historic Lantern",
                StepsJson = Steps(
                    ("intro", "Lantern", "Square illumination."),
                    ("rotate", "Inspect", "View the lantern design."),
                    ("complete", "Done", "Lantern inspected!")
                ),
            },
            new VirtualObject
            {
                Name = "Praça - Scroll",
                VirtualPlaceId = vpPraca.Id,
                ModelUrl = scrollObj,
                ModelUrlTexture = scrollAlbedo,
                PX = 2f,
                PY = 1.0f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Ancient Scroll",
                StepsJson = Steps(
                    ("intro", "Scroll", "Historical document."),
                    ("rotate", "Read", "See the ancient text."),
                    ("complete", "Done", "Scroll inspected!")
                ),
            },
            // ============================================================
            // PONTE - Nature/Bridge themed
            // ============================================================
            new VirtualObject
            {
                Name = "Ponte - Duck",
                VirtualPlaceId = vpPonte.Id,
                ModelUrl = duckObj,
                ModelUrlTexture = duckAlbedo,
                PX = -2f,
                PY = 0.5f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "River Duck",
                StepsJson = Steps(
                    ("intro", "Duck", "Bridge wildlife."),
                    ("rotate", "Observe", "Watch the duck."),
                    ("complete", "Done", "Duck observed!")
                ),
            },
            new VirtualObject
            {
                Name = "Ponte - Coin Bag",
                VirtualPlaceId = vpPonte.Id,
                ModelUrl = coinBagObj,
                ModelUrlTexture = coinBagAlbedo,
                PX = 2f,
                PY = 0.3f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Bridge Toll",
                StepsJson = Steps(
                    ("intro", "Toll", "Ancient bridge payment."),
                    ("rotate", "Coins", "See the old coins."),
                    ("complete", "Done", "Toll inspected!")
                ),
            },
            // ============================================================
            // HOUSE - Simple demo object
            // ============================================================
            new VirtualObject
            {
                Name = "House - Coin",
                VirtualPlaceId = vpHouse.Id,
                ModelUrl = coinObj,
                ModelUrlTexture = coinAlbedo,
                PX = 0f,
                PY = 0.5f,
                PZ = -2f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.3f,
                SY = 0.3f,
                SZ = 0.3f,
                TextDisplayed = "Lucky Coin",
                StepsJson = Steps(
                    ("intro", "Coin", "A shiny coin."),
                    ("rotate", "Flip", "Rotate the coin."),
                    ("complete", "Done", "Coin collected!")
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
                UserName = "test",
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("test")),
            };

            var twin = new UserTwin { User = user };

            db.Users.Add(user);
            db.UserTwins.Add(twin);

            await db.SaveChangesAsync();

            // ----------------------------
            // 6) Give the user a simple preference profile
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
