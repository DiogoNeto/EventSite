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
    public class NewsController : Controller
    {
        private EVENT_DBEntities db = new EVENT_DBEntities();

        // GET: News
        public ActionResult Index(int? id, int page = 0, int records = 15, string order = "dd")
        {
            ViewBag.eventId = id;

            if (db.TBL_EVENT.Find(id) == null)
            {
                //erro
                
                return HttpNotFound();
            }


            //var news = db.TBL_NEWS.Include(t => t.TBL_EVENT).Include(t => t.TBL_LANGUAGE);

            var news = db.TBL_NEWS.Where(e => e.EVENT_ID == id).Include(t => t.TBL_LANGUAGE);

            // sorting
            if (order != null)
            {
                ViewBag.ORDER = order;
                switch (order)
                {
                    case "dd":
                        news = news.OrderByDescending(t => t.CREATED);
                        break;
                    case "da":
                        news = news.OrderBy(t => t.CREATED);
                        break;
                    case "lud":
                        news = news.OrderByDescending(t => t.LAST_UPDT);
                        break;
                    case "lua":
                        news = news.OrderBy(t => t.LAST_UPDT);
                        break;
                    case "td":
                        news = news.OrderByDescending(t => t.TITLE);
                        break;
                    case "ta":
                        news = news.OrderBy(t => t.TITLE);
                        break;
                    default: break;
                }
            }

            if (page > -1)
            {
                ViewBag.NUMBER_OF_PAGES = news.Count() / records;
                news = news.Skip(page * records).Take(records);
            }


            ViewBag.PAGE = page;


            return View(news.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_NEWS tBL_NEWS = db.TBL_NEWS.Find(id);
            if (tBL_NEWS == null)
            {
                return HttpNotFound();
            }

            ViewBag.eventId = tBL_NEWS.EVENT_ID;

            return View(tBL_NEWS);
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
