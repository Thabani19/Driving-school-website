using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TKDrivingSchool.Models;
using System.Data.Entity;

namespace TKDrivingSchool.Controllers
{
    public class AdminDashboardController : Controller
    {

        private TKDrivingSchoolContext db = new TKDrivingSchoolContext();

        // ============================================================
        // MAIN DASHBOARD
        // ============================================================
        public ActionResult Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                AdminName = "Admin User",
                AdminEmail = "admin@tkdriving.co.za",

                // Statistics
                TotalStudents = db.Students.Count(),
                TotalInstructors = db.Instructors.Count(),
                TotalVehicles = db.Vehicles.Count(),
                TotalBookings = db.Bookings.Count(),

                // ⭐ FIXED: Today's Stats using DbFunctions.TruncateTime
                TodayBookings = db.Bookings
                    .Where(b => DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(DateTime.Today))
                    .Count(),
                TodayRevenue = db.Payments
                    .Where(p => p.PaymentStatus == "Completed"
                        && DbFunctions.TruncateTime(p.PaymentDate) == DbFunctions.TruncateTime(DateTime.Today))
                    .Sum(p => (decimal?)p.Amount) ?? 0,

                PendingBookings = db.Bookings
                    .Where(b => b.Status == "Pending")
                    .Count(),
                PendingReviews = db.Reviews
                    .Where(r => r.IsApproved == false)
                    .Count(),

                TotalRevenue = db.Payments
                    .Where(p => p.PaymentStatus == "Completed")
                    .Sum(p => (decimal?)p.Amount) ?? 0,
                MonthlyRevenue = db.Payments
                    .Where(p => p.PaymentStatus == "Completed"
                        && p.PaymentDate.Month == DateTime.Now.Month
                        && p.PaymentDate.Year == DateTime.Now.Year)
                    .Sum(p => (decimal?)p.Amount) ?? 0,

                RecentActivities = GetRecentActivities(),
                BookingStatusCounts = GetBookingStatusCounts(),
                MonthlyTrend = GetMonthlyTrend()
            };

            return View(viewModel);
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        private int GetCurrentAdminId()
        {
            var admin = db.Administrators.FirstOrDefault();
            return admin?.UserID ?? 1;
        }

        private List<ActivityViewModel> GetRecentActivities()
        {
            var activities = new List<ActivityViewModel>();

            // Recent Bookings
            var recentBookings = db.Bookings
                .Include(b => b.Student)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToList();

            foreach (var booking in recentBookings)
            {
                activities.Add(new ActivityViewModel
                {
                    Description = $"<strong>{booking.Student.FirstName} {booking.Student.LastName}</strong> booked a lesson",
                    TimeAgo = GetTimeAgo(booking.CreatedAt),
                    Icon = "fa-calendar-plus",
                    Color = "text-primary",
                    Type = "Booking"
                });
            }

            // Recent Payments
            var recentPayments = db.Payments
                .Include(p => p.Booking)
                .Include(p => p.Booking.Student)
                .Where(p => p.PaymentStatus == "Completed")
                .OrderByDescending(p => p.PaymentDate)
                .Take(3)
                .ToList();

            foreach (var payment in recentPayments)
            {
                activities.Add(new ActivityViewModel
                {
                    Description = $"<strong>{payment.Booking.Student.FirstName}</strong> paid R{payment.Amount:N2}",
                    TimeAgo = GetTimeAgo(payment.PaymentDate),
                    Icon = "fa-credit-card",
                    Color = "text-success",
                    Type = "Payment"
                });
            }

            // Recent Reviews
            var recentReviews = db.Reviews
                .Include(r => r.Student)
                .Where(r => r.IsApproved == false)
                .OrderByDescending(r => r.ReviewDate)
                .Take(3)
                .ToList();

            foreach (var review in recentReviews)
            {
                activities.Add(new ActivityViewModel
                {
                    Description = $"<strong>{review.Student.FirstName}</strong> left a {review.Rating}-star review (pending approval)",
                    TimeAgo = GetTimeAgo(review.ReviewDate),
                    Icon = "fa-star",
                    Color = "text-warning",
                    Type = "Review"
                });
            }

            return activities
                .OrderByDescending(a => a.TimeAgo)
                .Take(10)
                .ToList();
        }

        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)}w ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)}mo ago";
            return $"{(int)(timeSpan.TotalDays / 365)}y ago";
        }

        private Dictionary<string, int> GetBookingStatusCounts()
        {
            return db.Bookings
                .GroupBy(b => b.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Status ?? "Unknown", x => x.Count);
        }

        private List<MonthlyTrendViewModel> GetMonthlyTrend()
        {
            var currentYear = DateTime.Now.Year;
            var months = Enumerable.Range(1, 12);

            return months.Select(m => new MonthlyTrendViewModel
            {
                Month = m,
                MonthName = new DateTime(currentYear, m, 1).ToString("MMM"),
                BookingCount = db.Bookings
                    .Count(b => b.BookingDate.Month == m && b.BookingDate.Year == currentYear),
                Revenue = db.Payments
                    .Where(p => p.PaymentStatus == "Completed"
                        && p.PaymentDate.Month == m
                        && p.PaymentDate.Year == currentYear)
                    .Sum(p => (decimal?)p.Amount) ?? 0
            }).ToList();
        }

        // ============================================================
        // AJAX METHODS
        // ============================================================

        [HttpGet]
        public JsonResult GetRecentActivitiesJson()
        {
            var activities = GetRecentActivities();
            return Json(activities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStatsJson()
        {
            var stats = new
            {
                TotalStudents = db.Students.Count(),
                TotalInstructors = db.Instructors.Count(),
                TotalVehicles = db.Vehicles.Count(),
                TotalBookings = db.Bookings.Count(),
                // ⭐ FIXED: Use DbFunctions.TruncateTime here too
                TodayBookings = db.Bookings
                    .Count(b => DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(DateTime.Today)),
                PendingReviews = db.Reviews.Count(r => !r.IsApproved),
                TotalRevenue = db.Payments
                    .Where(p => p.PaymentStatus == "Completed")
                    .Sum(p => (decimal?)p.Amount) ?? 0
            };
            return Json(stats, JsonRequestBehavior.AllowGet);
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

    // ============================================================
    // VIEW MODELS
    // ============================================================

    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalBookings { get; set; }
        public int TodayBookings { get; set; }
        public decimal TodayRevenue { get; set; }
        public int PendingBookings { get; set; }
        public int PendingReviews { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public List<ActivityViewModel> RecentActivities { get; set; }
        public Dictionary<string, int> BookingStatusCounts { get; set; }
        public List<MonthlyTrendViewModel> MonthlyTrend { get; set; }
    }

    public class ActivityViewModel
    {
        public string Description { get; set; }
        public string TimeAgo { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
    }

    public class MonthlyTrendViewModel
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int BookingCount { get; set; }
        public decimal Revenue { get; set; }
    }


}
