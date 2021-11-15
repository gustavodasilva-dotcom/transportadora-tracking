using System;

namespace Transportadora.Tracking.Entities.Entities
{
    public class Ocorrencia
    {
        public string CodigoPedido { get; set; }

        public int PedidoId { get; set; }

        public int OcorrenciaId { get; set; }

        public int CodigoOcorrencia { get; set; }

        public DateTime DataOcorrencia { get; set; }
    }
}
