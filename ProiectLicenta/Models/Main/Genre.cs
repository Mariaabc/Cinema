using ProiectLicenta.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GenreMovie> GenreMovies { get; set; }
    }

    public class GenreDBContext : DbContext
    {
        public GenreDBContext() : base("DBConnection") { }
        public DbSet<Genre> Genres { get; set; }
    }
}