// See https://aka.ms/new-console-template for more information

using ExcelToCsv;

string dir = Environment.CurrentDirectory + "\\";
//var workbook = new Aspose.Cells.Workbook(dir + "sample.xlsx");

//workbook.Save(dir + "customers.csv", Aspose.Cells.SaveFormat.Csv);

//workbook.Save(dir + "contacts.csv", Aspose.Cells.SaveFormat.Csv);

var excelFilePath = dir + "sample.xlsx";
string output = Path.ChangeExtension(excelFilePath, ".csv");
ExcelFileHelper.SaveAsCsv(excelFilePath, output);

Console.ReadLine();