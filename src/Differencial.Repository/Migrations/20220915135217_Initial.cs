using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Differencial.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalistaProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    QtdPontuacao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalistaProduto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    CpfCnpj = table.Column<string>(type: "varchar(14)", unicode: false, maxLength: 14, nullable: true),
                    NomeRazaoSocial = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AtividadeNome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ContatoNome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ContatoTelefone = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    ContatoOutro = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ContatoAgendamento = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    Cep = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: true),
                    Logradouro = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Numero = table.Column<int>(type: "int", nullable: true),
                    Complemento = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    NomeMunicipio = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    SiglaUf = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Laudo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laudo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoAssunto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    NomeAssunto = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    TextoPadrao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoAssunto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoInspecao",
                columns: table => new
                {
                    IdTipoInspecao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    NomeTipoInspecao = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    DescricaoTipoInspecao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoInspecao", x => x.IdTipoInspecao);
                });

            migrationBuilder.CreateTable(
                name: "ClienteEndereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEndereco = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteEndereco", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClienteEndereco_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClienteEndereco_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    NomeOperador = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Telefone = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    UrlFoto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Cpf = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Rg = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IndAnalista = table.Column<bool>(type: "bit", nullable: false),
                    IndGerente = table.Column<bool>(type: "bit", nullable: false),
                    IndVistoriador = table.Column<bool>(type: "bit", nullable: false),
                    IndSolicitante = table.Column<bool>(type: "bit", nullable: false),
                    IndFinanceiro = table.Column<bool>(type: "bit", nullable: false),
                    IndAssessor = table.Column<bool>(type: "bit", nullable: false),
                    IdEndereco = table.Column<int>(type: "int", nullable: true),
                    IndAcessoSistema = table.Column<bool>(type: "bit", nullable: false),
                    IndPrimeiroAcesso = table.Column<bool>(type: "bit", nullable: false),
                    Senha = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    IndUsuarioSistema = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operador_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seguradora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    NomeSeguradora = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Cnpj = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: true),
                    Inscricao = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    RazaoSocial = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    IdEndereco = table.Column<int>(type: "int", nullable: true),
                    ContabilInspecoesDiaInicio = table.Column<int>(type: "int", nullable: false),
                    ContabilInspecoesDiaFim = table.Column<int>(type: "int", nullable: false),
                    ContabilInspetorDia = table.Column<int>(type: "int", nullable: false),
                    ContabilEmpresaDia = table.Column<int>(type: "int", nullable: false),
                    IndIntegracaoSolicitacaoPorEmail = table.Column<bool>(type: "bit", nullable: false),
                    EmailRemetenteSolicitacao = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    IndAgendaRepostaPorEmail = table.Column<bool>(type: "bit", nullable: false),
                    IndLaudoRepostaPorEmail = table.Column<bool>(type: "bit", nullable: false),
                    QtdQuilometroFranquia = table.Column<int>(type: "int", nullable: false),
                    VlrQuilometroExcedente = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguradora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguradora_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificacaoOperador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndLido = table.Column<bool>(type: "bit", nullable: false),
                    IdNotificacao = table.Column<int>(type: "int", nullable: false),
                    IdOperador = table.Column<int>(type: "int", nullable: false),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacaoOperador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificacaoOperador_Notificacao_IdNotificacao",
                        column: x => x.IdNotificacao,
                        principalTable: "Notificacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Analista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analista_Operador_Id",
                        column: x => x.Id,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogAuditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tabela = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    IdTabela = table.Column<int>(type: "int", nullable: false),
                    XMLDadosAnterior = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    XMLDadosPosterior = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    UsuarioAplicacao = table.Column<int>(type: "int", nullable: false),
                    Acao = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    DataAcao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioBanco = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAuditoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogAuditoria_Operador_UsuarioAplicacao",
                        column: x => x.UsuarioAplicacao,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vistoriador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdOperador = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    IdEnderecoBase = table.Column<int>(type: "int", nullable: false),
                    IndEnderecoBaseIgual = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vistoriador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vistoriador_Endereco_IdEnderecoBase",
                        column: x => x.IdEnderecoBase,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vistoriador_Operador_Id",
                        column: x => x.Id,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Filial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSeguradora = table.Column<int>(type: "int", nullable: false),
                    NomeFilial = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filial_Seguradora_IdSeguradora",
                        column: x => x.IdSeguradora,
                        principalTable: "Seguradora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    IdSeguradora = table.Column<int>(type: "int", nullable: false),
                    IdTipoInspecao = table.Column<int>(type: "int", nullable: false),
                    NomeProduto = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IndFranquiaQuilometragem = table.Column<bool>(type: "bit", nullable: false),
                    QtdFranquiaQuilometragem = table.Column<int>(type: "int", nullable: true),
                    VlrDespesa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrReceber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrQuilometragem = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndBlocoExtra = table.Column<bool>(type: "bit", nullable: false),
                    VlrBlocoExtr = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndQuilometragemVariavel = table.Column<bool>(type: "bit", nullable: false),
                    QtdQuilometragemFinal = table.Column<int>(type: "int", nullable: true),
                    QtdQuilometragemInicial = table.Column<int>(type: "int", nullable: true),
                    VlrQuilometragemReceber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndMetragemVariavel = table.Column<bool>(type: "bit", nullable: false),
                    QtdMetragemM2Inicial = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QtdMetragemM2Final = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrMetragemReceber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CodProdutoSeguradora = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    NomeProdutoSeguradora = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Seguradora_IdSeguradora",
                        column: x => x.IdSeguradora,
                        principalTable: "Seguradora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produto_TipoInspecao_IdTipoInspecao",
                        column: x => x.IdTipoInspecao,
                        principalTable: "TipoInspecao",
                        principalColumn: "IdTipoInspecao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdOperador = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    TipoSolicitante = table.Column<int>(type: "int", nullable: false),
                    IdSeguradora = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitante_Operador_Id",
                        column: x => x.Id,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitante_Seguradora_IdSeguradora",
                        column: x => x.IdSeguradora,
                        principalTable: "Seguradora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contrato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contrato_Produto_Id",
                        column: x => x.Id,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VistoriadorProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IndAtivo = table.Column<bool>(type: "bit", nullable: false),
                    IdVistoriador = table.Column<int>(type: "int", nullable: false),
                    IdProduto = table.Column<int>(type: "int", nullable: false),
                    IdContratoLancamento = table.Column<int>(type: "int", nullable: false),
                    IdContratoLancamentoValor = table.Column<int>(type: "int", nullable: false),
                    VlrPagamentoVistoria = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrQuilometroRodado = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VistoriadorProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VistoriadorProduto_Produto_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VistoriadorProduto_Vistoriador_IdVistoriador",
                        column: x => x.IdVistoriador,
                        principalTable: "Vistoriador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSeguradora = table.Column<int>(type: "int", nullable: false),
                    IdProduto = table.Column<int>(type: "int", nullable: false),
                    IdContratoLancamentoValor = table.Column<int>(type: "int", nullable: true),
                    IdVistoriador = table.Column<int>(type: "int", nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    IdEnderecoCliente = table.Column<int>(type: "int", nullable: false),
                    CodSeguradora = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CodSistemaLegado = table.Column<int>(type: "int", nullable: false),
                    TpSituacao = table.Column<int>(type: "int", nullable: true),
                    IdAnalista = table.Column<int>(type: "int", nullable: true),
                    TxtInformacoesAdicionais = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    IdSolicitante = table.Column<int>(type: "int", nullable: true),
                    CorretorNome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    CorretorTelefone = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    TxtJustificativaVistoriadorDefinido = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    VlrPagamentoVistoria = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrQuilometroRodado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoRotaVistoriaPrevista = table.Column<int>(type: "int", nullable: true),
                    DeslocamentoPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoDeslocamentoPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoTotalPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoRotaVistoriaRealizada = table.Column<int>(type: "int", nullable: true),
                    DeslocamentoRealizado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoDeslocamentoRealizado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoTotalRealizado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoDeslocamentoAcordado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrPagamentoVistoriaAcordado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustoTotalAcordado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SolicitanteNome = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    SolicitanteTelefone = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    SolicitanteEmail = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    VistoriadorCidadeBase = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TxtJustificativaDeslocamentoRealizado = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    IndCustoVistoriaAcordado = table.Column<bool>(type: "bit", nullable: false),
                    DthVistoriaRealizada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TxtJustificativaAnalistaDefinido = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    IndRelacionamentoAgendaInformada = table.Column<bool>(type: "bit", nullable: false),
                    NomeOperadorAgendaInformada = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    DthRelacionamentoAgendaInformada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoNotificacaoAgendaInformada = table.Column<int>(type: "int", nullable: true),
                    AreaContruida = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BlocoContruido = table.Column<int>(type: "int", nullable: true),
                    CasaConstruida = table.Column<int>(type: "int", nullable: true),
                    VlrRiscoSegurado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VlrHonorarioPreAcordo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QtdEquipamento = table.Column<int>(type: "int", nullable: true),
                    IndRelatorioExigenciaMelhoria = table.Column<bool>(type: "bit", nullable: false),
                    IndRotaDeVolta = table.Column<bool>(type: "bit", nullable: false),
                    IndUrgente = table.Column<bool>(type: "bit", nullable: false),
                    IdFilial = table.Column<int>(type: "int", nullable: true),
                    IdSolicitacaoOrigemReinspecao = table.Column<int>(type: "int", nullable: true),
                    ControleDthEmailCobrancaVistoria = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdOperadorApropriado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Analista_IdAnalista",
                        column: x => x.IdAnalista,
                        principalTable: "Analista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Endereco_IdEnderecoCliente",
                        column: x => x.IdEnderecoCliente,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Filial_IdFilial",
                        column: x => x.IdFilial,
                        principalTable: "Filial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Operador_IdOperadorApropriado",
                        column: x => x.IdOperadorApropriado,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Operador_IdOperadorCadastro",
                        column: x => x.IdOperadorCadastro,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Operador_IdOperadorModificacao",
                        column: x => x.IdOperadorModificacao,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Produto_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Seguradora_IdSeguradora",
                        column: x => x.IdSeguradora,
                        principalTable: "Seguradora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Solicitante_IdSolicitante",
                        column: x => x.IdSolicitante,
                        principalTable: "Solicitante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacao_Vistoriador_IdVistoriador",
                        column: x => x.IdVistoriador,
                        principalTable: "Vistoriador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoLancamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdContrato = table.Column<int>(type: "int", nullable: false),
                    TipoContratoLancamento = table.Column<int>(type: "int", nullable: false),
                    TipoParametroQuantitativoVariavel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoLancamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoLancamento_Contrato_IdContrato",
                        column: x => x.IdContrato,
                        principalTable: "Contrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agendamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false),
                    IdVistoriador = table.Column<int>(type: "int", nullable: false),
                    DthAgendamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IndCancelado = table.Column<bool>(type: "bit", nullable: false),
                    TipoAgendamento = table.Column<int>(type: "int", nullable: false),
                    MotivoCancelamentoReagendamento = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agendamento_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agendamento_Vistoriador_IdVistoriador",
                        column: x => x.IdVistoriador,
                        principalTable: "Vistoriador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtividadeProcesso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    NomeAtividadeProcesso = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false),
                    IdOperadorConcluida = table.Column<int>(type: "int", nullable: true),
                    TipoAtividade = table.Column<int>(type: "int", nullable: false),
                    TipoSituacaoAtividade = table.Column<int>(type: "int", nullable: false),
                    DthAssinada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DthDelegada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DthConcluida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IndRetrabalho = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeProcesso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadeProcesso_Operador_IdOperadorConcluida",
                        column: x => x.IdOperadorConcluida,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AtividadeProcesso_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cobertura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    NomeCobertura = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    VlrCobertura = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobertura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cobertura_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comunicacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdTipoAssunto = table.Column<int>(type: "int", nullable: true),
                    TipoComunicacao = table.Column<int>(type: "int", nullable: false),
                    Assunto = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    TextoComunicacao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunicacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comunicacao_Operador_IdOperadorCadastro",
                        column: x => x.IdOperadorCadastro,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comunicacao_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Foto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false),
                    GuidArquivo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArquivoDataModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArquivoNome = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    ArquivoExtencao = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    ArquivoTamanho = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true),
                    ArquivoAnexoPosicao = table.Column<int>(type: "int", nullable: false),
                    IndExcluida = table.Column<bool>(type: "bit", nullable: false),
                    TipoArquivoAnexo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foto_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LancamentoFinanceiroTotal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false),
                    TipoLancamentoFinanceiro = table.Column<int>(type: "int", nullable: false),
                    ValorLancamentoFinanceiroTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DthLancamentoPagamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoFinanceiroTotal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentoFinanceiroTotal_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacaoProcesso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    TextoMovimentacao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    TipoSituacaoProcesso = table.Column<int>(type: "int", nullable: false),
                    IdOperadorOrigem = table.Column<int>(type: "int", nullable: false),
                    IdOperadorDestino = table.Column<int>(type: "int", nullable: true),
                    DthMovimentacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DthApropriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DthConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TipoSituacaoMovimento = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacaoProcesso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacaoProcesso_Operador_IdOperadorDestino",
                        column: x => x.IdOperadorDestino,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacaoProcesso_Operador_IdOperadorOrigem",
                        column: x => x.IdOperadorOrigem,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimentacaoProcesso_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoLancamentoValor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdContratoLancamento = table.Column<int>(type: "int", nullable: false),
                    TipoQuantitativoVariacao = table.Column<int>(type: "int", nullable: false),
                    QuantitativoA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuantitativoB = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorLancamento = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorLancamentoQuantitativo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndPreAcordo = table.Column<bool>(type: "bit", nullable: false),
                    SiglaUf = table.Column<string>(type: "varchar(1)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoLancamentoValor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoLancamentoValor_ContratoLancamento_IdContratoLancamento",
                        column: x => x.IdContratoLancamento,
                        principalTable: "ContratoLancamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaudoFoto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdLaudo = table.Column<int>(type: "int", nullable: true),
                    QuadroFotosPosicao = table.Column<int>(type: "int", nullable: false),
                    IndQuadroFoto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaudoFoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaudoFoto_Foto_Id",
                        column: x => x.Id,
                        principalTable: "Foto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaudoFoto_Laudo_IdLaudo",
                        column: x => x.IdLaudo,
                        principalTable: "Laudo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LancamentoFinanceiro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DtCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdOperadorCadastro = table.Column<int>(type: "int", nullable: false),
                    IdOperadorModificacao = table.Column<int>(type: "int", nullable: false),
                    IdSolicitacao = table.Column<int>(type: "int", nullable: false),
                    TipoLancamentoFinanceiro = table.Column<int>(type: "int", nullable: false),
                    ValorLancamentoFinanceiro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescricaoLancamentoFinanceiro = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IdLancamentoFinanceiroTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoFinanceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentoFinanceiro_LancamentoFinanceiroTotal_IdLancamentoFinanceiroTotal",
                        column: x => x.IdLancamentoFinanceiroTotal,
                        principalTable: "LancamentoFinanceiroTotal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoFinanceiro_Operador_IdOperadorCadastro",
                        column: x => x.IdOperadorCadastro,
                        principalTable: "Operador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LancamentoFinanceiro_Solicitacao_IdSolicitacao",
                        column: x => x.IdSolicitacao,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamento_IdSolicitacao",
                table: "Agendamento",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamento_IdVistoriador",
                table: "Agendamento",
                column: "IdVistoriador");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeProcesso_IdOperadorConcluida",
                table: "AtividadeProcesso",
                column: "IdOperadorConcluida");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeProcesso_IdSolicitacao",
                table: "AtividadeProcesso",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteEndereco_IdCliente",
                table: "ClienteEndereco",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteEndereco_IdEndereco",
                table: "ClienteEndereco",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_Cobertura_IdSolicitacao",
                table: "Cobertura",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicacao_IdOperadorCadastro",
                table: "Comunicacao",
                column: "IdOperadorCadastro");

            migrationBuilder.CreateIndex(
                name: "IX_Comunicacao_IdSolicitacao",
                table: "Comunicacao",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoLancamento_IdContrato",
                table: "ContratoLancamento",
                column: "IdContrato");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoLancamentoValor_IdContratoLancamento",
                table: "ContratoLancamentoValor",
                column: "IdContratoLancamento");

            migrationBuilder.CreateIndex(
                name: "IX_Filial_IdSeguradora",
                table: "Filial",
                column: "IdSeguradora");

            migrationBuilder.CreateIndex(
                name: "IX_Foto_IdSolicitacao",
                table: "Foto",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFinanceiro_IdLancamentoFinanceiroTotal",
                table: "LancamentoFinanceiro",
                column: "IdLancamentoFinanceiroTotal");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFinanceiro_IdOperadorCadastro",
                table: "LancamentoFinanceiro",
                column: "IdOperadorCadastro");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFinanceiro_IdSolicitacao",
                table: "LancamentoFinanceiro",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoFinanceiroTotal_IdSolicitacao",
                table: "LancamentoFinanceiroTotal",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_LaudoFoto_IdLaudo",
                table: "LaudoFoto",
                column: "IdLaudo");

            migrationBuilder.CreateIndex(
                name: "IX_LogAuditoria_UsuarioAplicacao",
                table: "LogAuditoria",
                column: "UsuarioAplicacao");

            migrationBuilder.CreateIndex(
                name: "IX_Tabela",
                table: "LogAuditoria",
                column: "Tabela");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoProcesso_IdOperadorDestino",
                table: "MovimentacaoProcesso",
                column: "IdOperadorDestino");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoProcesso_IdOperadorOrigem",
                table: "MovimentacaoProcesso",
                column: "IdOperadorOrigem");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoProcesso_IdSolicitacao",
                table: "MovimentacaoProcesso",
                column: "IdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacaoOperador_IdNotificacao",
                table: "NotificacaoOperador",
                column: "IdNotificacao");

            migrationBuilder.CreateIndex(
                name: "IX_Operador_IdEndereco",
                table: "Operador",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_IdSeguradora",
                table: "Produto",
                column: "IdSeguradora");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_IdTipoInspecao",
                table: "Produto",
                column: "IdTipoInspecao");

            migrationBuilder.CreateIndex(
                name: "IX_Seguradora_IdEndereco",
                table: "Seguradora",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdAnalista",
                table: "Solicitacao",
                column: "IdAnalista");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdCliente",
                table: "Solicitacao",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdEnderecoCliente",
                table: "Solicitacao",
                column: "IdEnderecoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdFilial",
                table: "Solicitacao",
                column: "IdFilial");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdOperadorApropriado",
                table: "Solicitacao",
                column: "IdOperadorApropriado");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdOperadorCadastro",
                table: "Solicitacao",
                column: "IdOperadorCadastro");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdOperadorModificacao",
                table: "Solicitacao",
                column: "IdOperadorModificacao");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdProduto",
                table: "Solicitacao",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdSeguradora",
                table: "Solicitacao",
                column: "IdSeguradora");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdSolicitante",
                table: "Solicitacao",
                column: "IdSolicitante");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_IdVistoriador",
                table: "Solicitacao",
                column: "IdVistoriador");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitante_IdSeguradora",
                table: "Solicitante",
                column: "IdSeguradora");

            migrationBuilder.CreateIndex(
                name: "IX_Vistoriador_IdEnderecoBase",
                table: "Vistoriador",
                column: "IdEnderecoBase");

            migrationBuilder.CreateIndex(
                name: "IX_VistoriadorProduto_IdProduto",
                table: "VistoriadorProduto",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "UK_VistoriadorProduto",
                table: "VistoriadorProduto",
                columns: new[] { "IdVistoriador", "IdProduto", "IdContratoLancamento", "IdContratoLancamentoValor" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendamento");

            migrationBuilder.DropTable(
                name: "AnalistaProduto");

            migrationBuilder.DropTable(
                name: "AtividadeProcesso");

            migrationBuilder.DropTable(
                name: "ClienteEndereco");

            migrationBuilder.DropTable(
                name: "Cobertura");

            migrationBuilder.DropTable(
                name: "Comunicacao");

            migrationBuilder.DropTable(
                name: "ContratoLancamentoValor");

            migrationBuilder.DropTable(
                name: "LancamentoFinanceiro");

            migrationBuilder.DropTable(
                name: "LaudoFoto");

            migrationBuilder.DropTable(
                name: "LogAuditoria");

            migrationBuilder.DropTable(
                name: "MovimentacaoProcesso");

            migrationBuilder.DropTable(
                name: "NotificacaoOperador");

            migrationBuilder.DropTable(
                name: "TipoAssunto");

            migrationBuilder.DropTable(
                name: "VistoriadorProduto");

            migrationBuilder.DropTable(
                name: "ContratoLancamento");

            migrationBuilder.DropTable(
                name: "LancamentoFinanceiroTotal");

            migrationBuilder.DropTable(
                name: "Foto");

            migrationBuilder.DropTable(
                name: "Laudo");

            migrationBuilder.DropTable(
                name: "Notificacao");

            migrationBuilder.DropTable(
                name: "Contrato");

            migrationBuilder.DropTable(
                name: "Solicitacao");

            migrationBuilder.DropTable(
                name: "Analista");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Filial");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Solicitante");

            migrationBuilder.DropTable(
                name: "Vistoriador");

            migrationBuilder.DropTable(
                name: "TipoInspecao");

            migrationBuilder.DropTable(
                name: "Seguradora");

            migrationBuilder.DropTable(
                name: "Operador");

            migrationBuilder.DropTable(
                name: "Endereco");
        }
    }
}
