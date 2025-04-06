using OfficeOpenXml;
using System;
using System.IO;

namespace project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filePath = "C:\\Users\\h7660\\Desktop\\פרויקט גמר\\data\\data.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;// הגדרת רישוי
            ReadExcelData(filePath);
        }

        static void ReadExcelData(string filePath)
        {
            if (File.Exists(filePath))
            {
                // טוענים את הקובץ
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    // בוחרים בגיליון הראשון
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    // עוברים על כל התאים ומדפיסים את התוכן
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Text;
                            Console.Write(cellValue + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("הקובץ לא נמצא בנתיב שניתן.");
            }

        }
    }
}
