namespace SchoolPlatform;

class Student // Elev
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int GradeYear { get; set; } // Årskurs
    public string Class { get; set; }
    public string? SpecialNeed { get; set; }
    public bool SpecialDiet { get; set; }
    public DateTime? EndDate { get; set; }
}

class Teacher // Lärare
{
    public int TeacherId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? StreetAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public DateTime HireDate { get; set; }
    public string SubjectQualified { get; set; }
    public string EmploymentType { get; set; }
    public string? OtherSchool { get; set; }
    public bool IsActive { get; set; }
}

class Guardian // Vårdnadshavare
{
    public int GuardianId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
    public string? Relation { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public bool IsPrimaryContact { get; set; }
}

class Subject // Ämne
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string SubjectCode { get; set; }
    public string Description { get; set; }
}

class Room // Undervisningssal
{
    public int RoomId { get; set; }
    public int RoomNumber { get; set; }
    public int Capacity { get; set; }
    public string Equipment { get; set; }
    public bool IsActive { get; set; }
}

class GuardianStudent
{
    public int GardianId { get; set; }
    public int StudentId { get; set; }
}

class TeacherSubject
{
    public int TecherId { get; set; }
    public int SubjectId { get; set; }
}

class StudentTeacher
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
}

class Lesson // Lektion
{
    public int LessonId { get; set; }
    public int RoomId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public string LessonName { get; set; }
    public string LessonCode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxStudents { get; set; }
    public int IsActive { get; set; }
}

class StudentLesson 
{
    public int StudentId { get; set; }
    public int LessonId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
}

class SubjectGrade // Betyg
{
    public int SubjectGradeId { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public string GradeValue { get; set; }
    public DateTime GradeDate { get; set; }
    public string Term { get; set; }
    public bool IsFinal { get; set; }
    public string Comment { get; set; }

    public Student Student { get; set; }
    public Subject Subject { get; set; }
}

class Attendance // Närvaro
{
    public int AttendanceId { get; set; }
    public int StudentId { get; set; }
    public int LessonId { get; set; } 
    public DateTime DateTime { get; set; }
    public bool IsPresent { get; set; }
    public string? Comment { get; set; }
}



