using System.Data;
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
}


