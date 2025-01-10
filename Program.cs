using System;

namespace SchoolPlatform;

class Program 
{
    static void Main() 
    {
        DatabaseManager db = new DatabaseManager();

        while (true)
        {
            Console.WriteLine("1. Visa all information elever");
            Console.WriteLine("2. Visa all information lärare");
            Console.WriteLine("3. Visa all information Vårdnadshavare");

            Console.WriteLine("a. Avsluta");

            string choice = Console.ReadLine();

            switch (choice) 
            {
                case "1":
                    IEnumerable<Student> students = db.GetAllStudents();
                    foreach (Student s in students)
                    {
                        Console.WriteLine($"Id: {s.StudentId}, Name: {s.FirstName} {s.LastName}, PersonalNumber: {s.PersonalNumber}, Email: {s.Email}, PhoneNumber: {s.PhoneNumber}, GradeYear: {s.GradeYear}, Class: {s.Class}, SpecialNeed: {s.SpecialNeed}, SpecialDiet: {s.SpecialDiet}, EndDate: {(s.EndDate.HasValue ? s.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}");
                    }
                    break;

                case "2":
                    IEnumerable<Teacher> teachers = db.GetAllTeachers();
                    foreach (Teacher t in teachers)
                    {
                        Console.WriteLine($"Id: {t.TeacherId}, Name: {t.FirstName} {t.LastName}, PersonalNumber: {t.PersonalNumber}, Email: {t.Email}, PhoneNumber: {t.PhoneNumber}, Address: {t.StreetAddress}, PostalCode: {t.PostalCode}, City: {t.City}, HireDate: {t.HireDate:yyyy-MM-dd}, SubjectQualified: {t.SubjectQualified}, EmploymentType: {t.EmploymentType}, OtherSchool: {t.OtherSchool}, IsActive: {t.IsActive}");

                    }
                    break;

                case "3":
                    IEnumerable<Guardian> guardians = db.GetAllGuardians();
                    foreach (Guardian g in guardians)
                    {
                        Console.WriteLine($"Id: {g.GuardianId}, Name: {g.FirstName} {g.LastName}, PersonalNumber: {g.PersonalNumber}, Relation: {g.Relation}, Email: {g.Email}, PhoneNumber: {g.PhoneNumber}, Address: {g.StreetAddress}, PostalCode: {g.PostalCode}, City: {g.City}, IsPrimaryContact: {g.IsPrimaryContact}");
                    }
                    break;

                case "a":
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
}

