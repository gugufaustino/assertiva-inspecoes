using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differencial.Repository.Migrations
{
    public class InsertUsuarioSistema 
    {
        //public override void Up()
        //{
        //    // Para atender tanto criação de novo banco quanto ambeinte já existente
        //    // Operador Id=1 será sempre o sistema
        //    Sql(@"IF (SELECT Id FROM Operador WHERE Id = 1 ) IS NULL
        //          BEGIN; 
        //            SET IDENTITY_INSERT [dbo].[Operador] ON; 
        //            BEGIN TRANSACTION;
        //            INSERT INTO [dbo].[Operador]([Id], [DtCadastro], [IdOperadorCadastro], [DtModificacao], [IdOperadorModificacao], [NomeOperador], [Email], [IndUsuarioSistema], [IndAcessoSistema], [IndAtivo], [IndPrimeiroAcesso], [Senha], Cpf, Rg, DataNascimento, IndAnalista,IndGerente,IndVistoriador,IndSolicitante,IndFinanceiro)
        //            SELECT 1, '20180101 00:00', 1, '20180101 00:00', 1, N'Usuário Sistema', N'sistema.differencial@gmail.com', 1, 1, 1, 0, N'aGZtpOOr2Kw=', N'', N'', N'', 0,0,0,0,0
        //            COMMIT; RAISERROR (N'[dbo].[Operador]: Inserido Usuário Sistema', 10, 1) WITH NOWAIT;
        //            SET IDENTITY_INSERT [dbo].[Operador] OFF;
        //          END;
        //        ");
        //    /*[Senha=inicial]*/
        //}

        //public override void Down()
        //{
        //}
    }
}
