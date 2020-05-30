using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Connection
{
    public class ActorMovie
    {

        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key, Column(Order = 1)]
        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public string Role { get; set; }
        public string CharacterName { get; set; }
    }

    public class ActorMovieDBContext : DbContext
    {
        public ActorMovieDBContext() : base("DBConnection") { }
        public DbSet<ActorMovie> ActorMovies { get; set; }

    }
}
