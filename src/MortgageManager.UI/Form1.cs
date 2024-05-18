using MortgageManager.CMS;
using MortgageManager.Domain.Helpers;
using MortgageManager.Entities.Models;
using System.Data;

namespace MortgageManager.UI
{
    public partial class Form1 : Form
    {
        private Products _products;

        public Form1()
        {
            InitializeComponent();
            lblProductCount.Text = string.Empty;
            lblFilePath.Text = string.Empty;
        }

        private void ImportButtonClick(object sender, EventArgs e)
        {
            dialogImport.ShowDialog();
        }

        private async void UploadButtonClick(object sender, EventArgs e)
        {
            var mortgageCreator = new MortgageCreator();
            // twice?
            var csvManager = new CsvManager("_csv/users.csv");
            Products products = csvManager.ImportUsers();

            foreach (Product product in products.GetAll())
            {
                await mortgageCreator.UploadMortgageProduct(product);
            }
        }

        private void DialogImportFileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pnlInfo.Visible = true;
            lblFilePath.Text = dialogImport.FileName;
            var csvManager = new CsvManager("_csv/users.csv");
            Products products = csvManager.ImportUsers();
            int productCount = 0;

            DataTable dt = new DataTable();

            dt.Columns.Add("Product Code");
            dt.Columns.Add("Name");

            foreach (var product in products.GetAll())
            {
                dt.Rows.Add(product.ProductCode, product.Name);
                productCount++;
            }

            lblProductCount.Text = productCount.ToString();

            dataGridImported.DataSource = dt;
        }
    }
}
