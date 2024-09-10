using Differencial.Domain.Filters;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Differencial.Repository.Util
{
    public static class FiltroGenerico<T>
        where T : class, new()
    {
        /// <summary>
        /// Método genérico para consultas de datas
        ///
        /// Exemplos:
        ///    Em campos da mesma entidade-> FiltroGenerico<Entidade>.AplicarFiltroData(ref query, "Campo", typeof(DateTime), filter.Campo);
        ///    Em campos de Entidade relacionada-> FiltroGenerico<Entidade>.AplicarFiltroData(ref query, "EntidadeRelacionada.Campo", typeof(DateTime?), filter.Campo);
        ///    Em campos de Lista relacionada à entidade-> FiltroGenerico<Entidade>.AplicarFiltroData(ref query, "ListaRelacionada", typeof(DateTime), filter.Campo, true, "CampoDaLista")
        ///
        /// </summary>
        /// <param name="query">O objeto iQueryable de consulta</param>
        /// <param name="filter">O objeto de filtro</param>
        /// <param name="nomeCampo">O nome do campo a ser consultado, sendo campo de uma lista este se torna o nome da lista relacionada</param>
        /// <param name="tipo">O tipo correspondente ao campo(DateTime ou DateTime?)</param>
        /// <param name="filtroGrade">O valor a ser consultado</param>
        /// <param name="isLista">Boleano para saber se é filtro que será aplicado em uma lista relacionada à entidade</param>
        /// <param name="nomeCampoLista">Sendo um filtro de lista este parâmetro será usado para saber qual o campo da lista relacionada</param>
        public static void AplicarFiltroData(ref System.Linq.IQueryable<T> query, string nomeCampo, Type tipo, FiltroGrade filtroGrade = null, bool isLista = false, string nomeCampoLista = "")
        {
            var retorno = "";

            var underlyingType = Nullable.GetUnderlyingType(tipo);
            object valorFiltro = Convert.ChangeType(filtroGrade.Valor, underlyingType ?? tipo);

            if (tipo == typeof(DateTime))
                valorFiltro = Convert.ToDateTime(filtroGrade.Valor).Date;
            else if (tipo == typeof(DateTime?))
                valorFiltro = (DateTime?)Convert.ToDateTime(filtroGrade.Valor).Date;

            switch (filtroGrade.Tipo)
            {
                case FiltroTipo.Entre:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} >= @0 && {1} < @1)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} >= @0 && {0} < @1", nomeCampo);

                        object valorFiltro2 = Convert.ChangeType(filtroGrade.Valor2, underlyingType ?? tipo);
                        if (tipo == typeof(DateTime))
                            valorFiltro2 = Convert.ToDateTime(filtroGrade.Valor2).Date;
                        else if (tipo == typeof(DateTime?))
                            valorFiltro2 = (DateTime?)Convert.ToDateTime(filtroGrade.Valor2).Date;

                        query = query.Where(retorno, valorFiltro, ((DateTime)valorFiltro2).AddDays(1));
                    } break;
                case FiltroTipo.MaiorQue:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} > @0)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} > @0", nomeCampo);

                        query = query.Where(retorno, ((DateTime)valorFiltro).AddDays(1).AddMinutes(-1)).AsQueryable();
                    } break;
                case FiltroTipo.MenorQue:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} < @0)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} < @0", nomeCampo);

                        query = query.Where(retorno, valorFiltro).AsQueryable();
                    } break;
                case FiltroTipo.MaiorIgualQue:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} >= @0)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} >= @0", nomeCampo);

                        query = query.Where(retorno, valorFiltro).AsQueryable();
                    } break;
                case FiltroTipo.MenorIgualQue:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} <= @0)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} <= @0", nomeCampo);

                        query = query.Where(retorno, valorFiltro).AsQueryable();
                    } break;
                default:
                    {
                        if (isLista)
                            retorno = string.Format("{0}.Any({1} >= @0 && {1} < @1)", nomeCampo, nomeCampoLista);
                        else
                            retorno = string.Format("{0} >= @0 && {0} < @1", nomeCampo);

                        query = query.Where(retorno, valorFiltro, ((DateTime)valorFiltro).AddDays(1)).AsQueryable();
                    } break;
            }
            query.AsEnumerable<T>();
        }
    }
}