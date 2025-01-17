using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SchoolPlatform;

internal class DatabaseManager
{
    //  Hanterar alla databasoperationer för skolplattformen
    private IDbConnection Connect()
    {
        string connectionstring = File.ReadAllText("connectionstring.txt");
        IDbConnection connection = new SqlConnection(connectionstring);
        return connection;
    }

    // Hämtar alla elever från databasen
    public IEnumerable<Student> GetAllStudents()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Student> students = connection.Query<Student>("SELECT * FROM Student");
        return students;
    }

    // Hämtar alla elevers betyg
public IEnumerable<SubjectGrade> GetAllStudentGrades()
{
    const string sql = @"
        SELECT sg.*, s.*, subj.*
        FROM SubjectGrade sg
        JOIN Student s ON sg.StudentId = s.StudentId
        JOIN Subject subj ON sg.SubjectId = subj.SubjectId
        ORDER BY s.LastName, s.FirstName, s.Class, subj.SubjectName, sg.Comment";

        using IDbConnection connection = Connect();
        return connection.Query<SubjectGrade, Student, Subject, SubjectGrade>(
        sql,
        (grade, student, subject) => {
            grade.Student = student;
            grade.Subject = subject;
            return grade;
        },
        splitOn: "StudentId,SubjectId");
}
    // Hämtar alla elever med specialkost
    public IEnumerable<Student> GetStudentsWithSpecialDiet()
    {
        using IDbConnection connection = Connect();
        string sql = @"
        SELECT s.*
        FROM Student s
        WHERE s.SpecialDiet = 1
        ORDER BY s.Class, s.LastName";

        return connection.Query<Student>(sql);
    }

    // Hämtar alla elever tillsammans med deras mentorer
    public IEnumerable<dynamic> GetStudentsWithMentors()
    {
        using IDbConnection connection = Connect();
        string sql = @"
         WITH MentorInfo AS (SELECT DISTINCT s.Class, FIRST_VALUE
            (t.FirstName + ' ' + t.LastName) OVER (PARTITION BY s.Class ORDER BY t.TeacherId) as MentorName
            FROM Student s
            JOIN StudentTeacher st ON s.StudentId = st.StudentId
            JOIN Teacher t ON st.TeacherId = t.TeacherId
            WHERE t.IsMentor = 1)
        SELECT s.FirstName + ' ' + s.LastName as StudentName, s.Class, m.MentorName
        FROM Student s
        LEFT JOIN MentorInfo m ON s.Class = m.Class
        ORDER BY s.Class, s.LastName";

        return connection.Query(sql);
    }

    // Uppdaterar en elevs information
    public void UpdateStudent(Student student)
    {
        using IDbConnection connection = Connect();
        string sql = @"
        UPDATE Student 
        SET FirstName = @FirstName,
            LastName = @LastName,
            PersonalNumber = @PersonalNumber,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            GradeYear = @GradeYear,
            Class = @Class,
            SpecialNeed = @SpecialNeed,
            SpecialDiet = @SpecialDiet,
            EndDate = @EndDate
        WHERE StudentId = @StudentId";

        connection.Execute(sql, student);
    }

    // Hämtar alla lärare
    public IEnumerable<Teacher> GetAllTeachers()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Teacher> teachers = connection.Query<Teacher>("SELECT * FROM Teacher");
        return teachers;
    }

    // Beräknar antal elever per lärare
    public IEnumerable<dynamic> GetStudentCountPerTeacher()
    {
        using IDbConnection connection = Connect();
        string sql = @"
               SELECT t.FirstName + ' ' + t.LastName as TeacherName, COUNT(DISTINCT st.StudentId) as NumberOfStudents, STUFF((SELECT DISTINCT ', ' + s2.Class 
               FROM StudentTeacher st2
               JOIN Student s2 ON st2.StudentId = s2.StudentId
               WHERE st2.TeacherId = t.TeacherId
               FOR XML PATH('')), 1, 2, '') as Classes
        FROM Teacher t
        LEFT JOIN StudentTeacher st ON t.TeacherId = st.TeacherId
        LEFT JOIN Student s ON st.StudentId = s.StudentId
        WHERE t.IsActive = 1
        GROUP BY t.TeacherId, t.FirstName, t.LastName
        ORDER BY NumberOfStudents DESC";

        return connection.Query(sql);
    }

    // Uppdaterar en lärares information
    public void UpdateTeacher(Teacher teacher)
    {
        using IDbConnection connection = Connect();
        string sql = @"
        UPDATE Teacher 
        SET FirstName = @FirstName,
            LastName = @LastName,
            PersonalNumber = @PersonalNumber,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            StreetAddress = @StreetAddress,
            PostalCode = @PostalCode,
            City = @City,
            HireDate = @HireDate,
            SubjectQualified = @SubjectQualified,
            EmploymentType = @EmploymentType,
            OtherSchool = @OtherSchool,
            IsActive = @IsActive
        WHERE TeacherId = @TeacherId";

        connection.Execute(sql, teacher);
    }

    // Hämtar alla salar
    public IEnumerable<Room> GetAllRooms()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Room> rooms = connection.Query<Room>("SELECT * FROM Room");
        return rooms;
    }

    // Hämtar alla tillgängliga (ej aktiva) salar
    public IEnumerable<Room> GetAvailableRooms()
    {
        using IDbConnection connection = Connect();
        return connection.Query<Room>("SELECT * FROM Room WHERE IsActive = 0 ORDER BY RoomNumber");
    }

    public void UpdateRoom(Room room)
    {
        using IDbConnection connection = Connect();
        connection.Execute("UPDATE Room SET RoomNumber = @RoomNumber, Capacity = @Capacity, Equipment = @Equipment, IsActive = @IsActive WHERE RoomId = @RoomId", room);
    }

    // Lägg till en ny sal
    public int AddRoom(Room room)
    {
        using IDbConnection connection = Connect();
        string sql = @"
        INSERT INTO Room (RoomNumber, Capacity, Equipment, IsActive)
        VALUES (@RoomNumber, @Capacity, @Equipment, @IsActive);
        SELECT CAST(SCOPE_IDENTITY() as int)";

        return connection.QuerySingle<int>(sql, room);
    }

    // Tar bort en sal
    public void DeleteRoom(int roomId)
    {
        using IDbConnection connection = Connect();
        // Först kontrollera om salen används i några lektioner
        string checkSql = @"
        SELECT COUNT(*) 
        FROM Lesson 
        WHERE RoomId = @RoomId";

        int lessonCount = connection.QuerySingle<int>(checkSql, new { RoomId = roomId });

        if (lessonCount > 0)
        {
            throw new Exception("Kan inte ta bort salen eftersom den används i lektioner.");
        }

        string sql = "DELETE FROM Room WHERE RoomId = @RoomId";
        connection.Execute(sql, new { RoomId = roomId });
    }

    // Hämtar alla ämnen
    public IEnumerable<Subject> GetAllSubjects()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Subject> subjects = connection.Query<Subject>("SELECT * FROM Subject");
        return subjects;
    }

    // Hämtar alla lektioner
    public IEnumerable<Lesson> GetAllLessons()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Lesson> lessons = connection.Query<Lesson>("SELECT * FROM Lesson");
        return lessons;
    }

    // Hämtar all närvaroinformation för elever
    public IEnumerable<Attendance> GetAllAttendance()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Attendance> attendance = connection.Query<Attendance>("SELECT * FROM Attendance");
        return attendance;
    }

    // Hämtar alla vårdnadshavare
    public IEnumerable<Guardian> GetAllGuardians()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Guardian> teachers = connection.Query<Guardian>("SELECT * FROM Guardian");
        return teachers;
    }

    // Hämtar alla elever och deras vårdnadshavare
    public IEnumerable<(Student student, Guardian guardian)> GetStudentsAndGuardians()
    {
        using IDbConnection connection = Connect();
        string sql = @"
            SELECT s.*, g.*
            FROM Student s
            INNER JOIN GuardianStudent gs ON s.StudentId = gs.StudentId
            INNER JOIN Guardian g ON gs.GuardianId = g.GuardianId
            ORDER BY s.LastName, s.FirstName";

        return connection.Query<Student, Guardian, (Student student, Guardian guardian)>(
            sql,
            (student, guardian) => (student, guardian),
            splitOn: "GuardianId"
        );
    }

    // Hämtar en specifik sal baserat på dess ID.
    public Room GetRoomById(int roomId)
    {
        using IDbConnection connection = Connect();
        string sql = "SELECT * FROM Room WHERE RoomId = @RoomId";
        return connection.QuerySingleOrDefault<Room>(sql, new { RoomId = roomId });
    }
}
