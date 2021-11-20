namespace Transportadora.Tracking.Entities.Models.ViewModel
{
    public class DestinatarioViewModel
    {
        public string RazaoSocial { get; set; }
        
        public bool Empresa { get; set; }
        
        public string Cnpj { get; set; }

        public string Cpf { get; set; }

        public EnderecoViewModel Endereco { get; set; }
    }
}
