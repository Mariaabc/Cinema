using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Connection
{
    public class ActorMovieController : Controller
    {
        private MovieDBContext dm = new MovieDBContext();
        private ActorMovieDBContext dam = new ActorMovieDBContext();
        private ActorDBContext da = new ActorDBContext();

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(int ActorId, int MovieId, string Group)
        {
            Actor Actor = da.Actors.Find(ActorId);
            if (Actor.Description != null)
                if (Actor.Description.Length > 200)
                    Actor.Description = (Actor.Description).Substring(0, 200);
            ViewBag.Actor = Actor;

            Movie Movie = dm.Movies.Find(MovieId);
            if (Movie.Description != null)
                if (Movie.Description.Length > 200)
                    Movie.Description = (Movie.Description).Substring(0, 200);
            ViewBag.Movie = Movie;

            ViewBag.Group = Group;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(ActorMovie requestActorMovie, string Group)
        {
            ActorMovie am = new ActorMovie();
            am.MovieId = requestActorMovie.MovieId;
            am.ActorId = requestActorMovie.ActorId;
            am.Role = requestActorMovie.Role;
            am.CharacterName = requestActorMovie.CharacterName;

            dam.ActorMovies.Add(am);
            dam.SaveChanges();

            if (Group.Equals("Actor"))
                return RedirectToAction("Edit", Group, new { id = requestActorMovie.ActorId });
            else
                return RedirectToAction("Edit", Group, new { id = requestActorMovie.MovieId });
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(ActorMovie requestActorMovie, string Group)
        {
            ActorMovie conn = dam.ActorMovies.Where(p => p.ActorId == requestActorMovie.ActorId &&
            p.MovieId == requestActorMovie.MovieId).FirstOrDefault();

            dam.ActorMovies.Remove(conn);
            dam.SaveChanges();

            if (Group.Equals("Actor"))
                return RedirectToAction("Edit", Group, new { id = requestActorMovie.ActorId });
            else
                return RedirectToAction("Edit", Group, new { id = requestActorMovie.MovieId });
        }

    }
}