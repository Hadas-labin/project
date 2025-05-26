using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace project
{
    internal class LinearRegression
    {
        public double[] PredictMany(double[,] inputs)
        {
            int rows = inputs.GetLength(0);
            int cols = inputs.GetLength(1);
            if (Globals.theta.Length != cols)
                throw new ArgumentException("Dimensions of inputs and theta do not match.");

            double[] predictions = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                double prediction = 0;
                for (int j = 0; j < cols; j++)
                {
                    prediction += inputs[i, j] * Globals.theta[j];
                }
                predictions[i] = prediction;
            }
            return predictions;
        }
                 
        public static void Fit()
        {
            int i = 0 , j=0;
            double[,] Xs = Globals.Xs; 
            double[] Ys = Globals.Ys;
            double[,] Xt = TransposeMatrix(Xs); // X^T
            Console.WriteLine("Xt:");
            for (i = 0; i < Xt.GetLength(0); i++)
            {
                for ( j = 0; j < Xt.GetLength(1); j++)
                {
                    Console.Write(Xt[i, j].ToString("F4") + "\t");
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            double[,] XtX = MultiplyMatrices(Xt, Xs);             // X^T * X
            for( i = 0; i < XtX.GetLength(0); i++)
            {
                for ( j = 0; j < XtX.GetLength(1); j++)
                {
                    Console.Write(XtX[i, j].ToString("F4") + "\t");
                }
                Console.WriteLine();
            }

            double[,] XtXInv = InverseMatrix(XtX);                // (X^T * X)^-1
            for (i = 0; i < XtXInv.GetLength(0); i++)
            {
                for ( j = 0; j < XtXInv.GetLength(1); j++)
                {
                    Console.Write(XtXInv[i, j].ToString("F4") + "\t");
                }
                Console.WriteLine();
            }

            double[] XtY = MultiplyMatrixVector(Xt, Ys);         // X^T * y
            Globals.theta = MultiplyMatrixVector(XtXInv, XtY);  // (X^T * X)^-1 * (X^T * y)
        }

        public static (double, double) Predict(double[] XTest, double YTest)
        {
            // ✅ 1️⃣ הוספת הטיה (Bias) על ידי הוספת ערך 1 למערך הנתונים
            double[] XTestModified = new double[XTest.Length + 1];
            XTestModified[0] = 1; // ההטיה
            for (int i = 0; i < XTest.Length; i++)
            {
                XTestModified[i + 1] = XTest[i]; // שמירת הנתונים הקיימים
            }

            // ✅ 2️⃣ חישוב תחזית (`Y_pred`) באמצעות מכפלה וקטורית עם `theta` השמור
            double YPred = 0;
            for (int i = 0; i < XTestModified.Length; i++)
            {
                YPred += XTestModified[i] * Globals.theta[i]; // שימוש ב-`theta` השמור
            }

            // ✅ 3️⃣ חישוב אחוז השגיאה
            double ErrorPercentage = Math.Abs(YPred - YTest) / YTest * 100;

            return (YPred, ErrorPercentage);
        }

        //public static void performGradientDescent()
        //{
        //    double[,] X = new double[Globals.RowsCount - 1, Globals.ColCount];
        //    X = Globals.Xs;
        //    double[] y = Globals.Ys;
        //    int m = y.Length;
        //    int n = X.GetLength(1);
        //    double[] theta = Globals.theta;
        //    List<double> costHistory = new List<double>();
        //    Console.WriteLine("ערכים התחלתיים של θ:^^^^^^^^^^^^^^^^^^^^^^");
        //    for (int i = 0; i < theta.Length; i++)
        //    {
        //        Console.WriteLine($"θ[{i}] = {theta[i]:F6}");
        //    }

        //    for (int j = 0; j < n; j++)
        //    {
        //        double mean = 0;
        //        double std = 0;
        //        for (int i = 0; i < m; i++)
        //        {
        //            mean += X[i, j];
        //        }
        //        mean /= m;

        //        for (int i = 0; i < m; i++)
        //        {
        //            std += Math.Pow(X[i, j] - mean, 2);
        //        }
        //        std = Math.Sqrt(std / m);

        //        for (int i = 0; i < m; i++)
        //        {
        //            X[i, j] = (X[i, j] - mean) / std;
        //        }
        //    }

        //    int maxIterations = 1000;
        //    double learningRate = 0.01;
        //    double tolerance = 1e-6;
        //    double previousCost = double.MaxValue;
        //    int iter = 0;

        //    while (iter < maxIterations)
        //    {
        //        // 1. חישוב תחזיות
        //        double[] predictions = MultiplyMatrixVector(X, theta);

        //        // 2. חישוב שגיאות
        //        double[] errors = new double[m];
        //        for (int i = 0; i < m; i++)
        //        {
        //            errors[i] = predictions[i] - y[i];
        //        }

        //        // 3. עדכון פרמטרים
        //        for (int j = 0; j < n; j++)
        //        {
        //            double gradient = 0;
        //            for (int i = 0; i < m; i++)
        //            {
        //                gradient += errors[i] * X[i, j];
        //            }
        //            gradient /= m;
        //            theta[j] -= learningRate * gradient;
        //        }

        //        // 4. חישוב עלות חדשה והשוואה
        //        double[] updatedPredictions = MultiplyMatrixVector(X, theta);
        //        double cost = computeCostFunction(updatedPredictions, y);
        //        costHistory.Add(cost);

        //        if (iter == 0 || iter % 100 == 0 || iter == maxIterations - 1)
        //        {
        //            Console.WriteLine($"\nאיטרציה {iter}");
        //            Console.WriteLine($"עלות: {cost:F6}");
        //            Console.WriteLine("וקטור θ:");
        //            for (int i = 0; i < theta.Length; i++)
        //            {
        //                Console.WriteLine($"θ[{i}] = {theta[i]:F6}");
        //            }
        //        }

        //        if (Math.Abs(previousCost - cost) < tolerance)
        //        {
        //            Console.WriteLine($"עצירה באיטרציה {iter} כי השינוי קטן מאוד.");
        //            break; 
        //        }

        //        previousCost = cost;
        //        iter++;
        //    }

        //    SaveThetaToFile(theta);
        //    Globals.theta = theta;
        //    Console.WriteLine("Final cost: " + costHistory.Last());
        //}

        public static void performGradientDescent()
        {
            double[,] X = Globals.Xs;
            double[] y = Globals.Ys;
            int m = y.Length;
            int n = X.GetLength(1);
            double[] theta = Globals.theta;
            List<double> costHistory = new List<double>();

            Console.WriteLine("ערכים התחלתיים של θ:");
            for (int i = 0; i < theta.Length; i++)
            {
                Console.WriteLine($"θ[{i}] = {theta[i]:F6}");
            }

            // 🔹 נרמול הנתונים כדי למנוע גדלים לא תקינים
            //for (int j = 0; j < n; j++)
            //{
            //    double mean = 0;
            //    double std = 0;

            //    for (int i = 0; i < m; i++)
            //    {
            //        mean += X[i, j];
            //    }
            //    mean /= m;

            //    for (int i = 0; i < m; i++)
            //    {
            //        std += Math.Pow(X[i, j] - mean, 2);
            //    }
            //    std = Math.Sqrt(std / m);

            //    // ✅ תיקון: מניעת חילוק ב-0
            //    if (std == 0)
            //    {
            //        Console.WriteLine($"⚠ אזהרה: std[{j}] = 0, שינוי ל-1 כדי למנוע חילוק ב-0");
            //        std = 1;
            //    }

            //    for (int i = 0; i < m; i++)
            //    {
            //        X[i, j] = (X[i, j] - mean) / std;
            //    }
            //}

            int maxIterations = 1000;
            double learningRate = 0.1;
            double tolerance = 1e-6;
            double previousCost = double.MaxValue;
            int iter = 0;

            while (iter < maxIterations)
            {
                // 🔹 חישוב תחזיות
                double[] predictions = MultiplyMatrixVector(X, theta);

                // 🔹 חישוב שגיאות
                double[] errors = new double[m];
                for (int i = 0; i < m; i++)
                {
                    errors[i] = predictions[i] - y[i];
                }

                // 🔹 עדכון פרמטרים
                for (int j = 0; j < n; j++)
                {
                    double gradient = 0;
                    for (int i = 0; i < m; i++)
                    {
                        gradient += errors[i] * X[i, j];
                    }
                    gradient /= m;

                    // ✅ תיקון: מניעת `NaN` ב-gradient
                    if (double.IsNaN(gradient))
                    {
                        Console.WriteLine($"⚠ שגיאה: gradient[{j}] הפך ל-NaN! שינוי ל-0 כדי למנוע השפעה שגויה.");
                        gradient = 0;
                    }

                    theta[j] -= learningRate * gradient;
                }

                // 🔹 חישוב עלות חדשה והשוואה
                double[] updatedPredictions = MultiplyMatrixVector(X, theta);
                double cost = computeCostFunction(updatedPredictions, y);

                // ✅ תיקון: מניעת `NaN` בעלות
                if (double.IsNaN(cost))
                {
                    Console.WriteLine("⚠ שגיאה: עלות הפכה ל-NaN! עצירת Gradient Descent.");
                    break; // יציאה מהלולאה כדי למנוע חישובים שגויים
                }

                costHistory.Add(cost);

                if (iter == 0 || iter % 100 == 0 || iter == maxIterations - 1)
                {
                    Console.WriteLine($"\nאיטרציה {iter}");
                    Console.WriteLine($"עלות: {cost:F6}");
                    Console.WriteLine("וקטור θ:");
                    for (int i = 0; i < theta.Length; i++)
                    {
                        Console.WriteLine($"θ[{i}] = {theta[i]:F6}");
                    }
                }

                if (Math.Abs(previousCost - cost) < tolerance)
                {
                    Console.WriteLine($"עצירה באיטרציה {iter} כי השינוי קטן מאוד.");
                    break;
                }

                previousCost = cost;
                iter++;
            }

            SaveThetaToFile(theta);
            Globals.theta = theta;
            Console.WriteLine("Final cost: " + costHistory.Last());
        }

        public static void SaveThetaToFile(double[] theta)
        {
            using (StreamWriter writer = new StreamWriter(Globals.filePath, false)) // false => דריסה ולא הוספה
            {
                foreach (double value in theta)
                {
                    writer.WriteLine(value); // כל ערך בשורה משלו = עמודה
                }
            }
        }


        public static double computeCostFunction (double[] Y_pred, double[] Y_text)
        {
            double totalError = 0;
            int count = Y_pred.Length;

            for (int i = 0; i < count; i++)
            {
                double error = Math.Abs(Y_pred[i] - Y_text[i]); 
                totalError += error;  
            }

            double averageError = totalError / count;  
            double errorPercentage = (averageError / Y_text.Average()) * 100;  
            return errorPercentage;
        }


        private static double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            if (vector.Length != cols)
                throw new ArgumentException("Vector size must match number of matrix columns.");
            double[] result = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                double sum = 0;
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }
            return result;
        }
       
        private static double[,] MultiplyMatrices(double[,] A, double[,] B)
        {
            int rowsA = A.GetLength(0);
            int colsA = A.GetLength(1);
            int rowsB = B.GetLength(0);
            int colsB = B.GetLength(1);

            if (colsA != rowsB)
                throw new ArgumentException("Number of columns in A must match number of rows in B.");

            double[,] result = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += A[i, k] * B[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        private static double[,] InverseMatrix(double[,] Xs)
        {
            int rows = Xs.GetLength(0);
            int cols = Xs.GetLength(1);
            double[,] inverse = new double[rows, cols];
            double temp;
            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows; j++)
                    inverse[i, j] = (i == j) ? 1 : 0;
            for (int i = 0; i < cols; i++)
            {
                temp = Xs[i, i];
                if (temp == 0) return null;
                for (int j = 0; j < rows; j++)
                {
                    Xs[i, j] /= temp;
                    inverse[i, j] /= temp;
                }
                for (int k = 0; k < cols; k++)
                {
                    if (k != i)
                    {
                        temp = Xs[k, i];
                        for (int j = 0; j < rows; j++)
                        {
                            Xs[k, j] -= Xs[i, j] * temp;
                            inverse[k, j] -= inverse[i, j] * temp;
                        }
                    }
                }
            }
            return inverse;
        }

        private static double[,] TransposeMatrix(double[,] matrix)
        {
            try
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);
                double[,] transposed = new double[cols, rows];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        transposed[j, i] = matrix[i, j];
                    }
                }
                return transposed;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ TransposeMatrix: " + ex.Message);
                throw;
            }
        }

        public static void AddInterceptColumn()
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(Globals.filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    bool hasBiasColumn = false;
                    int colCount = worksheet.Dimension.End.Column;
                    int rows = worksheet.Dimension.End.Row;
                   
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[1, col].Text.Trim().Equals("Bias", StringComparison.OrdinalIgnoreCase))
                        {
                            hasBiasColumn = true;
                            break;
                        }
                    }

                    if (!hasBiasColumn)
                    {

                        int newCol = colCount + 1;
                        worksheet.Cells[1, newCol].Value = "Bias";

                        for (int i = 2; i <= rows; i++)
                        {
                            worksheet.Cells[i, newCol].Value = 1;
                        }

                        package.Save();
                    }
                    else
                    {
                        Console.WriteLine("'Bias' column already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
