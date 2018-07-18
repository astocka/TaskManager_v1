﻿using System;
using System.Linq;

namespace TaskManager
{
    public static class Filters
    {
        public static void Filter()
        {
            Console.Write("Enter the index of the filter: ");
            int userFilter = Convert.ToInt32(Console.ReadLine().Trim().ToLower());
            string word;

            try
            {
                switch (userFilter)
                {
                    case 1:
                        Console.Write("Enter the search word: ");
                        word = Console.ReadLine().Trim();
                        wordFilter(word);
                        break;
                    case 2:
                        startTimeFilter();
                        break;
                    case 3:
                        endTimeFilter();
                        break;
                    case 4:
                        allDayFilter();
                        break;
                    case 5:
                        importantTaskFilter();
                        break;

                    default:
                        ConsoleEx.Write("Wrong index number!", ConsoleColor.Red);
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            {
                ConsoleEx.Write("Wrong index number!", ConsoleColor.Red);
                Console.ReadLine();
            }
            catch (FormatException)
            {
                ConsoleEx.Write("Wrong data!", ConsoleColor.Red);
                Console.ReadLine();
            }
            catch (Exception)
            {
                ConsoleEx.Write("Something went wrong...", ConsoleColor.Red);
                Console.ReadLine();
            }
        }

        public static void wordFilter(string word)
        {
            var wordFilter = Program.taskList.Where(a => a.Description.Contains(word));
            foreach (TaskModel task in wordFilter)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
        }

        public static void startTimeFilter()
        {
            var start = Program.taskList.OrderBy(a => a.StartTime);
            foreach (TaskModel task in start)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
        }

        public static void endTimeFilter()
        {
            var end = Program.taskList.OrderBy(a => a.EndTime);
            foreach (TaskModel task in end)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
        }

        public static void allDayFilter()
        {
            var allDay = Program.taskList.Where(a => a.ImportantTask == true).OrderBy(a => a.StartTime);
            foreach (TaskModel task in allDay)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
        }

        public static void importantTaskFilter()
        {
            var important = Program.taskList.Where(a => a.ImportantTask == true).OrderBy(a => a.StartTime);
            foreach (TaskModel task in important)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
        }
    }
}



