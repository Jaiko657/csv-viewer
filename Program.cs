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
            if (args.Length == 0)
            {
                OpenFile();
            }
            else
            {
                if (args[0] == "-f")
                {
                    if (args.Length == 2)
                    {
                        OpenFile(args[1]);
                    }
                    else
                    {
                        Console.WriteLine("Usage: -f \".\\csv file.csv\"");
                    }
                }
            }
        }

        static string openFileMenu()
        {
            Console.WriteLine("Enter CSV File directory to open:");
            var Target = Console.ReadLine();
            if (Target == null) { return ""; }
            return Target;
        }

        static void OpenFile(string Target = "")
        {
            if (Target == "")
            {
                Target = openFileMenu();
            }
            Console.Clear();
            var height = CountLinesInFile(Target);

            //Declare array to store CSV in with correct height (needs height in initialise)
            string[][] table = new string[height][];

            //file lines
            string[] lines = File.ReadAllLines(Target);

            //loop through each file line
            int i = 0;
            int amountOfColumns = 0;
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                if (amountOfColumns < columns.Length)
                {
                    amountOfColumns = columns.Length;
                }
                table[i] = columns;
                i++;
            }
            int[] columnWidths = new int[amountOfColumns];
            for (int j = 0; j < amountOfColumns; j++)
            {
                columnWidths[j] = getColumnWidth(table, j);
            }

            Print2DArray(table, columnWidths);
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
                    Environment.Exit(0);
                    break;
                }
            }
        }
        static public void Print2DArray(string[][] table, int[] columnWidths)
        {
            foreach (string[] row in table)
            {
                PrintRow(row, columnWidths);
                PrintLine(columnWidths);
            }
        }

        static void PrintLine(int[] columnWidths)
        {
            Console.WriteLine(new string('-', columnWidths.Sum() + 1 + (3 * columnWidths.Length)));
        }

        static void PrintRow(string[] columns, int[] columnWidths)
        {
            string row = "|";

            int count = 0;
            foreach (string column in columns)
            {
                row += " " + CreateCell(column, columnWidths[count]) + " |";
                count++;
            }
            Console.WriteLine(row);
        }

        static string CreateCell(string text, int width)
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
        static int getColumnWidth(string[][] table, int row)
        {
            int count = 0;
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i][row].Length > count)
                {
                    count = table[i][row].Length;
                }
            }
            return count;
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

