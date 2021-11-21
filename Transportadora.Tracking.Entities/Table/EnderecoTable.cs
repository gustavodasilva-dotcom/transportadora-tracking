using System;

namespace Transportadora.Tracking.Entities.Table
{
    public class EnderecoTable
    {
        public int EnderecoID { get; set; }

        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Pais { get; set; }

        public int Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
