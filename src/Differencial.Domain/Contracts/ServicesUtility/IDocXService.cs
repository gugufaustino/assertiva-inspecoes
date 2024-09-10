using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface IDocXService
    {
        string MontarQuadro(List<ArquivoAnexo> lstFotos, bool bNumSeqLegenda);


    }
}