using Differencial.Domain.Contracts.Infra;
using System;
using System.Runtime.Caching;

namespace Differencial.Infra
{
    public class Cache : ICache
    {
        private  MemoryCache _cache;
        private  CacheItemPolicy _defaultPolicy;
        public Cache()
        {
            _cache = MemoryCache.Default;
            _defaultPolicy = new CacheItemPolicy();
            _defaultPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
        }

        public void Definir(string chave, object obj)
        {
            if (_cache.Contains(chave))
                _cache.Set(chave, obj, _defaultPolicy);
            else
                _cache.Add(chave, obj, _defaultPolicy);
        }
        public object Buscar(string chave) => _cache.Contains(chave) ? _cache[chave] : null;


    }
}
