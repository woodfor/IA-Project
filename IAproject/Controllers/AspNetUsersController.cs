using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IAproject.Models;
using Microsoft.AspNet.Identity;

namespace IAproject.Controllers
{
    public class AspNetUsersController : Controller
    {
        private MenuNewestEntities db = new MenuNewestEntities();

        // GET: AspNetUsers
        [Authorize(Roles="admin")]
        public ActionResult AdminPage()
        {
            return View(db.AspNetUsers.Find("440b3391-95f7-4803-ba5d-d9b970979063"));
        }

        public ActionResult ManageMenus()
        {
            return View(db.Menus.ToList());
        }
        public ActionResult ManageUsers()
        {
            return View(db.AspNetUsers.ToList());
        }
        public ActionResult DeleteUsers(string id)
        {
            db.AspNetUsers.Remove(db.AspNetUsers.Find(id));
            db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        public ActionResult DeleteMenus(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            foreach (var item in db.Reviews)
            {
                if (item.MenuID == menu.MenuID)
                    db.Reviews.Remove(item);
            }
            using (PersonalMenuEntities db = new PersonalMenuEntities())
            {
                foreach (var item in db.PersonalMenus)
                {
                    if (item.MenuID == menu.MenuID)
                        db.PersonalMenus.Remove(item);
                }
                db.SaveChanges();
            }
            db.SaveChanges();
            return RedirectToAction("ManageMenus");
        }
        public ActionResult EditMenu(int? id)
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
            //return RedirectToAction("ManageMenus","");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu([Bind(Include = "MenuID,Name,Description,Carlorie")] Menu menu, HttpPostedFileBase postedFile)
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
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageMenus");

            }

            return View(menu);
        }
        public ActionResult CreateMenu()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenu([Bind(Include = "MenuID,Name,Description,MenuPhoto,Carlorie")] Menu menu, HttpPostedFileBase postedFile)
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
                return RedirectToAction("ManageMenus");
            }

            return View(menu);
        }

        //public ActionResult
        // GET: AspNetUsers/Details/5
        public ActionResult MenuDetails(int? id)
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

        public ActionResult DeleteReview(int? id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        // GET: AspNetUsers/Create


        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Birthday,Carlories")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }
        
        // GET: AspNetUsers/Edit/5
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Birthday,Carlories")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

         public ActionResult AddFavourite(Menu menu)
       {
            if (User.Identity.IsAuthenticated)
            {
                PersonalMenu personalMenu = new PersonalMenu();

                string id = User.Identity.GetUserId();

                using (PersonalMenuEntities db = new PersonalMenuEntities())
                {
                    personalMenu.Id = id;
                    personalMenu.MenuID = menu.MenuID;
                    db.PersonalMenus.Add(personalMenu);
                    db.SaveChanges();
                }
     
               
                AspNetUser aspNetUser = db.AspNetUsers.Find(id);
                // aspNetUser.Menus.Add(menu);               

                return Redirect(Request.UrlReferrer.ToString());

                // personalMenu.MenuID;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
       }
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }



        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = db.AspNetUsers.Find(User.Identity.GetUserId());
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            else
            {
                user.Menus.Remove(menu);
                db.SaveChanges();
            }
            return View("Index",user);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
