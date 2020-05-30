using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Connection
{
    public class DirectorMovieController : Controller
    {
        private MovieDBContext dm = new MovieDBContext();
        private DirectorMovieDBContext ddm = new DirectorMovieDBContext();
        private DirectorDBContext ds = new DirectorDBContext();

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(DirectorMovie requestDirectorMovie, string Group)
        {
            DirectorMovie a = new DirectorMovie();
            a.MovieId = requestDirectorMovie.MovieId;
            a.DirectorId = requestDirectorMovie.DirectorId;

            ddm.DirectorMovies.Add(a);
            ddm.SaveChanges();

            if (Group.Equals("Director"))
                return RedirectToAction("Edit", Group, new { id = requestDirectorMovie.DirectorId });
            else
                return RedirectToAction("Edit", Group, new { id = requestDirectorMovie.MovieId });
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(DirectorMovie requestDirectorMovie, string Group)
        {
            DirectorMovie conn = ddm.DirectorMovies.Where(p => p.DirectorId == requestDirectorMovie.DirectorId && p.MovieId == requestDirectorMovie.MovieId).FirstOrDefault();
            ddm.DirectorMovies.Remove(conn);
            ddm.SaveChanges();

            if (Group.Equals("Director"))
                return RedirectToAction("Edit", Group, new { id = requestDirectorMovie.DirectorId });
            else
                return RedirectToAction("Edit", Group, new { id = requestDirectorMovie.MovieId });
        }

    }
}