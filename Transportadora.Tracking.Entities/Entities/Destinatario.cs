namespace Transportadora.Tracking.Entities.Entities
{
    public class Destinatario
    {
        public string RazaoSocial { get; set; }

        public bool Empresa { get; set; }

        public string Cnpj { get; set; }

        public string Cpf { get; set; }

        public Endereco Endereco { get; set; }
    }
}
