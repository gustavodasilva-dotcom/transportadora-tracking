using System.ComponentModel.DataAnnotations;
using Transportadora.Tracking.CustomDataAnnotations;

namespace Transportadora.Tracking.Entities.Models.InputModel
{
    public class EnderecoInputModel
    {
        [Cep(ErrorMessage = "O valor de Cep está inválido.")]
        [Required(ErrorMessage = "O valor da chave Cep está ausente.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O valor da chave Logradouro está ausente.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O valor da chave Numero está ausente.")]
        [Numerico(ErrorMessage = "O valor não corresponde a um valor númerico.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "O valor da chave Bairro está ausente.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O valor da chave Cidade está ausente.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O valor da chave Estado está ausente.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O valor da chave Pais está ausente.")]
        public string Pais { get; set; }
    }
}
