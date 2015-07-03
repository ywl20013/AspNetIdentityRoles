using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AspNetIdentityRoles.Areas.Config.Controllers
{
    using Models.User;
    public class OrganizationsController : Controller
    {
        private OrganizationDbContext db = new OrganizationDbContext();

        // GET: Organizations
        public ActionResult Index()
        {
            return View(db.Entities.ToList());
        }

        // GET: Organizations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Entities.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            Organization model = new Organization();
            model.STATUS = "normal";
            model.CRTIME = DateTime.Now;
            model.CRMAN = HttpContext.ApplicationInstance.Context.User.Identity.Name;
            model.ORDERNO = "999";
            return View(model);
        }

        // POST: Organizations/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "STATUS,CRMAN,CRTIME,MEMO,ID,NAME,PID,LVL,ID2,ORDERNO,OLDID")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                var GUID = Guid.NewGuid();
                organization.LINEID = GUID.ToString();
                if (organization.STATUS == null || organization.STATUS == "")
                    organization.STATUS = "normal";

                if (organization.CRTIME == null)
                    organization.CRTIME = DateTime.Now;
                if (organization.CRMAN == null)
                    organization.CRMAN = HttpContext.ApplicationInstance.Context.User.Identity.Name;


                db.Entities.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        // GET: Organizations/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Entities.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LINEID,STATUS,CRMAN,CRTIME,MEMO,ID,NAME,PID,LVL,ID2,ORDERNO,OLDID")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = db.Entities.Find(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Organization organization = db.Entities.Find(id);
            db.Entities.Remove(organization);
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
