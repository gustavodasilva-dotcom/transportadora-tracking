using System;
using System.Collections.Generic;

namespace Transportadora.Tracking.Entities.Models.ViewModel
{
    public class InfoOcorrenciaViewModel
    {
        public int PedidoID { get; set; }

        public string CodigoPedido { get; set; }

        public DateTime DataIntegracao { get; set; }

        public IndoPedidoViewModel InfoPedido { get; set; }

        public List<OcorrenciaViewModel> Ocorrencias { get; set; }
    }
}
