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
    public class KodasController : Controller
    {
        private KodasatDBEntities db = new KodasatDBEntities();
        Controllers.MyLibrary myLibrary = new MyLibrary();
        // GET: Kodas
        public ActionResult Index()
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["curchID"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }


            int church = Convert.ToInt32(Session["curchID"]);
            var kodas = db.Kodas.Where(m=>m.ChurchesID == church).Include(k => k.Church).Include(k => k.Login);
            
            return View(kodas.ToList());
        }
        // GET: Kodas/Details/5
        public ActionResult Details(int? id)
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["id"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Koda koda = db.Kodas.Find(id);
            if (koda == null)
            {
                return HttpNotFound();
            }
            return View(koda);
        }

        // GET: Kodas/Create
        public ActionResult Create()
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["curchID"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }

            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName");
            return View();
        }

        // POST: Kodas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Time,PeopleCount")] Koda koda)
        {

            if (ModelState.IsValid)
            {
                koda.PeopleDeposited = 0;
                koda.ChurchesID =Convert.ToInt32(Session["curchID"]);
                koda.fatherID = Convert.ToInt32(Session["id"]);
                db.Kodas.Add(koda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", koda.ChurchesID);
            ViewBag.fatherID = new SelectList(db.Logins, "Id", "Phone", koda.fatherID);
            return View(koda);
        }

        // GET: Kodas/Edit/5
        public ActionResult Edit(int? id)
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["curchID"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Koda koda = db.Kodas.Find(id);
            //In order to prevent user to edite old Date
            if (koda.Date < DateTime.Now)
            {
                return RedirectToAction("Index");
            }

            // To check url security id
            if (koda.ChurchesID != Convert.ToInt32(Session["curchID"]))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (koda == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", koda.ChurchesID);
            ViewBag.fatherID = new SelectList(db.Logins, "Id", "Phone", koda.fatherID);
            return View(koda);
        }

        // POST: Kodas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Time,PeopleCount")] Koda koda)
        {
           
            if (ModelState.IsValid)
            {
                
                Koda k1 = db.Kodas.Find(koda.Id);
                //In order to prevent user to edite old Date
                if(koda.Date<DateTime.Now)
                {
                    return RedirectToAction("Index");
                }


                k1.Date = koda.Date;
                k1.Time = koda.Time;
                k1.PeopleCount = koda.PeopleCount;
                if(k1.PeopleCount<k1.PeopleDeposited)
                {
                    ModelState.AddModelError("PeopleCount", "يجب ان يكون عدد الأشخاص فى القداس أكبر من" + k1.PeopleDeposited);
                    koda.PeopleDeposited = k1.PeopleDeposited;
                    return View(koda);
                }
                db.Entry(k1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchesID = new SelectList(db.Churches, "Id", "churchName", koda.ChurchesID);
            ViewBag.fatherID = new SelectList(db.Logins, "Id", "Phone", koda.fatherID);
            return View(koda);
        }

        // GET: Kodas/Delete/5
        public ActionResult Delete(int? id)
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["curchID"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Koda koda = db.Kodas.Find(id);

            //In order to prevent user to edite old Date
            if (koda.Date < DateTime.Now)
            {
                return RedirectToAction("Index");
            }
            // To check url security id
            if (koda.ChurchesID != Convert.ToInt32(Session["curchID"]))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (koda == null)
            {
                return HttpNotFound();
            }

            

            return View(koda);
        }

        // POST: Kodas/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Koda koda = db.Kodas.Find(id);
            //In order to prevent user to edite old Date
            if (koda.Date >= DateTime.Now)
            {
                db.Kodas.Remove(koda);
                db.SaveChanges();
            }

            
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
