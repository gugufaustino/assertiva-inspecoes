using Differencial.Domain.Contracts.Entities;
using Differencial.Domain.Entities;
using Differencial.Domain.Filters;
using System.Collections.Generic;

namespace Differencial.Domain.Contracts.Services
{
    public interface ILogAuditoriaService

	{
		IEnumerable<LogAuditoria> Listar(int IdTabela, IEntity entidade );

        IEnumerable<LogAuditoria> Listar(List<IEntity> entidades);

        IEnumerable<LogAuditoria> ListarDapper(List<IEntity> entidades);

        void Salvar(int codigoUsuarioLogado, LogAuditoria entidade);

		void Excluir(int codigoUsuarioLogado, int id);

        List<LogAuditoria> ListarTodosEF();

        List<LogAuditoria> ListarTodosDP();

        IEnumerable<LogAuditoria> Listar(LogAuditoriaFilter filtro);

        IEnumerable<LogAuditoria> ListarDP(LogAuditoriaFilter filtro);
    }
}