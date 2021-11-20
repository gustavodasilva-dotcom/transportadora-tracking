using System;

namespace Transportadora.Tracking.Entities.Table
{
    public class ItemsTable
    {
        public int ItemID { get; set; }

        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public int Quantidade { get; set; }

        public double Preco { get; set; }

        public int Ativo { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
