using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventDAL.Model;
using EventSite.Models;
using System.IO;

namespace EventSite.Controllers
{
    public class AgendaController : Controller
    {
        private EVENT_DBEntities db = new EVENT_DBEntities();

        // GET: Agenda
        public ActionResult Index(int? id)
        {
            ViewBag.eventId = id;

            if (db.TBL_EVENT.Find(id) == null)
            {
                //erro
                return HttpNotFound();
            }

            int BlockSize = 5;
            var agendas = AgendaManager.GetAgendas(id, 1, BlockSize);
            return View(agendas);

            // var agenda = db.TBL_AGENDA.Where(e => e.EVENT_ID == id).Include(t => t.TBL_LANGUAGE);

            // var tBL_AGENDA = db.TBL_AGENDA.Include(t => t.TBL_EVENT).Include(t => t.TBL_LANGUAGE);

            // return View(agenda.ToList());
        }



        [ChildActionOnly]
        public ActionResult AgendaList(List<TBL_AGENDA> Model)
        {
            return PartialView(Model);
        }

        [HttpPost]
        public ActionResult InfinateScroll(int? id, int BlockNumber)
        {
            //////////////// THis line of code only for demo. Needs to be removed ///////////////
            // System.Threading.Thread.Sleep(3000);
            ////////////////////////////////////////////////////////////////////////////////////////
            ViewBag.eventId = id;
            int BlockSize = 5;
            var agendas = AgendaManager.GetAgendas(id, BlockNumber, BlockSize);

            JsonModel jsonModel = new JsonModel();
            jsonModel.NoMoreData = agendas.Count < BlockSize;
            jsonModel.HTMLString = RenderPartialViewToString("AgendaList", agendas);

            return Json(jsonModel);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }




        // GET: Agenda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_AGENDA tBL_AGENDA = db.TBL_AGENDA.Find(id);
            if (tBL_AGENDA == null)
            {
                return HttpNotFound();
            }

            ViewBag.eventId = tBL_AGENDA.EVENT_ID;

            return View(tBL_AGENDA);
        }



        public ActionResult Calendario(int? id)
        {
            ViewBag.eventId = id;

            if (db.TBL_EVENT.Find(id) == null)
            {
                //erro
                return HttpNotFound();
            }

            int BlockSize = 5;
            var agendas = AgendaManager.GetAgendas(id, 1, BlockSize);
            return View(agendas);
        }




        // GET: Agenda/Create
        public ActionResult Create()
        {
            ViewBag.EVENT_ID = new SelectList(db.TBL_EVENT, "ROW_ID", "NAME");
            ViewBag.LANGUAGE_ID = new SelectList(db.TBL_LANGUAGE, "ROW_ID", "NAME");
            return View();
        }

        // POST: Agenda/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ROW_ID,CREATED,CREATED_BY,LAST_UPDT,LAST_UPDT_BY,TITLE,EVENT_ID,TEXT_DESCRIPTION,EFF_FROM_DATE,EFF_TO_DATE,DELETED,DATA,LANGUAGE_ID,LOCAL,HORARIO,DESCRIPTION_2,IMAGE,LINK,TAG")] TBL_AGENDA tBL_AGENDA)
        {
            if (ModelState.IsValid)
            {
                db.TBL_AGENDA.Add(tBL_AGENDA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EVENT_ID = new SelectList(db.TBL_EVENT, "ROW_ID", "NAME", tBL_AGENDA.EVENT_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.TBL_LANGUAGE, "ROW_ID", "NAME", tBL_AGENDA.LANGUAGE_ID);
            return View(tBL_AGENDA);
        }

        // GET: Agenda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_AGENDA tBL_AGENDA = db.TBL_AGENDA.Find(id);
            if (tBL_AGENDA == null)
            {
                return HttpNotFound();
            }
            ViewBag.EVENT_ID = new SelectList(db.TBL_EVENT, "ROW_ID", "NAME", tBL_AGENDA.EVENT_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.TBL_LANGUAGE, "ROW_ID", "NAME", tBL_AGENDA.LANGUAGE_ID);
            return View(tBL_AGENDA);
        }

        // POST: Agenda/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ROW_ID,CREATED,CREATED_BY,LAST_UPDT,LAST_UPDT_BY,TITLE,EVENT_ID,TEXT_DESCRIPTION,EFF_FROM_DATE,EFF_TO_DATE,DELETED,DATA,LANGUAGE_ID,LOCAL,HORARIO,DESCRIPTION_2,IMAGE,LINK,TAG")] TBL_AGENDA tBL_AGENDA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBL_AGENDA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EVENT_ID = new SelectList(db.TBL_EVENT, "ROW_ID", "NAME", tBL_AGENDA.EVENT_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.TBL_LANGUAGE, "ROW_ID", "NAME", tBL_AGENDA.LANGUAGE_ID);
            return View(tBL_AGENDA);
        }

        // GET: Agenda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TBL_AGENDA tBL_AGENDA = db.TBL_AGENDA.Find(id);
            if (tBL_AGENDA == null)
            {
                return HttpNotFound();
            }
            return View(tBL_AGENDA);
        }

        // POST: Agenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TBL_AGENDA tBL_AGENDA = db.TBL_AGENDA.Find(id);
            db.TBL_AGENDA.Remove(tBL_AGENDA);
            db.SaveChanges();
            return RedirectToAction("Index");
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
