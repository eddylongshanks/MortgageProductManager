using MortgageManager.CMS;
using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Models;

namespace MortgageManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var csvManager = new CsvManager("_csv/users.csv");
            var mortgageManager = new MortgageCreator();

            Products products = csvManager.ImportUsers();

            List<Task> taskList = new List<Task>();

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

            int total = 0;
            while (taskList.Any())
            {
                Task finishedTask = await Task.WhenAny(taskList);
                taskList.Remove(finishedTask);
                total += 1;
                Console.WriteLine($"Products created: {total}. Jobs left: {taskList.Count}");
            }

            await Task.WhenAll(taskList);
            PrintCompletionMessage(true);
        }

        private static void PrintCompletionMessage(bool productCreated)
        {
            if (productCreated)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"All Products Created.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to create product: ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
