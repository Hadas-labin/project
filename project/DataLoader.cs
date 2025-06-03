using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class DataLoader
    {
        private static string filePath = "C:\\Users\\h7660\\Desktop\\LinearRegression\\data\\data.xlsx";

        public static string getFilePath()
        {
            return filePath;
        }
        public static void ReadExcelData()
        {
            XLWorkbook workbook = null;
            Console.WriteLine("Opening file from: " + filePath);
            const int startRow = 2;
            const int columnB = 2;
            const int columnC = 3;
            const int columnD = 4;
            const int columnE = 5;
            const int columnF = 6;
            const int columnY = 7;
            const int expectedColumns = 4;
            try
            {
                using (workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet(1);
                    if (worksheet != null)
                    {
                        var range = worksheet.RangeUsed();
                        int rowsCount = range.RowCount();
                        int colsCount = range.ColumnCount();
                        Globals.RowsCount = rowsCount - (startRow - 1);
                        Globals.ColCount = expectedColumns;

                        if (rowsCount < startRow)
                        {
                            Console.WriteLine($"אזהרה: מספר השורות בקובץ קטן משורת ההתחלה ({startRow}). לא ייקראו נתונים.");
                            throw new Exception($"אזהרה: מספר השורות בקובץ קטן משורת ההתחלה ({startRow}). לא ייקראו נתונים.");
                        }

                        if (colsCount < columnF) // בדיקה שאכן יש לפחות 6 עמודות
                        {
                            Console.WriteLine($"אזהרה: מספר העמודות בקובץ קטן מהצפוי ({columnF}). ייתכן שחלק מהנתונים לא ייקראו.");
                        }

                        Globals.Xs = new double[Globals.RowsCount, Globals.ColCount];
                        Globals.Ys = new double[Globals.RowsCount];
                        double T_liquids = 0;
                        for (int i = 2; i <= rowsCount; i++)
                        {
                            int rowIndex = i - startRow;
                            // ניסיון לקרוא ולהמיר ל-double עם בדיקה
                            if (TryGetDouble(worksheet, i, columnB, out double bValue)) Globals.Xs[rowIndex, 0] = bValue;
                            if (TryGetDouble(worksheet, i, columnC, out double cValue)) Globals.Xs[rowIndex, 1] = cValue;
                            if (TryGetDouble(worksheet, i, columnD, out double dValue) &&
                                TryGetDouble(worksheet, i, columnE, out double eValue))
                            {
                                eValue = eValue / 100.0;
                                T_liquids = (1 - eValue) * dValue + eValue * Globals.iceTemperature;
                                Globals.Xs[rowIndex, 2] = dValue;
                            }
                            if (TryGetDouble(worksheet, i, columnF, out double fValue)) Globals.Xs[rowIndex, 3] = fValue;
                            if (TryGetDouble(worksheet, i, columnY, out double yValue)) Globals.Ys[rowIndex] = yValue;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"שגיאה בעת קריאת קובץ האקסל : {ex.Message}");
                Console.WriteLine($"🔍 מקור השגיאה:\n{ex.StackTrace}");
                throw ex;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
            }
        }

        private static bool TryGetDouble(IXLWorksheet worksheet, int row, int column, out double value)
        {
            var cellValue = worksheet.Cell(row, column).Value;
            if (!cellValue.IsBlank && double.TryParse(cellValue.ToString(), out value))
            {
                return true;
            }
            else
            {
                Console.WriteLine($"אזהרה: לא ניתן להמיר ערך בשורה {row}, עמודה {column} ל-double. הערך יאופס ל-0.");
                value = 0;
                return false;
            }
        }
    }
}

