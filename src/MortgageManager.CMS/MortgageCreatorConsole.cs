using Kontent.Ai.Management.Configuration;
using Kontent.Ai.Management;
using MortgageManager.Entities.Models;
using Kontent.Ai.Management.Models.Items;
using Kontent.Ai.Management.Models.Shared;
using Kontent.Ai.Management.Models.LanguageVariants.Elements;
using Kontent.Ai.Management.Models.LanguageVariants;
using Kontent.Ai.Management.Models.Workflow;
using Kontent.Ai.Management.Exceptions;
using MortgageManager.CMS.Models;
using MortgageManager.CMS.Mappers;

namespace MortgageManager.CMS
{
    public class MortgageCreatorConsole
    {
        //private IConfiguration _config = new ConfigurationBuilder()
        //    .AddUserSecrets<Settings>()
        //    .Build();

        private IProductMortgageMapper _mortgageMapper = new ProductMortgageMapper();

        private ManagementClient _client = new ManagementClient(new ManagementOptions
        {
            EnvironmentId = null,
            ApiKey = null
        });

        public async Task<bool> UploadMortgageProduct(Product product)
        {
            ProductMortgage productMortgage = _mortgageMapper.Map(product);

            bool productCreated = false;
            bool productPageCreated = false;
            bool productPageUpdated = false;
            bool productUpdated = false;

            await DeleteExistingScripted(productMortgage.Codename, $"page_{productMortgage.Codename}"); //temporary

            if (!await ItemExists(productMortgage.PageCodename))
            {
                productPageCreated = await CreateProductPage(productMortgage);
            }
            if (!await ItemExists(productMortgage.Codename))
            {
                productCreated = await CreateProduct(productMortgage);
            }
            if (productPageCreated && productCreated) 
            {
                productPageUpdated = await CreateProductLink(productMortgage.PageCodename, productMortgage.Codename, "product");
                productUpdated = await CreateProductLink(productMortgage.Codename, productMortgage.PageCodename, "details_page");
            }

            return productPageUpdated && productUpdated;
        }


        private async Task<bool> CreateProduct(ProductMortgage productMortgage)
        {   
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = $"SCRIPTED_Product - Mortgage - {productMortgage.ProductCode} - {productMortgage.Name}",
                Codename = productMortgage.Codename,
                Type = Reference.ByCodename("product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });
            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productMortgage.Codename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements = new dynamic[]
                    {
                        productMortgage.GetTextElementFor(nameof(productMortgage.Name)),
                        productMortgage.GetTextElementFor(nameof(productMortgage.ProductCode)),
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

        private async Task<bool> CreateProductPage(ProductMortgage productMortgage)
        {
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = $"SCRIPTED_Page - Mortgage - {productMortgage.ProductCode} - {productMortgage.Name}",
                Codename = productMortgage.PageCodename,
                Type = Reference.ByCodename("web_product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });
            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productMortgage.PageCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements = new dynamic[]
                    {
                        new TextElement { Element = Reference.ByCodename("title"), Value = productMortgage.Name },
                    },
                    Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
                });
            }

            return response != null;
        }

        private async Task<bool> CreateProductLink(string targetCodename, string contentItemToLinkCodename, string linkedItemCodename)
        {
            var response = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(targetCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
            {
                Elements = new dynamic[]
                {
                    new LinkedItemsElement { Element = Reference.ByCodename(linkedItemCodename), Value = [Reference.ByCodename(contentItemToLinkCodename)] },
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

        // remove this later
        private async Task DeleteExistingScripted(string item, string page)
        {
            try
            {
                await _client.DeleteContentItemAsync(Reference.ByCodename(item));
                Console.WriteLine($"'{item}' deleted");
                await _client.DeleteContentItemAsync(Reference.ByCodename(page));
                Console.WriteLine($"'{page}' deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
