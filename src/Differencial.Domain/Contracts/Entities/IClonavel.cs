using System;

namespace Differencial.Domain.Contracts.Entities
{
    public interface IClonavel<T> : ICloneable
    { 
        /// <summary>
        ///  Implementa operação de clonagem executa cópia em profundidade(deep copy)
        /// </summary>
        /// <returns></returns>
        T DeepClone();

        /// <summary>
        /// Implementa uma cópia superficial(shallow copy)
        /// </summary>
        /// <returns></returns>
        new object Clone();
    }
}
