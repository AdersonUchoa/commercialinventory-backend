using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Categoria
{
    public class UpdateCategoriaRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MinLength(5, ErrorMessage = "O nome deve conter no mínimo 5 caracteres.")]
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
    }
}
