using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToCsv
{
    public class ExcelFileHelper
    {
        public static bool SaveAsCsv(string excelFilePath, string destinationCsvFilePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IExcelDataReader reader = null;
                if (excelFilePath.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (excelFilePath.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                if (reader == null)
                    return false;

                var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var csvContent = string.Empty;
                int row_no = 0;
                while (row_no < ds.Tables[0].Rows.Count)
                {
                    var arr = new List<string>();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        arr.Add(ds.Tables[0].Rows[row_no][i].ToString());
                    }
                    row_no++;
                    csvContent += string.Join(",", arr) + "\n";
                }
                StreamWriter csv = new StreamWriter(destinationCsvFilePath, false);
                csv.Write(csvContent);
                csv.Close();
                return true;
            }
        }

        public static bool SaveAsCsv(Stream fileStream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var unicodeEncoding = Encoding.GetEncoding("utf-8");

            //using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IExcelDataReader reader = null;
                //if (excelFilePath.EndsWith(".xls"))
                //{
                //    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                //}
                //else if (excelFilePath.EndsWith(".xlsx"))
                //{
                //    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //}
                reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);

                if (reader == null)
                    return false;


                var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = false
                    }
                });

                var csvContent = string.Empty;
                int row_no = 0;
                while (row_no < ds.Tables[0].Rows.Count)
                {
                    var arr = new List<string>();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        var originalData = ds.Tables[0].Rows[row_no][i].ToString();
                        var data = originalData?.Replace("\"", "\"\"").Trim();
                        arr.Add(String.Format("\"{0}\"", data??string.Empty));
                    }
                    row_no++;
                    csvContent += string.Join(",", arr) + "\n";
                }


                var destinationCsvFilePath = Path.Combine(Environment.CurrentDirectory, "Customers.csv");
                StreamWriter csv = new StreamWriter(destinationCsvFilePath, false, unicodeEncoding);
                csv.Write(csvContent);
                csv.Close();
                return true;
            }
        }
    }
}
