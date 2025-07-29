using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.Beneficiarios.DaoBeneficiario cli = new DAL.Beneficiarios.DaoBeneficiario();
            return cli.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="nome">nome do beneficiário</param>
        /// <param name="cpf">cpf do beneficiário</param>
        /// <param name="id">id do beneficiário</param>
        public void Alterar(string nome, string cpf, long id)
        {
            DAL.Beneficiarios.DaoBeneficiario cli = new DAL.Beneficiarios.DaoBeneficiario();
            cli.Alterar(nome, cpf, id);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiário</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario cli = new DAL.Beneficiarios.DaoBeneficiario();
            cli.Excluir(id);
        }

        /// <summary>
        /// Verifica a existência de um beneficiário pelo CPF e ID do cliente
        /// </summary>
        /// <param name="cpf">cpf do beneficiário</param>
        /// <param name="idCliente">id do cliente</param>
        /// <returns></returns>
        public bool VerificarExistencia(string cpf, long idCliente)
        {
            DAL.Beneficiarios.DaoBeneficiario cli = new DAL.Beneficiarios.DaoBeneficiario();
            return cli.VerificarExistencia(cpf, idCliente);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiário</param>
        /// <returns></returns>
        public List<Beneficiario> ListarPorClienteId(long id)
        {
            DAL.Beneficiarios.DaoBeneficiario cli = new DAL.Beneficiarios.DaoBeneficiario();
            return cli.ListarPorClienteId(id);
        }
    }
}
