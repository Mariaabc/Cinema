using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Connection
{
    public class GenreMovieController : Controller
    {
        private MovieDBContext dm = new MovieDBContext();
        private GenreMovieDBContext dgm = new GenreMovieDBContext();
        private GenreDBContext dg = new GenreDBContext();


        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(GenreMovie requestGenreMovie)
        {

            GenreMovie gm = new GenreMovie();
            gm.MovieId = requestGenreMovie.MovieId;
            gm.GenreId = requestGenreMovie.GenreId;

            dgm.GenreMovies.Add(gm);
            dgm.SaveChanges();

            return RedirectToAction("Edit", "Movie", new { id = requestGenreMovie.MovieId });
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(GenreMovie requestGenreMovie, string Group)
        {
            GenreMovie gm = dgm.GenreMovies.Where(p => p.GenreId == requestGenreMovie.GenreId && p.MovieId == requestGenreMovie.MovieId).FirstOrDefault();
            dgm.GenreMovies.Remove(gm);
            dgm.SaveChanges();

            if (Group.Equals("Genre"))
                return RedirectToAction("Edit", Group, new { id = requestGenreMovie.GenreId });
            else
                return RedirectToAction("Edit", Group, new { id = requestGenreMovie.MovieId });

        }
    }
}