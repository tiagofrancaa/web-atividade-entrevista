using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF do beneficiário
        /// </summary>
        [Required]
        [Cpf(ErrorMessage = "CPF inválido.")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome do beneficiário
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Identificador do Cliente
        /// </summary>
        public long IdCliente { get; set; }

        public bool IsEdit { get; set; } = false;

        public bool IsDelete { get; set; } = false;
    }
}