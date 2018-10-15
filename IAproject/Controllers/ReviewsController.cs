using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAproject.Models;

namespace IAproject.Controllers
{
    public class ReviewsController : Controller
    {
        Menu menutmp;
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
            ViewBag.Menu = menu.MenuID;
            foreach (var tmp in reviewlist)
            {
                tmp.Menu = menu;
            }
            
            return View(reviewlist);
        }

        public ActionResult AddReview([Bind(Include = "ReviewText")] Review review, int menuid)
        {
            
            Review reviewtmp = new Review();
            
            ModelState.Clear();
            TryValidateModel(review);
            if (ModelState.IsValid)
            {
                review.MenuID = menuid;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("ProvideSuggest","SeeMenu");
            }
            return RedirectToAction("ProvideSuggest", "SeeMenu");
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
