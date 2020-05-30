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
    public class StudioController : Controller
    {
        private StudioDBContext ds = new StudioDBContext();
        private StudioMovieDBContext dcsm = new StudioMovieDBContext();
        private MovieDBContext dm = new MovieDBContext();
        private CommentDBContext dc = new CommentDBContext();
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            var studios = from studio in ds.Studios
                          orderby studio.Id
                          select studio;

            foreach (var s in studios)
            {
                if (s.Description != null)
                    if (s.Description.Length > 200)
                        s.Description = (s.Description).Substring(0, 200);
            }

            ViewBag.Studios = studios;
            return View();
        }

        public ActionResult Search(string KeyWord)
        {
            ViewBag.KeyWord = KeyWord;

            List<Studio> studios = ds.Studios.Where(p => p.Name.Contains(KeyWord)).Select(p => p).ToList();

            foreach (var s in studios)
            {
                if (s.Description != null)
                    if (s.Description.Length > 200)
                        s.Description = (s.Description).Substring(0, 200);
            }

            ViewBag.Studios = studios;
            return View();
        }
        public ActionResult Show(int id)
        {
            Studio studio = ds.Studios.Find(id);
            ViewBag.Studio = studio;

            // comentarii
            List<Comment> comment = dc.Comments.Where(p => p.Group.Equals("Studio") && p.IdGroup == id && p.IdParent == 0).Select(p => p).ToList();

            foreach (var c in comment)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.Comments = comment;

            List<Comment> commentc = dc.Comments.Where(p => p.Group.Equals("Studio") && p.IdGroup == id && p.IdParent != 0).Select(p => p).ToList();
            foreach (var c in commentc)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.ChildComments = commentc;

            // comentarii
            List<Movie> movies = dcsm.StudioMovies.Where(p => p.Studio.Id == id).Select(p => p.Movie).ToList();
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
        public ActionResult New(Studio studio)
        {
            try
            {
                ds.Studios.Add(studio);
                ds.SaveChanges();
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
            Studio studio = ds.Studios.Find(id);
            ViewBag.Studio = studio;

            List<Movie> mvs = dcsm.StudioMovies.Where(p => p.Studio.Id == id).Select(p => p.Movie).ToList();
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
        public ActionResult Edit(int id, Studio requestStudio)
        {
            try
            {
                Studio studio = ds.Studios.Find(id);
                if (TryUpdateModel(studio))
                {
                    studio.Name = requestStudio.Name;
                    studio.Description = requestStudio.Description;
                    studio.Logo = requestStudio.Logo;
                    ds.SaveChanges();
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
            List<StudioMovie> con = dcsm.StudioMovies.Where(p => p.Movie.Id == id).ToList();

            foreach (StudioMovie c in con)
            {
                dcsm.StudioMovies.Remove(c);
                dcsm.SaveChanges();
            }

            Studio studio = ds.Studios.Find(id);
            ds.Studios.Remove(studio);
            ds.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}