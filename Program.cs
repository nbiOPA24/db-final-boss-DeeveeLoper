using System;

namespace SchoolPlatform;

class Program 
{
    static void Main()
    {
        DatabaseManager db = new DatabaseManager();

        while (true)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("                    === SKOLPLATTFORMEN ===                 ");
            Console.WriteLine("Välj ett alternativ genom att ange motsvarande siffra (1-18)");
            Console.WriteLine("============================================================");
            Console.WriteLine("➤ ELEVER");
            Console.WriteLine("1. Visa alla elever");
            Console.WriteLine("2. Visa alla elevers betyg för 2024");
            Console.WriteLine("3. Visa alla elever med specialkost");
            Console.WriteLine("4. Visa elever och deras mentor");
            Console.WriteLine("5. Uppdatera elev");
            Console.WriteLine("===============================================");
            Console.WriteLine("➤ PERSONAL");
            Console.WriteLine("6. Visa alla lärare");
            Console.WriteLine("7. Visa antal elever per lärare");
            Console.WriteLine("8. Uppdatera lärare");
            Console.WriteLine("===============================================");
            Console.WriteLine("➤ SALAR");
            Console.WriteLine("9. Visa alla salar");
            Console.WriteLine("10. Visa lediga salar");
            Console.WriteLine("11. Uppdatera sal");
            Console.WriteLine("12. Lägg till en ny sal");
            Console.WriteLine("13. Ta bort sal");
            Console.WriteLine("===============================================");
            Console.WriteLine("➤ UNDERVISNING");
            Console.WriteLine("14. Visa alla skolämnen");
            Console.WriteLine("15. Visa alla lektioner");
            Console.WriteLine("16. Visa närvarorapport för alla elever i åk 8A, 8B, 9A och 9B (2025-01-07)");
            Console.WriteLine("===============================================");
            Console.WriteLine("➤ VÅRDNADSHAVARE");
            Console.WriteLine("17. Visa alla vårdnadshavare och deras information");
            Console.WriteLine("18. Visa alla elever med deras vårdnadshavare");
            Console.WriteLine("===============================================");
            Console.WriteLine("a. Avsluta");
            Console.WriteLine("===============================================");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    IEnumerable<Student> students = db.GetAllStudents();

                    PrintHeader("Elever");

                    foreach (Student s in students)
                    {
                        Console.WriteLine($"Id: {s.StudentId}");
                        Console.WriteLine($"Namn: {s.FirstName} {s.LastName}");
                        Console.WriteLine($"Personnummer: {s.PersonalNumber}");
                        Console.WriteLine($"Epost: {s.Email}");
                        Console.WriteLine($"Telefon: {s.PhoneNumber}");
                        Console.WriteLine($"Årskurs: {s.GradeYear}");
                        Console.WriteLine($"Klass: {s.Class}");
                        Console.WriteLine($"Särskilt stöd: {s.SpecialNeed}");
                        Console.WriteLine($"Specialkost: {s.SpecialDiet}");
                        Console.WriteLine($"Avslutat: {(s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "2":
                    IEnumerable<SubjectGrade> grades = db.GetAllStudentGrades();

                    PrintHeader("Elevers betyg");

                    foreach (var g in grades)
                    {
                        Console.WriteLine($"Elev: {g.Student.FirstName} {g.Student.LastName}");
                        Console.WriteLine($"Ämne: {g.Subject.SubjectName}");
                        Console.WriteLine($"Betyg: {g.GradeValue}");
                        Console.WriteLine($"Termin: {g.Term}");
                        Console.WriteLine($"Slutbetyg: {(g.IsFinal ? "Ja" : "Nej")}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "3":
                    var specialDietStudents = db.GetStudentsWithSpecialDiet();

                    PrintHeader("Elever med specialkost");

                    foreach (var student in specialDietStudents)
                    {
                        Console.WriteLine($"Namn: {student.FirstName} {student.LastName}");
                        Console.WriteLine($"Klass: {student.Class}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "4":

                    PrintHeader("Elever och deras klassmentorer");

                    var classMentorships = db.GetStudentsWithClassMentors();
                    string currentClass = "";

                    foreach (var c in classMentorships)
                    {
                        if (currentClass != c.Class)
                        {                                            
                            currentClass = c.Class;
                            Console.WriteLine($"\nKlass {c.Class}");
                            Console.WriteLine($"Mentor: {c.MentorName}");
                            Console.WriteLine("-----------------------------------------------");
                        }
                        Console.WriteLine($"Elev: {c.StudentName}");
                    }
                    break;

                case "5":

                    PrintHeader("Uppdatera elev");

                    Console.WriteLine("\nAnge ID på eleven som ska uppdateras:");
                    int updateStudentId = int.Parse(Console.ReadLine());

                    // Hämta befintlig elev
                    var allStudents = db.GetAllStudents();

                    var existingStudent = allStudents.FirstOrDefault(s => s.StudentId == updateStudentId);

                    if (existingStudent != null)
                    {
                        existingStudent.FirstName = GetUpdatedValue("Förnamn", existingStudent.FirstName);
                        existingStudent.LastName = GetUpdatedValue("Efternamn", existingStudent.LastName);
                        existingStudent.PersonalNumber = GetUpdatedValue("Personnummer", existingStudent.PersonalNumber);
                        existingStudent.Email = GetUpdatedValue("Email", existingStudent.Email);
                        existingStudent.PhoneNumber = GetUpdatedValue("Telefon", existingStudent.PhoneNumber);
                        existingStudent.GradeYear = int.Parse(GetUpdatedValue("Årskurs", existingStudent.GradeYear.ToString()));
                        existingStudent.Class = GetUpdatedValue("Klass", existingStudent.Class);
                        existingStudent.SpecialNeed = GetUpdatedValue("Särskilt stöd", existingStudent.SpecialNeed);
                        existingStudent.SpecialDiet = bool.Parse(GetUpdatedValue("Specialkost (true/false)", existingStudent.SpecialDiet.ToString()));
                        existingStudent.EndDate = DateTime.TryParse(GetUpdatedValue("Slutdatum (YYYY-MM-DD eller tryck Enter)",
                        existingStudent.EndDate?.ToString("yyyy-MM-dd") ?? ""), out DateTime endDate) ? endDate : (DateTime?)null;

                        db.UpdateStudent(existingStudent);
                        Console.WriteLine("Eleven har uppdaterats!");
                    }
                    else
                    {
                        Console.WriteLine("Hittade ingen elev med det ID:t");
                    }
                    break;

                case "6":

                    PrintHeader("Lärare");

                    IEnumerable<Teacher> teachers = db.GetAllTeachers();
                    foreach (Teacher t in teachers)
                    {
                        Console.WriteLine($"Id: {t.TeacherId}");
                        Console.WriteLine($"Namn: {t.FirstName} {t.LastName}");
                        Console.WriteLine($"Personnummer: {t.PersonalNumber}");
                        Console.WriteLine($"Epost: {t.Email}");
                        Console.WriteLine($"Telefon: {t.PhoneNumber}");
                        Console.WriteLine($"Adress: {t.StreetAddress}");
                        Console.WriteLine($"Postnummer: {t.PostalCode}");
                        Console.WriteLine($"Stad: {t.City}");
                        Console.WriteLine($"Anställningsdatum: {t.HireDate:yyyy-MM-dd}");
                        Console.WriteLine($"Kvalificerad inom ämnen: {t.SubjectQualified}");
                        Console.WriteLine($"Anställningsform: {t.EmploymentType}");
                        Console.WriteLine($"Annan skola: {t.OtherSchool}");
                        Console.WriteLine($"Aktiv: {t.IsActive}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;


                case "7":

                    PrintHeader("Antal elever per lärare");

                    var teacherLoads = db.GetStudentCountPerTeacher();

                    foreach (var t in teacherLoads)
                    {
                        Console.WriteLine($"Lärare: {t.TeacherName}");
                        Console.WriteLine($"Antal elever: {t.NumberOfStudents}");
                        Console.WriteLine($"Klasser: {(string.IsNullOrEmpty(t.Classes) ? "Inga klasser" : t.Classes)}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "8":

                    PrintHeader("Uppdatera lärare");

                    Console.WriteLine("\nAnge ID på lärare som ska uppdateras:");

                    int updateTeacherId = int.Parse(Console.ReadLine());

                    // Hämta befintlig lärare
                    var allTeachers = db.GetAllTeachers();

                    var existingTeacher = allTeachers.FirstOrDefault(s => s.TeacherId == updateTeacherId);

                    if (existingTeacher != null)
                    {
                        existingTeacher.FirstName = GetUpdatedValue("Förnamn", existingTeacher.FirstName);
                        existingTeacher.LastName = GetUpdatedValue("Efternamn", existingTeacher.LastName);
                        existingTeacher.PersonalNumber = GetUpdatedValue("Personnummer", existingTeacher.PersonalNumber);
                        existingTeacher.Email = GetUpdatedValue("Email", existingTeacher.Email);
                        existingTeacher.PhoneNumber = GetUpdatedValue("Telefon", existingTeacher.PhoneNumber);
                        existingTeacher.StreetAddress = GetUpdatedValue("Adress", existingTeacher.StreetAddress);
                        existingTeacher.PostalCode = GetUpdatedValue("Postnummer", existingTeacher.PostalCode);
                        existingTeacher.City = GetUpdatedValue("Stad", existingTeacher.City);
                        existingTeacher.SubjectQualified = GetUpdatedValue("Ämnesexpertis", existingTeacher.SubjectQualified);
                        existingTeacher.EmploymentType = GetUpdatedValue("Anställningstyp", existingTeacher.EmploymentType);
                        existingTeacher.OtherSchool = GetUpdatedValue("Andra skolor", existingTeacher.OtherSchool);
                        existingTeacher.IsActive = bool.Parse(GetUpdatedValue("Aktiv (true/false)", existingTeacher.IsActive.ToString()));

                        db.UpdateTeacher(existingTeacher);
                        Console.WriteLine("Lärare har uppdaterats!");
                    }
                    else
                    {
                        Console.WriteLine("Hittade ingen lärare med det ID:t");
                    }
                    break;

                case "9":
                    IEnumerable<Room> rooms = db.GetAllRooms();

                    PrintHeader("Salar");

                    foreach (Room r in rooms)
                    {
                        Console.WriteLine($"Id: {r.RoomId}");
                        Console.WriteLine($"Salsnummer: {r.RoomNumber}");
                        Console.WriteLine($"Kapacitet: {r.Capacity}");
                        Console.WriteLine($"Utrustning: {(string.IsNullOrEmpty(r.Equipment) ? "Ingen utrustning listad" : r.Equipment)}");
                        Console.WriteLine($"Aktiv: {r.IsActive}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;


                case "10":
                    IEnumerable<Room> availableRooms = db.GetAvailableRooms();

                    PrintHeader("Lediga salar");

                    foreach (Room room in availableRooms)
                    {
                        Console.WriteLine($"Id: {room.RoomId}");
                        Console.WriteLine($"Sal: {room.RoomNumber}");
                        Console.WriteLine($"Kapacitet: {room.Capacity}");
                        Console.WriteLine($"Utrustning: {room.Equipment}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "11":
                    PrintHeader("Uppdatera sal");
                    Console.WriteLine("\nAnge ID på salen som ska uppdateras:");
                    int updateRoomId = int.Parse(Console.ReadLine());

                    var allRooms = db.GetAllRooms();
                    var existingRoom = allRooms.FirstOrDefault(r => r.RoomId == updateRoomId);

                    if (existingRoom != null)
                    {
                        existingRoom.RoomNumber = int.Parse(GetUpdatedValue("Salsnummer", existingRoom.RoomNumber.ToString()));
                        existingRoom.Capacity = int.Parse(GetUpdatedValue("Kapacitet", existingRoom.Capacity.ToString()));
                        existingRoom.Equipment = GetUpdatedValue("Utrustning", existingRoom.Equipment);
                        existingRoom.IsActive = bool.Parse(GetUpdatedValue("Aktiv (true/false)", existingRoom.IsActive.ToString()));

                        db.UpdateRoom(existingRoom);
                        Console.WriteLine("Salen har uppdaterats!");
                    }
                    else
                    {
                        Console.WriteLine("Hittade ingen sal med det ID:t");
                    }
                    break;


                case "12":

                    PrintHeader("Lägg till en ny sal");

                    var newRoom = new Room();

                    try
                    {
                        Console.Write("Salsnummer: ");
                        newRoom.RoomNumber = int.Parse(Console.ReadLine());
                        Console.Write("Kapacitet (antal platser): ");
                        newRoom.Capacity = int.Parse(Console.ReadLine());
                        Console.Write("Utrustning (separera med kommatecken): ");
                        newRoom.Equipment = Console.ReadLine();
                        newRoom.IsActive = true;  // Ny sal är aktiv by default

                        Console.WriteLine("\nÄr du säker på att du vill lägga till salen? (ja/nej)");
                        if (Console.ReadLine().ToLower() == "ja")
                        {
                            int newRoomId = db.AddRoom(newRoom);
                            Console.WriteLine($"Salen har lagts till med ID: {newRoomId}");
                        }
                        else
                        {
                            Console.WriteLine("Operationen avbröts.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Felaktig inmatning. Salsnummer och kapacitet måste vara siffror.");
                    }
                    break;

                case "13":

                    PrintHeader("Ta bort sal");

                    Console.WriteLine("Ange ID på salen som ska tas bort (eller 0 för att avbryta):");
                    if (!int.TryParse(Console.ReadLine(), out int deleteRoomId) || deleteRoomId == 0)
                    {
                        Console.WriteLine("Borttagning avbruten.");
                        break;
                    }
                    try
                    {
                        if (ConfirmAction("ta bort salen"))
                        {
                            db.DeleteRoom(deleteRoomId);
                            Console.WriteLine("Salen har tagits bort!");
                        }
                        else
                        {
                            Console.WriteLine("Borttagning avbruten.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Felaktig inmatning. ID måste vara siffror.");
                    }
                    break;

                case "14":
                    IEnumerable<Subject> subjects = db.GetAllSubjects();

                    PrintHeader("Skolämnen");

                    foreach (Subject sub in subjects)
                    {
                        Console.WriteLine($"Id: {sub.SubjectId}");
                        Console.WriteLine($"Namn: {sub.SubjectName}");
                        Console.WriteLine($"Ämneskod: {sub.SubjectCode}");
                        Console.WriteLine($"Beskrivning: {sub.Description}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "15":
                    IEnumerable<Lesson> lessons = db.GetAllLessons();

                    PrintHeader("Lektioner");

                    foreach (Lesson l in lessons)
                    {
                        Console.WriteLine($"Id: {l.LessonId}");
                        Console.WriteLine($"Namn: {l.LessonName}");
                        Console.WriteLine($"Lektionskod: {l.LessonCode}");
                        Console.WriteLine($"Rum-ID: {l.RoomId}");
                        Console.WriteLine($"Lärar-ID: {l.TeacherId}");
                        Console.WriteLine($"Ämnes-ID: {l.SubjectId}");
                        Console.WriteLine($"Startdatum: {l.StartDate:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Slutdatum: {l.EndDate:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Maximalt antal elever: {l.MaxStudents}");
                        Console.WriteLine($"Aktiv: {l.IsActive}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "16":
   
                    IEnumerable<Attendance> attendance = db.GetAllAttendance();

                    PrintHeader("Elevers närvaro");

                    foreach (Attendance a in attendance)
                    {
                        Console.WriteLine($"Id: {a.AttendanceId}");
                        Console.WriteLine($"Elev-ID: {a.StudentId}");
                        Console.WriteLine($"Lektion-ID: {a.LessonId}");
                        Console.WriteLine($"Startdatum: {a.DateTime:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Närvarande: {a.IsPresent}");
                        Console.WriteLine($"Kommentar: {(string.IsNullOrEmpty(a.Comment) ? "Ingen kommentar" : a.Comment)}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "17":
                    IEnumerable<Guardian> guardians = db.GetAllGuardians();
                    PrintHeader("Vårdnadshavare");
                    foreach (Guardian g in guardians)
                    {
                        Console.WriteLine($"Id: {g.GuardianId}");
                        Console.WriteLine($"Namn: {g.FirstName} {g.LastName}");
                        Console.WriteLine($"Personnummer: {g.PersonalNumber}");
                        Console.WriteLine($"Relation: {g.Relation}");
                        Console.WriteLine($"Epost: {g.Email}");
                        Console.WriteLine($"Telefon: {g.PhoneNumber}");
                        Console.WriteLine($"Adress: {g.StreetAddress}");
                        Console.WriteLine($"Postnummer: {g.PostalCode}");
                        Console.WriteLine($"Stad: {g.City}");
                        Console.WriteLine($"Huvudkontakt: {g.IsPrimaryContact}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "18":
                    var studentsWithGuardians = db.GetStudentsAndGuardians();

                    PrintHeader("Elever och deras vårdnadshavare");

                    foreach (var (student, guardian) in studentsWithGuardians)
                    {
                        Console.WriteLine($"Elev: {student.FirstName} {student.LastName}");
                        Console.WriteLine($"Klass: {student.Class}");
                        Console.WriteLine($"Vårdnadshavare: {guardian.FirstName} {guardian.LastName}");
                        Console.WriteLine($"Relation: {guardian.Relation}");
                        Console.WriteLine("-----------------------------------------------");
                    }
                    break;

                case "a":
                    return;
                default:
                    Console.WriteLine("Felaktigt val");
                    break;
            }

        }
    }
    
    private static string GetUpdatedValue(string prompt, string currentValue)
    {
        Console.Write($"{prompt} (nuvarande värde: {currentValue}): ");
        string input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }

    private static bool ConfirmAction(string action)
    {
        Console.WriteLine($"\nÄr du säker på att du vill {action}? (ja/nej)");
        return Console.ReadLine().ToLower() == "ja";
    }
    // Metod för header
    private static void PrintHeader(string header)
    {
        Console.WriteLine($"\n{header}");
        Console.WriteLine("===============================================");
    }
}

