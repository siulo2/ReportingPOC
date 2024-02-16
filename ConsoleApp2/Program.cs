using CityU.Common.ReportEngine;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace ConsoleApp2
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
            DataTable dt = setupDataTable();
            dt.TableName = "test_dataset";
            DataTable[] datasource = new DataTable[] { dt };

            string rdl = "C:/Users/siulo2/Documents/GitHub/ReportingPOC/WebApplication2/Report1.rdlc";

            ReportGenerator g = new ReportGenerator(rdl, datasource);
            string renderedfile = g.renderToFile("pdf");

            Console.WriteLine("Report generated to " + renderedfile);
        }
    }
}
