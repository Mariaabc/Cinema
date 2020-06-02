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
    public class StudioMovie
    {
        [Key, Column(Order = 0)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key, Column(Order = 1)]
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
    }

    public class StudioMovieDBContext : DbContext
    {
        public StudioMovieDBContext() : base("DBConnection") { }
        public DbSet<StudioMovie> StudioMovies { get; set; }
    }
}