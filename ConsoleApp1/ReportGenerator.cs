using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.Interfaces;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityU.Common.ReportEngine
{
    public class ReportGenerator
    {
        private string rdlPath;
        private LocalReport localReport;

        public ReportGenerator(string rdlPath)
        {
            this.rdlPath = rdlPath;
            this.localReport = new LocalReport();
            localReport.ReportPath = this.rdlPath;
        }

        public ReportGenerator(string rdlPath, DataTable[] dataSources) : this(rdlPath)
        {
            setDataSources(dataSources);
        }

        public void setDataSources(DataTable[] dataSources)
        {
            this.localReport.DataSources.Clear();
            foreach (DataTable t in dataSources)
            {
                ReportDataSource s = new ReportDataSource(t.TableName, t);
                this.localReport.DataSources.Add(s);
            }
        }

        public byte[] render(string format)
        {
            byte[] bytes = this.localReport.Render(format);
            return bytes;
        }

        /**
         * Render report in a format to a temp file 
         * @return the filename the report is written to.
         */
        public string renderToFile(string format)
        {
            return writefile(render(format));
        }

        private string writefile(byte[] data, string path = null)
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
    }
}
