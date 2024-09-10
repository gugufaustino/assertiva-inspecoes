using Differencial.Repository.Context;
using System;

namespace Differencial.Repository
{
    public class ConfigurationUtil
    {
        public void seed(ref DifferencialContext context)
        {
            try
            {
                
                seedStoredProcedures(ref context);
                seedTriggers(ref context);
                seedFunctions(ref context);
                
            }
            catch /*(DbEntityValidationException dbEx)*/
            {
                //foreach (var validationErrors in dbEx.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        Trace.TraceInformation("Entity:{0} Property: {1} Error: {2}",
                //                                validationErrors.Entry.Entity.GetType().FullName,
                //                                validationError.PropertyName,
                //                                validationError.ErrorMessage);
                //    }
                //}

                throw;
            }
        }

        #region Métodos auxiliares

        /// <summary>
        /// Método para execução das triggers necessárias
        /// </summary>
        /// <param name="context">O context do DB</param>
        private void seedTriggers(ref DifferencialContext context)
        {
            //context.Database.ExecuteSqlCommand(BancoDados.DroparTriggersTable);
            //context.Database.ExecuteSqlCommand(BancoDados.VerificacaoCriacaoTriggerAuditoria);
        }

        /// <summary>
        /// Método para execução das procedures necessárias
        /// </summary>
        /// <param name="context">O context do DB</param>
        private void seedStoredProcedures(ref DifferencialContext context)
        {
            //context.Database.ExecuteSqlCommand(VerificarExistenciaObjeto(BancoDados.NomeProcedureSetUserContext));
            //context.Database.ExecuteSqlCommand(BancoDados.sp_setusercontext);
        }

        /// <summary>
        /// Método para verificação se a procedure existe
        /// </summary>
        /// <param name="nomeObjeto">O nome da procedure/trigger</param>
        /// <param name="trigger">Se o objeto a ser dropado é uma trigger</param>
        /// <returns>O sql para verificação da existência da procedure</returns>
        private static string VerificarExistenciaObjeto(string nomeObjeto, string tipo = "P")
        {
            string sql = "IF object_id(\'" + nomeObjeto + "\') IS NOT NULL " + Environment.NewLine;
            if (tipo == "T")
                sql += "EXEC (\'DROP TRIGGER " + nomeObjeto + "\')";
            else if (tipo == "P")
                sql += "EXEC (\'DROP PROCEDURE " + nomeObjeto + "\')";
            else if (tipo == "F")
                sql += "EXEC (\'DROP FUNCTION " + nomeObjeto + "\')";
            return sql;
        }

        /// <summary>
        /// Método para execução de function
        /// </summary>
        /// <param name="context">O context do DB</param>
        private void seedFunctions(ref DifferencialContext context)
        {
            //context.Database.ExecuteSqlCommand(VerificarExistenciaObjeto(BancoDados.NomefnVistoriadorDistancia, "F"));
            //context.Database.ExecuteSqlCommand(VerificarExistenciaObjeto(BancoDados.NomefnCalcDistancia, "F"));

            //context.Database.ExecuteSqlCommand(BancoDados.fnVistoriadorDistancia);
            //context.Database.ExecuteSqlCommand(BancoDados.fnCalcDistancia);
        }

        #endregion Métodos auxiliares
    }
}
