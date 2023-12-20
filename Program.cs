using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace csv_viewer
{
    internal class Program
    {
        const int MAX_COL_WIDTH = 25; 
        static int Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Provide a text file");
                return 1;
            }

            string filePath = "..\\..\\..\\" + args[0];
            if (!Debugger.IsAttached) filePath = args[0];
            var (data, rowCount) = parseCsv(filePath);
            if(data == null)
            {
                Console.WriteLine("Invalid file path");
                return 2;
            }

            Console.WriteLine($"\t{rowCount}rows x {data.Count()}cols");
            var colWidths = getColWidths(data);
            printData(data, colWidths);
            return 0;
        }

        private static void printData(List<List<string>> data, List<int> colWidths)
        {
            for(int row = 0; row < data.Count; row++)
            {
                Console.Write(pad((row+1).ToString(), 3));
                for(int col = 0; col < data[row].Count; col++)
                {
                    Console.Write(pad(data[row][col], colWidths[col]));
                }
                Console.WriteLine();
            }
        }

        private static string pad(string str, int width)
        {
            if(width > MAX_COL_WIDTH+3)
            {
                width = MAX_COL_WIDTH+3;
            }
            if(str.Length > MAX_COL_WIDTH)
            {
                str = str.Substring(0, MAX_COL_WIDTH-1) + "...";
            }
            string padding = new string(' ', 1+width-str.Length);
            var output = str + padding;
            return output;
        }

        private static List<int> getColWidths(List<List<string>> data)
        {
            var widths = new List<int>();
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < data[row].Count; col++)
                {
                    if (col >= widths.Count)
                    {
                        widths.Add(0);
                    }
                    int width = data[row][col].Length;
                    if (width > widths[col])
                    {
                        widths[col] = width;
                    }
                }
            }
            return widths;
        }

        private static (List<List<string>>?, int) parseCsv(string filePath)
        {
            if (!File.Exists(filePath)) return (null, -1);

            var data = new List<List<string>>();
            using StreamReader r = new(filePath);
            string? line;
            int rowCount = 0;
            while ((line = r.ReadLine()) != null)
            {
                var values = splitLine(line);

                data.Add(values);
                if (values.Count > rowCount) rowCount = values.Count;
            }
            return (data, rowCount);
        }
        private static List<string> splitLine(string line)
        {
            var results = new List<string>();
            bool inQuotes = false;
            string currentField = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"' && (i == 0 || line[i - 1] != '\\'))
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    results.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }

            results.Add(currentField);
            return results;
        }
    }
}
