using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kodasat.Models;

namespace Kodasat.Controllers
{
    public class LoginsController : Controller
    {
        private KodasatDBEntities db = new KodasatDBEntities();

        public ActionResult signIn()
        {

            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.ClearHeaders();
            //Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            //Response.AddHeader("Pragma", "no-cache");

            Session["id"] = null;
            Session["name"] = null;
            Session["curchID"] = null;
            Session.RemoveAll();
            return View();
        }
        [HttpPost]
        [ActionName("signIn")]
        public ActionResult signInPost([Bind(Include = "Email,Password")] Login login)
        {

            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.ClearHeaders();
            //Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            //Response.AddHeader("Pragma", "no-cache");

            var logInfo=db.Logins.Where(m=>m.Email == login.Email && m.Password == login.Password).FirstOrDefault();
            if (logInfo == null)
            {
                ModelState.AddModelError("Password", "يوجد خطأ فى الأيميل أو الباسورد ");
                return View();
            }

            Session["id"] =logInfo.Id.ToString();
            Session["name"] = logInfo.Name.ToString();
            Session["curchID"] = logInfo.ChurchesID.ToString();
            return RedirectToAction("index", "Kodas");
           
        }
        // GET: Logins
        public ActionResult Index()
        {
            var logins = db.Logins.Include(l => l.Church);
            return View(logins.ToList());
        }

        // GET: Logins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // GET: Logins/Create
        public ActionResult Create()
        {
            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Phone,Email,Password,Name,Type,ChurchesID")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", login.ChurchesID);
            return View(login);
        }

        // GET: Logins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", login.ChurchesID);
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Phone,Email,Password,Name,Type,ChurchesID")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", login.ChurchesID);
            return View(login);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Logins.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Logins.Find(id);
            db.Logins.Remove(login);
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
