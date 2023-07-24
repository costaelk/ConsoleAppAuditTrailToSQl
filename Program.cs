using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Reflection;

namespace ConsoleAppAuditTrailToSQl
{

    internal class Program
    {

        public static void Main(string[] args)
        {

            DateTime currentDateTime = DateTime.Now; // Get current date
            string Data = currentDateTime.ToString("yyyyMMdd_HHmmss"); //Convert date to string
            string sourceFilePath = args[0]; //Full path to csv source file
            string destinationFolderPath = args[1]; //Full path to the destination folder
            string DataSource = args[2];// SQL Server instance name
            string UserID = args[3]; // Username Sqlserver
            string Password = args[4]; // Sql Server user password
            string InitialCatalog = args[5]; // Database Name SQL Server
            var filename = destinationFolderPath + "LOGAuditTrail0_" + Data + ".csv"; //Concatenate string by destination
            Directory.CreateDirectory(destinationFolderPath); //Create the destination folder if it doesn't exist
            File.Copy(sourceFilePath, filename, true); //Copy the file from the source folder to the destination folder

            try
            {
                //Count all the lines of the file and remove the last one
                var lines = File.ReadAllLines(filename);
                var count = lines.Length;
                count = count - 1;

                // Database Credentia
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = InitialCatalog;

                //Open the connection and do the Bulk insert
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = $"BULK INSERT dbo.LOGAuditTrail\r\nFROM '{filename}'\r\nWITH (FORMAT = 'CSV',\r\nFIELDTERMINATOR = ';',\r\nLASTROW = {count},\r\nROWTERMINATOR = '\\n',\r\nFIRSTROW = 2\r\n);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                    Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                        Thread.Sleep(5000);
                        //if everything is correct, delete the file
                        File.Delete(filename);
                        
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }            
           
        }
    }
}