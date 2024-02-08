using Microsoft.Reporting.WinForms;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.IO;


namespace ConsoleApp1
{
    internal class Program
    {
        static DataTable setupDataTable()
        {
            OracleConnection connection = new OracleConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["admsystem"].ConnectionString;
            connection.Open();

            System.Data.DataSet ds = new System.Data.DataSet("test_dataset");
            string sql = "SELECT * FROM test_data";
            OracleDataAdapter da = new OracleDataAdapter(sql, connection);
            da.Fill(ds);
            connection.Close();
            DataTable dt = ds.Tables[0];
            return dt;
        }

        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Console.WriteLine("Fetching data");
            DataTable dataTable = setupDataTable();

            Console.WriteLine("Preparing templates");
            ReportDataSource ds = new ReportDataSource("test_dataset", dataTable);
            LocalReport lreport = new LocalReport();
            lreport.ReportPath = "C:/Users/siulo2/Documents/GitHub/ReportingPOC/WebApplication2/Report1.rdlc";
            lreport.DataSources.Add(ds);

            Console.WriteLine("Rendering");
            byte[] bytes = lreport.Render("PDF");

            Console.WriteLine("Wrting file");
            DateTime d = DateTime.Now;            
            string filename = "c:/temp/RDL-" + d.ToString("yyyyMMdd-HHmmss") + ".pdf";
            FileStream fs = new FileStream(filename, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            Console.WriteLine("Report generated to " + filename);
        }
    }
}
