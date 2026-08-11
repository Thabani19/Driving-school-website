namespace TKDrivingSchool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        AdminLevel = c.String(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.BookingChanges",
                c => new
                    {
                        ChangeID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        ChangedByUserID = c.Int(nullable: false),
                        ChangeType = c.String(),
                        OldScheduleID = c.Int(nullable: false),
                        NewScheduleID = c.Int(),
                        Reason = c.String(),
                        ChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChangeID)
                .ForeignKey("dbo.Bookings", t => t.BookingID)
                .ForeignKey("dbo.Users", t => t.ChangedByUserID)
                .Index(t => t.BookingID)
                .Index(t => t.ChangedByUserID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ScheduleID = c.Int(nullable: false),
                        LessonTypeID = c.Int(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        PaymentStatus = c.String(),
                        BookingNotes = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ConfirmedAt = c.DateTime(),
                        CancelledAt = c.DateTime(),
                        CancellationReason = c.String(),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.LessonTypes", t => t.LessonTypeID)
                .ForeignKey("dbo.Schedules", t => t.ScheduleID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .Index(t => t.StudentID)
                .Index(t => t.ScheduleID)
                .Index(t => t.LessonTypeID);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        LessonDate = c.DateTime(nullable: false),
                        AttendanceStatus = c.String(),
                        LessonStatus = c.String(),
                        InstructorNotes = c.String(),
                        ProgressNotes = c.String(),
                        CompletedAt = c.DateTime(),
                        StudentRating = c.Int(),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Bookings", t => t.BookingID)
                .Index(t => t.BookingID);
            
            CreateTable(
                "dbo.LessonTypes",
                c => new
                    {
                        LessonTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Duration = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LessonTypeID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        BookingID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethod = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentStatus = c.String(),
                        TransactionReference = c.String(),
                        PaymentGateway = c.String(),
                        RefundDate = c.DateTime(),
                        RefundReason = c.String(),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Bookings", t => t.BookingID)
                .Index(t => t.BookingID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        BookingID = c.Int(nullable: false),
                        InstructorID = c.Int(),
                        VehicleID = c.Int(),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        ReviewDate = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        ReviewType = c.String(),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Bookings", t => t.BookingID)
                .ForeignKey("dbo.Instructors", t => t.InstructorID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .ForeignKey("dbo.Vehicles", t => t.VehicleID)
                .Index(t => t.StudentID)
                .Index(t => t.BookingID)
                .Index(t => t.InstructorID)
                .Index(t => t.VehicleID);
            
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Phone = c.String(),
                        LicenceType = c.String(),
                        AvailabilityStatus = c.String(),
                        HourlyRate = c.Decimal(precision: 18, scale: 2),
                        YearsOfExperience = c.Int(),
                        HireDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ScheduleID = c.Int(nullable: false, identity: true),
                        InstructorID = c.Int(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        IsAvailable = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ScheduleID)
                .ForeignKey("dbo.Instructors", t => t.InstructorID)
                .ForeignKey("dbo.Vehicles", t => t.VehicleID)
                .Index(t => t.InstructorID)
                .Index(t => t.VehicleID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        RegistrationNumber = c.String(nullable: false, maxLength: 50),
                        Make = c.String(),
                        Model = c.String(),
                        Year = c.Int(nullable: false),
                        VehicleType = c.String(),
                        AvailabilityStatus = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Mileage = c.Int(),
                        LastServiceDate = c.DateTime(),
                        NextServiceDue = c.DateTime(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.VehicleID)
                .Index(t => t.RegistrationNumber, unique: true);
            
            CreateTable(
                "dbo.VehicleMaintenances",
                c => new
                    {
                        MaintenanceID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        MaintenanceDate = c.DateTime(nullable: false),
                        Type = c.String(),
                        Description = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NextServiceDate = c.DateTime(),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaintenanceID)
                .ForeignKey("dbo.Vehicles", t => t.VehicleID)
                .Index(t => t.VehicleID);
            
            CreateTable(
                "dbo.InstructorUnavailabilities",
                c => new
                    {
                        UnavailabilityID = c.Int(nullable: false, identity: true),
                        InstructorID = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Reason = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UnavailabilityID)
                .ForeignKey("dbo.Instructors", t => t.InstructorID)
                .Index(t => t.InstructorID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        DateOfBirth = c.DateTime(),
                        Phone = c.String(),
                        Address = c.String(),
                        LicenceType = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        TotalLessonsCompleted = c.Int(),
                        TotalLessonsBooked = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.CourseEnrolments",
                c => new
                    {
                        EnrolmentID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        EnrolmentDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        CompletionDate = c.DateTime(),
                        ProgressPercentage = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.EnrolmentID)
                .ForeignKey("dbo.Courses", t => t.CourseID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .Index(t => t.StudentID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false),
                        Description = c.String(),
                        NumberOfLessons = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Duration = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CourseID);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        PaymentMethodID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        MethodType = c.String(),
                        Provider = c.String(),
                        AccountNumberMasked = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentMethodID)
                .ForeignKey("dbo.Students", t => t.StudentID)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        NotificationType = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ReadAt = c.DateTime(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.NotificationID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        AuditID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        Action = c.String(),
                        Entity = c.String(),
                        EntityID = c.Int(),
                        OldValues = c.String(),
                        NewValues = c.String(),
                        IPAddress = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AuditID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        ReportName = c.String(nullable: false),
                        ReportType = c.String(),
                        Parameters = c.String(),
                        ResultData = c.String(),
                        GeneratedByUserID = c.Int(),
                        GeneratedAt = c.DateTime(nullable: false),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.Users", t => t.GeneratedByUserID)
                .Index(t => t.GeneratedByUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "GeneratedByUserID", "dbo.Users");
            DropForeignKey("dbo.AuditLogs", "UserID", "dbo.Users");
            DropForeignKey("dbo.Students", "UserID", "dbo.Users");
            DropForeignKey("dbo.Notifications", "UserID", "dbo.Users");
            DropForeignKey("dbo.Instructors", "UserID", "dbo.Users");
            DropForeignKey("dbo.BookingChanges", "ChangedByUserID", "dbo.Users");
            DropForeignKey("dbo.BookingChanges", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Bookings", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Bookings", "ScheduleID", "dbo.Schedules");
            DropForeignKey("dbo.Reviews", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Reviews", "StudentID", "dbo.Students");
            DropForeignKey("dbo.PaymentMethods", "StudentID", "dbo.Students");
            DropForeignKey("dbo.CourseEnrolments", "StudentID", "dbo.Students");
            DropForeignKey("dbo.CourseEnrolments", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Reviews", "InstructorID", "dbo.Instructors");
            DropForeignKey("dbo.InstructorUnavailabilities", "InstructorID", "dbo.Instructors");
            DropForeignKey("dbo.Schedules", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleMaintenances", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Schedules", "InstructorID", "dbo.Instructors");
            DropForeignKey("dbo.Reviews", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Payments", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Bookings", "LessonTypeID", "dbo.LessonTypes");
            DropForeignKey("dbo.Lessons", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.Administrators", "UserID", "dbo.Users");
            DropIndex("dbo.Reports", new[] { "GeneratedByUserID" });
            DropIndex("dbo.AuditLogs", new[] { "UserID" });
            DropIndex("dbo.Notifications", new[] { "UserID" });
            DropIndex("dbo.PaymentMethods", new[] { "StudentID" });
            DropIndex("dbo.CourseEnrolments", new[] { "CourseID" });
            DropIndex("dbo.CourseEnrolments", new[] { "StudentID" });
            DropIndex("dbo.Students", new[] { "UserID" });
            DropIndex("dbo.InstructorUnavailabilities", new[] { "InstructorID" });
            DropIndex("dbo.VehicleMaintenances", new[] { "VehicleID" });
            DropIndex("dbo.Vehicles", new[] { "RegistrationNumber" });
            DropIndex("dbo.Schedules", new[] { "VehicleID" });
            DropIndex("dbo.Schedules", new[] { "InstructorID" });
            DropIndex("dbo.Instructors", new[] { "UserID" });
            DropIndex("dbo.Reviews", new[] { "VehicleID" });
            DropIndex("dbo.Reviews", new[] { "InstructorID" });
            DropIndex("dbo.Reviews", new[] { "BookingID" });
            DropIndex("dbo.Reviews", new[] { "StudentID" });
            DropIndex("dbo.Payments", new[] { "BookingID" });
            DropIndex("dbo.Lessons", new[] { "BookingID" });
            DropIndex("dbo.Bookings", new[] { "LessonTypeID" });
            DropIndex("dbo.Bookings", new[] { "ScheduleID" });
            DropIndex("dbo.Bookings", new[] { "StudentID" });
            DropIndex("dbo.BookingChanges", new[] { "ChangedByUserID" });
            DropIndex("dbo.BookingChanges", new[] { "BookingID" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Administrators", new[] { "UserID" });
            DropTable("dbo.Reports");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.Notifications");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseEnrolments");
            DropTable("dbo.Students");
            DropTable("dbo.InstructorUnavailabilities");
            DropTable("dbo.VehicleMaintenances");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Schedules");
            DropTable("dbo.Instructors");
            DropTable("dbo.Reviews");
            DropTable("dbo.Payments");
            DropTable("dbo.LessonTypes");
            DropTable("dbo.Lessons");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookingChanges");
            DropTable("dbo.Users");
            DropTable("dbo.Administrators");
        }
    }
}
