using Application.Interfaces;
using Application.Requests.Produto;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/produtos")]
    public class ProdutoController : BaseController
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CreateProdutoRequest request, CancellationToken cancellationToken)
        {
            var result = await _produtoService.AddAsync(request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateProdutoRequest request, CancellationToken cancellationToken)
        {
            var result = await _produtoService.UpdateAsync(id, request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _produtoService.DeleteAsync(id, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync([FromQuery] GetProdutosRequest request, CancellationToken cancellationToken)
        {
            var result = await _produtoService.GetAllAsync(request, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _produtoService.GetByIdAsync(id, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
