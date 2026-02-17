using System;
using System.Data;
using Microsoft.Data.SqlClient;

public class MainClass
{
    public static void UpdateTable(string query)
    {
        string cs = @"Data Source=SSN\SQLEXPRESS;
                      Database=TrainingDB;
                      Integrated Security=True;
                      Encrypt=True;
                      TrustServerCertificate=True;";

        DataSet result = new DataSet();   // Stored in the form of XML

        using var con = new SqlConnection(cs);
        using (var cmd = new SqlCommand(query, con))
        {
            SqlDataAdapter ad = new SqlDataAdapter(cmd);

            // Auto generate Insert/Update/Delete commands
            SqlCommandBuilder builder = new SqlCommandBuilder(ad);

            con.Open();

            ad.Fill(result);

            //DataRow rw = result.Tables[0].NewRow();      // Creating a row with the same structure as of Table[0]
            //rw["FullName"] = "Shyam";        // Filling rows
            //rw["Department"] = "IT";
            //rw["Salary"] = 40000;
            //// Adding rows to the DataSet
            //result.Tables[0].Rows.Add(rw);
            //// Updating the Table[0]
            //ad.Update(result.Tables[0]);
            //Console.WriteLine("Updated");

            // Linq to the DataTable 
            var rows = result.Tables[0].AsEnumerable();

            Console.WriteLine("Printing through Linq");
            var output = rows.Where(r => r.Field<decimal>("Salary") > 10000).Select(r=>r.Field<string>("FullName")).ToList();
            output.ForEach(Console.WriteLine);

            Console.WriteLine("\nGrouped by Department");
            var grouped = rows.GroupBy(r => r.Field<String>("Department")).Select(g => new { Department = g.Key, Count = g.Count(), AvgSalary = g.Average(x => x.Field<decimal>("Salary"))}).OrderByDescending(x => x.AvgSalary).ToList();
            grouped.ForEach(Console.WriteLine);

            Console.WriteLine("\nOrder By Salary");
            var orderBySalary = rows.OrderByDescending(r => r.Field<decimal>("Salary")).ThenBy(r => r.Field<string>("FullName")).Select(r => new { Name = r.Field<string>("FullName"), Salary = r.Field<decimal>("Salary") }).ToList();
            orderBySalary.ForEach(Console.WriteLine);
        }

    }

    public static void LinqOperations(DataTable table)
    {
        var rows = table.AsEnumerable();
        Console.WriteLine("Printing through Linq");
        var output = rows.Where(r => r.Field<decimal>("Salary") > 10000).Select(r => r.Field<string>("FullName")).ToList();
        output.ForEach(Console.WriteLine);
    }

    public static void Main(string[] args)
    {
        UpdateTable("SELECT * FROM Employees");
    }
}
