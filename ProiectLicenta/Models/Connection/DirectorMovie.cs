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
    public class DirectorMovie
    {
        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key, Column(Order = 1)]
        public int DirectorId { get; set; }
        public Director Director { get; set; }
    }

    public class DirectorMovieDBContext : DbContext
    {
        public DirectorMovieDBContext() : base("DBConnection") { }
        public DbSet<DirectorMovie> DirectorMovies { get; set; }
    }
}