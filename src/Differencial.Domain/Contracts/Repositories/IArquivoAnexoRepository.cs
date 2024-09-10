using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Repositories
{
	public interface IArquivoAnexoRepository : IRepository<ArquivoAnexo>
	{
        
        IEnumerable<ArquivoAnexo> ListarArquivoSolicitacao(int idSolicitacao, TipoArquivoAnexoEnum tipoArquivoAnexoEnum);
      
    }
}