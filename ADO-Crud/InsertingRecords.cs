using Microsoft.Data.SqlClient;

class InsertingRecords
{
    static void Main(string[] args)
    {
        string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";  // Database if not accepting, use InitialCatalog
        string sql = "Insert into dbo.Employees(FullName,Department,Salary) Values (@name,@dept,@salary)";

        Console.Write("Enter Name : ");  var name = Console.ReadLine();
        Console.Write("Enter Department : ");  var dept= Console.ReadLine();
        Console.Write("Enter Salary: ");  decimal salary = decimal.Parse(Console.ReadLine()??"0");

        using var con = new SqlConnection(cs);
        using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@dept", dept);
        cmd.Parameters.AddWithValue("@salary", salary);

        con.Open();
        int rows = cmd.ExecuteNonQuery();    // Non Query : Queries that dont return values like Insert/Update/Delete 

        Console.WriteLine(rows == 1 ? "Inserted" : "Not inserted");

        con.Close();

    }
}