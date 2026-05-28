using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Produto;
using Application.Responses;
using Application.Responses.Produto;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.SeedWorks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutoService> _logger;

        public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork, ILogger<ProdutoService> logger, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProdutoResponse>> AddAsync(CreateProdutoRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var categoria = await _categoriaRepository.ExistsAsync(request.CategoriaId, cancellationToken);
                if (!categoria) return new ApiResponse<ProdutoResponse>(false, HttpStatusCode.BadRequest, null, "Categoria informada não existe.", null);

                var produto = _mapper.Map<Produto>(request);

                var created = _produtoRepository.Add(produto);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<ProdutoResponse>(created);

                return new ApiResponse<ProdutoResponse>(true, HttpStatusCode.Created, response, "Produto criado com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Produto");
                return new ApiResponse<ProdutoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<ProdutoResponse>> UpdateAsync(int id, UpdateProdutoRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var produto = await _produtoRepository.GetByIdAsync(id, cancellationToken);
                if (produto == null) return new ApiResponse<ProdutoResponse>(false, HttpStatusCode.NotFound, null, "Produto não encontrado.", null);

                var categoria = await _categoriaRepository.ExistsAsync(request.CategoriaId, cancellationToken);
                if (!categoria) return new ApiResponse<ProdutoResponse>(false, HttpStatusCode.BadRequest, null, "Categoria informada não existe.", null);

                produto.Update(request.Nome, request.Descricao, request.Preco, request.CategoriaId);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<ProdutoResponse>(produto);

                return new ApiResponse<ProdutoResponse>(true, HttpStatusCode.OK, response, "Produto atualizado com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Produto");
                return new ApiResponse<ProdutoResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await _produtoRepository.DeleteAsync(id, cancellationToken);
                if (!deleted) return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Produto não encontrado.", null);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, true, "Produto deletado com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Produto with id {Id}", id);
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, false, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<ProdutoByIdResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var produto = await _produtoRepository.GetByIdNoTrackingAsync(id, cancellationToken);
                if (produto == null) return new ApiResponse<ProdutoByIdResponse>(false, HttpStatusCode.NotFound, null, "Produto não encontrado.", null);

                var response = _mapper.Map<ProdutoByIdResponse>(produto);

                return new ApiResponse<ProdutoByIdResponse>(true, HttpStatusCode.OK, response, "Produto encontrado com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Produto with id {Id}", id);
                return new ApiResponse<ProdutoByIdResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<PaginatedResult<ProdutoResponse>>> GetAllAsync(GetProdutosRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.SearchName))
                    request.SearchName = request.SearchName.Trim();

                var (produtos, total) = await _produtoRepository.GetAllAsync(request.Page, request.Limit, request.SearchName, cancellationToken);

                var response = _mapper.Map<IReadOnlyList<ProdutoResponse>>(produtos);
                
                var paginatedResult = new PaginatedResult<ProdutoResponse>(response, total, request.Page, request.Limit);
                
                return new ApiResponse<PaginatedResult<ProdutoResponse>>(true, HttpStatusCode.OK, paginatedResult, "Produtos listados com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Produtos");
                return new ApiResponse<PaginatedResult<ProdutoResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }
    }
}
