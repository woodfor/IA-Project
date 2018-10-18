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
using Microsoft.AspNet.Identity.Owin;

namespace IAproject.Controllers
{
    public class SeeMenuController : Controller
    {
        private MenuNewestEntities db = new MenuNewestEntities();
        public ActionResult ProvideSuggest([Bind(Include = "CalResult")] Calculation cal)
        {
            List<Menu> menulist = new List<Menu>();
            //if (User.Identity.IsAuthenticated)
            //{

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
            //else
            //{
            //    //ViewBag.authenticated = "Alert";
            //    //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert("Hello this is an Alert")</SCRIPT>");
            //    // Response.Write("Please login to see your suggested menu");
            //    // Response.Write("<script>alert('Please login to see your suggested menu ')</script>");
            //   // Response.Write(" <script type='text / javascript'>window.onload = function () {alert( ' Please login to see your suggested menu.' ); } </script> ");
            //    return RedirectToAction("Login", "Account", routeValues: null);
            //}
           
            
        

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
            return View();
        }

        // POST: SeeMenu/Create
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
                return RedirectToAction("ProvideSuggest");
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
