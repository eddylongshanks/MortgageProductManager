using MortgageManager.CMS;
using MortgageManager.DataAccess.Helpers;
using MortgageManager.Entities.Helpers;
using MortgageManager.Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using MortgageManager.CMS.Mappers;

namespace MortgageManager
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = CreateServices();

            Application app = services.GetRequiredService<Application>();
            await app.ExecuteAsync();
        }

        private static ServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddLogging(builder => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole()
                    );
            services.AddSingleton<CsvManager>();
            services.AddSingleton<MortgageCreator>();
            services.AddSingleton<IProductMortgageMapper, ProductMortgageMapper>();
            services.AddSingleton<Application>();

            return services.BuildServiceProvider();
        }

        public class Application(ILogger<Application> logger, CsvManager csvManager, MortgageCreator mortgageManager)
        {
            public async Task ExecuteAsync()
            {
                int failureCount = 0;
                csvManager.FileName = "_csv/users.csv";
                Products products = csvManager.ImportProducts();

                List<Task<Product>> taskList = [];

                foreach (var product in products.GetAll())
                {
                    try
                    {
                        taskList.Add(mortgageManager.UploadMortgageProduct(product));
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }

                while (taskList.Any())
                {
                    Task<Product> finishedTask = await Task.WhenAny(taskList);
                    if (!finishedTask.IsCompletedSuccessfully)
                        failureCount++;
                    taskList.Remove(finishedTask);
                    PrintProcessedState(finishedTask, taskList.Count);
                }

                await Task.WhenAll(taskList);
                PrintCompletionMessage(failureCount);
            }

            private void PrintProcessedState(Task<Product> finishedTask, int tasksRemaining)
            {
                if (finishedTask.IsCompletedSuccessfully)
                {
                    if (finishedTask.Result.Status == ProductStatus.ProcessedSuccessfully)
                        Console.WriteLine($"""Product: "{finishedTask.Result.ProductCode}" created. {tasksRemaining} products remaining...""");
                    else
                        logger.LogWarning($"{finishedTask.Result.ProductCode}: {finishedTask.Result.Status}");
                }
                else
                {
                    logger.LogError($"{finishedTask.Exception?.Message}");
                }
            }

            private void PrintCompletionMessage(int failureCount)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"All tasks processed.");
                
                if (failureCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($" {failureCount} products were invalid.");
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
