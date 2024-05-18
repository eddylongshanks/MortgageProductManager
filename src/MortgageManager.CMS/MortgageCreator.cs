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

namespace MortgageManager.CMS
{
    public class MortgageCreator
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
            Guid itemId;
            itemId = await CreateProduct(product);

            //var identifier = new LanguageVariantIdentifier(Reference.ById(itemId), Reference.ByCodename("default"));
            var identifier = new LanguageVariantIdentifier(Reference.ByCodename(product.Name?.Replace(" ", "_").Replace("-", "_").ToLower()), Reference.ByCodename("default"));

            await UpdateProductDetails(identifier, product);

            return true;
        }

        private async Task UpdateProductDetails(LanguageVariantIdentifier identifier, Product product)
        {
            var variantResponse = await _client.UpsertLanguageVariantAsync(identifier, 
                new LanguageVariantUpsertModel()
                {
                    Elements =
                        [
                            new TextElement { Element=Reference.ByCodename("product_code"), Value = "CHRIS_SCRIPT1"},
                            new TextElement { Element=Reference.ByCodename("name"), Value = product.Name},
                            new TextElement { Element=Reference.ByCodename("initial_interest_rate"), Value = "99"},
                            new TextElement { Element=Reference.ByCodename("standard_variable_rate"), Value = "98"},
                            new TextElement { Element=Reference.ByCodename("comparison_cost"), Value = "95"},
                            new TextElement { Element=Reference.ByCodename("maximum_ltv____"), Value = "97"},
                            new TextElement { Element=Reference.ByCodename("fees"), Value = "96"},
                            
                        ]
                },
                new WorkflowStepIdentifier(Reference.ByCodename("default"), Reference.ByCodename("draft")));
        }

        private async Task<Guid> CreateProduct(Product product)
        {
            var response = await _client.CreateContentItemAsync(new ContentItemCreateModel
            {
                Name = product.Name,
                Codename = product.Name?.Replace(" ", "_").Replace("-", "_").ToLower(),
                Type = Reference.ByCodename("product_mortgage"),
                Collection = Reference.ByCodename("default"),
            });

            return response.Id;
        }
    }
}
