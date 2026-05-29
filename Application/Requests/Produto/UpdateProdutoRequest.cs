using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Produto
{
    public class UpdateProdutoRequest
    {
        [MinLength(5, ErrorMessage = "O nome deve conter no mínimo 5 caracteres.")]
        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        [Range(typeof(decimal), "0", "99999999.99", ErrorMessage = "O preço deve ser entre 0 e 99.999.999,99.")]
        public decimal? Preco { get; set; }

        public int? CategoriaId { get; set; }
    }
}
