using System.ComponentModel.DataAnnotations;
using Transportadora.Tracking.CustomDataAnnotations;

namespace Transportadora.Tracking.Entities.Models.InputModel
{
    public class RemetenteInputModel
    {
        [Required(ErrorMessage = "O valor da chave RazaoSocial está ausente.")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "O valor da chave Empresa está ausente.")]
        public bool Empresa { get; set; }

        [Cnpj(ErrorMessage = "O CNPJ do remetente está inválido.")]
        public string Cnpj { get; set; }

        [Cpf(ErrorMessage = "O CPF do remetente está inválido.")]
        public string Cpf { get; set; }

        public EnderecoInputModel Endereco { get; set; }
    }
}
