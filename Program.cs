using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
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
            var (data, maxWidth) = parseCsv(filePath);
            if(data == null)
            {
                Console.WriteLine("Invalid file path");
                return 2;
            }

            Console.WriteLine($"\t{maxWidth}rows x {data.Count()}cols");
            var colWidths = getColWidths(data);
            printData(data, colWidths);
            return 0;
        }

        private static void printData(List<string[]> data, List<int> colWidths)
        {
            for(int row = 0; row < data.Count; row++)
            {
                Console.Write(pad((row+1).ToString(), 3));
                for(int col = 0; col < data[row].Length; col++)
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

        private static List<int> getColWidths(List<string[]> data)
        {
            var widths = new List<int>();
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < data[row].Length; col++)
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

        private static (List<string[]>?, int) parseCsv(string filePath)
        {
            if (!File.Exists(filePath)) return (null, -1);

            var data = new List<string[]>();
            using StreamReader r = new(filePath);
            string? line;
            int maxWidth = 0;
            while ((line = r.ReadLine()) != null)
            {
                //TODO(IMPROVEMENT): could be improved lol
                string[] values = line.Split(',');
                data.Add(values);
                if (values.Length > maxWidth) maxWidth = values.Length;
            }
            return (data, maxWidth);
        }
    }


}
