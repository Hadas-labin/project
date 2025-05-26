using OfficeOpenXml;
using System;
using System.IO;

namespace project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const double iceTemperature = -78.5;
            Console.WriteLine("קרח");
            double flour = double.Parse(Console.ReadLine());
            Console.WriteLine("מים");
            double water = double.Parse(Console.ReadLine());
            while (flour != 0 && water !=0)
            {
                double e = flour / 100.0;
                double T_liquids = (1 - e) * water + e * iceTemperature;
                //Console.WriteLine("T_liquids = " + T_liquids);
                Console.WriteLine($"T_liquids = {T_liquids:F2}°C");
                Console.WriteLine("קרח");
                flour = double.Parse(Console.ReadLine());
                Console.WriteLine("מים");
                water = double.Parse(Console.ReadLine());
            }

            //try {   
            //    Globals.ReadExcelData();
            //    LinearRegression.Fit();
            //    LinearRegression.performGradientDescent();
            //    // ✅ 1️⃣ Getting user input
            //    Console.WriteLine("Enter the amount of flour:");
            //    double flour = double.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter the amount of oil:");
            //    double oil = double.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter the temperature of the water:");
            //    double waterTemp = double.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter the amount of ice (in grams):");
            //    double iceAmount = double.Parse(Console.ReadLine());
            //    iceAmount = iceAmount / 100.0;

            //    Console.WriteLine("Enter the final expected temperature of the dough:");
            //    double finalValue = double.Parse(Console.ReadLine());

            //    // ✅ חישוב טמפרטורת נוזלים משוקללת לפי כמויות
            //    double T_liquids = (1 - iceAmount) * waterTemp + iceAmount * Globals.iceTemperature;

            //    // ✅ יצירת וקטור קלט עבור פונקציית הניבוי
            //    double[] XTest = { flour, oil, T_liquids };

            //    // ✅ חיזוי ובדיקת שגיאה
            //    (double YPred, double ErrorPercentage) = LinearRegression.Predict(XTest, finalValue);

            //    // ✅ 4️⃣ Displaying the results
            //    Console.WriteLine($"\n🔍 Predicted value: {YPred:F6}");
            //    Console.WriteLine($"📌 Error percentage: {ErrorPercentage:F2}%");

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"🔍 מקור השגיאה:\n{ex.StackTrace}:");
            //    Console.WriteLine($"⚠ שגיאה קריטית בתוכנית: {ex.Message}");


            //}
        }
        public static double Predict(double T_liquids, double[] theta)
        {
            // חישוב התוצאה על פי וקטור זטה
            double predictedValue = 0;
            for (int i = 0; i < theta.Length; i++)
            {
                predictedValue += theta[i] * T_liquids;
            }

            return predictedValue;
        }
    }
}
