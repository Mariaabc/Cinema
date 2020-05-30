using Microsoft.AspNet.Identity;
using ProiectLicenta.Models;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Main
{
    public class CommentController : Controller
    {
        private CommentDBContext dc = new CommentDBContext();
        private Models.ApplicationDbContext db = Models.ApplicationDbContext.Create();

        public ActionResult Index()
        {
            List<Comment> comment = dc.Comments.Where(p => p.Group.Equals("Wish") && p.IdParent == 0).Select(p => p).ToList();

            foreach (var c in comment)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.Comments = comment;

            List<Comment> commentc = dc.Comments.Where(p => p.Group.Equals("Wish") && p.IdParent != 0).Select(p => p).ToList();
            foreach (var c in commentc)
            {
                ApplicationUser user = db.Users.Find(c.UserId);
                c.CurentUserName = user.UserName;
                c.CurentUser = User.Identity.GetUserId();
            }
            ViewBag.ChildComments = commentc;

            return View();
        }

        public ActionResult New(Comment comment)
        {
            comment.DT = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();
            dc.Comments.Add(comment);
            dc.SaveChanges();
            if (comment.Group.Equals("Wish"))
                return RedirectToAction("Index", "Comment");
            return RedirectToAction("Show", comment.Group, new { id = comment.IdGroup });
        }

        public ActionResult Delete(int id)
        {
            Comment comment = dc.Comments.Find(id);
            if (comment.IdParent == 0)
            {
                List<Comment> x = dc.Comments.Where(p => p.Group.Equals(comment.Group) && p.IdParent == comment.Id).Select(p => p)
                    .ToList();
                foreach (var c in x)
                {
                    dc.Comments.Remove(c);
                }
            }
            dc.Comments.Remove(comment);
            dc.SaveChanges();
            if (comment.Group.Equals("Wish"))
                return RedirectToAction("Index", "Comment");
            return RedirectToAction("Show", comment.Group, new { id = comment.IdGroup });
        }
    }
}