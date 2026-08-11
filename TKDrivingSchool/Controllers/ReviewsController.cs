using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TKDrivingSchool.Models;

namespace TKDrivingSchool.Controllers
{
    public class ReviewsController : Controller
    {
        private TKDrivingSchoolContext db = new TKDrivingSchoolContext();

        // GET: Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Booking).Include(r => r.Instructor).Include(r => r.Student).Include(r => r.Vehicle);
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

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Status");
            ViewBag.InstructorID = new SelectList(db.Instructors, "UserID", "FirstName");
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName");
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNumber");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReviewID,StudentID,BookingID,InstructorID,VehicleID,Rating,Comment,ReviewDate,IsApproved,ReviewType")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Status", review.BookingID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "UserID", "FirstName", review.InstructorID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", review.StudentID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNumber", review.VehicleID);
            return View(review);
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
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Status", review.BookingID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "UserID", "FirstName", review.InstructorID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", review.StudentID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNumber", review.VehicleID);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReviewID,StudentID,BookingID,InstructorID,VehicleID,Rating,Comment,ReviewDate,IsApproved,ReviewType")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Status", review.BookingID);
            ViewBag.InstructorID = new SelectList(db.Instructors, "UserID", "FirstName", review.InstructorID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", review.StudentID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistrationNumber", review.VehicleID);
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
