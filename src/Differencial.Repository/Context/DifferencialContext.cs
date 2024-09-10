using Differencial.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Differencial.Repository.Context
{

    public class DifferencialContext : DbContext, IDifferencialContext
    {
        #region Adicionar entidades
        public DbSet<Seguradora> Seguradora { get; set; }
        public DbSet<LogAuditoria> LogAuditoria { get; set; }
        public DbSet<TipoInspecao> TipoInspecao { get; set; }
        public DbSet<Operador> Operador { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Vistoriador> Vistoriador { get; set; }
        public DbSet<VistoriadorProduto> VistoriadorProduto { get; set; }
        public DbSet<Solicitacao> Solicitacao { get; set; }
        public DbSet<Agendamento> Agendamento { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<ClienteEndereco> ClienteEndereco { get; set; }
        public DbSet<ArquivoAnexo> Foto { get; set; }
        public DbSet<Laudo> Laudo { get; set; }
        public DbSet<LaudoFoto> LaudoFoto { get; set; }
        public DbSet<Cobertura> Cobertura { get; set; }
        public DbSet<MovimentacaoProcesso> MovimentacaoProcesso { get; set; }
        public DbSet<AtividadeProcesso> AtividadeProcesso { get; set; }
        public DbSet<Solicitante> Solicitante { get; set; }
        public DbSet<LancamentoFinanceiro> LancamentoFinanceiro { get; set; }
        public DbSet<LancamentoFinanceiroTotal> LancamentoFinanceiroTotal { get; set; }
        public DbSet<Analista> Analista { get; set; }
        public DbSet<AnalistaProduto> AnalistaProduto { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<ContratoLancamento> ContratoLancamento { get; set; }
        public DbSet<ContratoLancamentoValor> ContratoLancamentoValor { get; set; }
        public DbSet<Comunicacao> Comunicacao { get; set; }
        public DbSet<TipoAssunto> TipoAssunto { get; set; }
        public DbSet<Filial> Filial { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }
        public DbSet<NotificacaoOperador> NotificacaoOperador { get; set; }

        #endregion Adicionar entidades

        public DifferencialContext(DbContextOptions<DifferencialContext> options)
            : base(options)
        {

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DifferencialContext, Migrations.Configuration>());
            //Database.SetInitializer(new DifferencialUpdateDatabase());
            //Database.SetInitializer(new DifferencialInitializer());
            //Database.Initialize(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //// modelBuilder.Conventions.Add(new FunctionConvention()); 
            //// Adiciona todos os tipos complexos usados ​​pelas funções.
            //modelBuilder.Conventions.Add(new CodeFirstStoreFunctions.FunctionsConvention<DifferencialContext>("dbo"));
            //modelBuilder.ComplexType<OperadorDistancia>();

            //aplica padrão para entidades nao mapeadas
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()))
            {
                if (property.ClrType == typeof(string))
                    property.SetColumnType("varchar(1)");

                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    property.SetColumnType("decimal(18,2)");
            }

            // Adicionar Mappings
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DifferencialContext).Assembly);

            //apos mapeamento, substitui as mapeadas nvarchar para varchar
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties()))
            {
                if (property.ClrType == typeof(string))
                    property.SetIsUnicode(false);

            }

            //Impedindo exclusão em cascata
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);

        }
        public void SaveChanges(int usuarioaplicacao)
        {
            SetUserContext(usuarioaplicacao);
            this.SaveChanges();
        }

        #region Métodos auxiliares

        private void SetUserContext(int usuarioaplicacao)
        {
            // Database.ExecuteSqlCommand("EXEC " + BancoDados.NomeProcedureSetUserContext + "'" + usuarioaplicacao + "'");
        }

        #endregion

        #region Functio 
        //// Install-Package EntityFramework.Functions -DependencyVersion Highest
        //[Function(FunctionType.TableValuedFunction, "FunctionTable", "CodeFirstStoreFunctions", Schema = "dbo")]
        //public IQueryable<OperadorDistancia> FunctionTable([Parameter(DbType = "float", Name = "param1")]double latIni,
        //                                                    [Parameter(DbType = "float", Name = "param2")]double lonIni,
        //                                                    [Parameter(DbType = "int", Name = "param3")]int idProtuto)
        //{ 
        //    ObjectParameter paramLat = new ObjectParameter("param1", typeof(float)) { Value = latIni } ;
        //    ObjectParameter paramLong = new ObjectParameter("param2", typeof(float)) { Value = lonIni };
        //    ObjectParameter paramProduto = new ObjectParameter("param3", typeof(int)) { Value = idProtuto };
        //    //  return this.ObjectContext().CreateQuery<Distancia>($"[dbo].[{nameof(this.fnCalcDistancia)}](@{nameof(latIni)}, @{nameof(lonIni)}, @{nameof(latFim)}, @{nameof(lonFim)})", latIniParameter, lonIniParameter, latFimIdParameter, lonFimIdParameter);

        //    var obj =  this.ObjectContext().CreateQuery<OperadorDistancia>("FunctionTable(@param1, @param2, @param3)", paramLat, paramLong, paramProduto);

        //    return obj;

        //}

        #endregion
    }

}