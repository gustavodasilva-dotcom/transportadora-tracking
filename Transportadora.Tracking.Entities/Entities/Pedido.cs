using System.Collections.Generic;

namespace Transportadora.Tracking.Entities.Entities
{
    public class Pedido
    {
        public string CodigoPedido { get; set; }

        public Remetente Remetente { get; set; }

        public Destinatario Destintario { get; set; }

        public List<Item> Items { get; set; }
    }
}
