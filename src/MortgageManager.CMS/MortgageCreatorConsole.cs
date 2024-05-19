using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management.Models.Types.Elements;
using Kontent.Ai.Management.Models.Types;
using Kontent.Ai.Management;
using MortgageManager.Entities.Models;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Modules.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Kontent.Ai.Management.Exceptions;

namespace MortgageManager.CMS
{
    public class MortgageCreatorConsole
    {
        //private IConfiguration _config = new ConfigurationBuilder()
        //    .AddUserSecrets<Settings>()
        //    .Build();

        private ManagementClient _client = new ManagementClient(new ManagementOptions
        {
            EnvironmentId = null,
            ApiKey = null
        });

        public async Task<bool> UploadMortgageProduct(Product product)
        {
            await DeleteExistingScripted(); //temporary
            bool productCreated = false;
            bool productPageCreated = false;
            bool productPageUpdated = false;
            bool productUpdated = false;
            var productPageCodename = "mortgage_scripted_page";
            var productItemCodename = "mortgage_scripted_product";
            if (!await ItemExists(productPageCodename))
            {
                productPageCreated = await CreateProductPage(productPageCodename);
            }
            if (!await ItemExists(productItemCodename))
            {
                productCreated = await CreateProduct(product, productItemCodename, productPageCodename);
            }
            if (productPageCreated && productCreated) 
            {
                productPageUpdated = await LinkProductPageToProduct(productItemCodename, productPageCodename);
                productUpdated = await LinkProductToProductPage(productItemCodename, productPageCodename);
            }
            
            //var identifier = new LanguageVariantIdentifier(Reference.ById(itemId), Reference.ByCodename("default"));
            //var identifier = new LanguageVariantIdentifier(Reference.ByCodename(product.Name?.Replace(" ", "_").Replace("-", "_").ToLower()), Reference.ByCodename("default"));

            //await UpdateProductDetails(identifier, product);

            return productCreated;
        }

        private async Task DeleteExistingScripted()
        {
            try
            {
                await _client.DeleteContentItemAsync(Reference.ByCodename("mortgage_scripted_page"));
                Console.WriteLine("'mortgage_scripted_page' deleted");
                await _client.DeleteContentItemAsync(Reference.ByCodename("mortgage_scripted_product"));
                Console.WriteLine("'mortgage_scripted_product' deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<bool> CreateProduct(Product product, string productItemCodename, string productPageCodename)
        {
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = "CHRIS_Scripted Mortgage Product",
                Codename = productItemCodename,
                Type = Reference.ByCodename("product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });
            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productItemCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements = new dynamic[]
                    {
                        new TextElement { Element = Reference.ByCodename("product_code"), Value = "CHRISSCRIPTED1"},
                        new TextElement { Element = Reference.ByCodename("name"), Value = product.Name},
                        new TextElement { Element = Reference.ByCodename("initial_interest_rate"), Value = "99"},
                        new TextElement { Element = Reference.ByCodename("standard_variable_rate"), Value = "98"},
                        new TextElement { Element = Reference.ByCodename("comparison_cost"), Value = "95"},
                        new TextElement { Element = Reference.ByCodename("maximum_ltv____"), Value = "97"},
                        new TextElement { Element = Reference.ByCodename("fees"), Value = "96"},
                    },
                    Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
                });
            }
            return response != null;
        }

        private async Task<bool> CreateProductPage(string productPageCodename)
        {
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = "Page - CHRIS_Scripted Mortgage Product",
                Codename = productPageCodename,
                Type = Reference.ByCodename("web_product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });
            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productPageCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements = new dynamic[]
                    {
                        new TextElement { Element = Reference.ByCodename("title"), Value = response.Name },
                    },
                    Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
                });
            }

            return response != null;
        }

        private async Task<bool> LinkProductPageToProduct(string productItemCodename, string productPageCodename)
        {
            var response = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productPageCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
            {
                Elements = new dynamic[]
                {
                    new LinkedItemsElement { Element = Reference.ByCodename("product"), Value = [Reference.ByCodename(productItemCodename)] },
                },
                Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
            });

            return response != null;
        }

        private async Task<bool> LinkProductToProductPage(string productItemCodename, string productPageCodename)
        {
            var response = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productItemCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
            {
                Elements = new dynamic[]
                {
                    new LinkedItemsElement { Element = Reference.ByCodename("details_page"), Value = [Reference.ByCodename(productPageCodename)] },
                },
                Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
            });

            return response != null;
        }

        protected async Task<bool> ItemExists(string elementCodename)
        {
            var identifier = Reference.ByCodename(elementCodename);

            try
            {
                var contentItemModel = await _client.GetContentItemAsync(identifier);
                return contentItemModel != null;
            }
            catch (ManagementException)
            {
                return false;
            }
        }
    }
}
