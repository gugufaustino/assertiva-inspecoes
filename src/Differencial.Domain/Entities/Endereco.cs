using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Resources;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Mvc;
using Differencial.Domain.EntitiesMetadata;

namespace Differencial.Domain.Entities
{
    [ModelMetadataType(typeof(EnderecoMetadata))]
	public class Endereco : IEntity
	{

        public Endereco()
        {
           // Operador = new HashSet<Operador>();
           
        }

		[Column("Id")]
		public int Id { get; set; }

        [Column("DtCadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("DtModificacao")]
        public DateTime DataModificacao { get; set; }

        [Column("IdOperadorCadastro")]
        public int IdOperadorCadastro { get; set; }

        [Column("IdOperadorModificacao")]
        public int IdOperadorModificacao { get; set; }

        [Column("Cep")]
		public string Cep { get; set; }

		[Column("Logradouro")]
		public string Logradouro { get; set; }

		[Column("Numero")]
		public int? Numero { get; set; }

		[Column("Complemento")]
		public string Complemento { get; set; }

		[Column("Bairro")]
		public string Bairro { get; set; }

		[Column("NomeMunicipio")]
		public string NomeMunicipio { get; set; }

		[Column("SiglaUf")]
		public string SiglaUf { get; set; }

		[Column("Latitude")]
		public double? Latitude { get; set; }

		[Column("Longitude")]
		public double? Longitude { get; set; } 

        #region "Relacionamentos"
       // public virtual ICollection<Operador> Operador { get; set; }

        #endregion "Relacionamentos"

        // Valida os dados da entidade
        public void Validate()
		{
			var validationResultsManager = new ValidationResultsManager();

            // Required 
             
            if (NomeMunicipio.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Endereço", "Nome Município");

            if (SiglaUf.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Endereço", "Sigla UF");

            if (Bairro.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Endereço", "Bairro");

            if (Logradouro.IsNullOrEmpty())
                validationResultsManager.AddValidationResultNotValid(MensagensValidacao.CampoRequeridoInvalido, "Endereço", "Logradouro");
 
            // Optional
            if (Cep.IsNullOrEmpty() == false && Cep.Length > 9)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "CEP", "9"));
			if(Logradouro.IsNullOrEmpty() == false && Logradouro.Length > 250)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Logradouro", "250"));
			if(Complemento.IsNullOrEmpty() == false && Complemento.Length > 80)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Complemento", "80"));
			if(Bairro.IsNullOrEmpty() == false && Bairro.Length > 80)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Bairro", "80"));
			if(NomeMunicipio.IsNullOrEmpty() == false && NomeMunicipio.Length > 80)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Nome Municipio", "80"));
			if(SiglaUf.IsNullOrEmpty() == false && SiglaUf.Length > 2)
                validationResultsManager.AddValidationResultNotValid(string.Format(MensagensValidacao.CampoTamanhoMaximoInvalido, "Sigla UF", "2"));

            if (validationResultsManager.HasError)
				validationResultsManager.ThrowException();
		}

		// Clona os dados da entidade
		public object Clone()
		{
			var entidade = new Endereco();
			entidade.Cep = this.Cep;
			entidade.Logradouro = this.Logradouro;
			entidade.Numero = this.Numero;
			entidade.Complemento = this.Complemento;
			entidade.Bairro = this.Bairro;
			entidade.NomeMunicipio = this.NomeMunicipio;
			entidade.SiglaUf = this.SiglaUf;
			entidade.Latitude = this.Latitude;
			entidade.Longitude = this.Longitude;
			return entidade;
		}
	}

	public enum CampoOrdenacaoEndereco
	{
		Cep,
		Logradouro,
		Numero,
		Bairro,
		NomeMunicipio,
		SiglaUf,
	}
}