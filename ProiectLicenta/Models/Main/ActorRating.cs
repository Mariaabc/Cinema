using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class ActorRating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string UserId { get; set; }
        public string RatingType { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }

    public class ActorRatingDBContext : DbContext
    {
        public ActorRatingDBContext() : base("DBConnection") { }
        public DbSet<ActorRating> ActorRatings { get; set; }
    }
}
