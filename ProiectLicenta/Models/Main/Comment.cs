using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string Group { get; set; }
        public int IdGroup { get; set; }
        public int IdParent { get; set; }
        public DateTime DT { get; set; }
        public bool IsApproved { get; set; }
        [NotMapped]
        public string CurentUser { get; set; }
        [NotMapped]
        public string CurentUserName { get; set; }
        [NotMapped]
        public int IsAResponse { get; set; }

    }

    public class CommentDBContext : DbContext
    {
        public CommentDBContext() : base("DBConnection") { }
        public DbSet<Comment> Comments { get; set; }
    }
}