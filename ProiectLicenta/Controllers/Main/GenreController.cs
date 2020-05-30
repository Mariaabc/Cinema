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
    public class GenreController : Controller
    {
        private CommentDBContext dc = new CommentDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private GenreMovieDBContext dcgm = new GenreMovieDBContext();
        private GenreDBContext dg = new GenreDBContext();
        private MovieRatingDBContext dmr = new MovieRatingDBContext();
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Show(int id)
        {
            var genres = from genr in dg.Genres
                         orderby genr.Name
                         select genr;
            ViewBag.Genres = genres;

            Genre genre = dg.Genres.Find(id);
            ViewBag.Genre = genre;

            List<Movie> movies = dcgm.GenreMovies.Where(p => p.Genre.Id == id).Select(p => p.Movie).ToList();

            foreach (var m in movies)
            {
                if (m.Description != null)
                    if (m.Description.Length > 200)
                        m.Description = (m.Description).Substring(0, 200);

                List<int> ratings = dmr.MovieRatings.Where(p => p.MovieId == m.Id).Select(p => p.Value).ToList();

                m.AverageRating = 10;
                if (ratings.Count() != 0)
                {
                    m.AverageRating = (float)ratings.Sum() / ratings.Count();
                }

                string userid = User.Identity.GetUserId();
                m.UserRating = dmr.MovieRatings.Where(p => p.MovieId == m.Id && p.UserId == userid).Select(p => p.Value).FirstOrDefault();
            }

            ViewBag.Movies = movies;

            return View();
        }

        public ActionResult Search(int id, string KeyWord)
        {
            var genres = from genr in dg.Genres
                         orderby genr.Id
                         select genr;
            ViewBag.Genres = genres;

            Genre genre = dg.Genres.Find(id);
            ViewBag.Genre = genre;

            List<Movie> mov = dcgm.GenreMovies.Where(p => p.Genre.Id == id).Select(p => p.Movie).ToList();
            List<Movie> movies = mov.Where(p => p.Name.Contains(KeyWord)).Select(p => p).ToList();

            ViewBag.KeyWord = KeyWord;

            foreach (var m in movies)
            {
                if (m.Description != null)
                    if (m.Description.Length > 160)
                        m.Description = (m.Description).Substring(0, 160);

                List<int> g = dmr.MovieRatings.Where(p => p.MovieId == m.Id).Select(p => p.Value).ToList();

                float avg = 10;
                if (g.Count() != 0)
                {
                    avg = (float)g.Sum() / g.Count();
                }

                m.AverageRating = avg;

                string userid = User.Identity.GetUserId();
                int a = dmr.MovieRatings.Where(p => p.MovieId == m.Id && p.UserId == userid).Select(p => p.Value).FirstOrDefault();
                m.UserRating = a;
            }

            ViewBag.Movies = movies;
            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Genre genre)
        {
            try
            {
                dg.Genres.Add(genre);
                dg.SaveChanges();
                return RedirectToAction("Show", "Genre", new { id = genre.Id });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Genre genre = dg.Genres.Find(id);
            ViewBag.Genre = genre;


            List<Movie> cmovies = dcgm.GenreMovies.Where(p => p.Genre.Id == id).Select(p => p.Movie).ToList();

            foreach (var m in cmovies)
            {
                if (m.Description != null)
                    if (m.Description.Length > 200)
                        m.Description = (m.Description).Substring(0, 200);
            }
            ViewBag.CurrentMovies = cmovies;

            var movies = from movie in dm.Movies
                         orderby movie.Name
                         select movie;
            ViewBag.Movies = movies;

            return View();

        }
        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int Id, Genre requestGenre)
        {
            try
            {
                Genre genre = dg.Genres.Find(Id);
                if (TryUpdateModel(genre))
                {
                    genre.Name = requestGenre.Name;
                    genre.Description = requestGenre.Description;
                    dg.SaveChanges();
                }
                return RedirectToAction("Show", new { id = Id });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            List<GenreMovie> conn = dcgm.GenreMovies.Where(p => p.Genre.Id == id).ToList();

            foreach (GenreMovie c in conn)
            {
                dcgm.GenreMovies.Remove(c);
                dcgm.SaveChanges();
            }


            Genre genre = dg.Genres.Find(id);
            dg.Genres.Remove(genre);
            dg.SaveChanges();
            return RedirectToAction("Index", "Movie");
        }
    }
}