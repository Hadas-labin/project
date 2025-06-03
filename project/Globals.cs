
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.IO;


namespace project
{
    public static class Globals
    {
        public static int ColCount;
        public static int RowsCount;
        public static double[,] Xs;
        public static double[] Ys;
        public static double[] theta;
        public const double iceTemperature = -78.5;


    }
}