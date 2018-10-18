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
      //  private static string tmpdata;
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
            
            menuview.Add("Name: " + menu.Name);
            menuview.Add("Carlorie: " + menu.Carlorie+"");
            menuview.Add("Description: " + menu.Description);
            ViewData["menuview"] = menuview;
            ViewBag.viewphoto = "~/Uploads/" + menu.MenuPhoto;
            List<Review> reviewlist = new List<Review>();
            
            Random r = new Random((int)DateTime.Now.Ticks);
            reviewlist = db.Reviews.Where(x => x.MenuID == menu.MenuID).ToList().OrderBy(x => r.Next()).Take(5).ToList();
            if (User.Identity.IsAuthenticated)
            {
                foreach (var item in db.AspNetUsers.Find(User.Identity.GetUserId()).Menus)
                {
                    if (item.MenuID == menutmp.MenuID)
                    {
                        ViewBag.containmenu = "Menu contained";
                        return View(reviewlist);
                    }
                }
            }
            
            

                return View(reviewlist);
        }

        public ActionResult BackToLisk()
        {
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult AddFavourite()
        {
            if (User.Identity.IsAuthenticated)
            {
                //foreach (var item in db.AspNetUsers.Find(User.Identity.GetUserId()).Menus)

                //{
                //    if (item.MenuID == menutmp.MenuID)
                //        return Content(" <script type='text / javascript'>window.onload = function () {alert( ' Please login to see your suggested menu.' ); } </script> ");

                //}

               
                return RedirectToAction("AddFavourite", "AspNetUsers", menutmp);
                
            }
            else
            {
                return RedirectToAction("Login", "Account", routeValues: null);
            }
            
            
        }

       //Response.Redirect(Request.UrlReferrer.ToString());

        public ActionResult AddReview([Bind(Include = "ReviewText")] Review review)
        {
            //Menu menu = ViewBag.Menu;                       
            ModelState.Clear();
            TryValidateModel(review);
           // if (ModelState.IsValid)
          //  {
                review.Id = User.Identity.GetUserId();
                review.MenuID = menutmp.MenuID;
                review.Username = User.Identity.Name;
                db.Reviews.Add(review);
                db.SaveChanges();
                TempData["YourReview"] = review.ReviewText;
            return Redirect(Request.UrlReferrer.ToString());
            // }
            //   return HttpNotFound();
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
        public ActionResult DeleteMenu()
        {
            
            using (PersonalMenuEntities db = new PersonalMenuEntities())
            {
                string userid = User.Identity.GetUserId();
                List<PersonalMenu> tmp =
                db.PersonalMenus.Where(x => x.Id.Equals(userid) && x.MenuID.Equals(menutmp.MenuID)).ToList();
                db.PersonalMenus.RemoveRange(tmp);
                db.SaveChanges();

            }
                                    
            return RedirectToAction("Create", "Reviews",menutmp);
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
