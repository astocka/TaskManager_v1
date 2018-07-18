using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManager
{
    class Program
    {
        public static List<TaskModel> taskList { get; private set; } = new List<TaskModel>();

        static void Main(string[] args)
        {
            string command = "";

            do
            {
                Console.Clear();
                MainMenu.Menu();
                Console.Write("Enter the command: ");
                command = Console.ReadLine().Trim().ToLower();

                if (command == "exit")
                    break;

                switch (command)
                {
                    case "add":
                        AddTask(taskList);
                        break;
                    case "remove":
                        RemoveTask(taskList);
                        break;
                    case "show":
                        ShowTasks(taskList);
                        break;
                    case "sort":
                        SortTasks(taskList);
                        break;
                    case "save":
                        SaveTasks(taskList);
                        break;
                    case "load":
                        LoadTasks(taskList);
                        break;
                    case "help":
                        HelpMenu.Help();
                        break;

                    default:
                        ConsoleEx.Write("Wrong command.", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
            while (true);
        }

        // AddTask method take  data from user and create new TaskModel
        private static TaskModel CreateTask()
        {
            Console.Clear();
            MainMenu.Menu();

            string description;
            DateTime startTime;
            DateTime? endTime;
            bool? allDayTask;
            bool? importantTask;

            ConsoleEx.WriteLine(":: NEW TASK ::", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);
            Console.WriteLine(">>> Insert the data according to the scheme below (use comma [,] as a separator): ");
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine(">>> TASK SCHEME:\ndescription,start_time,end_time,[all-day task (optional)],[important task (optional)]", ConsoleColor.Green);
            Console.WriteLine();
            ConsoleEx.WriteLine("Description [required]:\ncontent of the task", ConsoleColor.Gray);
            Console.WriteLine();
            ConsoleEx.WriteLine("Start_time [required]:\nthe start date of the task in format: [YYYY-MM-DD]", ConsoleColor.Gray);
            Console.WriteLine();
            ConsoleEx.WriteLine("End_time [optional]:\nthe end date of the task in format: [YYYY-MM-DD] --> leave empty if it is all-day task", ConsoleColor.Gray);
            Console.WriteLine();
            ConsoleEx.WriteLine("All-day task [optional]:\nenter [true] or [false]", ConsoleColor.Gray);
            Console.WriteLine();
            ConsoleEx.WriteLine("Important task [optional]:\nenter [true] or [false]", ConsoleColor.Gray);
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkCyan);
            Console.WriteLine();
            Console.Write("Enter new task [in scheme]: ");

            string taskData = Console.ReadLine().Trim();
            string[] checkData = taskData.Split(",");

            try
            {
                if (checkData[0] == null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    description = checkData[0]; // set Description
                }
            }
            catch (NullReferenceException)
            {
                ConsoleEx.Write("Missing task description.", ConsoleColor.Red);
                return null;
            }

            try
            {
                char[] chars = checkData[1].ToCharArray();
                if (checkData[1].Trim().Length == 10 && chars[4] == '-' && chars[7] == '-')
                {
                    startTime = DateTime.Parse(checkData[1].Trim()); // set StartTime
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                ConsoleEx.Write("Incorrect date format.", ConsoleColor.Red);
                return null;
            }

            if (string.IsNullOrWhiteSpace(checkData[2])) // EndTime is optional - set null if it is empty
            {
                endTime = null;
            }
            else
            {
                try
                {
                    char[] chars = checkData[1].ToCharArray();
                    if (checkData[2].Trim().Length == 10 && chars[4] == '-' && chars[7] == '-')
                    {
                        endTime = DateTime.Parse(checkData[2].Trim()); // set EndTime
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                catch (Exception)
                {
                    ConsoleEx.Write("Incorrect date format.", ConsoleColor.Red);
                    return null;
                }
            }

            if (checkData[3].Trim() == "true") // set AllDayTask flag
            {
                allDayTask = true;
            }
            else if (checkData[3].Trim() == "false")
            {
                allDayTask = false;
            }
            else
            {
                allDayTask = null;
            }

            if (checkData[4].Trim() == "true") // set ImportantTask flag
            {
                importantTask = true;
            }
            else if (checkData[4].Trim() == "false")
            {
                importantTask = false;
            }
            else
            {
                importantTask = null;
            }
            return new TaskModel(description, startTime, endTime, allDayTask, importantTask);
        }

        // AddTask method add new TaskModel object to taskList
        private static void AddTask(List<TaskModel> taskList)
        {
            var newTask = CreateTask();
            if (newTask != null)
            {
                taskList.Add(newTask);
                ConsoleEx.WriteLine("\nThe task was successfully added.", ConsoleColor.Green);
                Console.Write("Press ENTER to continue... ");
                Console.ReadLine();
            }
        }

        // RemoveTask method delete a task
        private static void RemoveTask(List<TaskModel> taskList)
        {
            int taskIndex = 0;

            Console.Clear();
            MainMenu.Menu();
            ConsoleEx.WriteLine(" :: REMOVE TASK ::", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine(">>> List of tasks:", ConsoleColor.Gray);
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkCyan);

            for (int i = 0; i < taskList.Count; i++) // shows all tasks from the taskList
            {
                ConsoleEx.WriteLine($"Task number {i + 1}:", ConsoleColor.DarkRed);
                Console.WriteLine($"{taskList[i].ExportToString()}");
                Console.WriteLine();
            }
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkCyan);
            Console.WriteLine();

            Console.Write("Enter the index of the task to delete: ");
            taskIndex = int.Parse(Console.ReadLine().Trim());
            try
            {
                taskList.RemoveAt(taskIndex - 1);
                ConsoleEx.WriteLine($"Task number {taskIndex} has been deleted.", ConsoleColor.Green);
            }
            catch (FormatException)
            {
                ConsoleEx.Write("Wrong data format.", ConsoleColor.Red);
            }
            catch (IndexOutOfRangeException)
            {
                ConsoleEx.Write($"There is no #{taskIndex} task.", ConsoleColor.Red);
            }
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void SortTasks(List<TaskModel> taskList)
        {
            Console.Clear();
            MainMenu.Menu();
            ConsoleEx.WriteLine(" :: TASK FILTER ::", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);
            Console.WriteLine(">>> Tasks filters: [1] Word, [2] Start date, [3] End date, [4] All-day tasks, [5] Important tasks");
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkCyan);
            Console.Write(" | Task ".PadRight(15));
            Console.Write(" | Start time ".PadLeft(39));
            Console.Write(" | End time ".PadLeft(20));
            Console.Write(" | All-day ".PadLeft(24));
            Console.WriteLine(" | Important ".PadLeft(15));
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkCyan);

            Filters.Filter();
            Console.WriteLine();
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkCyan);
            Console.Write("ENTER to continue... ");
            Console.ReadKey();
        }

        //  ShowTasks method shows all added/uploaded tasks ordered by important tasks and start date
        private static void ShowTasks(List<TaskModel> taskList)
        {
            Console.Clear();
            MainMenu.Menu();
            ConsoleEx.WriteLine(" :: SUMMARY ::", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);
            Console.Write(" | Task ".PadRight(15));
            Console.Write(" | Start time ".PadLeft(39));
            Console.Write(" | End time ".PadLeft(20));
            Console.Write(" | All-day ".PadLeft(24));
            Console.WriteLine(" | Important ".PadLeft(15));
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkCyan);

            var importantTasks = taskList.Where(a => a.ImportantTask == true).OrderBy(a => a.StartTime);
            foreach (TaskModel task in importantTasks)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }

            var otherTasks = taskList.Where(a => a.ImportantTask == false || a.ImportantTask == null)
                    .OrderBy(a => a.StartTime);
            foreach (TaskModel task in otherTasks)
            {
                Console.Write($" | {task.Description}".PadRight(40));
                Console.Write($" | {task.StartTime}");
                Console.Write($" | {task.EndTime}");
                Console.Write($" | {task.AllDayTask}".PadLeft(11));
                Console.Write($" | {task.ImportantTask}".PadLeft(13));
                Console.WriteLine();
            }
            Console.WriteLine();
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkCyan);

            Console.WriteLine();
            Console.Write("Press ENTER to continue... ");
            Console.ReadKey();
        }

        // SaveTasks method saves all tasks from taskList to a file Data.csv
        private static void SaveTasks(List<TaskModel> taskList)
        {
            List<string> tasksToString = new List<string>();

            Console.Clear();
            MainMenu.Menu();
            ConsoleEx.WriteLine(" :: SAVE TO A FILE :: ", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);

            foreach (TaskModel task in taskList)
            {
                tasksToString.Add(task.ExportToCsvFile());
            }

            File.WriteAllLines("Data.csv", tasksToString);
            ConsoleEx.WriteLine(" >>> The tasks were successfully saved to [Data.csv] file.", ConsoleColor.Green);
            Console.WriteLine();
            Console.Write("Press ENTER to continue... ");
            Console.ReadKey();
        }

        // LoadTasks method loads all tasks form a file
        private static void LoadTasks(List<TaskModel> taskList)
        {
            string path = "";
            string[] listUpload = null;

            Console.Clear();
            MainMenu.Menu();
            ConsoleEx.WriteLine(" :: LOADING TASKS FROM A FILE :: ", ConsoleColor.DarkCyan);
            ConsoleEx.WriteLine("".PadLeft(115, '^'), ConsoleColor.DarkCyan);
            Console.WriteLine();
            Console.Write("Enter the file name: ");
            path = Console.ReadLine().Trim();

            if (File.Exists(path) == false)
            {
                ConsoleEx.Write("File was not found.", ConsoleColor.Red);
                Console.ReadLine();
                return;
            }

            string[] linesFromLoadedFile = File.ReadAllLines(path);
            List<string[]> tasksFromLoadedFile = new List<string[]>();
            char[] separators = { ',', ';' };

            foreach (string item in linesFromLoadedFile)
            {
                tasksFromLoadedFile.Add(item.Split(separators));
                try
                {
                    foreach (string[] loadedTask in tasksFromLoadedFile)
                    {
                        DateTime? endTime;
                        if (string.IsNullOrWhiteSpace(loadedTask[2]))
                        {
                            endTime = null;
                        }
                        else
                        {
                            endTime = DateTime.Parse(loadedTask[2]);
                        }

                        bool? allDayTask;
                        if (string.IsNullOrWhiteSpace(loadedTask[3]))
                        {
                            allDayTask = null;
                        }
                        else
                        {
                            allDayTask = bool.Parse(loadedTask[3]);
                        }

                        bool? importantTask;
                        if (string.IsNullOrWhiteSpace(loadedTask[4]))
                        {
                            importantTask = null;
                        }
                        else
                        {
                            importantTask = bool.Parse(loadedTask[4]);
                        }

                        taskList.Add(new TaskModel(loadedTask[0], DateTime.Parse(loadedTask[1]), endTime, allDayTask, importantTask));

                    }
                    tasksFromLoadedFile.Clear();
                }
                catch (FormatException)
                {
                    ConsoleEx.Write("Wrong file format.", ConsoleColor.Red);
                    Console.ReadLine();
                    return;
                }
                catch (Exception)
                {
                    ConsoleEx.Write("An unknown error occurred.", ConsoleColor.Red);
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine();
            ConsoleEx.WriteLine(" >>> Tasks from the file was successfully loaded.\n", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }
    }
}

