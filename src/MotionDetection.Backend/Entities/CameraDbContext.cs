using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend.Entities
{
	public class CameraDbContext : IdentityDbContext<User>
    {
        public DbSet<Location> Locations { get; set; }
		public DbSet<Camera> Cameras { get; set; }

		protected override void OnConfiguring(
			DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseSqlite("Data Source=camera.db");

	    protected override void OnModelCreating(ModelBuilder modelBuilder)
	    {
		    base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCamera>()
		                .HasKey(pc => new { pc.UserId, pc.CameraId });

		    modelBuilder.Entity<UserCamera>()
		                .HasOne(pc => pc.User)
		                .WithMany(p => p.UserCameras)
		                .HasForeignKey(pc => pc.UserId);

		    modelBuilder.Entity<UserCamera>()
		                .HasOne(pc => pc.Camera)
		                .WithMany(c => c.UserCameras)
		                .HasForeignKey(pc => pc.CameraId);
	    }
    }
}