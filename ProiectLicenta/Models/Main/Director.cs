using ProiectLicenta.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }

        public virtual ICollection<DirectorMovie> DirectorMovies { get; set; }
    }

    public class DirectorDBContext : DbContext
    {
        public DirectorDBContext() : base("DBConnection") { }
        public DbSet<Director> Directors { get; set; }
    }
}