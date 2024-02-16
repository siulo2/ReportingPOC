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

        static byte[] generate(string path, ReportDataSource[] ds, string format)
        {
            LocalReport lreport = new LocalReport();
            lreport.ReportPath = path;
            foreach (ReportDataSource d in ds)
            {
                lreport.DataSources.Add(d);
            }
            byte[] bytes = lreport.Render(format);
            return bytes;
        }

        static string writefile(byte[] data, string path = null)
        {
            if (path == null)
            {
                path = Path.GetTempFileName();
            }
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Close();
            return path;
        }

        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            ReportDataSource[] datasource = new ReportDataSource[] {
                new ReportDataSource("test_dataset", setupDataTable())
            };

            string report = "C:/Users/siulo2/Documents/GitHub/ReportingPOC/WebApplication2/Report1.rdlc";
            string renderedfile = writefile(generate(report, datasource, "pdf"));
            Console.WriteLine("Report generated to " +  renderedfile);
        }
    }
}
