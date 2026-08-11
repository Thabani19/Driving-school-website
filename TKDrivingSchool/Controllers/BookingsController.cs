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
    public class BookingsController : Controller
    {
        private TKDrivingSchoolContext db = new TKDrivingSchoolContext();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Lesson).Include(b => b.LessonType).Include(b => b.Payment).Include(b => b.Schedule).Include(b => b.Student);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.BookingID = new SelectList(db.Lessons, "BookingID", "AttendanceStatus");
            ViewBag.LessonTypeID = new SelectList(db.LessonTypes, "LessonTypeID", "Name");
            ViewBag.BookingID = new SelectList(db.Payments, "BookingID", "PaymentMethod");
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "Status");
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,StudentID,ScheduleID,LessonTypeID,BookingDate,Status,PaymentStatus,BookingNotes,CreatedAt,ConfirmedAt,CancelledAt,CancellationReason")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookingID = new SelectList(db.Lessons, "BookingID", "AttendanceStatus", booking.BookingID);
            ViewBag.LessonTypeID = new SelectList(db.LessonTypes, "LessonTypeID", "Name", booking.LessonTypeID);
            ViewBag.BookingID = new SelectList(db.Payments, "BookingID", "PaymentMethod", booking.BookingID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "Status", booking.ScheduleID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", booking.StudentID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingID = new SelectList(db.Lessons, "BookingID", "AttendanceStatus", booking.BookingID);
            ViewBag.LessonTypeID = new SelectList(db.LessonTypes, "LessonTypeID", "Name", booking.LessonTypeID);
            ViewBag.BookingID = new SelectList(db.Payments, "BookingID", "PaymentMethod", booking.BookingID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "Status", booking.ScheduleID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", booking.StudentID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,StudentID,ScheduleID,LessonTypeID,BookingDate,Status,PaymentStatus,BookingNotes,CreatedAt,ConfirmedAt,CancelledAt,CancellationReason")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingID = new SelectList(db.Lessons, "BookingID", "AttendanceStatus", booking.BookingID);
            ViewBag.LessonTypeID = new SelectList(db.LessonTypes, "LessonTypeID", "Name", booking.LessonTypeID);
            ViewBag.BookingID = new SelectList(db.Payments, "BookingID", "PaymentMethod", booking.BookingID);
            ViewBag.ScheduleID = new SelectList(db.Schedules, "ScheduleID", "Status", booking.ScheduleID);
            ViewBag.StudentID = new SelectList(db.Students, "UserID", "FirstName", booking.StudentID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
