﻿using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (boCliente.VerificarExistencia(model.CPF))
                ModelState.AddModelError("CPF", "CPF já cadastrado!");

            List<BeneficiarioModel> beneficiarios = model.BeneficiariosJson is null ? new List<BeneficiarioModel>() : JsonConvert.DeserializeObject<List<BeneficiarioModel>>(model.BeneficiariosJson);

            foreach (var beneficiario in beneficiarios)
            {
                var validationContext = new ValidationContext(beneficiario);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(beneficiario, validationContext, validationResults, true))
                {
                    foreach (var validationResult in validationResults)
                    {
                        ModelState.AddModelError("Beneficiarios", "CPF do Beneficiário é inválido!");
                    }
                }
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            model.Id = boCliente.Incluir(new Cliente()
            {
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = model.CPF
            });

            foreach (var beneficiario in beneficiarios)
            {
                beneficiario.IdCliente = model.Id;

                boBeneficiario.Incluir(new Beneficiario
                {
                    Nome = beneficiario.Nome,
                    CPF = beneficiario.CPF,
                    IdCliente = model.Id
                });
            }

            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            List<BeneficiarioModel> beneficiarios = model.BeneficiariosJson is null ? new List<BeneficiarioModel>() : JsonConvert.DeserializeObject<List<BeneficiarioModel>>(model.BeneficiariosJson);

            foreach (var beneficiario in beneficiarios)
            {
                var validationContext = new ValidationContext(beneficiario);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(beneficiario, validationContext, validationResults, true))
                {
                    foreach (var validationResult in validationResults)
                    {
                        ModelState.AddModelError("Beneficiarios", "CPF do Beneficiário é inválido!");
                    }
                }
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            bo.Alterar(new Cliente()
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone,
                CPF = model.CPF
            });

            foreach (var beneficiario in beneficiarios)
            {
                if (beneficiario.IsDelete)
                {
                    boBeneficiario.Excluir(beneficiario.Id);
                    continue;
                }

                if (beneficiario.IsEdit)
                {
                    boBeneficiario.Alterar(beneficiario.Nome, beneficiario.CPF, beneficiario.Id);
                    continue;
                }

                beneficiario.IdCliente = model.Id;

                boBeneficiario.Incluir(new Beneficiario
                {
                    Nome = beneficiario.Nome,
                    CPF = beneficiario.CPF,
                    IdCliente = model.Id
                });
            }

            return Json("Cadastro alterado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            Cliente cliente = boCliente.Consultar(id);

            Models.ClienteModel model = null;

            if (cliente != null)
            {
                List<Beneficiario> beneficiarios = boBeneficiario.ListarPorClienteId(id);

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiarios.Select(b => new BeneficiarioModel
                    {
                        Id = b.Id,
                        Nome = b.Nome,
                        CPF = b.CPF,
                        IdCliente = b.IdCliente
                    }).ToList(),
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}