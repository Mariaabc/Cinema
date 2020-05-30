using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Main
{
    public class ScheduleController : Controller
    {
        private ScheduleDBContext ds = new ScheduleDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private MovieRatingDBContext dmr = new MovieRatingDBContext();

        public ActionResult Index()
        {

            var schedules = from schedule in ds.Schedules
                            orderby schedule.BeginDateTime
                            select schedule;

            foreach (var sch in schedules)
            {
                sch.Movie = dm.Movies.Find(sch.MovieId);
            }
            ViewBag.Schedules = schedules;

            List<Schedule> s = schedules.ToList();
            if(s.Count!=0)
            if (s[0].BeginDateTime.Date == DateTime.Now.Date)
            {
                ViewBag.Ok = "1";
            }
            ViewBag.Day = DateTime.Now;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            var movies = from movie in dm.Movies
                         orderby movie.Id
                         select movie;
            ViewBag.Movies = movies;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Schedule schedule, DateTime Time)
        {
            schedule.BeginDateTime = new DateTime(schedule.BeginDateTime.Year, schedule.BeginDateTime.Month,
                schedule.BeginDateTime.Day, Time.Hour, Time.Minute, Time.Second);

            ds.Schedules.Add(schedule);
            ds.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Schedule schedule = ds.Schedules.Find(id);
            ds.Schedules.Remove(schedule);
            ds.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}