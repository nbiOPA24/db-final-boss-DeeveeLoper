using System;

namespace SchoolPlatform;

class Program 
{
    static void Main() 
    {
        DatabaseManager db = new DatabaseManager();

        while (true)
        {
            Console.WriteLine("=== SKOLPLATTFORMEN ===");
            Console.WriteLine("Välj ett alternativ genom att ange motsvarande siffra (1-7)");
            Console.WriteLine("===============================================");
            Console.WriteLine("1. Lista alla elever och deras information");
            Console.WriteLine("2. Lista alla lärare och deras information");
            Console.WriteLine("3. Lista alla vårdnadshavare och deras information");
            Console.WriteLine("4. Lista alla skolämnen");
            Console.WriteLine("5. Lista alla studiesalar");
            Console.WriteLine("6. Lista alla lektioner");
            Console.WriteLine("7. Visa närvarorapport för alla elever i åk 8A, 8B, 9A och 9B (2025-01-07)");
            Console.WriteLine("===============================================");

            Console.WriteLine("a. Avsluta");

            string choice = Console.ReadLine();

            switch (choice) 
            {
                case "1":
                    IEnumerable<Student> students = db.GetAllStudents();
                    foreach (Student s in students)
                    {
                        Console.WriteLine($"Id: {s.StudentId}, Namn: {s.FirstName} {s.LastName}, Personnummer: {s.PersonalNumber}, Epost: {s.Email}, Telefon: {s.PhoneNumber}, Årskurs: {s.GradeYear}, Klass: {s.Class}, Särskilt stöd: {s.SpecialNeed}, Specialkost: {s.SpecialDiet}, Avslutat: {(s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}");
                    }
                    break;
                case "2":
                    IEnumerable<Teacher> teachers = db.GetAllTeachers();
                    foreach (Teacher t in teachers)
                    {
                        Console.WriteLine($"Id: {t.TeacherId}, Namn: {t.FirstName} {t.LastName}, Personnummer: {t.PersonalNumber}, Epost: {t.Email}, Telefon: {t.PhoneNumber}, Addres: {t.StreetAddress}, Postnummer: {t.PostalCode}, Stad: {t.City}, Anställningsdatum: {t.HireDate:yyyy-MM-dd}, Kvalificerad inom ämnen: {t.SubjectQualified}, Anställningsform: {t.EmploymentType}, Annan skola: {t.OtherSchool}, Aktiv: {t.IsActive}");
                    }
                    break;
                case "3":
                    IEnumerable<Guardian> guardians = db.GetAllGuardians();
                    foreach (Guardian g in guardians)
                    {
                        Console.WriteLine($"Id: {g.GuardianId}, Namn: {g.FirstName} {g.LastName}, Personnummer: {g.PersonalNumber}, Relation: {g.Relation}, Epost: {g.Email}, Telefon: {g.PhoneNumber}, Addres: {g.StreetAddress}, Postnummer: {g.PostalCode}, Stad: {g.City}, Huvudkontakt: {g.IsPrimaryContact}");
                    }
                    break;
                case "4":
                    IEnumerable<Subject> subjects = db.GetAllSubjects();
                    foreach (Subject sub in subjects)
                    {
                        Console.WriteLine($"Id: {sub.SubjectId}, Namn: {sub.SubjectName}, Ämneskod: {sub.SubjectCode}, Beskrivning: {sub.Description}");
                    }
                    break;
                case "5":
                    IEnumerable<Room> rooms = db.GetAllRooms();
                    foreach (Room r in rooms)
                    {
                        Console.WriteLine($"Id: {r.RoomId}, Salsnummer: {r.RoomNumber}, Kapacitet: {r.Capacity}, Utrustning: {(string.IsNullOrEmpty(r.Equipment) ? "Ingen utrustning listad" : r.Equipment)}, Aktiv: {r.IsActive}");
                    }
                    break;
                case "6":
                    IEnumerable<Lesson> lessons = db.GetAllLessons();
                    foreach (Lesson l in lessons)
                    {
                        Console.WriteLine($"Id: {l.LessonId}, Namn: {l.LessonName}, Lektionskod: {l.LessonCode}, Rum-ID: {l.RoomId}, Lärar-ID: {l.TeacherId}, Ämnes-ID: {l.SubjectId}, Startdatum: {l.StartDate:yyyy-MM-dd HH:mm}, Slutdatum: {l.EndDate:yyyy-MM-dd HH:mm}, Maximalt antal elever: {l.MaxStudents}, Aktiv: {l.IsActive}");
                    }
                    break;
                case "7":
                    IEnumerable<Attendance> attendance = db.GetAllAttendance();
                    foreach (Attendance a in attendance)
                    {
                        Console.WriteLine($"Id: {a.AttendanceId}, Elev-ID: {a.StudentId}, Lektion-ID: {a.LessonId}, Startdatum: {a.DateTime:yyyy-MM-dd HH:mm}, Närvarande: {a.IsPresent}, Kommentar: {(string.IsNullOrEmpty(a.Comment) ? "Ingen kommentar" : a.Comment)}");
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
}

