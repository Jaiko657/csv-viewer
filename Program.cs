using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
           OpenFile();
        }

        static void OpenFile()
        {
            Console.WriteLine("Enter CSV File directory to open:");
            var Target = Console.ReadLine();
            string[][] table = new string[CountLinesInFile(Target)][];
            //file lines
            string[] lines = File.ReadAllLines(Target);
            int i = 0;
            //loop through each file line
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                table[i] = columns;
                i++;
            }
            Console.Clear();
            Console.WriteLine();
            Print2DArray(table);
            Console.WriteLine();
            Menu();
        }

        static void Menu()
        {

            string menuText = "type q to exit   type a to open another";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menuText.Length / 2)) + "}", menuText));
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.A)
                { 
                    Console.Clear();
                    OpenFile();
                    break;
                }
                if (key.Key == ConsoleKey.Q)
                {
                    //Environment.Exit(0);
                    break;
                }
            }
        }
        static public void Print2DArray(string[][] table)
        {
            //Console.WriteLine(table[0][1]);
            foreach (string[] row in table)
            {
                PrintRow(row);
                PrintLine();
            }
        }

        static int tableWidth = Console.WindowWidth-4;

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth-7));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            Console.WriteLine(row);
        }
        static int GetColumnWidth(string[][] table, int row)
        {
            int count = 0;
            for (int i = 0; i < table.Length; i++)
            {
                Console.WriteLine(table[i][row]);
                if (table[i][row].Length > count)
                {
                    count = table[i][row].Length;
                }
            }
            return count;
        }
        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static int CountLinesInFile(string f)
        {
            int count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
