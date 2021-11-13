using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.Entities.Models.InputModel
{
    public class ItemInputModel
    {
        [Required(ErrorMessage = "O valor da chave Codigo está ausente.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O valor da chave Descricao está ausente.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O valor da chave Quantidade está ausente.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O valor da chave Preco está ausente.")]
        public string Preco { get; set; }
    }
}
