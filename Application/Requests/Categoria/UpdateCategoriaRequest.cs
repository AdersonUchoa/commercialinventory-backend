using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Categoria
{
    public class UpdateCategoriaRequest
    {
        [MinLength(5, ErrorMessage = "O nome deve conter no mínimo 5 caracteres.")]
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
    }
}
