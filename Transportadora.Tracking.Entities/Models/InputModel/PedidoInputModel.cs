using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.Entities.Models.InputModel
{
    public class PedidoInputModel
    {
        [Required(ErrorMessage = "O valor da chave CodigoPedido está ausente.")]
        public string CodigoPedido { get; set; }

        public RemetenteInputModel Remetente { get; set; }

        public DestinatarioInputModel Destintario { get; set; }

        public List<ItemInputModel> Items { get; set; }
    }
}
