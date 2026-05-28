using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Produto
{
    public class UpdateProdutoRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(5, ErrorMessage = "O nome deve conter no mínimo 5 caracteres.")]
        public string Nome { get; set; } = null!;

        public string? Descricao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo.")]
        public decimal? Preco { get; set; }

        public int? CategoriaId { get; set; }
    }
}
