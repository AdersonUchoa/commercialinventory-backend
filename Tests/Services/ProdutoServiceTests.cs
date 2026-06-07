using Application.Requests.Produto;
using Application.Responses.Categoria;
using Application.Responses.Produto;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.SeedWorks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Tests.Services
{
    public class ProdutoServiceTests
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProdutoService>> _loggerMock;
        private readonly ProdutoService _produtoService;
        private readonly Faker _faker;

        public ProdutoServiceTests()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProdutoService>>();
            _faker = new Faker("pt_BR");

            _produtoService = new ProdutoService(
                _produtoRepositoryMock.Object,
                _categoriaRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task AddAsync_QuandoCategoriaNaoExistir_RetornarBadRequest()
        {
            var request = new Faker<CreateProdutoRequest>("pt_BR")
                .RuleFor(r => r.Nome, f => f.Commerce.ProductName())
                .RuleFor(r => r.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(r => r.Preco, f => f.Random.Decimal(1, 100))
                .RuleFor(r => r.CategoriaId, 1)
                .Generate();

            _categoriaRepositoryMock
                .Setup(repo => repo.ExistsAsync(request.CategoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _produtoService.AddAsync(request);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Categoria informada não existe.");
            result.Data.Should().BeNull();

            _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ComDadosValidados_RetornarCreated()
        {
            var request = new Faker<CreateProdutoRequest>("pt_BR")
                .RuleFor(r => r.Nome, f => f.Commerce.ProductName())
                .RuleFor(r => r.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(r => r.Preco, f => f.Random.Decimal(1, 100))
                .RuleFor(r => r.CategoriaId, 1)
                .Generate();

            var produto = new Produto
            {
                Id = 1,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                CategoriaId = request.CategoriaId
            };

            var categoria = new Categoria
            {
                Id = request.CategoriaId,
                Nome = _faker.Commerce.Categories(1).First(),
                Descricao = _faker.Lorem.Sentence()
            };

            var categoriaResponse = new CategoriaResponse
            (
                categoria.Id,
                categoria.Nome,
                categoria.Descricao
            );

            var respostaEsperada = new ProdutoResponse(
                1,
                request.Nome,
                request.Descricao,
                request.Preco,
                categoriaResponse
            );

            _categoriaRepositoryMock
                .Setup(r => r.ExistsAsync(request.CategoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<Produto>(request)).Returns(produto);
            _mapperMock.Setup(m => m.Map<ProdutoResponse>(produto)).Returns(respostaEsperada);

            _produtoRepositoryMock
                .Setup(repo => repo.AddAsync(produto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(produto);

            var result = await _produtoService.AddAsync(request);

            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Message.Should().Be("Produto criado com sucesso.");
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(respostaEsperada);

            _produtoRepositoryMock.Verify(repo => repo.AddAsync(produto, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_QuandoOcorreExcecao_RetornarInternalServerError()
        {
            var request = new Faker<CreateProdutoRequest>("pt_BR")
                .RuleFor(r => r.Nome, f => f.Commerce.ProductName())
                .RuleFor(r => r.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(r => r.Preco, f => f.Random.Decimal(1, 100))
                .RuleFor(r => r.CategoriaId, 1)
                .Generate();

            _categoriaRepositoryMock
                .Setup(repo => repo.ExistsAsync(request.CategoriaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<Produto>(request)).Throws(new Exception("Erro ao mapear Produto"));

            var result = await _produtoService.AddAsync(request);

            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Erro interno do servidor. Tente novamente mais tarde.");
            result.Data.Should().BeNull();
        }
    }
}