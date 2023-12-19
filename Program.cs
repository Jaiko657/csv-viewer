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
                Console.WriteLine("Invalid text file");
                return 2;
            }

            Console.WriteLine($"\t{maxWidth}rows x {data.Count()}cols");
            printData(data);
            return 0;
        }

        private static void printData(List<string[]> data)
        {
            for(int row = 0; row < data.Count; row++)
            {
                Console.Write(row+1);
                for(int col = 0; col < data[row].Length; col++)
                {
                    Console.Write("\t" + data[row][col]);
                }
                Console.WriteLine();
            }
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
