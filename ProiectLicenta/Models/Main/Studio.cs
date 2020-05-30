using ProiectLicenta.Models.Connection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<StudioMovie> StudioMovies { get; set; }
    }

    public class StudioDBContext : DbContext
    {
        public StudioDBContext() : base("DBConnection") { }
        public DbSet<Studio> Studios { get; set; }
    }
}