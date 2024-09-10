using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Domain.Contracts.Infra
{
    public interface ICache 
    {
        void Definir(string chave, object obj);

        object Buscar(string chave);
    }
}
