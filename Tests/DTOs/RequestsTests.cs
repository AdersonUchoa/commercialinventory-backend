using Application.Requests.Produto;
using Bogus;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Tests.DTOs
{
    public class RequestsTests
    {
        private readonly Faker _faker;

        public RequestsTests()
        {
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void CreateProdutoRequest_WithNegativePrice_ShouldFail()
        {
            var request = new Faker<CreateProdutoRequest>("pt_BR")
                .RuleFor(r => r.Nome, f => f.Commerce.ProductName())
                .RuleFor(r => r.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(r => r.Preco, f => f.Random.Decimal(-100m, -1m))
                .RuleFor(r => r.CategoriaId, 1)
                .Generate();

            var validationContext = new ValidationContext(request);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);

            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);

            validationResults.Should().Contain(vr => vr.ErrorMessage == "O preço deve ser entre 0 e 99.999.999,99.");
        }
    }
}
