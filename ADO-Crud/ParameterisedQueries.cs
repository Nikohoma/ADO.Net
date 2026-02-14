using System.Data;
using Microsoft.Data.SqlClient;

class ParameterisedQuery
{
    static void Main(string[] args)
    {
        //GetRecordsByDept("HR");
        ReadQueryOnEmployee(@"Select EmployeeId, FullName, Salary FROM dbo.Employees Where Department = 'HR' ");
    }

    public static void GetRecordsByDept(string dept)
    {
        string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";  // Database if not accepting, use InitialCatalog
        string sql = @"Select EmployeeId, FullName, Salary FROM dbo.Employees Where Department = @dept";

        //Console.Write("Enter Department : "); var dept = Console.ReadLine();

        using var con = new SqlConnection(cs);
        using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@dept", dept);

        con.Open();

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var salary = reader.GetDecimal(2);

                Console.WriteLine($"ID : {id} , Name : {name} , Salary : {salary}");
            }
        }
        con.Close();
    }

    public static void ReadQueryOnEmployee(string query)
    {
        string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";  // Database if not accepting, use InitialCatalog
        using var con = new SqlConnection(cs);
        using var cmd = new SqlCommand(query, con);
        DataSet result = new DataSet();
        con.Open();

        using (var reader = cmd.ExecuteReader())
        {
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);
            adapter1.Fill(result);
        }
        result.GetXml();
        con.Close();


    }

}