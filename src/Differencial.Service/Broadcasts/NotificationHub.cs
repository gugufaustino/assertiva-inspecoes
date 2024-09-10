using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Differencial.Domain.Contracts.Infra;
using Differencial.Domain.DTO;
using Differencial.Domain.Contracts.Repositories;
using Differencial.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;

namespace Differencial.Service
{
    public class NotificationHub : Hub
    {
        private readonly IOperadorRepository _repositorio;
        private readonly IUsuarioService _usuarioService;
        public NotificationHub(IOperadorRepository operadorRepository, 
                                IHttpContextAccessor httpContextAccessor,
                                IUsuarioService usuarioService)
        {
            _repositorio = operadorRepository;
            _usuarioService = usuarioService;
        }

        private readonly static ConnectionMapping<NotificationClient> _connections = new();

        public override Task OnConnectedAsync()
        {
            
            var operador = GetOperador(_usuarioService);

            _connections.Add(operador, Context.ConnectionId);
            JoinGroups(operador);
            return base.OnConnectedAsync();
        }
   

        public override Task OnDisconnectedAsync(Exception stopCalled)
        {
            var opClient = GetOperador(_usuarioService);

            _connections.Remove(opClient, Context.ConnectionId);

            return base.OnDisconnectedAsync(stopCalled);
        }

        //public override Task OnReconnected()
        //{
        //    var opClient = GetOperador(Context.Request.QueryString["IdOperador"]);

        //    if (!_connections.GetConnections(opClient).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(opClient, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}

        private NotificationClient GetOperador(IUsuarioService usuarioService)
        {
            var op = usuarioService.UsuarioAutenticado;
            var opCliente = new NotificationClient
            {
                IdOperador = op.Id,
                Name = op.NomeOperador,
            };

            // Cria um grupo exclusivo/privado somente com operador
            opCliente.Groups.Add(opCliente.IdOperador.ToString()); 

            if (op.IndGerente) opCliente.Groups.Add(TipoPapelEnum.Gerente.ToString());
            if (op.IndAssessor) opCliente.Groups.Add(TipoPapelEnum.Assessor.ToString());
            if (op.IndAnalista) opCliente.Groups.Add(TipoPapelEnum.Analista.ToString());
            if (op.IndVistoriador) opCliente.Groups.Add(TipoPapelEnum.Vistoriador.ToString());
            if (op.IndFinanceiro) opCliente.Groups.Add(TipoPapelEnum.Financeiro.ToString());
            if (op.IndSolicitante) opCliente.Groups.Add(TipoPapelEnum.Solicitante.ToString());


            return opCliente;
        }

        private void JoinGroups(NotificationClient operador)
        {
            var connIds = _connections.GetConnections(operador);
            foreach (var groupName in operador.Groups)
            {
                foreach (var connectionId in connIds)
                {
                    Groups.AddToGroupAsync(connectionId, groupName);
                }
            }
        }

        private void LeaveGroup(string groupName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

    }
}