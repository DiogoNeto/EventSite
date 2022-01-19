using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventDAL.Model;

namespace EventSite.Controllers
{
    public class EventController : Controller
    {
        private EVENT_DBEntities db = new EVENT_DBEntities();

        // GET: Event
        public ActionResult Index()
        {
            var tBL_EVENT = db.TBL_EVENT.Include(t => t.TBL_LANGUAGE);
            return View(tBL_EVENT.ToList());
        }

        // GET: Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_EVENT tBL_EVENT = db.TBL_EVENT.Find(id);
            if (tBL_EVENT == null)
            {
                return HttpNotFound();
            }
            return View(tBL_EVENT);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
