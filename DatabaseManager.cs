using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SchoolPlatform;

internal class DatabaseManager
{
    private IDbConnection Connect()
    {
        string connectionstring = File.ReadAllText("connectionstring.txt");
        IDbConnection connection = new SqlConnection(connectionstring);
        return connection;
    }

    public IEnumerable<Student> GetAllStudents()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Student> students = connection.Query<Student>("SELECT * FROM Student");
        return students;
    }

    public IEnumerable<Teacher> GetAllTeachers()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Teacher> teachers = connection.Query<Teacher>("SELECT * FROM Teacher");
        return teachers;
    }

    public IEnumerable<Guardian> GetAllGuardians()
    {
        using IDbConnection connection = Connect();
        IEnumerable<Guardian> teachers = connection.Query<Guardian>("SELECT * FROM Guardian");
        return teachers;
    }
}

