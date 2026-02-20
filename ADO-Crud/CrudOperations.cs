using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CRUDOps
{
    public class CrudOperations
    {
        #region Read
        // Working
        public static DataSet ReadFromTable(string query)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            DataSet result = new DataSet();
            using var con = new SqlConnection(cs);
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(result);
                con.Close();
            }
            result.WriteXml("Output");
            Console.WriteLine("Query Result :");
            return result;
        }

        #endregion

        #region Insert
        // Working
        public static string CreateInsertQuery(string input)  //"Ajay Finance 50000"
        {
            string[] parts = input.Split(" ");
            return $"Insert into dbo.Employees(FullName, Department, Salary) Values ('{parts[0]}','{parts[1]}',{decimal.Parse(parts[2])})";
        }

        // Working : Not needed, Use ExecuteNonQuery()
        public static void InsertIntoDB(string query)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            using var con = new SqlConnection(cs);
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows == 1 ? "Inserted" : "Not Inserted");
                con.Close();
            }
        }
        #endregion

        #region Delete
        public static string DeleteIdQuery(int id)
        {
            string query = $"Delete From dbo.Employees Where EmployeeId = {id}";
            return query;
        }
        #endregion

        #region Update
        /// <summary>
        /// Method to get the Query to update Salary. Pass the output to ExecuteNonQuery()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        public static string UpdateSalaryQuery(int id, decimal salary)
        {
            string query = $"Update dbo.Employees Set Salary = {salary} Where EmployeeId = {id}";
            return query;
        }
        #endregion

        #region NonQuery Execution Method
        /// <summary>
        /// Common Method to execute dml commands
        /// </summary>
        /// <param name="query"></param>
        public static void ExecuteNonQuery(string query)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            using var con = new SqlConnection(cs);
            using (var cmd = new SqlCommand(query, con))
            {
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected == 1 ? "Query Executed" : "Query Not Executed");
                con.Close();
            }
        }
        #endregion
    }

    public class MainClass
    {
        public static void Main(string[] args)
        {
            //////Insert
            //var insertQuery = CrudOperations.CreateInsertQuery("Keshav Finance 30000");
            //Console.WriteLine(insertQuery);
            //CrudOperations.ExecuteNonQuery(insertQuery);

            //// Read
            //DataSet data = CrudOperations.ReadFromTable("Select * FROM dbo.Employees");
            //Console.WriteLine(data.GetXml());

            ////////Delete
            //var deleteQuery = CrudOperations.DeleteIdQuery(11);
            //CrudOperations.ExecuteNonQuery(deleteQuery);

            //////Update 
            //var updateSalaryQuery = CrudOperations.UpdateSalaryQuery(8, 30000);
            //CrudOperations.ExecuteNonQuery(updateSalaryQuery);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("================MENU==================");
                Console.WriteLine("Press 1 to Add Employee");
                Console.WriteLine("Press 2 to Get All Employee");
                Console.WriteLine("Press 3 to Delete Employee");
                Console.WriteLine("Press 4 to Update Employee Salary");
                Console.WriteLine("Press 5 to exit.");

                Console.WriteLine("Enter your Choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter Department: ");
                        string dep = Console.ReadLine();
                        Console.WriteLine("Enter Salary: ");
                        string salary = Console.ReadLine();
                        string inQuery = $"{name} {dep} {salary}";
                        var insertQuery = CrudOperations.CreateInsertQuery(inQuery);
                        Console.WriteLine(insertQuery);
                        CrudOperations.ExecuteNonQuery(insertQuery);
                        break;
                    case "2":
                        DataSet data = CrudOperations.ReadFromTable("Select * FROM dbo.Employees");
                        Console.WriteLine(data.GetXml());
                        break;
                    case "3":
                        Console.WriteLine("Enter Id to delete: ");
                        int id = int.Parse(Console.ReadLine());
                        var deleteQuery = CrudOperations.DeleteIdQuery(id);
                        CrudOperations.ExecuteNonQuery(deleteQuery);
                        break;
                    case "4":
                        Console.WriteLine("Enter Id: ");
                        int Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Salary: ");
                        decimal Salary = Decimal.Parse(Console.ReadLine());
                        var updateSalaryQuery = CrudOperations.UpdateSalaryQuery(Id,Salary);
                        CrudOperations.ExecuteNonQuery(updateSalaryQuery);
                        break;
                    case "5":
                        flag = false;
                        return;
                }
            }

        }
    }
}
