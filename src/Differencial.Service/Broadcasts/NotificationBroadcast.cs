using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Differencial.Domain.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Differencial.Service
{
    public class NotificationBroadcast
    {

        private static readonly IServiceProvider _requestServices = new HttpContextAccessor().HttpContext.RequestServices;

        // Singleton instance
        private readonly static Lazy<NotificationBroadcast> _instance = new Lazy<NotificationBroadcast>(
            () => new NotificationBroadcast((IHubContext<NotificationHub>)_requestServices.GetService(typeof(IHubContext<NotificationHub>))));
        //IHubContext<NotificationHub>
        //<NotificationHub>

        private IHubContext<NotificationHub> _context;
        public NotificationBroadcast(IHubContext<NotificationHub> context)
        {
            _context = context;
        }

        public static NotificationBroadcast Instance => _instance.Value;

        public void NovaNotificacaoTodos(NotificacaoDTO notificacao)
        {
            _context.Clients.All.SendAsync("novaNotificacao", objToJSON(notificacao));
        }
        public void NovaNotificacaoGroup(NotificacaoDTO notificacao, string groupName)
        {
            _context.Clients.Group(groupName).SendAsync("novaNotificacao", objToJSON(notificacao));
        }

        public void NovaNotificacaoCliente(NotificacaoDTO notificacao, int idOperador)
        {
            _context.Clients.Group(idOperador.ToString()).SendAsync("novaNotificacao", objToJSON(notificacao));
        }

        private string objToJSON(object objData)
        {
            using (var sw = new System.IO.StringWriter())
            {
                JsonSerializer.Create(new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Culture = System.Globalization.CultureInfo.CurrentCulture

                }).Serialize(sw, objData);
                return sw.ToString();
            }
        }

    }
}