using ProiectLicenta.Models.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime BirthDate { get; set; }
        [NotMapped]
        public float AverageRating { get; set; }
        [NotMapped]
        public float UserRating { get; set; }

        public virtual ICollection<ActorRating> ActorRatings { get; set; }
        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
    }

    public class ActorDBContext : DbContext
    {
        public ActorDBContext() : base("DBConnection") { }
        public DbSet<Actor> Actors { get; set; }
    }
}