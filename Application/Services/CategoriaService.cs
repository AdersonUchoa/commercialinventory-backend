using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Categoria;
using Application.Responses;
using Application.Responses.Categoria;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.SeedWorks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriaService> _logger;

        public CategoriaService(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork, ILogger<CategoriaService> logger, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CategoriaResponse>> AddAsync(CreateCategoriaRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(request);

                var created = await _categoriaRepository.AddAsync(categoria, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CategoriaResponse>(created);

                return new ApiResponse<CategoriaResponse>(true, HttpStatusCode.Created, response, "Categoria criada com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Categoria");
                return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<CategoriaResponse>> UpdateAsync(int id, UpdateCategoriaRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id, cancellationToken);
                if (categoria == null) return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.NotFound, null, "Categoria não encontrada.", null);

                categoria.Update(request.Nome, request.Descricao);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<CategoriaResponse>(categoria);

                return new ApiResponse<CategoriaResponse>(true, HttpStatusCode.OK, response, "Categoria atualizada com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Categoria with id {Id}", id);
                return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var hasProducts = await _categoriaRepository.HasProductsAsync(id, cancellationToken);
                if (hasProducts) return new ApiResponse<bool>(false, HttpStatusCode.Conflict, false, "Não é possível excluir uma categoria que possua produtos vinculados.", null);

                var deleted = await _categoriaRepository.DeleteAsync(id, cancellationToken);
                if (!deleted) return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Categoria não encontrada.", null);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, true, "Categoria deletada com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Categoria with id {Id}", id);
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, false, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<CategoriaByIdResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdNoTrackingAsync(id, cancellationToken);
                if (categoria == null) return new ApiResponse<CategoriaByIdResponse>(false, HttpStatusCode.NotFound, null, "Categoria não encontrada.", null);

                var response = _mapper.Map<CategoriaByIdResponse>(categoria);

                return new ApiResponse<CategoriaByIdResponse>(true, HttpStatusCode.OK, response, "Categoria encontrada com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Categoria with id {Id}", id);
                return new ApiResponse<CategoriaByIdResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }

        public async Task<ApiResponse<PaginatedResult<CategoriaResponse>>> GetAllAsync(GetCategoriasRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(request.SearchName))
                    request.SearchName = request.SearchName.Trim();

                var (categorias, total) = await _categoriaRepository.GetAllAsync(request.Page, request.Limit, request.SearchName, cancellationToken);

                var response = _mapper.Map<IReadOnlyList<CategoriaResponse>>(categorias);

                var paginatedResult = new PaginatedResult<CategoriaResponse>(response, total, request.Page, request.Limit);

                return new ApiResponse<PaginatedResult<CategoriaResponse>>(true, HttpStatusCode.OK, paginatedResult, "Categorias recuperadas com sucesso.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categorias");
                return new ApiResponse<PaginatedResult<CategoriaResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", null);
            }
        }
    }
}
