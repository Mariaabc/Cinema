using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProiectLicenta.Models.Main
{
    public class Schedule
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public DateTime BeginDateTime { get; set; }
        public string Duration { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }

    public class ScheduleDBContext : DbContext
    {
        public ScheduleDBContext() : base("DBConnection") { }
        public DbSet<Schedule> Schedules { get; set; }
    }
}