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
using MortgageManager.CMS.Helpers;

namespace MortgageManager.CMS
{
    public class MortgageCreatorConsole()
    {
        //private IConfiguration _config = new ConfigurationBuilder()
        //    .AddUserSecrets<Settings>()
        //    .Build();

        private IProductMortgageMapper _mortgageMapper = new ProductMortgageMapper();
        private static readonly Credentials _credentials = new();
        private readonly ManagementClient _client = new ManagementClient(new ManagementOptions
        {
            EnvironmentId = _credentials.EnvironmentId,
            ApiKey = _credentials.ApiKey,
        });

        public async Task<bool> UploadMortgageProduct(Product product)
        {
            IProductMortgage productMortgage = _mortgageMapper.Map(product);

            bool productPageUpdated = false;
            bool productUpdated = false;

            await DeleteExistingScripted(productMortgage); //temporary

            if (await ItemExists(productMortgage.PageCodename) || await ItemExists(productMortgage.Codename))
                return false;            

            bool productPageCreated = await CreateProductPage(productMortgage);
            bool productCreated = await CreateProduct(productMortgage);
            
            if (productPageCreated && productCreated) 
            {
                productPageUpdated = await CreateProductLink(productMortgage.PageCodename, productMortgage.Codename, "product");
                productUpdated = await CreateProductLink(productMortgage.Codename, productMortgage.PageCodename, "details_page");
            }

            return productPageUpdated && productUpdated;
        }


        private async Task<bool> CreateProduct(IProductMortgage productMortgage)
        {   
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = $"Product - Mortgage - {productMortgage.ProductCode} - {productMortgage.Name}",
                Codename = productMortgage.Codename,
                Type = Reference.ByCodename("product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });

            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productMortgage.Codename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements = BuildElementList(productMortgage),
                    Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
                });
            }

            return response != null;
        }

        private async Task<bool> CreateProductPage(IProductMortgage productMortgage)
        {
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = $"Page - Mortgage - {productMortgage.ProductCode} - {productMortgage.Name}",
                Codename = productMortgage.PageCodename,
                Type = Reference.ByCodename("web_product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });

            if (response != null)
            {
                var variantResponse = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(productMortgage.PageCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
                {
                    Elements =
                    [
                        new TextElement { Element = Reference.ByCodename("title"), Value = productMortgage.Name },
                    ],
                    Workflow = new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("scripted"))
                });
            }

            return response != null;
        }

        private async Task<bool> CreateProductLink(string targetCodename, string contentItemToLinkCodename, string linkedItemCodename)
        {
            var response = await _client.UpsertLanguageVariantAsync(new LanguageVariantIdentifier(Reference.ByCodename(targetCodename), Reference.ByCodename("default")), new LanguageVariantUpsertModel()
            {
                Elements =
                [
                    new LinkedItemsElement { Element = Reference.ByCodename(linkedItemCodename), Value = [Reference.ByCodename(contentItemToLinkCodename)] },
                ],
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
            catch (ManagementException ex)
            {
                throw new Exception($"There was an unexpected error: {ex.Message}");
            }
        }

        protected IEnumerable<dynamic> BuildElementList(IProductMortgage productMortgage)
        {
            var elementsToSend = new List<dynamic>()
            {
                productMortgage.GetMultipleChoiceElementFor(nameof(productMortgage.ClientType)),
                productMortgage.GetTextElementFor(nameof(productMortgage.ComparisonCost)),
                productMortgage.GetMultipleChoiceElementFor(nameof(productMortgage.DealTerm)),
                productMortgage.GetTextElementFor(nameof(productMortgage.Fees)),
                productMortgage.GetTextElementFor(nameof(productMortgage.InitialInterestRate)),
                productMortgage.GetTextElementFor(nameof(productMortgage.MaximumLtv)),
                productMortgage.GetMultipleChoiceElementFor(nameof(productMortgage.MortgageTypes)),
                productMortgage.GetTextElementFor(nameof(productMortgage.Name)),
                productMortgage.GetTextElementFor(nameof(productMortgage.ProductCode)),
                productMortgage.GetMultipleChoiceElementFor(nameof(productMortgage.RateType)),
                productMortgage.GetTextElementFor(nameof(productMortgage.StandardVariableRate)),
            };

            if (productMortgage.MaturityDate != null)
                elementsToSend.Add(productMortgage.GetDateTimeElementForMaturityDate());

            return elementsToSend.AsEnumerable();
        }

        // remove this later
        private async Task DeleteExistingScripted(IProductMortgage productMortgage)
        {
            try
            {
                await _client.DeleteContentItemAsync(Reference.ByCodename(productMortgage.Codename));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Deleted Product: '{productMortgage.Codename}'");

                await _client.DeleteContentItemAsync(Reference.ByCodename(productMortgage.PageCodename));
                Console.WriteLine($"Deleted Product page: '{productMortgage.PageCodename}'");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
