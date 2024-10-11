// Task Tracker CLI Project in C# - Step by Step Explanation

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Step 1: Define the Task class to represent each task with required properties


// Step 2: Create the Program class to handle user input and manage tasks
namespace TaskTracker
{
    class Program
    {
        // Step 3: Define the file path where tasks will be stored
        private static string filePath = "tasks.json";
        
        static void Main(string[] args)
        {
            // Step 4: Handle case when no arguments are provided
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a command.");
                return;
            }

            string command = args[0].ToLower();
            List<Task> tasks = LoadTasks();

            // Step 5: Handle different commands based on user input
            HandleCommand(command, args, tasks);

            // Step 6: Save tasks after processing user command
            SaveTasks(tasks);
        }

        // Step 7: Handle different commands based on user input
        static void HandleCommand(string command, string[] args, List<Task> tasks)
        {
            switch (command)
            {
                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide a task description.");
                    }
                    else
                    {
                        AddTask(tasks, args[1]);
                    }
                    break;
                case "update":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Please provide a task ID and new description.");
                    }
                    else
                    {
                        UpdateTask(tasks, int.Parse(args[1]), args[2]);
                    }
                    break;
                case "delete":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide a task ID.");
                    }
                    else
                    {
                        DeleteTask(tasks, int.Parse(args[1]));
                    }
                    break;
                case "mark-in-progress":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide a task ID.");
                    }
                    else
                    {
                        UpdateTaskStatus(tasks, int.Parse(args[1]), "in-progress");
                    }
                    break;
                case "mark-done":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please provide a task ID.");
                    }
                    else
                    {
                        UpdateTaskStatus(tasks, int.Parse(args[1]), "done");
                    }
                    break;
                case "list":
                    if (args.Length == 1)
                    {
                        ListTasks(tasks, "all");
                    }
                    else
                    {
                        ListTasks(tasks, args[1].ToLower());
                    }
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }

        // Step 8: Load tasks from the JSON file
        static List<Task> LoadTasks()
        {
            if (!File.Exists(filePath))
            {
                return new List<Task>();
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Task>>(json);
        }

        // Step 9: Save tasks to the JSON file
        static void SaveTasks(List<Task> tasks)
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        // Step 10: Add a new task
        static void AddTask(List<Task> tasks, string description)
        {
            int newId = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
            Task newTask = new Task
            {
                Id = newId,
                Description = description,
                Status = "todo",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            tasks.Add(newTask);
            Console.WriteLine($"Task added successfully (ID: {newTask.Id})");
        }

        // Step 11: Update an existing task's description
        static void UpdateTask(List<Task> tasks, int id, string newDescription)
        {
            Task task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }
            task.Description = newDescription;
            task.UpdatedAt = DateTime.Now;
            Console.WriteLine("Task updated successfully.");
        }

        // Step 12: Delete a task
        static void DeleteTask(List<Task> tasks, int id)
        {
            Task task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }
            tasks.Remove(task);
            Console.WriteLine("Task deleted successfully.");
        }

        // Step 13: Update a task's status
        static void UpdateTaskStatus(List<Task> tasks, int id, string status)
        {
            Task task = tasks.Find(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }
            task.Status = status;
            task.UpdatedAt = DateTime.Now;
            Console.WriteLine("Task status updated successfully.");
        }

        // Step 14: List tasks based on their status
        static void ListTasks(List<Task> tasks, string filter)
        {
            List<Task> filteredTasks = filter switch
            {
                "done" => tasks.FindAll(t => t.Status == "done"),
                "todo" => tasks.FindAll(t => t.Status == "todo"),
                "in-progress" => tasks.FindAll(t => t.Status == "in-progress"),
                _ => tasks
            };

            foreach (Task task in filteredTasks)
            {
                Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, CreatedAt: {task.CreatedAt}, UpdatedAt: {task.UpdatedAt}");
            }
        }
    }
}