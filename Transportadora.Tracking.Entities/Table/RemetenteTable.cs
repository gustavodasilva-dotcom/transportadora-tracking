using System;

namespace Transportadora.Tracking.Entities.Table
{
    public class RemetenteTable
    {
        public int RemetenteID { get; set; }

        public string RazaoSocial { get; set; }

        public int Empresa { get; set; }

        public string Cnpj { get; set; }

        public string Cpf { get; set; }

        public int EnderecoID { get; set; }

        public int Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
