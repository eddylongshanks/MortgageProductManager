using MortgageManager.CMS;
using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Helpers;
using MortgageManager.Entities.Models;
using System.Diagnostics;

namespace MortgageManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var csvManager = new CsvManager("_csv/users.csv");
            var mortgageManager = new MortgageCreator();

            Products products = csvManager.ImportUsers();

            List<Task<Product>> taskList = [];

            foreach (var product in products.GetAll())
            {
                try
                {
                    taskList.Add(mortgageManager.UploadMortgageProduct(product));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }                
            }

            while (taskList.Any())
            {
                Task<Product> finishedTask = await Task.WhenAny(taskList);
                taskList.Remove(finishedTask);
                PrintProcessedState(finishedTask, taskList.Count);
            }

            await Task.WhenAll(taskList);
            PrintCompletionMessage();
        }

        private static void PrintProcessedState(Task<Product> finishedTask, int tasksRemaining)
        {
            if (finishedTask.IsCompletedSuccessfully)
            {
                Console.Write($"Product: {finishedTask.Result.ProductCode} status: ");

                if (finishedTask.Result.Status != ProductStatus.ProcessedSuccessfully)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($"{finishedTask.Result.Status}");
            }
            else
            {
                Console.Write($"Error processing task: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{finishedTask.Exception?.Message}");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($", {tasksRemaining} products remaining...");
        }

        private static void PrintCompletionMessage()
        {            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"All tasks processed.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
