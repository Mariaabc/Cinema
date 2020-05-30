using ProiectLicenta.Models.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Image { get; set; }
        public string YTTrailerLink { get; set; }
        public bool ApprovedComments { get; set; }
        [NotMapped]
        public float AverageRating { get; set; }
        [NotMapped]
        public float UserRating { get; set; }

        public virtual ICollection<MovieRating> MovieRatings { get; set; }
        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<DirectorMovie> DirectorMovies { get; set; }
        public virtual ICollection<GenreMovie> GenreMovies { get; set; }
        public virtual ICollection<StudioMovie> StudioMovies { get; set; }
    }

    public class MovieDBContext : DbContext
    {
        public MovieDBContext() : base("DBConnection") { }
        public DbSet<Movie> Movies { get; set; }
    }
}
