using System;

namespace Transportadora.Tracking.Entities.Models.InputModel
{
    public class OcorrenciaInputModel
    {
        public string CodigoPedido { get; set; }

        public int CodigoOcorrencia { get; set; }

        public DateTime DataOcorrencia { get; set; }
    }
}
