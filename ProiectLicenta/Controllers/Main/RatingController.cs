using Microsoft.AspNet.Identity;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Main
{
    public class RatingController : Controller
    {
        private MovieRatingDBContext dmr = new MovieRatingDBContext();
        private ActorRatingDBContext dar = new ActorRatingDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private ActorDBContext da = new ActorDBContext();

        public ActionResult Index()
        {
            var uid = User.Identity.GetUserId();

            List<ActorRating> actorRatings = dar.ActorRatings.Where(p => p.UserId == uid).ToList();
            foreach (var a in actorRatings)
            {
                a.Actor = da.Actors.Find(a.ActorId);
                if (a.Actor.Description != null)
                    if (a.Actor.Description.Length > 200)
                        a.Actor.Description = (a.Actor.Description).Substring(0, 200);

                List<int> userR = dar.ActorRatings.Where(p => p.ActorId == a.Actor.Id).Select(p => p.Value).ToList();

                if (userR.Count() != 0)
                {
                    a.Actor.AverageRating = (float)userR.Sum() / userR.Count();
                }
                else
                {
                    a.Actor.AverageRating = 10;
                }

                string userid = User.Identity.GetUserId();
                userR = dar.ActorRatings.Where(p => p.ActorId == a.Actor.Id && p.UserId == userid).Select(p => p.Value).ToList();
                a.Actor.UserRating = userR.Sum();
            }
            ViewBag.ActorRatings = actorRatings;


            List<MovieRating> movieRatings = dmr.MovieRatings.Where(p => p.UserId == uid).ToList();
            foreach (var m in movieRatings)
            {
                m.Movie = dm.Movies.Find(m.MovieId);
                if (m.Movie.Description != null)
                    if (m.Movie.Description.Length > 200)
                        m.Movie.Description = (m.Movie.Description).Substring(0, 200);

                List<int> ratings = dmr.MovieRatings.Where(p => p.MovieId == m.Movie.Id).Select(p => p.Value).ToList();

                m.Movie.AverageRating = 10;
                if (ratings.Count() != 0)
                {
                    m.Movie.AverageRating = (float)ratings.Sum() / ratings.Count();
                }

                string userid = User.Identity.GetUserId();
                m.Movie.UserRating = dmr.MovieRatings.Where(p => p.MovieId == m.Movie.Id && p.UserId == userid).Select(p => p.Value).FirstOrDefault();

            }
            ViewBag.MovieRatings = movieRatings;

            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult NewA(ActorRating requestActorRating)
        {
            try
            {
                //List<MovieRating> mr = dmr.MovieRatings.Where(p => p.UserId.Equals(requestMovieRating.UserId) && p.MovieId == requestMovieRating.MovieId).ToList();
                List<ActorRating> ar = dar.ActorRatings.Where(p => p.UserId.Equals(requestActorRating.UserId) && p.ActorId == requestActorRating.ActorId).ToList();

                foreach (var a in ar)
                {
                    dar.ActorRatings.Remove(a);
                    dar.SaveChanges();
                }

                ActorRating nar = new ActorRating();

                nar.Value = requestActorRating.Value;
                nar.UserId = requestActorRating.UserId;
                nar.ActorId = requestActorRating.ActorId;
                nar.Actor = requestActorRating.Actor;

                dar.ActorRatings.Add(nar);
                dar.SaveChanges();

                return RedirectToAction("Show", "Actor", new { id = requestActorRating.ActorId });
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult NewM(MovieRating requestMovieRating)
        {
            try
            {
                List<MovieRating> mr = dmr.MovieRatings.Where(p => p.UserId.Equals(requestMovieRating.UserId) && p.MovieId == requestMovieRating.MovieId).ToList();

                foreach (var a in mr)
                {
                    dmr.MovieRatings.Remove(a);
                    dmr.SaveChanges();
                }

                MovieRating m = new MovieRating();

                m.Value = requestMovieRating.Value;
                m.UserId = requestMovieRating.UserId;
                m.MovieId = requestMovieRating.MovieId;
                m.Movie = requestMovieRating.Movie;

                dmr.MovieRatings.Add(m);
                dmr.SaveChanges();

                return RedirectToAction("Show", "Movie", new { id = requestMovieRating.MovieId });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult DeleteM(int id)
        {
            MovieRating mr = dmr.MovieRatings.Find(id);
            dmr.MovieRatings.Remove(mr);
            dmr.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteA(int id)
        {
            ActorRating ar = dar.ActorRatings.Find(id);
            dar.ActorRatings.Remove(ar);
            dar.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}