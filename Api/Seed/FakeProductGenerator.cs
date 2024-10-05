using Api.Models;
using Bogus;

namespace Api.Seed
{
    public static class FakeProductGenerator
    {
        public static List<Product> GenerateProductList(int count = 10)
        {
            var categories = new[] { "Категория 1", "Категория 2", "Категория 3"};
            var specialTags = new[] { "Новинка", "Популярный", "Рекомендуемый"};

            return new Faker<Product>("ru")
                .RuleFor(x => x.Id, v => v.IndexFaker + 1)
                .RuleFor(x => x.Name, v => v.Commerce.ProductName())
                .RuleFor(x => x.Description, v => v.Lorem.Sentence())
                .RuleFor(x => x.Category, v => v.PickRandom(categories))
                .RuleFor(x => x.SpecialTag, v => v.PickRandom(specialTags))
                .RuleFor(x => x.Price, v => Math.Round(new Decimal(v.Random.Double(1, 10000)), 2))
                .RuleFor(x => x.Image, v => @"https://placehold.co/200")
                .Generate(count);
        }
    }
}