using ProiectLicenta.Models.Connection;
using ProiectLicenta.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectLicenta.Controllers.Connection
{
    public class StudioMovieController : Controller
    {
        private MovieDBContext dm = new MovieDBContext();
        private StudioMovieDBContext dsm = new StudioMovieDBContext();
        private StudioDBContext ds = new StudioDBContext();


        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult New(StudioMovie requestStudioMovie, string Group)
        {
            StudioMovie sm = new StudioMovie();

            sm.MovieId = requestStudioMovie.MovieId;
            sm.StudioId = requestStudioMovie.StudioId;

            dsm.StudioMovies.Add(sm);
            dsm.SaveChanges();

            if (Group.Equals("Studio"))
                return RedirectToAction("Edit", Group, new { id = requestStudioMovie.StudioId });
            else
                return RedirectToAction("Edit", Group, new { id = requestStudioMovie.MovieId });
        }

        [Authorize(Roles = "Editor,Administrator")]
        public ActionResult Delete(StudioMovie requestStudioMovie, string Group)
        {
            StudioMovie conn = dsm.StudioMovies.Where(p => p.StudioId == requestStudioMovie.StudioId && p.MovieId == requestStudioMovie.MovieId).FirstOrDefault();
            dsm.StudioMovies.Remove(conn);
            dsm.SaveChanges();

            if (Group.Equals("Studio"))
                return RedirectToAction("Edit", Group, new { id = requestStudioMovie.StudioId });
            else
                return RedirectToAction("Edit", Group, new { id = requestStudioMovie.MovieId });
        }
    }
}