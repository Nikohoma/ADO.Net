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
        public static void ReadFromTable(string query)
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

            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Department",-15} {"Salary",10}");  // -5 : left align 5 characters, 20: right align 20 whitespaces after "Name"
            Console.WriteLine(new string('-', 55));
            foreach (DataRow row in result.Tables[0].Rows)
            {
                Console.WriteLine(
                    $"{row["EmployeeId"],-5} " +
                    $"{row["FullName"],-20} " +
                    $"{row["Department"],-15} " +
                    $"{row["Salary"],10}"
                );
            }

        }

        #endregion

        #region Insert
        // Injection Proof
        public static void InsertEmployee(string name, string department, decimal salary)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            using var con = new SqlConnection(cs);
            using var cmd = new SqlCommand("Insert into dbo.Employees (FullName, Department, Salary) Values (@name, @department, @salary)", con);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@department", department);
            cmd.Parameters.AddWithValue("@salary", salary);

            con.Open();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Employee Inserted.");
        }

        #endregion

        #region Delete
        
        public static void DeleteEmployee(int id)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            using var con = new SqlConnection(cs);
            using var cmd = new SqlCommand("Delete from dbo.Employees Where EmployeeId = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Employee Deleted.");
        }

        #endregion

        #region Update
        /// <summary>
        /// Method to get the Query to update Salary. Pass the output to ExecuteNonQuery()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        public static void UpdateSalary(int id, decimal newSalary)
        {
            string cs = @"Data Source=SSN\SQLEXPRESS;Database=TrainingDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Command Timeout=30";
            using var con = new SqlConnection(cs);
            using var cmd = new SqlCommand("Update dbo.Employees Set Salary = @salary Where EmployeeId = @id", con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@salary", newSalary);
            con.Open();
            cmd.ExecuteNonQuery();
            Console.WriteLine("Salary Updated.");
        }

        #endregion

  
    }

    public class MainClass
    {
        public static void Main(string[] args)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("\n================Employee Repository==================\n");
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
                        decimal salary = Decimal.Parse(Console.ReadLine());
                        CrudOperations.InsertEmployee(name, dep, salary);
                        break;
                    case "2":
                        CrudOperations.ReadFromTable("Select * FROM dbo.Employees");
                        break;
                    case "3":
                        Console.WriteLine("Enter Id to delete: ");
                        int id = int.Parse(Console.ReadLine());
                        CrudOperations.DeleteEmployee(id);
                        break;
                    case "4":
                        Console.WriteLine("Enter Id: ");
                        int Id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Salary: ");
                        decimal Salary = Decimal.Parse(Console.ReadLine());
                        CrudOperations.UpdateSalary(Id, Salary);
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        flag = false;
                        return;
                    default:
                        Console.WriteLine("Look at the Menu and enter a valid option.");
                        break;
                }
            }

        }
    }
}
