using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Kodasat.Models;
using Kodasat.Report;

namespace Kodasat.Controllers
{
    public class DepositedsController : Controller
    {
        private KodasatDBEntities db = new KodasatDBEntities();
        private MyLibrary myLibrary = new MyLibrary();

        // GET: Depositeds
        public ActionResult Index(int? id)
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Session["curchID"] == null)
            {
                return RedirectToAction("signIn", "Logins");
            }

            int church = Convert.ToInt32(Session["curchID"]);
            var depositeds = db.Depositeds.Where(m=>m.kodasID == id && m.churchesID == church).Include(d => d.Church).Include(d => d.Login).Include(d => d.Koda);
            // To check url security id
            Koda koda = db.Kodas.Find(id);
            if (koda.ChurchesID != Convert.ToInt32(Session["curchID"]))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session["kodasId"] = id;
            return View(depositeds.ToList());
        }

        public ActionResult Report(int id)
        {
            String dateKodas, TimeKodas;
            var item = db.Kodas.Find(id);
            dateKodas = "Date:" + item.Date.ToString("dd/MM/yyyy");
            TimeKodas = "Time:" + item.Time.Value;
            // To check url security id
            if (item.ChurchesID != Convert.ToInt32(Session["curchID"]))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var depositeds = db.Depositeds.Where(m=>m.kodasID == id).ToList();
            DepositedReport derpositedReport = new DepositedReport();
            
            
            byte[] abytes = derpositedReport.PrepareReport(depositeds,dateKodas,TimeKodas);
            ViewBag.Title = "PDF";
            return File(abytes, "application/pdf");
        }

        // GET: Depositeds/Details/5
        public ActionResult Details()
        {
            //To prevent back browser button
            myLibrary.PreventBackButtonBrowser(Response);
            if (Convert.ToInt32(Session["flag"]) == 0)
                return RedirectToAction("Create", "Depositeds");
            //********************************************************//

            Deposited deposited = (Deposited)TempData["DepositeInfo"];
            ViewBag.count = 0;
            var attend = db.Depositeds.Where(m => m.NationalID == deposited.NationalID).Select(m => m.AttendedOn);
            foreach (DateTime item in attend)
            {

                if ((DateTime.Now - item).TotalDays < 30)
                {
                    ViewBag.count++;
                }
            }
            if (ViewBag.count > 0)
            {
                ViewBag.repeat = "عذراً ولكن لا يمكنك الحضور لأنك حضرت منذ فتره قريبه";
               // ViewBag.disaple = disapled;
            }  
            else
            {
                ViewBag.repeat = "نأسف ولكن لا يوجد مواعيد متاحة";
            }
                
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Deposited deposited = db.Depositeds.Find(id);
            //if (deposited == null)
            //{
            //    return HttpNotFound();
            //}
            
            var name= db.Churches.Find(deposited.churchesID).churchName;
            //TempData["churchID"] = deposited.churchesID;
            TempData["churchName"] = Convert.ToString(name);
            TempData["natID"] = deposited.NationalID;
            TempData["full"] = deposited.fullName;
            TempData["koID"] = deposited.kodasID;
            ViewBag.kodasList = db.Kodas.Where(k => (k.PeopleCount - k.PeopleDeposited) > 0 && k.ChurchesID==deposited.churchesID).ToList();
            //ViewBag.KodasList = new SelectList(kodasList, "Id", "kodasDateAndTime");
            return View(deposited);
        }

        [RequireHttps]
        [ActionName("Details")]
        [HttpPost]
        public ActionResult DetailsPost([Bind(Include = "kodasID")]Deposited deposited)
        {
            Session["flag"] = 0;
            if (TempData["natID"] == null)
            {
                return Redirect("index");
            }
            Koda koda = db.Kodas.Find(deposited.kodasID);
            deposited.NationalID = TempData["natID"]+"";
            deposited.fullName = TempData["full"]+"";
            deposited.churchesID = koda.ChurchesID;
            deposited.AttendedOn = koda.Date;
            deposited.FatherID = koda.fatherID;
            Koda k = db.Kodas.Where(m => m.Id == deposited.kodasID).FirstOrDefault();
            k.PeopleDeposited++;
            db.Entry(koda).State = EntityState.Modified;
            db.Depositeds.Add(deposited);
            db.SaveChanges();
            TempData["natID"] = null;
            TempData["dep"] = "تم التسجيل بنجاح";
            return Redirect("Create");
        }

        // GET: Depositeds/Create
        public ActionResult Create()
        {

            
            Session["flag"] = 0;
            ViewBag.churchesID = new SelectList(db.Churches, "Id", "churchName");
            //ViewBag.FatherID = new SelectList(db.Logins, "Id", "Phone");
            //ViewBag.kodasID = new SelectList(db.Kodas, "Id", "Id");

            return View();
        }

        // POST: Depositeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NationalID,fullName,churchesID")] Deposited deposited)
        {
            Session["flag"] = 1;
            if (ModelState.IsValid)
            {
                TempData["DepositeInfo"] = deposited;
                //db.Depositeds.Add(deposited);
                //db.SaveChanges();
                return RedirectToAction("Details");
            }

            ViewBag.churchesID = new SelectList(db.Churches, "Id", "churchName", deposited.churchesID);
            //ViewBag.FatherID = new SelectList(db.Logins, "Id", "Phone", deposited.FatherID);
            //ViewBag.kodasID = new SelectList(db.Kodas, "Id", "Id", deposited.kodasID);
            return View(deposited);
        }

        // GET: Depositeds/Edit/5
        public ActionResult Edit(int? id)
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
            Deposited deposited = db.Depositeds.Find(id);
            if (deposited == null)
            {
                return HttpNotFound();
            }
            ViewBag.churchesID = new SelectList(db.Churches, "Id", "churchName", deposited.churchesID);
            ViewBag.FatherID = new SelectList(db.Logins, "Id", "Phone", deposited.FatherID);
            ViewBag.kodasID = new SelectList(db.Kodas, "Id", "Id", deposited.kodasID);
            return View(deposited);
        }

        // POST: Depositeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NationalID,fullName,kodasID,AttendedOn,FatherID,churchesID,NumberFriends")] Deposited deposited)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deposited).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.churchesID = new SelectList(db.Churches, "Id", "churchName", deposited.churchesID);
            ViewBag.FatherID = new SelectList(db.Logins, "Id", "Phone", deposited.FatherID);
            ViewBag.kodasID = new SelectList(db.Kodas, "Id", "Id", deposited.kodasID);
            return View(deposited);
        }

        //// GET: Depositeds/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    //To prevent back browser button
        //    myLibrary.PreventBackButtonBrowser(Response);
        //    if (Session["id"] == null)
        //    {
        //        return RedirectToAction("signIn", "Logins");
        //    }

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Deposited deposited = db.Depositeds.Find(id);
        //    if (deposited == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(deposited);
        //}

        // POST: Depositeds/Delete/5
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Deposited deposited = db.Depositeds.Find(id);
            Koda koda = db.Kodas.Find(deposited.kodasID);
            --koda.PeopleDeposited;
            db.Entry(koda).State = EntityState.Modified;
            db.Depositeds.Remove(deposited);
            db.SaveChanges();
            return RedirectToAction("Index/"+ deposited.kodasID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Boolean NationalIDIsValid(long ID)
        {
            long num = ID;
            int flag = 0;
            num = num / 100000;
            long check = num % 100;
            //To check Governorate
            int[] Governorate = { 1,2,3,4,11,12,13,14,15,16,17,18,19,21,22,23,24,25,26,27,28,29,31,32,33,34,35,88};
            foreach(int x in Governorate)
            {
                if (check == x)
                {
                    flag = 1;
                    break;
                }
                    
            }
            if (flag == 0)
                return false;
            num = num / 100;
            check = num % 100;
            // to Check Days
            if (check < 1 || check > 31)
                return false;
            num = num / 100;
            check = num % 100;
            //To Check Month
            if (check < 1 || check > 12)
                return false;
            num = num / 10000;
            check = num % 10;
            //To check First Num ==>2 from 1900 to 1999  && Num ==>3 from 2900 to 2999   
            if (check > 3 || check < 2)
                return false;

            return true;
        }
    }
}
