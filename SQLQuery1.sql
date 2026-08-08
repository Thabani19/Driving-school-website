Create database DrivingSchoolRegistration
use DrivingSchoolRegistration 

CREATE TABLE Student
(
    IdentityNumber char(13) PRIMARY KEY NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Address VARCHAR(255) NOT NULL,
    RegistrationDate DATE NOT NULL
)

Select *from Student