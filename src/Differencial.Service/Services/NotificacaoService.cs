using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain.Contracts.Services;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using Differencial.Domain.UOW;
using System.Collections.Generic;
using Differencial.Domain.Contracts.Entities;
using System;
using Differencial.Domain;
using Differencial.Domain.Util.ExtensionMethods;
using Differencial.Domain.Annotation;
using System.Linq;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.DTO;

namespace Differencial.Service.Services
{
    public class NotificacaoService : Service, INotificacaoService
    {
        INotificacaoRepository _notificacaoRepositorio;
        private IOperadorService _operadorService;
        private INotificacaoOperadorService _notificacaoOperadorService;
        private IUsuarioService _usuarioService;

        public NotificacaoService(IUnitOfWork uow, IUsuarioService usuarioService,
            INotificacaoRepository notificacaoRepositorio,
            INotificacaoOperadorService notificacaoOperadorService,
            IOperadorService operadorService)
            : base(uow)
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _operadorService = operadorService;
            _notificacaoOperadorService = notificacaoOperadorService;
            _usuarioService = usuarioService;
        }

        public IEnumerable<Notificacao> Listar(NotificacaoFilter filtro)
        {
            return TryCatch(() =>
            {
                return _notificacaoRepositorio.Where(filtro);
            });
        }

        public void Salvar(Notificacao entidade)
        {
            TryCatch(() =>
            {
                entidade.Validate();

                if (entidade.Id == 0)
                    _notificacaoRepositorio.Add(entidade);
                else
                    _notificacaoRepositorio.Update(entidade);
            });
        }

        public void Excluir(int id)
        {
            TryCatch(() =>
            {
                _notificacaoRepositorio.Delete(id);
            });
        }

        public void SalvarTransmitirNotificacaoProcesso(INotificacaoProcesso processo)
        {
            string grupoNotificacao = string.Empty;
            Notificacao notificacao;
            List<int> idsOperador;
            switch (processo.TpSituacao)
            {
                case TipoSituacaoProcessoEnum.DevolvidoParaGerencia:
                    grupoNotificacao = TipoPapelEnum.Gerente.ToString();
                    idsOperador = _operadorService.Listar(new OperadorFilter { IndAtivo = true, IndGerente = true }).Select(i => i.Id).ToList();
                    notificacao = new Notificacao
                    {
                        Descricao = "Pedido {0}".Formata(processo.TpSituacao.GetAttributeOfType<SituacaoProcessoAttribute>().Name.ToLower()),
                        IdSolicitacao = processo.Id,
                    };
                    break;

                default:
                    throw new NotImplementedException();

            }
            PrepararNotificacaoOperador(notificacao, idsOperador);
            _notificacaoOperadorService.Salvar(notificacao.NotificacaoOperador.ToList());
            Salvar(notificacao);

            //transmitir
            NotificationBroadcast.Instance.NovaNotificacaoGroup(new NotificacaoDTO
            {
                mensagem = notificacao.Descricao,
                cod = notificacao.IdSolicitacao,
                indLido = false,
                data = notificacao.DataCadastro
            }, grupoNotificacao);
        }

        private void PrepararNotificacaoOperador(Notificacao notificacao, List<int> IdsOperador)
        {
            var lstNotificacaoOperador = IdsOperador.Select(idOp => new NotificacaoOperador
            {
                IdOperador = idOp,
                IdNotificacao = notificacao.Id,
                Notificacao = notificacao,
                IndLido = false
            }).ToList();

            notificacao.NotificacaoOperador = lstNotificacaoOperador;
        }

        public List<NotificacaoOperador> MinhasNotificacoes()
        {
            return TryCatch(() =>
            {
                return _notificacaoOperadorService.MinhasNotificacoes(_usuarioService.Id).ToList();
            });
        }

        public void ExcluirTodasMinhas()
        {
            TryCatch(() =>
            {
                foreach (var notifi in MinhasNotificacoes())
                {
                    _notificacaoOperadorService.Excluir(notifi.Id);
                }
                
            });
        }
    }
}