using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;

namespace ProiectLicenta.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DBConnection", throwIfV1Schema: false)
        {
        }
                public DbSet<ActorMovie> ActorMovies { get; set; }        public DbSet<DirectorMovie> DirectorMovies { get; set; }        public DbSet<GenreMovie> GenreMovies { get; set; }        public DbSet<StudioMovie> StudioMovies { get; set; }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }        public DbSet<ActorRating> ActorRatings { get; set; }        public DbSet<Comment> Comments { get; set; }        public DbSet<Director> Directors { get; set; }        public DbSet<Genre> Genres { get; set; }        public DbSet<MovieRating> MovieRatings { get; set; }        public DbSet<Schedule> Schedules { get; set; }        public DbSet<Studio> Studios { get; set; }        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}