using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAproject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IAproject.Controllers
{
    public class SeeMenuController : Controller
    {
        private static decimal tmpcal;
        
        private MenuNewestEntities db = new MenuNewestEntities();

        // Search database to find suggested menu
        // Code Source: https://msdn.microsoft.com/en-us/library/bb308959.aspx
        public ActionResult ProvideSuggest([Bind(Include = "CalResult")] Calculation cal)
        {
            List<Menu> menulist = new List<Menu>();
            //if (User.Identity.IsAuthenticated)
            //{

            if (User.Identity.IsAuthenticated)
            {

                ViewBag.AddToRecord = cal.CalResult;
                tmpcal = (decimal)cal.CalResult;
            }
            if (cal.CalResult == 0)
                {

                   return RedirectToAction("Index", "Home");

                }
                else
                {


                    try
                    {
                        int Value = Convert.ToInt32(cal.CalResult);
                        menulist = db.Menus.Where(x => Value > 0 && (Value - 500 < x.Carlorie && x.Carlorie <= Value)).ToList();

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("{0} is not correct", cal.CalResult);
                    }

                    return View(menulist);
                }
            }
    
        // Show suggested menu    
        public ActionResult ReturnSuggest()
        {
            List<Menu> menulist = new List<Menu>();

                try
                {
                    int Value = Convert.ToInt32(tmpcal);
                    menulist = db.Menus.Where(x => Value > 0 && (Value - 500 < x.Carlorie && x.Carlorie <= Value)).ToList();

                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not correct", tmpcal);
                }

                return View("ProvideSuggest",menulist);
          
        }

        // User intends to add calorie to record. Action below interacts with database.
        public ActionResult Addrecord([Bind(Include = "Id,Carlories,UserID,CreateDate")] CalRecord cal)
        {
            string userid = User.Identity.GetUserId();
            List<CalRecord> calRecords = new List<CalRecord>();
            calRecords=db.CalRecords.Where(x =>x.UserId.Equals(userid) && x.CreateDate.Equals(System.DateTime.Today)).ToList();
            if(calRecords.Count!=0)
            {
                foreach (var rec in calRecords)
                {
                    rec.Calories = tmpcal;
                }
                db.SaveChanges();
            }
            else
            {
                cal.Calories = tmpcal;
                cal.UserId = User.Identity.GetUserId();
                cal.CreateDate = System.DateTime.Today;
                db.CalRecords.Add(cal);
                db.SaveChanges();

            }
            
            return RedirectToAction("ReturnSuggest");
        }

        public ActionResult RedirToCreate()
        {
            return RedirectToAction("Create","SeeMenu");
        }
       

        // GET: SeeMenu
        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        // GET: SeeMenu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Create", "Reviews", menu);
        }
        



        // GET: SeeMenu/Create
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
        }

        // POST: SeeMenu/Create
        // User upload menu.
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuID,Name,Description,MenuPhoto,Carlorie")] Menu menu, HttpPostedFileBase postedFile)
        {
            ModelState.Clear();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            menu.MenuPhoto = myUniqueFileName;
            
            TryValidateModel(menu);
            if (ModelState.IsValid)
            {
                string serverPath = Server.MapPath("~/Uploads/");
                string fileExtension = Path.GetExtension(postedFile.FileName);
                string filePath = menu.MenuPhoto + fileExtension;
                menu.MenuPhoto = filePath;
                postedFile.SaveAs(serverPath + menu.MenuPhoto);
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("ReturnSuggest");
            }

            return View(menu);
        }

        // GET: SeeMenu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: SeeMenu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuID,Name,Description,MenuPhoto,Carlorie")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: SeeMenu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: SeeMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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
