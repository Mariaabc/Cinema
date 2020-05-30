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
    public class GenreMovie
    {
        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key, Column(Order = 1)]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }

    public class GenreMovieDBContext : DbContext
    {
        public GenreMovieDBContext() : base("DBConnection") { }
        public DbSet<GenreMovie> GenreMovies { get; set; }
    }
}