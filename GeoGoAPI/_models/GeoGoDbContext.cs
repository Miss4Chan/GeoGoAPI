using GeoGoAPI._models.entities;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._models;

public class GeoGoDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserTwin> UserTwins { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryWeights> CategoryWeights { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<VirtualPlace> VirtualPlaces { get; set; }
    public DbSet<VirtualObject> VirtualObjects { get; set; }
    public DbSet<PlaceLikes> PlaceLikes { get; set; }
    public DbSet<InteractionEvent> InteractionEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasIndex(u => u.UserName).IsUnique();
            e.HasOne(u => u.Twin)
                .WithOne(t => t.User!)
                .HasForeignKey<UserTwin>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.Property(c => c.Name).IsRequired();
            e.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<CategoryWeights>(e =>
        {
            e.HasKey(cw => new { cw.UserTwinId, cw.CategoryId });

            e.HasOne(cw => cw.UserTwin)
                .WithMany(t => t.CategoryWeights)
                .HasForeignKey(cw => cw.UserTwinId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(cw => cw.Category)
                .WithMany(c => c.CategoryWeights)
                .HasForeignKey(cw => cw.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Place>(e =>
        {
            e.Property(p => p.Name).IsRequired();

            e.HasQueryFilter(p => !p.IsDeleted);

            e.HasOne(p => p.Category)
                .WithMany(c => c.Places)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(p => p.PlaceLikes)
                .WithOne(pl => pl.Place!)
                .HasForeignKey(pl => pl.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.VirtualPlace)
                .WithOne(vp => vp.Place!)
                .HasForeignKey<VirtualPlace>(vp => vp.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VirtualPlace>(e =>
        {
            e.HasIndex(vp => vp.PlaceId).IsUnique();
            e.HasQueryFilter(vp => !vp.Place!.IsDeleted);
            e.HasMany(vp => vp.VirtualObjects)
                .WithOne(vo => vo.VirtualPlace!)
                .HasForeignKey(vo => vo.VirtualPlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(vp => vp.InteractionEvents)
                .WithOne(ie => ie.VirtualPlace!)
                .HasForeignKey(ie => ie.VirtualPlaceId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<VirtualObject>(e =>
        {
            e.Property(vo => vo.Name).IsRequired();
            e.Property(vo => vo.ModelUrl).IsRequired();

            e.HasQueryFilter(vo => !vo.VirtualPlace!.Place!.IsDeleted);

            e.HasMany(vo => vo.InteractionEvents)
                .WithOne(ie => ie.VirtualObject!)
                .HasForeignKey(ie => ie.VirtualObjectId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PlaceLikes>(e =>
        {
            e.HasKey(pl => new { pl.UserTwinId, pl.PlaceId });
            e.HasQueryFilter(pl => !pl.Place!.IsDeleted);
            e.HasOne(pl => pl.UserTwin)
                .WithMany(t => t.PlaceLikes)
                .HasForeignKey(pl => pl.UserTwinId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pl => pl.Place)
                .WithMany(p => p.PlaceLikes)
                .HasForeignKey(pl => pl.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(pl => pl.InteractionEvents)
                .WithOne(ie => ie.PlaceLike!)
                .HasForeignKey(ie => new { ie.UserTwinId, ie.PlaceId })
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<InteractionEvent>(e =>
        {
            e.Property(ie => ie.EventType).IsRequired();
            e.Property(ie => ie.Timestamp)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasOne(ie => ie.UserTwin)
                .WithMany()
                .HasForeignKey(ie => ie.UserTwinId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
