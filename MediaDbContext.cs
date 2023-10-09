using Microsoft.EntityFrameworkCore;
using MediaCollectionMVC.Models;

namespace MediaCollectionMVC
{
    public class MediaDbContext : DbContext
    {
        public MediaDbContext(DbContextOptions<MediaDbContext> options): base(options)
        {

        }

        public DbSet<ScannedMedium> ScannedMedia { get; set; } = null!;
    }
}
