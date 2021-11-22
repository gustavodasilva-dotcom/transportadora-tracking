using System.Collections.Generic;

namespace Transportadora.Tracking.Entities.Models.ViewModel
{
    public class PedidoViewModel
    {
        public int PedidoID { get; set; }

        public string CodigoPedido { get; set; }

        public string DataIntegracao { get; set; }

        public RemetenteViewModel Remetente { get; set; }

        public DestinatarioViewModel Destintario { get; set; }

        public List<ItemViewModel> Items { get; set; }
    }
}
