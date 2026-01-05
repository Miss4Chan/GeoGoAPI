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
            "https://github.com/lCubosl/geogo-objects/blob/main/castle_test.obj";

        const string pracaObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/main/coin.obj";

        const string ponteObj =
            "https://raw.githubusercontent.com/lCubosl/geogo-objects/refs/heads/main/bridge_mesh.obj";

        const string castleTexAsset = "models/castle_albedo.png";
        const string coinTexAsset = "models/coin_albedo.png";
        const string bridgeTexAsset = "models/bridge_albedo.png";

        db.VirtualObjects.AddRange(
            // Castelo
            new VirtualObject
            {
                Name = "Castelo - Object A",
                VirtualPlaceId = vpCastelo.Id,
                ModelUrl = casteloObj,
                ModelUrlTexture = castleTexAsset,
                PX = -0.2f,
                PY = 0f,
                PZ = -1.3f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.5f,
                SY = 0.5f,
                SZ = 0.5f,
                TextDisplayed = "Welcome to the castle!",
            },
            // Praça
            new VirtualObject
            {
                Name = "Praça - Object A",
                VirtualPlaceId = vpPraca.Id,
                ModelUrl = pracaObj,
                ModelUrlTexture = coinTexAsset,
                PX = -0.2f,
                PY = 0f,
                PZ = -1.3f,
                RX = 0f,
                RY = 0f,
                RZ = 0f,
                SX = 0.5f,
                SY = 0.5f,
                SZ = 0.5f,
                TextDisplayed = "Central square vibe.",
            },
            // Ponte
            new VirtualObject
            {
                Name = "Ponte - Object A",
                VirtualPlaceId = vpPonte.Id,
                ModelUrl = ponteObj,
                ModelUrlTexture = bridgeTexAsset,
                PX = 0f,
                PY = 0f,
                PZ = -1.4f,
                RX = 0f,
                RY = 15f,
                RZ = 0f,
                SX = 0.55f,
                SY = 0.55f,
                SZ = 0.55f,
                TextDisplayed = "Bridge checkpoint.",
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
