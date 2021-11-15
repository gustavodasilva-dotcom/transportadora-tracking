using System.ComponentModel.DataAnnotations;
using Transportadora.Tracking.CustomDataAnnotations;

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
        [Preco(ErrorMessage = "O valor informado não pode ser considerado um preço.")]
        public string Preco { get; set; }
    }
}
