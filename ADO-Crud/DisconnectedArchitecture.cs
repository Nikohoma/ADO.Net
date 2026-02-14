using Microsoft.Data.SqlClient;
using System.Data;

class DisconnectedArchitecture
{
    static void Main()
    {
        string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
        string sql = "SELECT EmployeeId, FullName, Department, Salary FROM dbo.Employees ORDER BY EmployeeId ; Select Top 1 * FROM dbo.Employees";
        DataSet ds = new DataSet();  // Can have multiple tables. so we can pass multiple queries of multiple tables.

        using (var con = new SqlConnection(cs))
        using (var cmd = new SqlCommand(sql, con))
        {
            con.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            con.Close();
        }
        ds.WriteXml("TestData"); // In bin folder
        Console.WriteLine(ds.GetXml());  // Read XML
        Console.Write("\nTable 0 : "); Console.WriteLine(ds.Tables[0]);
        Console.WriteLine(ds.Tables[1]);


    }

    private static SqlCommand GetInsertCommand(SqlConnection con)
    {
        SqlCommand sqlCommand = new SqlCommand("INSERT INTO DEPT (ID, DEPTNAME) VALUES (@ID, @DEPTNAME)", con);
        sqlCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID");
        sqlCommand.Parameters.Add("@DEPTNAME", SqlDbType.VarChar, 50, "DEPTNAME");
        return sqlCommand;

    }

}