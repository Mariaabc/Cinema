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
    public class DirectorController : Controller
    {
        private CommentDBContext dc = new CommentDBContext();
        private DirectorDBContext dd = new DirectorDBContext();
        private DirectorMovieDBContext dcdm = new DirectorMovieDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var directors = from director in dd.Directors
                            orderby director.Name
                            select director;

            foreach (var d in directors)
            {
                if (d.Description != null)
                    if (d.Description.Length > 200)
                        d.Description = (d.Description).Substring(0, 200);
            }

            ViewBag.Directors = directors;
            return View();
        }

        public ActionResult Search(string KeyWord)
        {
            ViewBag.KeyWord = KeyWord;

            List<Director> directors = dd.Directors.Where(p => p.Name.Contains(KeyWord)).Select(p => p).ToList();

            foreach (var d in directors)
            {
                if (d.Description != null)
                    if (d.Description.Length > 200)
                        d.Description = (d.Description).Substring(0, 200);
            }

            ViewBag.Directors = directors;
            return View();
        }
        public ActionResult Show(int id)
        {
            Director director = dd.Directors.Find(id);
            ViewBag.Director = director;

            // comentarii
            List<Comment> comment = dc.Comments.Where(p => p.Group.Equals("Director") && p.IdGroup == id && p.IdParent == 0).Select(p => p).ToList();

            foreach (var c in comment)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.Comments = comment;

            List<Comment> commentc = dc.Comments.Where(p => p.Group.Equals("Director") && p.IdGroup == id && p.IdParent != 0).Select(p => p).ToList();
            foreach (var c in commentc)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.ChildComments = commentc;

            // comentarii

            List<Movie> movies = dcdm.DirectorMovies.Where(p => p.Director.Id == id).Select(p => p.Movie).ToList();
            foreach (var m in movies)
            {
                if (m.Description != null)
                    if (m.Description.Length > 200)
                        m.Description = (m.Description).Substring(0, 200);
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
        public ActionResult New(Director director)
        {
            try
            {
                dd.Directors.Add(director);
                dd.SaveChanges();
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
            Director director = dd.Directors.Find(id);
            ViewBag.Director = director;

            List<Movie> mvs = dcdm.DirectorMovies.Where(p => p.Director.Id == id).Select(p => p.Movie).ToList();
            foreach (var m in mvs)
            {
                if (m.Description != null)
                    if (m.Description.Length > 200)
                        m.Description = (m.Description).Substring(0, 200);
            }
            ViewBag.CurrentMovies = mvs;
            List<Movie> movies = dm.Movies.OrderBy(p => p.Name).ToList();
            foreach (var mov in mvs)
            {
                var index = movies.FindIndex(dr => dr.Id == mov.Id);
                if (index >= 0)
                {
                    movies.RemoveAt(index);
                }
            }
            ViewBag.Movies = movies;

            return View();
        }
        [HttpPut]
        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Edit(int id, Director requestDirector)
        {
            try
            {
                Director director = dd.Directors.Find(id);
                if (TryUpdateModel(director))
                {
                    director.Name = requestDirector.Name;
                    director.Description = requestDirector.Description;
                    director.Img = requestDirector.Img;
                    dd.SaveChanges();
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
            List<DirectorMovie> con = dcdm.DirectorMovies.Where(p => p.Movie.Id == id).ToList();

            foreach (DirectorMovie c in con)
            {
                dcdm.DirectorMovies.Remove(c);
                dcdm.SaveChanges();
            }

            Director director = dd.Directors.Find(id);
            dd.Directors.Remove(director);
            dd.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}