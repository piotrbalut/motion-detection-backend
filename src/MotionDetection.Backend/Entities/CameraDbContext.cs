using Microsoft.EntityFrameworkCore;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend.Entities
{
	public class CameraDbContext : DbContext
	{
		public DbSet<Location> Locations { get; set; }
		public DbSet<Camera> Cameras { get; set; }

		protected override void OnConfiguring(
			DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseSqlite("Data Source=camera.db");
	}
}