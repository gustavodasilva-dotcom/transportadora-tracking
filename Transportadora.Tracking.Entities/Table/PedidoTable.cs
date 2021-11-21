using System;

namespace Transportadora.Tracking.Entities.Table
{
    public class PedidoTable
    {
        public int PedidoID { get; set; }

        public string CodigoPedido { get; set; }

        public int RemetenteID { get; set; }

        public int DestinatarioID { get; set; }

        public int Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
