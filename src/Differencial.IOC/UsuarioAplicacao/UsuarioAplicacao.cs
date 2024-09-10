using AutoMapper;
using Differencial.Domain;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.DTO;
using Differencial.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
namespace Differencial.Infra
{

    public class UsuarioAplicacao : IUsuarioService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsuarioAplicacao(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => httpContextAccessor.HttpContext.User;

        private UsuarioLogadoDTO _usuarioLogado;
        public int Id => UsuarioAutenticado.Id;
        public string NomeOperador => UsuarioAutenticado.NomeOperador;


        public UsuarioLogadoDTO UsuarioAutenticado
        {
            get
            {
                if (Autenticado())
                {
                    if (_usuarioLogado == null)
                    {
                        var claimUsuario = User.Claims.First(i => i.Type == nameof(UsuarioLogadoDTO));
                        _usuarioLogado = JsonSerializer.Deserialize<UsuarioLogadoDTO>(claimUsuario.Value);
                    }
                    return _usuarioLogado;
                }
                else
                    return default;
            }
           
        }

        public bool Autenticado()
        {
            return User.Identity.IsAuthenticated;
        }

        public async Task Autenticar(Operador operador)
        {
            UsuarioLogadoDTO usuario = MontarUsuarioLogado(operador);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usuario.Id.ToString()));
            claims.Add(new Claim(nameof(UsuarioLogadoDTO), JsonSerializer.Serialize(usuario)));

            var authProperties = new AuthenticationProperties
            {
                // Refreshing the authentication session should be allowed.
                AllowRefresh = true,

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static UsuarioLogadoDTO MontarUsuarioLogado(Operador operador)
        {
            UsuarioLogadoDTO usuario = Mapper.Map<UsuarioLogadoDTO>(operador);

            usuario.NomeSeguradoraSolicitante = operador.IndSolicitante ? operador.Solicitante.Seguradora.NomeSeguradora : string.Empty;
            usuario.TipoPapel = new List<TipoPapelEnum>();

            if (usuario.IndGerente)
                usuario.TipoPapel.Add(TipoPapelEnum.Gerente);

            if (usuario.IndSolicitante)
                usuario.TipoPapel.Add(TipoPapelEnum.Solicitante);

            if (usuario.IndVistoriador)
                usuario.TipoPapel.Add(TipoPapelEnum.Vistoriador);

            if (usuario.IndAnalista)
                usuario.TipoPapel.Add(TipoPapelEnum.Analista);

            if (usuario.IndFinanceiro)
                usuario.TipoPapel.Add(TipoPapelEnum.Financeiro);

            if (usuario.IndAssessor)
                usuario.TipoPapel.Add(TipoPapelEnum.Assessor);

            if (usuario.IndUsuarioSistema)
                usuario.TipoPapel.Add(TipoPapelEnum.UsuarioSistema);

            return usuario;
        }

        public async Task Remover()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }

}
