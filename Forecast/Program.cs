using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Proto;

namespace Forecast
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Load a CSV file.
            string filePath = @"C:\Users\Fabrizio\Documents\Visual Studio 2010\Projects\Forecast\Forecast\sheets\test.csv";
            CSV csv = readCsvFile(filePath);
            ForecastModel model = new ForecastModel(csv, 1000);
            ForecastItem reduced = model.reduce(1000, 10);

            string sourceFileName = Path.GetFileName(filePath);
            string sourceFileDirectory = Path.GetDirectoryName(filePath);
            string nameWithoutExt = sourceFileName.Substring(0, sourceFileName.IndexOf('.'));

            string outFile = sourceFileDirectory + @"\" + nameWithoutExt + "_forecast.csv";

            List<string> linesOut = new List<string>();
            linesOut.Add("Percentile, Value, Density");

            double prevVal = 0.0; // reduced[0].Value;

            double totalProb = 0.0;
            foreach (ModelAtom a in reduced)
            {
                double density = a.Prob / Math.Abs(a.Value - prevVal);
                totalProb += a.Prob;
                linesOut.Add("" + totalProb * 100 + "," + a.Value + "," + density);

                prevVal = a.Value;
            }
            System.IO.File.WriteAllLines(outFile, linesOut.ToArray());
        }


        static CSV readCsvFile(string fileName)
        {
            CSV csv = new CSV();
            using (CsvFileReader reader = new CsvFileReader(fileName))
            {

                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    string[] newRow = new string[row.Count];
                    csv.Add(newRow);
                    int col = 0;

                    foreach (string s in row)
                    {
                        newRow[col] = s;
                        //Console.WriteLine (s);
                        col++;
                    }
                }
            }

            csv.makeRectangular();
            return csv;
        }
    }
}
