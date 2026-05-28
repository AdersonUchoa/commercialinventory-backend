using Application.Interfaces;
using Application.Requests.Categoria;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categorias")]
    public class CategoriaController : BaseController
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CreateCategoriaRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoriaService.AddAsync(request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateCategoriaRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoriaService.UpdateAsync(id, request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _categoriaService.DeleteAsync(id, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync([FromQuery] GetCategoriasRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoriaService.GetAllAsync(request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _categoriaService.GetByIdAsync(id, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
