using Microsoft.Extensions.Logging;
using MortgageManager.CMS;
using MortgageManager.DataAccess.Helpers;
using MortgageManager.Entities.Helpers;
using MortgageManager.Entities.Models;
using System.Data;

namespace MortgageManager.UI
{
    public partial class Form1 : Form
    {
        private Products _products;
        private readonly ILogger<Application> _logger;
        private readonly CsvManager _csvManager;
        private readonly MortgageCreator _mortgageCreator;

        public Form1(ILogger<Application> logger, CsvManager csvManager, MortgageCreator mortgageCreator)
        {
            InitializeComponent();
            _logger = logger;
            _csvManager = csvManager;
            _mortgageCreator = mortgageCreator;
            lblProductCount.Text = string.Empty;
            lblFilePath.Text = string.Empty;
            lblOverallStatus.Text = string.Empty;
        }

        private void ImportButtonClick(object sender, EventArgs e)
        {
            dialogImport.ShowDialog();
        }

        private async void UploadButtonClick(object sender, EventArgs e)
        {
            int failureCount = 0;
            Products products = _csvManager.ImportUsers();

            List<Task<Product>> taskList = [];

            foreach (var product in products.GetAll())
            {
                try
                {
                    taskList.Add(_mortgageCreator.UploadMortgageProduct(product));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
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
                foreach (DataGridViewRow row in dataGridImported.Rows)
                {
                    var item = (DataRowView)row.DataBoundItem;

                    if (item?.Row[0].ToString() == finishedTask.Result.ProductCode)
                    {
                        switch (finishedTask.Result.Status)
                        {
                            case ProductStatus.ProductNotCreated:
                            case ProductStatus.ProductPageNotCreated:
                                row.Cells[2].Style.ForeColor = Color.Red;
                                break;
                            case ProductStatus.InProgress:
                            case ProductStatus.AlreadyExists:
                            default:
                                row.Cells[2].Style.ForeColor = Color.Orange;
                                break;
                            case ProductStatus.ProcessedSuccessfully:
                                row.Cells[2].Style.ForeColor = Color.Green;
                                break;
                        }

                        item.Row[2] = finishedTask.Result.Status;
                    }
                }

                if (finishedTask.Result.Status != ProductStatus.ProcessedSuccessfully)
                {
                    lblOverallStatus.Text += $"Problem with Product: {finishedTask.Result.ProductCode}... {finishedTask.Result.Status}" + Environment.NewLine;
                    _logger.LogWarning($"{finishedTask.Result.ProductCode}: {finishedTask.Result.Status}");
                }
            }
            else
            {
                lblOverallStatus.Text += $"{finishedTask.Exception?.Message}" + Environment.NewLine;
                _logger.LogError($"{finishedTask.Exception?.Message}");
            }
        }

        private void PrintCompletionMessage(int failureCount)
        {
            lblOverallStatus.Text += $"All tasks processed.";

            if (failureCount > 0)
            {
                lblOverallStatus.Text += $" {failureCount} products were invalid.";
            }

            // move this out?
            foreach (DataGridViewRow row in dataGridImported.Rows)
            {
                var item = (DataRowView)row.DataBoundItem;

                if (item?.Row[2].ToString() == "Unprocessed")
                {
                    row.Cells[2].Style.ForeColor = Color.Red;
                }

            }
        }

        private void DialogImportFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pnlInfo.Visible = true;
            lblFilePath.Text = dialogImport.FileName;
            Products products = _csvManager.ImportUsers();
            int productCount = 0;

            DataTable dt = new DataTable();

            dt.Columns.Add("Product Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("Progress");

            foreach (var product in products.GetAll())
            {
                dt.Rows.Add(product.ProductCode, product.Name, nameof(ProductStatus.Unprocessed));
                productCount++;
            }

            lblProductCount.Text = productCount.ToString();

            dataGridImported.DataSource = dt;
            dataGridImported.Columns[2].DefaultCellStyle.ForeColor = Color.Gray;
            btnUpload.Enabled = true;
        }
    }
}
