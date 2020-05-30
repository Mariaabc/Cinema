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
    public class MovieController : Controller
    {
        private CommentDBContext dc = new CommentDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private GenreDBContext dg = new GenreDBContext();
        private ActorDBContext da = new ActorDBContext();
        private StudioDBContext ds = new StudioDBContext();
        private DirectorDBContext dd = new DirectorDBContext();
        private GenreMovieDBContext dcgm = new GenreMovieDBContext();
        private ActorMovieDBContext dcam = new ActorMovieDBContext();
        private StudioMovieDBContext dcsm = new StudioMovieDBContext();
        private DirectorMovieDBContext dcdm = new DirectorMovieDBContext();
        private MovieRatingDBContext dmr = new MovieRatingDBContext();
        private Models.ApplicationDbContext db = Models.ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var movies = from movie in dm.Movies
                         orderby movie.Name
                         select movie;

            var genres = from genr in dg.Genres
                         orderby genr.Name
                         select genr;
            ViewBag.Genres = genres;

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

        public ActionResult Search(string KeyWord)
        {
            var genres = from genr in dg.Genres
                         orderby genr.Id
                         select genr;
            ViewBag.Genres = genres;

            List<Movie> movies = dm.Movies.Where(p => p.Name.Contains(KeyWord)).Select(p => p).ToList();
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

        public ActionResult Show(int id)
        {
            Movie movie = dm.Movies.Find(id);

            // comentarii
            List<Comment> comment = dc.Comments.Where(p => p.Group.Equals("Movie") && p.IdGroup == id && p.IdParent == 0).Select(p => p).ToList();

            foreach (var c in comment)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.Comments = comment;

            List<Comment> commentc = dc.Comments.Where(p => p.Group.Equals("Movie") && p.IdGroup == id && p.IdParent != 0).Select(p => p).ToList();
            foreach (var c in commentc)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = db.Users.Find(c.UserId).UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.ChildComments = commentc;
            //comentarii

            // rating utilizator
                List<int> gg = dmr.MovieRatings.Where(p => p.MovieId == id).Select(p => p.Value).ToList();

                float avg = 0;
                if (gg.Count() != 0)
                {
                    avg = (float)gg.Sum() / gg.Count();
                }

                ViewBag.AverageRating = avg;

                if (User.Identity.IsAuthenticated)
                {
                    string userid = User.Identity.GetUserId();
                    gg = dmr.MovieRatings.Where(p => p.MovieId == id && p.UserId == userid).Select(p => p.Value).ToList();
                    ViewBag.UserRating = gg.Sum();
                }
                else movie.UserRating = 0;
                ViewBag.Movie = movie;
            // rating utilizator

            List<Genre> g = dcgm.GenreMovies.Where(p => p.Movie.Id == id).Select(p => p.Genre).ToList();
            ViewBag.Genres = g;

            List<ActorMovie> a = dcam.ActorMovies.Where(p => p.Movie.Id == id).Select(p => p).ToList();
            foreach (var x in a)
            {
                x.Actor = da.Actors.Where(p => p.Id == x.ActorId).Select(p => p).FirstOrDefault();
            }
            ViewBag.Actors = a;

            List<Studio> studios = dcsm.StudioMovies.Where(p => p.Movie.Id == id).Select(p => p.Studio).ToList();
            foreach (var s in studios)
            {
                if (s.Description != null)
                    if (s.Description.Length > 200)
                        s.Description = (s.Description).Substring(0, 200);
            }
            ViewBag.Studios = studios;

            List<Director> directors = dcdm.DirectorMovies.Where(p => p.Movie.Id == id).Select(p => p.Director).ToList();
            foreach (var d in directors)
            {
                if (d.Description != null)
                    if (d.Description.Length > 200)
                        d.Description = (d.Description).Substring(0, 200);
            }
            ViewBag.Directors = directors;

            ViewBag.UserId = User.Identity.GetUserId();

            return View();
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(Movie movie)
        {
            try
            {
                movie = movie;
                movie.YTTrailerLink = movie.YTTrailerLink.Replace("watch?v=", "embed/");
                dm.Movies.Add(movie);
                dm.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Movie movie = dm.Movies.Find(id);
            ViewBag.Movie = movie;

            List<ActorMovie> am = dcam.ActorMovies.Where(p => p.Movie.Id == id).Select(p => p).ToList();
            ViewBag.CurrentActors = am;
            List<Actor> a = da.Actors.OrderBy(p => p.Name).ToList();
            foreach (var act in am)
            {
                act.Actor = da.Actors.Where(p => p.Id == act.ActorId).FirstOrDefault();
                var index = a.FindIndex(at => at.Id == act.ActorId);
                if (index >= 0)
                {
                    a.RemoveAt(index);
                }
            }
            ViewBag.Actors = a;

            List<Genre> g = dcgm.GenreMovies.Where(p => p.Movie.Id == id).Select(p => p.Genre).ToList();
            ViewBag.CurrentGenres = g;
            List<Genre> genres = dg.Genres.OrderBy(p => p.Name).ToList();
            foreach (var gen in g)
            {
                var index = genres.FindIndex(gr => gr.Id == gen.Id);
                if (index >= 0)
                {
                    genres.RemoveAt(index);
                }
            }
            ViewBag.Genres = genres;

            List<Studio> s = dcsm.StudioMovies.Where(p => p.Movie.Id == id).Select(p => p.Studio).ToList();
            ViewBag.CurrentStudios = s;
            List<Studio> studios = ds.Studios.OrderBy(p => p.Name).ToList();
            foreach (var stu in s)
            {
                var index = studios.FindIndex(st => st.Id == stu.Id);
                if (index >= 0)
                {
                    studios.RemoveAt(index);
                }
            }
            ViewBag.Studios = studios;

            List<Director> d = dcdm.DirectorMovies.Where(p => p.Movie.Id == id).Select(p => p.Director).ToList();
            ViewBag.CurrentDirectors = d;
            List<Director> directors = dd.Directors.OrderBy(p => p.Name).ToList();
            foreach (var dir in d)
            {
                var index = directors.FindIndex(dr => dr.Id == dir.Id);
                if (index >= 0)
                {
                    directors.RemoveAt(index);
                }
            }
            ViewBag.Directors = directors;

            return View();
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, Movie requestMovie)
        {
            try
            {
                Movie movie = dm.Movies.Find(id);
                if (TryUpdateModel(movie))
                {
                    movie.Name = requestMovie.Name;
                    movie.Description = requestMovie.Description;
                    movie.YTTrailerLink = requestMovie.YTTrailerLink.Replace("watch?v=", "embed/");
                    movie.Image = requestMovie.Image;

                    dm.SaveChanges();
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
            List<GenreMovie> conn = dcgm.GenreMovies.Where(p => p.Movie.Id == id).ToList();
            foreach (GenreMovie c in conn)
            {
                dcgm.GenreMovies.Remove(c);
                dcgm.SaveChanges();
            }

            List<ActorMovie> con = dcam.ActorMovies.Where(p => p.Movie.Id == id).ToList();
            foreach (ActorMovie c in con)
            {
                dcam.ActorMovies.Remove(c);
                dcam.SaveChanges();
            }

            List<StudioMovie> cons = dcsm.StudioMovies.Where(p => p.Movie.Id == id).ToList();
            foreach (StudioMovie c in cons)
            {
                dcsm.StudioMovies.Remove(c);
                dcsm.SaveChanges();
            }

            List<DirectorMovie> cond = dcdm.DirectorMovies.Where(p => p.Movie.Id == id).ToList();
            foreach (DirectorMovie c in cond)
            {
                dcdm.DirectorMovies.Remove(c);
                dcdm.SaveChanges();
            }


            Movie movie = dm.Movies.Find(id);
            dm.Movies.Remove(movie);
            dm.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
