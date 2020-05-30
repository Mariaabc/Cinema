using Microsoft.AspNet.Identity;
using ProiectLicenta.Models;
using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Main
{
    public class ActorController : Controller
    {
        private CommentDBContext dc = new CommentDBContext();
        private ActorDBContext da = new ActorDBContext();
        private ActorRatingDBContext dar = new ActorRatingDBContext();
        private ActorMovieDBContext dcam = new ActorMovieDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private Models.ApplicationDbContext db = Models.ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var actors = from actor in da.Actors
                         orderby actor.Name
                         select actor;

            foreach (var a in actors)
            {
                if (a.Description != null)
                    if (a.Description.Length > 200)
                        a.Description = (a.Description).Substring(0, 200);

                List<int> userR = dar.ActorRatings.Where(p => p.ActorId == a.Id).Select(p => p.Value).ToList();

                if (userR.Count() != 0)
                {
                    a.AverageRating = (float)userR.Sum() / userR.Count();
                }
                else
                {
                    a.AverageRating = 10;
                }

                string userid = User.Identity.GetUserId();
                userR = dar.ActorRatings.Where(p => p.ActorId == a.Id && p.UserId == userid).Select(p => p.Value).ToList();
                a.UserRating = userR.Sum();
            }

            ViewBag.Actors = actors;
            return View();
        }

        public ActionResult Search(string KeyWord)
        {
            List<Actor> actors = da.Actors.Where(p => p.Name.Contains(KeyWord)).Select(p => p).ToList();
            ViewBag.KeyWord = KeyWord;

            foreach (var a in actors)
            {
                if (a.Description != null)
                    if (a.Description.Length > 200)
                        a.Description = (a.Description).Substring(0, 200);
                List<int> userR = dar.ActorRatings.Where(p => p.ActorId == a.Id).Select(p => p.Value).ToList();

                if (userR.Count() != 0)
                {
                    a.AverageRating = (float)userR.Sum() / userR.Count();
                }
                else
                {
                    a.AverageRating = 10;
                }

                string userid = User.Identity.GetUserId();
                userR = dar.ActorRatings.Where(p => p.ActorId == a.Id && p.UserId == userid).Select(p => p.Value).ToList();
                a.UserRating = userR.Sum();
            }

            ViewBag.Actors = actors;
            return View();
        }

        public ActionResult Show(int id)
        {
            Actor actor = da.Actors.Find(id);
            ViewBag.Actor = actor;

            // rating utilizator
            List<int> gg = dar.ActorRatings.Where(p => p.ActorId == id).Select(p => p.Value).ToList();
            float avg = 0;
            if (gg.Count() != 0)
            {
                avg = (float)gg.Sum() / gg.Count();
            }

            ViewBag.AverageRating = avg;

            if (User.Identity.IsAuthenticated)
            {
                string userid = User.Identity.GetUserId();
                gg = dar.ActorRatings.Where(p => p.ActorId == id && p.UserId == userid).Select(p => p.Value).ToList();
                ViewBag.UserRating = gg.Sum();
            }
            else actor.UserRating = 0;
            // rating utilizator

            // comentarii
            List<Comment> comment = dc.Comments.Where(p => p.Group.Equals("Actor") && p.IdGroup == id && p.IdParent == 0).Select(p => p).ToList();

            foreach (var c in comment)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.Comments = comment;

            List<Comment> commentc = dc.Comments.Where(p => p.Group.Equals("Actor") && p.IdGroup == id && p.IdParent != 0).Select(p => p).ToList();
            foreach (var c in commentc)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.ChildComments = commentc;

            // comentarii

            List<ActorMovie> movie = dcam.ActorMovies.Where(p => p.Actor.Id == id).Select(p => p).OrderBy(p => p.Movie.ReleaseDate).ToList();
            foreach (var m in movie)
            {
                m.Movie = dm.Movies.Find(m.MovieId);
            }
            ViewBag.Movies = movie;

            ViewBag.UserId = User.Identity.GetUserId();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                List<ActorRating> aa = dar.ActorRatings.Where(p => p.UserId.Equals(userId) && p.ActorId == id).Select(p => p).ToList();
                ViewBag.MovieRatings = aa;
            }

            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Actor actor)
        {
            try
            {
                da.Actors.Add(actor);
                da.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        //  [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Actor actor = da.Actors.Find(id);
            ViewBag.Actor = actor;

            List<ActorMovie> am = dcam.ActorMovies.Where(p => p.Actor.Id == id).Select(p => p).ToList();
            ViewBag.CurrentMovies = am;
            List<Movie> m = dm.Movies.OrderBy(p => p.Name).ToList();
            foreach (var mov in am)
            {
                mov.Movie = dm.Movies.Where(p => p.Id == mov.MovieId).FirstOrDefault();
                var index = m.FindIndex(at => at.Id == mov.MovieId);
                if (index >= 0)
                {
                    m.RemoveAt(index);
                }
            }

            ViewBag.Movies = m;

            return View();
        }
        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, Actor requestActor)
        {
            try
            {
                Actor actor = da.Actors.Find(id);
                if (TryUpdateModel(actor))
                {
                    actor.Name = requestActor.Name;
                    actor.Description = requestActor.Description;
                    actor.BirthDate = requestActor.BirthDate;
                    actor.Image = requestActor.Image;
                    da.SaveChanges();
                }
                return RedirectToAction("Show", new { id = id });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            List<ActorMovie> con = dcam.ActorMovies.Where(p => p.Movie.Id == id).ToList();

            foreach (ActorMovie c in con)
            {
                dcam.ActorMovies.Remove(c);
                dcam.SaveChanges();
            }

            Actor actor = da.Actors.Find(id);
            da.Actors.Remove(actor);
            da.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
