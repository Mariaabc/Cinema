using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class MovieRating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string UserId { get; set; }
        public string RatingType { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }

    public class MovieRatingDBContext : DbContext
    {
        public MovieRatingDBContext() : base("DBConnection") { }
        public DbSet<MovieRating> MovieRatings { get; set; }
    }
}