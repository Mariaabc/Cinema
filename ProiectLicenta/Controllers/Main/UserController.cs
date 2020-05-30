using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProiectLicenta.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProiectLicenta.Controllers.Main
{
    public class UserController : Controller
    {
        private Models.ApplicationDbContext db = Models.ApplicationDbContext.Create();
        // [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<ApplicationUser> users = db.Users.ToList();
            ViewBag.Users = users;

            return View();
        }
        // [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            ViewBag.User = user;

            var roles = from role in db.Roles
                        select role;
            ViewBag.UserRoles = roles;

            return View();
        }
        // [Authorize(Roles = "Administrator")]
        [HttpPut]
        public ActionResult Edit(string id, ApplicationUser newData, string Role)
        {
            ApplicationUser user = db.Users.Find(id);
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    if (user.PhoneNumber != null)
                        user.PhoneNumber = newData.PhoneNumber;

                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Id);
                    }
                    UserManager.AddToRole(id, Role);

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(user);
            }

        }

        public ActionResult You(string id)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.User = user;

            return View();
        }

        [HttpPut]
        public ActionResult You(ApplicationUser newData)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(id);

            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    if (user.PhoneNumber != null)
                        user.PhoneNumber = newData.PhoneNumber;
                    db.SaveChanges();
                }
                return RedirectToAction("You");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(user);
            }

        }

        public ActionResult Delete(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Id);
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();

                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(user);
            }
        }
    }
}