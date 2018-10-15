using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAproject.Models;
using Microsoft.AspNet.Identity;

namespace IAproject.Controllers
{
    public class ReviewsController : Controller
    {
        protected static Menu menutmp;
        private MenuNewestEntities db = new MenuNewestEntities();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Menu);
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        public ActionResult Create(Menu menu)
        {
            menutmp = menu;
            List<string> menuview = new List<string>();
            menuview.Add("Name " + menu.Name);
            menuview.Add("Carlorie " + menu.Carlorie+"");
            menuview.Add("Description " + menu.Description);
            ViewData["menuview"] = menuview;
            ViewBag.viewphoto = "~/Uploads/" + menu.MenuPhoto;
            List<Review> reviewlist = new List<Review>();
            reviewlist = db.Reviews.Where(x => x.MenuID == menu.MenuID).ToList();
            ViewBag.Menu = menu;
            
            
            return View(reviewlist);
        }

        public ActionResult AddFavourite()
        {
           

            if (db.AspNetUsers.Find(User.Identity.GetUserId()).Menus.Contains(menutmp))
            {
                return Content(" <script>function window.onload() {alert( ' Please login to see your suggested menu.' ); } </script> ");
               // Response.Write(" <script>function window.onload() {alert( ' Please login to see your suggested menu.' ); } </script> ");
            }
            else
            {
                return RedirectToAction("AddFavourite", "AspNetUsers", menutmp);
            }
            
        }

       //Response.Redirect(Request.UrlReferrer.ToString());

        public ActionResult AddReview([Bind(Include = "ReviewText")] Review review)
        {
            //Menu menu = ViewBag.Menu;                       
            ModelState.Clear();
            TryValidateModel(review);
            if (ModelState.IsValid)
            {
                review.MenuID = menutmp.MenuID;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Create","Reviews",menutmp);
            }
            return RedirectToAction("Create", "Reviews",menutmp);
        }


        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewData["Menus"] = db.Menus;
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewNum,ReviewText,Stars,MenuID")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MenuID = new SelectList(db.Menus, "MenuID", "Name", review.MenuID);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
