 
-- GO
-- /****** Object:  Database [differencial]    Script Date: 09/09/2024 22:46:51 ******/

-- GO
-- ALTER DATABASE [differencial] SET COMPATIBILITY_LEVEL = 130
-- GO
-- IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
-- begin
-- EXEC [differencial].[dbo].[sp_fulltext_database] @action = 'enable'
-- end
-- GO
-- ALTER DATABASE [differencial] SET ANSI_NULL_DEFAULT OFF 
-- GO
-- ALTER DATABASE [differencial] SET ANSI_NULLS OFF 
-- GO
-- ALTER DATABASE [differencial] SET ANSI_PADDING OFF 
-- GO
-- ALTER DATABASE [differencial] SET ANSI_WARNINGS OFF 
-- GO
-- ALTER DATABASE [differencial] SET ARITHABORT OFF 
-- GO
-- ALTER DATABASE [differencial] SET AUTO_CLOSE ON 
-- GO
-- ALTER DATABASE [differencial] SET AUTO_SHRINK OFF 
-- GO
-- ALTER DATABASE [differencial] SET AUTO_UPDATE_STATISTICS ON 
-- GO
-- ALTER DATABASE [differencial] SET CURSOR_CLOSE_ON_COMMIT OFF 
-- GO
-- ALTER DATABASE [differencial] SET CURSOR_DEFAULT  GLOBAL 
-- GO
-- ALTER DATABASE [differencial] SET CONCAT_NULL_YIELDS_NULL OFF 
-- GO
-- ALTER DATABASE [differencial] SET NUMERIC_ROUNDABORT OFF 
-- GO
-- ALTER DATABASE [differencial] SET QUOTED_IDENTIFIER OFF 
-- GO
-- ALTER DATABASE [differencial] SET RECURSIVE_TRIGGERS OFF 
-- GO
-- ALTER DATABASE [differencial] SET  DISABLE_BROKER 
-- GO
-- ALTER DATABASE [differencial] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
-- GO
-- ALTER DATABASE [differencial] SET DATE_CORRELATION_OPTIMIZATION OFF 
-- GO
-- ALTER DATABASE [differencial] SET TRUSTWORTHY OFF 
-- GO
-- ALTER DATABASE [differencial] SET ALLOW_SNAPSHOT_ISOLATION OFF 
-- GO
-- ALTER DATABASE [differencial] SET PARAMETERIZATION SIMPLE 
-- GO
-- ALTER DATABASE [differencial] SET READ_COMMITTED_SNAPSHOT ON 
-- GO
-- ALTER DATABASE [differencial] SET HONOR_BROKER_PRIORITY OFF 
-- GO
-- ALTER DATABASE [differencial] SET RECOVERY SIMPLE 
-- GO
-- ALTER DATABASE [differencial] SET  MULTI_USER 
-- GO
-- ALTER DATABASE [differencial] SET PAGE_VERIFY CHECKSUM  
-- GO
-- ALTER DATABASE [differencial] SET DB_CHAINING OFF 
-- GO
-- ALTER DATABASE [differencial] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
-- GO
-- ALTER DATABASE [differencial] SET TARGET_RECOVERY_TIME = 60 SECONDS 
-- GO
-- ALTER DATABASE [differencial] SET DELAYED_DURABILITY = DISABLED 
-- GO
-- ALTER DATABASE [differencial] SET ACCELERATED_DATABASE_RECOVERY = OFF  
-- GO
-- ALTER DATABASE [differencial] SET QUERY_STORE = OFF
-- GO
-- USE [differencial]
GO
/****** Object:  UserDefinedFunction [dbo].[fnCalcDistancia]    Script Date: 09/09/2024 22:46:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnCalcDistancia] 
							(
							@latIni float, @lonIni float, @latFim float, @lonFim float  
							)
							RETURNS float
							AS
							BEGIN

							DECLARE @Result as float, @arcoA as float, @arcoB  float, @arcoC as float, @auxPI as FLOAT;

							SET @auxPi = Pi() / 180;
							SET @arcoA = (@lonFim - @lonIni) * @auxPi;
							SET @arcoB = (90 - @latFim) * @auxPi;
							SET @arcoC = (90 - @latIni) * @auxPi;
							SET @Result = Cos(@arcoB) * Cos(@arcoC) + Sin(@arcoB) * Sin(@arcoC) * Cos(@arcoA);
							SET @Result = (40030 * ((180 / Pi()) * Acos(@Result))) /360

							RETURN Round(@Result,4)
							END
GO
/****** Object:  UserDefinedFunction [dbo].[FunctionTable]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FunctionTable]
							(
								@param1 float,
								@param2 float,
								@param3 int 
							)
							RETURNS @returntable TABLE
							(
								Id int,
								NomeOperador varchar(300),
								DistanciaRaio float null,
								DistanciaRota float null
							) 
							AS 
							BEGIN

							DECLARE @Top AS INT

							/*
							DECLARE @param1 AS FLOAT, @param2 AS FLOAT
							SET @param1 = -30.008597 
							SET @param2 = -51.191220 
							*/


								INSERT @returntable

									SELECT TOP 10
											Operador.Id,
											Operador.NomeOperador,
											/* DISTANCIA = CASE WHEN ENDERECO.Latitude is NULL THEN NULL ELSE dbo.fnCalcDistancia(@param1, @param2, ENDERECO.Latitude, ENDERECO.Longitude) END */
											DISTANCIA =  dbo.fnCalcDistancia(@param1, @param2, Endereco.Latitude, Endereco.Longitude),
											NULL
									FROM Operador		
									INNER JOIN Vistoriador ON Operador.Id = Vistoriador.Id
									INNER JOIN VistoriadorProduto ON VistoriadorProduto.IdVistoriador = Vistoriador.Id
									INNER JOIN Endereco ON Vistoriador.IdEnderecoBase = Endereco.Id
									WHERE Endereco.Latitude IS NOT NULL 
									AND VistoriadorProduto.IndAtivo = 1 
									AND VistoriadorProduto.IdProduto = @param3
									ORDER BY 3


								RETURN
							END
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Agendamento]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agendamento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
	[IdVistoriador] [int] NOT NULL,
	[DthAgendamento] [datetime2](7) NULL,
	[IndCancelado] [bit] NOT NULL,
	[TipoAgendamento] [int] NOT NULL,
	[MotivoCancelamentoReagendamento] [varchar](1000) NULL,
 CONSTRAINT [PK_Agendamento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Analista]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Analista](
	[Id] [int] NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
 CONSTRAINT [PK_Analista] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnalistaProduto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalistaProduto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[QtdPontuacao] [int] NULL,
 CONSTRAINT [PK_AnalistaProduto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AtividadeProcesso]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AtividadeProcesso](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[NomeAtividadeProcesso] [varchar](250) NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
	[IdOperadorConcluida] [int] NULL,
	[TipoAtividade] [int] NOT NULL,
	[TipoSituacaoAtividade] [int] NOT NULL,
	[DthAssinada] [datetime2](7) NULL,
	[DthDelegada] [datetime2](7) NULL,
	[DthConcluida] [datetime2](7) NULL,
	[IndRetrabalho] [bit] NULL,
 CONSTRAINT [PK_AtividadeProcesso] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[CpfCnpj] [varchar](14) NULL,
	[NomeRazaoSocial] [varchar](250) NULL,
	[AtividadeNome] [varchar](250) NULL,
	[ContatoNome] [varchar](250) NULL,
	[ContatoTelefone] [varchar](13) NULL,
	[ContatoOutro] [varchar](250) NULL,
	[ContatoAgendamento] [varchar](1) NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClienteEndereco]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClienteEndereco](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdEndereco] [int] NOT NULL,
 CONSTRAINT [PK_ClienteEndereco] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cobertura]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cobertura](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[NomeCobertura] [varchar](250) NOT NULL,
	[VlrCobertura] [decimal](18, 2) NULL,
	[IdSolicitacao] [int] NULL,
 CONSTRAINT [PK_Cobertura] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comunicacao]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comunicacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdTipoAssunto] [int] NULL,
	[TipoComunicacao] [int] NOT NULL,
	[Assunto] [varchar](250) NULL,
	[TextoComunicacao] [varchar](1000) NULL,
	[IdSolicitacao] [int] NOT NULL,
 CONSTRAINT [PK_Comunicacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contrato]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contrato](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
 CONSTRAINT [PK_Contrato] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContratoLancamento]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContratoLancamento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdContrato] [int] NOT NULL,
	[TipoContratoLancamento] [int] NOT NULL,
	[TipoParametroQuantitativoVariavel] [int] NOT NULL,
 CONSTRAINT [PK_ContratoLancamento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContratoLancamentoValor]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContratoLancamentoValor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdContratoLancamento] [int] NOT NULL,
	[TipoQuantitativoVariacao] [int] NOT NULL,
	[QuantitativoA] [decimal](18, 2) NULL,
	[QuantitativoB] [decimal](18, 2) NULL,
	[ValorLancamento] [decimal](18, 2) NULL,
	[ValorLancamentoQuantitativo] [decimal](18, 2) NULL,
	[IndPreAcordo] [bit] NOT NULL,
	[SiglaUf] [varchar](1) NULL,
 CONSTRAINT [PK_ContratoLancamentoValor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Endereco]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Endereco](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[Cep] [varchar](9) NULL,
	[Logradouro] [varchar](250) NULL,
	[Numero] [int] NULL,
	[Complemento] [varchar](80) NULL,
	[Bairro] [varchar](80) NULL,
	[NomeMunicipio] [varchar](80) NULL,
	[SiglaUf] [varchar](2) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
 CONSTRAINT [PK_Endereco] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Filial]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Filial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSeguradora] [int] NOT NULL,
	[NomeFilial] [varchar](250) NOT NULL,
 CONSTRAINT [PK_Filial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Foto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Foto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
	[GuidArquivo] [uniqueidentifier] NOT NULL,
	[ArquivoDataModificacao] [datetime2](7) NOT NULL,
	[ArquivoNome] [varchar](250) NULL,
	[ArquivoExtencao] [varchar](5) NULL,
	[ArquivoTamanho] [bigint] NOT NULL,
	[Descricao] [varchar](1000) NULL,
	[ArquivoAnexoPosicao] [int] NOT NULL,
	[IndExcluida] [bit] NOT NULL,
	[TipoArquivoAnexo] [int] NOT NULL,
 CONSTRAINT [PK_Foto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LancamentoFinanceiro]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LancamentoFinanceiro](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
	[TipoLancamentoFinanceiro] [int] NOT NULL,
	[ValorLancamentoFinanceiro] [decimal](18, 2) NOT NULL,
	[DescricaoLancamentoFinanceiro] [varchar](250) NOT NULL,
	[IdLancamentoFinanceiroTotal] [int] NOT NULL,
 CONSTRAINT [PK_LancamentoFinanceiro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LancamentoFinanceiroTotal]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LancamentoFinanceiroTotal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
	[TipoLancamentoFinanceiro] [int] NOT NULL,
	[ValorLancamentoFinanceiroTotal] [decimal](18, 2) NOT NULL,
	[DthLancamentoPagamento] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_LancamentoFinanceiroTotal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Laudo]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Laudo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
 CONSTRAINT [PK_Laudo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LaudoFoto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LaudoFoto](
	[Id] [int] NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdLaudo] [int] NULL,
	[QuadroFotosPosicao] [int] NOT NULL,
	[IndQuadroFoto] [bit] NOT NULL,
 CONSTRAINT [PK_LaudoFoto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogAuditoria]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogAuditoria](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tabela] [varchar](128) NOT NULL,
	[IdTabela] [int] NOT NULL,
	[XMLDadosAnterior] [varchar](1) NULL,
	[XMLDadosPosterior] [varchar](1) NULL,
	[UsuarioAplicacao] [int] NOT NULL,
	[Acao] [varchar](1) NOT NULL,
	[DataAcao] [datetime2](7) NOT NULL,
	[UsuarioBanco] [varchar](128) NULL,
 CONSTRAINT [PK_LogAuditoria] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovimentacaoProcesso]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimentacaoProcesso](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[TextoMovimentacao] [varchar](1000) NULL,
	[TipoSituacaoProcesso] [int] NOT NULL,
	[IdOperadorOrigem] [int] NOT NULL,
	[IdOperadorDestino] [int] NULL,
	[DthMovimentacao] [datetime2](7) NOT NULL,
	[DthApropriacao] [datetime2](7) NULL,
	[DthConclusao] [datetime2](7) NULL,
	[TipoSituacaoMovimento] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
 CONSTRAINT [PK_MovimentacaoProcesso] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificacao]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](500) NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSolicitacao] [int] NOT NULL,
 CONSTRAINT [PK_Notificacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificacaoOperador]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificacaoOperador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IndLido] [bit] NOT NULL,
	[IdNotificacao] [int] NOT NULL,
	[IdOperador] [int] NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
 CONSTRAINT [PK_NotificacaoOperador] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operador]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operador](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[NomeOperador] [varchar](250) NULL,
	[Telefone] [varchar](13) NULL,
	[UrlFoto] [varchar](50) NULL,
	[Email] [varchar](250) NULL,
	[Cpf] [varchar](20) NULL,
	[Rg] [varchar](20) NULL,
	[DataNascimento] [datetime2](7) NULL,
	[IndAnalista] [bit] NOT NULL,
	[IndGerente] [bit] NOT NULL,
	[IndVistoriador] [bit] NOT NULL,
	[IndSolicitante] [bit] NOT NULL,
	[IndFinanceiro] [bit] NOT NULL,
	[IndAssessor] [bit] NOT NULL,
	[IdEndereco] [int] NULL,
	[IndAcessoSistema] [bit] NOT NULL,
	[IndPrimeiroAcesso] [bit] NOT NULL,
	[Senha] [varchar](250) NULL,
	[IndUsuarioSistema] [bit] NOT NULL,
	[SenhaConfirmacao] [varchar](250) NULL,
 CONSTRAINT [PK_Operador] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Produto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[IdSeguradora] [int] NOT NULL,
	[IdTipoInspecao] [int] NOT NULL,
	[NomeProduto] [varchar](250) NOT NULL,
	[IndFranquiaQuilometragem] [bit] NOT NULL,
	[QtdFranquiaQuilometragem] [int] NULL,
	[VlrDespesa] [decimal](18, 2) NULL,
	[VlrReceber] [decimal](18, 2) NULL,
	[VlrQuilometragem] [decimal](18, 2) NULL,
	[IndBlocoExtra] [bit] NOT NULL,
	[VlrBlocoExtr] [decimal](18, 2) NULL,
	[IndQuilometragemVariavel] [bit] NOT NULL,
	[QtdQuilometragemFinal] [int] NULL,
	[QtdQuilometragemInicial] [int] NULL,
	[VlrQuilometragemReceber] [decimal](18, 2) NULL,
	[IndMetragemVariavel] [bit] NOT NULL,
	[QtdMetragemM2Inicial] [decimal](18, 2) NULL,
	[QtdMetragemM2Final] [decimal](18, 2) NULL,
	[VlrMetragemReceber] [decimal](18, 2) NULL,
	[CodProdutoSeguradora] [varchar](1) NULL,
	[NomeProdutoSeguradora] [varchar](250) NULL,
 CONSTRAINT [PK_Produto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seguradora]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seguradora](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[NomeSeguradora] [varchar](250) NOT NULL,
	[Cnpj] [varchar](18) NULL,
	[Inscricao] [varchar](250) NULL,
	[RazaoSocial] [varchar](1000) NULL,
	[IdEndereco] [int] NULL,
	[ContabilInspecoesDiaInicio] [int] NOT NULL,
	[ContabilInspecoesDiaFim] [int] NOT NULL,
	[ContabilInspetorDia] [int] NOT NULL,
	[ContabilEmpresaDia] [int] NOT NULL,
	[IndIntegracaoSolicitacaoPorEmail] [bit] NOT NULL,
	[EmailRemetenteSolicitacao] [varchar](250) NULL,
	[IndAgendaRepostaPorEmail] [bit] NOT NULL,
	[IndLaudoRepostaPorEmail] [bit] NOT NULL,
	[QtdQuilometroFranquia] [int] NOT NULL,
	[VlrQuilometroExcedente] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Seguradora] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solicitacao]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdSeguradora] [int] NOT NULL,
	[IdProduto] [int] NOT NULL,
	[IdContratoLancamentoValor] [int] NULL,
	[IdVistoriador] [int] NULL,
	[IdCliente] [int] NULL,
	[IdEnderecoCliente] [int] NOT NULL,
	[CodSeguradora] [varchar](50) NULL,
	[CodSistemaLegado] [int] NOT NULL,
	[TpSituacao] [int] NULL,
	[IdAnalista] [int] NULL,
	[TxtInformacoesAdicionais] [varchar](1000) NULL,
	[IdSolicitante] [int] NULL,
	[CorretorNome] [varchar](250) NULL,
	[CorretorTelefone] [varchar](13) NULL,
	[TxtJustificativaVistoriadorDefinido] [varchar](1000) NULL,
	[VlrPagamentoVistoria] [decimal](18, 2) NULL,
	[VlrQuilometroRodado] [decimal](18, 2) NULL,
	[TipoRotaVistoriaPrevista] [int] NULL,
	[DeslocamentoPrevisto] [decimal](18, 2) NULL,
	[CustoDeslocamentoPrevisto] [decimal](18, 2) NULL,
	[CustoTotalPrevisto] [decimal](18, 2) NULL,
	[TipoRotaVistoriaRealizada] [int] NULL,
	[DeslocamentoRealizado] [decimal](18, 2) NULL,
	[CustoDeslocamentoRealizado] [decimal](18, 2) NULL,
	[CustoTotalRealizado] [decimal](18, 2) NULL,
	[CustoDeslocamentoAcordado] [decimal](18, 2) NULL,
	[VlrPagamentoVistoriaAcordado] [decimal](18, 2) NULL,
	[CustoTotalAcordado] [decimal](18, 2) NULL,
	[SolicitanteNome] [varchar](1) NULL,
	[SolicitanteTelefone] [varchar](1) NULL,
	[SolicitanteEmail] [varchar](1) NULL,
	[VistoriadorCidadeBase] [varchar](100) NULL,
	[TxtJustificativaDeslocamentoRealizado] [varchar](1000) NULL,
	[IndCustoVistoriaAcordado] [bit] NOT NULL,
	[DthVistoriaRealizada] [datetime2](7) NULL,
	[TxtJustificativaAnalistaDefinido] [varchar](1) NULL,
	[IndRelacionamentoAgendaInformada] [bit] NOT NULL,
	[NomeOperadorAgendaInformada] [varchar](1) NULL,
	[DthRelacionamentoAgendaInformada] [datetime2](7) NULL,
	[TipoNotificacaoAgendaInformada] [int] NULL,
	[AreaContruida] [decimal](18, 2) NULL,
	[BlocoContruido] [int] NULL,
	[CasaConstruida] [int] NULL,
	[VlrRiscoSegurado] [decimal](18, 2) NULL,
	[VlrHonorarioPreAcordo] [decimal](18, 2) NULL,
	[QtdEquipamento] [int] NULL,
	[IndRelatorioExigenciaMelhoria] [bit] NOT NULL,
	[IndRotaDeVolta] [bit] NOT NULL,
	[IndUrgente] [bit] NOT NULL,
	[IdFilial] [int] NULL,
	[IdSolicitacaoOrigemReinspecao] [int] NULL,
	[ControleDthEmailCobrancaVistoria] [datetime2](7) NULL,
	[IdOperadorApropriado] [int] NULL,
 CONSTRAINT [PK_Solicitacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solicitante]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitante](
	[Id] [int] NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdOperador] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[TipoSolicitante] [int] NOT NULL,
	[IdSeguradora] [int] NULL,
 CONSTRAINT [PK_Solicitante] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoAssunto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoAssunto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[NomeAssunto] [varchar](250) NOT NULL,
	[TextoPadrao] [varchar](1000) NULL,
 CONSTRAINT [PK_TipoAssunto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoInspecao]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoInspecao](
	[IdTipoInspecao] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[NomeTipoInspecao] [varchar](250) NOT NULL,
	[DescricaoTipoInspecao] [varchar](1000) NULL,
	[IndAtivo] [bit] NOT NULL,
 CONSTRAINT [PK_TipoInspecao] PRIMARY KEY CLUSTERED 
(
	[IdTipoInspecao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vistoriador]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vistoriador](
	[Id] [int] NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IdOperador] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[IdEnderecoBase] [int] NOT NULL,
	[IndEnderecoBaseIgual] [bit] NOT NULL,
 CONSTRAINT [PK_Vistoriador] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VistoriadorProduto]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VistoriadorProduto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DtCadastro] [datetime2](7) NOT NULL,
	[DtModificacao] [datetime2](7) NOT NULL,
	[IdOperadorCadastro] [int] NOT NULL,
	[IdOperadorModificacao] [int] NOT NULL,
	[IndAtivo] [bit] NOT NULL,
	[IdVistoriador] [int] NOT NULL,
	[IdProduto] [int] NOT NULL,
	[IdContratoLancamento] [int] NOT NULL,
	[IdContratoLancamentoValor] [int] NOT NULL,
	[VlrPagamentoVistoria] [decimal](18, 2) NULL,
	[VlrQuilometroRodado] [decimal](18, 2) NULL,
 CONSTRAINT [PK_VistoriadorProduto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Agendamento_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Agendamento_IdSolicitacao] ON [dbo].[Agendamento]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Agendamento_IdVistoriador]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Agendamento_IdVistoriador] ON [dbo].[Agendamento]
(
	[IdVistoriador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AtividadeProcesso_IdOperadorConcluida]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_AtividadeProcesso_IdOperadorConcluida] ON [dbo].[AtividadeProcesso]
(
	[IdOperadorConcluida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AtividadeProcesso_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_AtividadeProcesso_IdSolicitacao] ON [dbo].[AtividadeProcesso]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClienteEndereco_IdCliente]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_ClienteEndereco_IdCliente] ON [dbo].[ClienteEndereco]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ClienteEndereco_IdEndereco]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_ClienteEndereco_IdEndereco] ON [dbo].[ClienteEndereco]
(
	[IdEndereco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cobertura_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Cobertura_IdSolicitacao] ON [dbo].[Cobertura]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comunicacao_IdOperadorCadastro]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Comunicacao_IdOperadorCadastro] ON [dbo].[Comunicacao]
(
	[IdOperadorCadastro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comunicacao_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Comunicacao_IdSolicitacao] ON [dbo].[Comunicacao]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ContratoLancamento_IdContrato]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_ContratoLancamento_IdContrato] ON [dbo].[ContratoLancamento]
(
	[IdContrato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ContratoLancamentoValor_IdContratoLancamento]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_ContratoLancamentoValor_IdContratoLancamento] ON [dbo].[ContratoLancamentoValor]
(
	[IdContratoLancamento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Filial_IdSeguradora]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Filial_IdSeguradora] ON [dbo].[Filial]
(
	[IdSeguradora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Foto_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Foto_IdSolicitacao] ON [dbo].[Foto]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LancamentoFinanceiro_IdLancamentoFinanceiroTotal]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LancamentoFinanceiro_IdLancamentoFinanceiroTotal] ON [dbo].[LancamentoFinanceiro]
(
	[IdLancamentoFinanceiroTotal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LancamentoFinanceiro_IdOperadorCadastro]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LancamentoFinanceiro_IdOperadorCadastro] ON [dbo].[LancamentoFinanceiro]
(
	[IdOperadorCadastro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LancamentoFinanceiro_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LancamentoFinanceiro_IdSolicitacao] ON [dbo].[LancamentoFinanceiro]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LancamentoFinanceiroTotal_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LancamentoFinanceiroTotal_IdSolicitacao] ON [dbo].[LancamentoFinanceiroTotal]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LaudoFoto_IdLaudo]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LaudoFoto_IdLaudo] ON [dbo].[LaudoFoto]
(
	[IdLaudo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_LogAuditoria_UsuarioAplicacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_LogAuditoria_UsuarioAplicacao] ON [dbo].[LogAuditoria]
(
	[UsuarioAplicacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tabela]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Tabela] ON [dbo].[LogAuditoria]
(
	[Tabela] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimentacaoProcesso_IdOperadorDestino]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_MovimentacaoProcesso_IdOperadorDestino] ON [dbo].[MovimentacaoProcesso]
(
	[IdOperadorDestino] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimentacaoProcesso_IdOperadorOrigem]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_MovimentacaoProcesso_IdOperadorOrigem] ON [dbo].[MovimentacaoProcesso]
(
	[IdOperadorOrigem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimentacaoProcesso_IdSolicitacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_MovimentacaoProcesso_IdSolicitacao] ON [dbo].[MovimentacaoProcesso]
(
	[IdSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_NotificacaoOperador_IdNotificacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_NotificacaoOperador_IdNotificacao] ON [dbo].[NotificacaoOperador]
(
	[IdNotificacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Operador_IdEndereco]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Operador_IdEndereco] ON [dbo].[Operador]
(
	[IdEndereco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Produto_IdSeguradora]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Produto_IdSeguradora] ON [dbo].[Produto]
(
	[IdSeguradora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Produto_IdTipoInspecao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Produto_IdTipoInspecao] ON [dbo].[Produto]
(
	[IdTipoInspecao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Seguradora_IdEndereco]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Seguradora_IdEndereco] ON [dbo].[Seguradora]
(
	[IdEndereco] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdAnalista]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdAnalista] ON [dbo].[Solicitacao]
(
	[IdAnalista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdCliente]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdCliente] ON [dbo].[Solicitacao]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdEnderecoCliente]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdEnderecoCliente] ON [dbo].[Solicitacao]
(
	[IdEnderecoCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdFilial]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdFilial] ON [dbo].[Solicitacao]
(
	[IdFilial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdOperadorApropriado]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdOperadorApropriado] ON [dbo].[Solicitacao]
(
	[IdOperadorApropriado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdOperadorCadastro]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdOperadorCadastro] ON [dbo].[Solicitacao]
(
	[IdOperadorCadastro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdOperadorModificacao]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdOperadorModificacao] ON [dbo].[Solicitacao]
(
	[IdOperadorModificacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdProduto]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdProduto] ON [dbo].[Solicitacao]
(
	[IdProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdSeguradora]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdSeguradora] ON [dbo].[Solicitacao]
(
	[IdSeguradora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdSolicitante]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdSolicitante] ON [dbo].[Solicitacao]
(
	[IdSolicitante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitacao_IdVistoriador]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitacao_IdVistoriador] ON [dbo].[Solicitacao]
(
	[IdVistoriador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Solicitante_IdSeguradora]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Solicitante_IdSeguradora] ON [dbo].[Solicitante]
(
	[IdSeguradora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vistoriador_IdEnderecoBase]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_Vistoriador_IdEnderecoBase] ON [dbo].[Vistoriador]
(
	[IdEnderecoBase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VistoriadorProduto_IdProduto]    Script Date: 09/09/2024 22:46:52 ******/
CREATE NONCLUSTERED INDEX [IX_VistoriadorProduto_IdProduto] ON [dbo].[VistoriadorProduto]
(
	[IdProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_VistoriadorProduto]    Script Date: 09/09/2024 22:46:52 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UK_VistoriadorProduto] ON [dbo].[VistoriadorProduto]
(
	[IdVistoriador] ASC,
	[IdProduto] ASC,
	[IdContratoLancamento] ASC,
	[IdContratoLancamentoValor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Agendamento]  WITH CHECK ADD  CONSTRAINT [FK_Agendamento_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[Agendamento] CHECK CONSTRAINT [FK_Agendamento_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[Agendamento]  WITH CHECK ADD  CONSTRAINT [FK_Agendamento_Vistoriador_IdVistoriador] FOREIGN KEY([IdVistoriador])
REFERENCES [dbo].[Vistoriador] ([Id])
GO
ALTER TABLE [dbo].[Agendamento] CHECK CONSTRAINT [FK_Agendamento_Vistoriador_IdVistoriador]
GO
ALTER TABLE [dbo].[Analista]  WITH CHECK ADD  CONSTRAINT [FK_Analista_Operador_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Analista] CHECK CONSTRAINT [FK_Analista_Operador_Id]
GO
ALTER TABLE [dbo].[AtividadeProcesso]  WITH CHECK ADD  CONSTRAINT [FK_AtividadeProcesso_Operador_IdOperadorConcluida] FOREIGN KEY([IdOperadorConcluida])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[AtividadeProcesso] CHECK CONSTRAINT [FK_AtividadeProcesso_Operador_IdOperadorConcluida]
GO
ALTER TABLE [dbo].[AtividadeProcesso]  WITH CHECK ADD  CONSTRAINT [FK_AtividadeProcesso_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[AtividadeProcesso] CHECK CONSTRAINT [FK_AtividadeProcesso_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[ClienteEndereco]  WITH CHECK ADD  CONSTRAINT [FK_ClienteEndereco_Cliente_IdCliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([Id])
GO
ALTER TABLE [dbo].[ClienteEndereco] CHECK CONSTRAINT [FK_ClienteEndereco_Cliente_IdCliente]
GO
ALTER TABLE [dbo].[ClienteEndereco]  WITH CHECK ADD  CONSTRAINT [FK_ClienteEndereco_Endereco_IdEndereco] FOREIGN KEY([IdEndereco])
REFERENCES [dbo].[Endereco] ([Id])
GO
ALTER TABLE [dbo].[ClienteEndereco] CHECK CONSTRAINT [FK_ClienteEndereco_Endereco_IdEndereco]
GO
ALTER TABLE [dbo].[Cobertura]  WITH CHECK ADD  CONSTRAINT [FK_Cobertura_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[Cobertura] CHECK CONSTRAINT [FK_Cobertura_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[Comunicacao]  WITH CHECK ADD  CONSTRAINT [FK_Comunicacao_Operador_IdOperadorCadastro] FOREIGN KEY([IdOperadorCadastro])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Comunicacao] CHECK CONSTRAINT [FK_Comunicacao_Operador_IdOperadorCadastro]
GO
ALTER TABLE [dbo].[Comunicacao]  WITH CHECK ADD  CONSTRAINT [FK_Comunicacao_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[Comunicacao] CHECK CONSTRAINT [FK_Comunicacao_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[Contrato]  WITH CHECK ADD  CONSTRAINT [FK_Contrato_Produto_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Produto] ([Id])
GO
ALTER TABLE [dbo].[Contrato] CHECK CONSTRAINT [FK_Contrato_Produto_Id]
GO
ALTER TABLE [dbo].[ContratoLancamento]  WITH CHECK ADD  CONSTRAINT [FK_ContratoLancamento_Contrato_IdContrato] FOREIGN KEY([IdContrato])
REFERENCES [dbo].[Contrato] ([Id])
GO
ALTER TABLE [dbo].[ContratoLancamento] CHECK CONSTRAINT [FK_ContratoLancamento_Contrato_IdContrato]
GO
ALTER TABLE [dbo].[ContratoLancamentoValor]  WITH CHECK ADD  CONSTRAINT [FK_ContratoLancamentoValor_ContratoLancamento_IdContratoLancamento] FOREIGN KEY([IdContratoLancamento])
REFERENCES [dbo].[ContratoLancamento] ([Id])
GO
ALTER TABLE [dbo].[ContratoLancamentoValor] CHECK CONSTRAINT [FK_ContratoLancamentoValor_ContratoLancamento_IdContratoLancamento]
GO
ALTER TABLE [dbo].[Filial]  WITH CHECK ADD  CONSTRAINT [FK_Filial_Seguradora_IdSeguradora] FOREIGN KEY([IdSeguradora])
REFERENCES [dbo].[Seguradora] ([Id])
GO
ALTER TABLE [dbo].[Filial] CHECK CONSTRAINT [FK_Filial_Seguradora_IdSeguradora]
GO
ALTER TABLE [dbo].[Foto]  WITH CHECK ADD  CONSTRAINT [FK_Foto_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[Foto] CHECK CONSTRAINT [FK_Foto_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[LancamentoFinanceiro]  WITH CHECK ADD  CONSTRAINT [FK_LancamentoFinanceiro_LancamentoFinanceiroTotal_IdLancamentoFinanceiroTotal] FOREIGN KEY([IdLancamentoFinanceiroTotal])
REFERENCES [dbo].[LancamentoFinanceiroTotal] ([Id])
GO
ALTER TABLE [dbo].[LancamentoFinanceiro] CHECK CONSTRAINT [FK_LancamentoFinanceiro_LancamentoFinanceiroTotal_IdLancamentoFinanceiroTotal]
GO
ALTER TABLE [dbo].[LancamentoFinanceiro]  WITH CHECK ADD  CONSTRAINT [FK_LancamentoFinanceiro_Operador_IdOperadorCadastro] FOREIGN KEY([IdOperadorCadastro])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[LancamentoFinanceiro] CHECK CONSTRAINT [FK_LancamentoFinanceiro_Operador_IdOperadorCadastro]
GO
ALTER TABLE [dbo].[LancamentoFinanceiro]  WITH CHECK ADD  CONSTRAINT [FK_LancamentoFinanceiro_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[LancamentoFinanceiro] CHECK CONSTRAINT [FK_LancamentoFinanceiro_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[LancamentoFinanceiroTotal]  WITH CHECK ADD  CONSTRAINT [FK_LancamentoFinanceiroTotal_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[LancamentoFinanceiroTotal] CHECK CONSTRAINT [FK_LancamentoFinanceiroTotal_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[LaudoFoto]  WITH CHECK ADD  CONSTRAINT [FK_LaudoFoto_Foto_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Foto] ([Id])
GO
ALTER TABLE [dbo].[LaudoFoto] CHECK CONSTRAINT [FK_LaudoFoto_Foto_Id]
GO
ALTER TABLE [dbo].[LaudoFoto]  WITH CHECK ADD  CONSTRAINT [FK_LaudoFoto_Laudo_IdLaudo] FOREIGN KEY([IdLaudo])
REFERENCES [dbo].[Laudo] ([Id])
GO
ALTER TABLE [dbo].[LaudoFoto] CHECK CONSTRAINT [FK_LaudoFoto_Laudo_IdLaudo]
GO
ALTER TABLE [dbo].[LogAuditoria]  WITH CHECK ADD  CONSTRAINT [FK_LogAuditoria_Operador_UsuarioAplicacao] FOREIGN KEY([UsuarioAplicacao])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[LogAuditoria] CHECK CONSTRAINT [FK_LogAuditoria_Operador_UsuarioAplicacao]
GO
ALTER TABLE [dbo].[MovimentacaoProcesso]  WITH CHECK ADD  CONSTRAINT [FK_MovimentacaoProcesso_Operador_IdOperadorDestino] FOREIGN KEY([IdOperadorDestino])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[MovimentacaoProcesso] CHECK CONSTRAINT [FK_MovimentacaoProcesso_Operador_IdOperadorDestino]
GO
ALTER TABLE [dbo].[MovimentacaoProcesso]  WITH CHECK ADD  CONSTRAINT [FK_MovimentacaoProcesso_Operador_IdOperadorOrigem] FOREIGN KEY([IdOperadorOrigem])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[MovimentacaoProcesso] CHECK CONSTRAINT [FK_MovimentacaoProcesso_Operador_IdOperadorOrigem]
GO
ALTER TABLE [dbo].[MovimentacaoProcesso]  WITH CHECK ADD  CONSTRAINT [FK_MovimentacaoProcesso_Solicitacao_IdSolicitacao] FOREIGN KEY([IdSolicitacao])
REFERENCES [dbo].[Solicitacao] ([Id])
GO
ALTER TABLE [dbo].[MovimentacaoProcesso] CHECK CONSTRAINT [FK_MovimentacaoProcesso_Solicitacao_IdSolicitacao]
GO
ALTER TABLE [dbo].[NotificacaoOperador]  WITH CHECK ADD  CONSTRAINT [FK_NotificacaoOperador_Notificacao_IdNotificacao] FOREIGN KEY([IdNotificacao])
REFERENCES [dbo].[Notificacao] ([Id])
GO
ALTER TABLE [dbo].[NotificacaoOperador] CHECK CONSTRAINT [FK_NotificacaoOperador_Notificacao_IdNotificacao]
GO
ALTER TABLE [dbo].[Operador]  WITH CHECK ADD  CONSTRAINT [FK_Operador_Endereco_IdEndereco] FOREIGN KEY([IdEndereco])
REFERENCES [dbo].[Endereco] ([Id])
GO
ALTER TABLE [dbo].[Operador] CHECK CONSTRAINT [FK_Operador_Endereco_IdEndereco]
GO
ALTER TABLE [dbo].[Produto]  WITH CHECK ADD  CONSTRAINT [FK_Produto_Seguradora_IdSeguradora] FOREIGN KEY([IdSeguradora])
REFERENCES [dbo].[Seguradora] ([Id])
GO
ALTER TABLE [dbo].[Produto] CHECK CONSTRAINT [FK_Produto_Seguradora_IdSeguradora]
GO
ALTER TABLE [dbo].[Produto]  WITH CHECK ADD  CONSTRAINT [FK_Produto_TipoInspecao_IdTipoInspecao] FOREIGN KEY([IdTipoInspecao])
REFERENCES [dbo].[TipoInspecao] ([IdTipoInspecao])
GO
ALTER TABLE [dbo].[Produto] CHECK CONSTRAINT [FK_Produto_TipoInspecao_IdTipoInspecao]
GO
ALTER TABLE [dbo].[Seguradora]  WITH CHECK ADD  CONSTRAINT [FK_Seguradora_Endereco_IdEndereco] FOREIGN KEY([IdEndereco])
REFERENCES [dbo].[Endereco] ([Id])
GO
ALTER TABLE [dbo].[Seguradora] CHECK CONSTRAINT [FK_Seguradora_Endereco_IdEndereco]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Analista_IdAnalista] FOREIGN KEY([IdAnalista])
REFERENCES [dbo].[Analista] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Analista_IdAnalista]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Cliente_IdCliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Cliente_IdCliente]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Endereco_IdEnderecoCliente] FOREIGN KEY([IdEnderecoCliente])
REFERENCES [dbo].[Endereco] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Endereco_IdEnderecoCliente]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Filial_IdFilial] FOREIGN KEY([IdFilial])
REFERENCES [dbo].[Filial] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Filial_IdFilial]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Operador_IdOperadorApropriado] FOREIGN KEY([IdOperadorApropriado])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Operador_IdOperadorApropriado]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Operador_IdOperadorCadastro] FOREIGN KEY([IdOperadorCadastro])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Operador_IdOperadorCadastro]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Operador_IdOperadorModificacao] FOREIGN KEY([IdOperadorModificacao])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Operador_IdOperadorModificacao]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Produto_IdProduto] FOREIGN KEY([IdProduto])
REFERENCES [dbo].[Produto] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Produto_IdProduto]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Seguradora_IdSeguradora] FOREIGN KEY([IdSeguradora])
REFERENCES [dbo].[Seguradora] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Seguradora_IdSeguradora]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Solicitante_IdSolicitante] FOREIGN KEY([IdSolicitante])
REFERENCES [dbo].[Solicitante] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Solicitante_IdSolicitante]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH CHECK ADD  CONSTRAINT [FK_Solicitacao_Vistoriador_IdVistoriador] FOREIGN KEY([IdVistoriador])
REFERENCES [dbo].[Vistoriador] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Vistoriador_IdVistoriador]
GO
ALTER TABLE [dbo].[Solicitante]  WITH CHECK ADD  CONSTRAINT [FK_Solicitante_Operador_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Solicitante] CHECK CONSTRAINT [FK_Solicitante_Operador_Id]
GO
ALTER TABLE [dbo].[Solicitante]  WITH CHECK ADD  CONSTRAINT [FK_Solicitante_Seguradora_IdSeguradora] FOREIGN KEY([IdSeguradora])
REFERENCES [dbo].[Seguradora] ([Id])
GO
ALTER TABLE [dbo].[Solicitante] CHECK CONSTRAINT [FK_Solicitante_Seguradora_IdSeguradora]
GO
ALTER TABLE [dbo].[Vistoriador]  WITH CHECK ADD  CONSTRAINT [FK_Vistoriador_Endereco_IdEnderecoBase] FOREIGN KEY([IdEnderecoBase])
REFERENCES [dbo].[Endereco] ([Id])
GO
ALTER TABLE [dbo].[Vistoriador] CHECK CONSTRAINT [FK_Vistoriador_Endereco_IdEnderecoBase]
GO
ALTER TABLE [dbo].[Vistoriador]  WITH CHECK ADD  CONSTRAINT [FK_Vistoriador_Operador_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Operador] ([Id])
GO
ALTER TABLE [dbo].[Vistoriador] CHECK CONSTRAINT [FK_Vistoriador_Operador_Id]
GO
ALTER TABLE [dbo].[VistoriadorProduto]  WITH CHECK ADD  CONSTRAINT [FK_VistoriadorProduto_Produto_IdProduto] FOREIGN KEY([IdProduto])
REFERENCES [dbo].[Produto] ([Id])
GO
ALTER TABLE [dbo].[VistoriadorProduto] CHECK CONSTRAINT [FK_VistoriadorProduto_Produto_IdProduto]
GO
ALTER TABLE [dbo].[VistoriadorProduto]  WITH CHECK ADD  CONSTRAINT [FK_VistoriadorProduto_Vistoriador_IdVistoriador] FOREIGN KEY([IdVistoriador])
REFERENCES [dbo].[Vistoriador] ([Id])
GO
ALTER TABLE [dbo].[VistoriadorProduto] CHECK CONSTRAINT [FK_VistoriadorProduto_Vistoriador_IdVistoriador]
GO
/****** Object:  StoredProcedure [dbo].[sp_setusercontext]    Script Date: 09/09/2024 22:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_setusercontext] 
								@cod_usuario VARCHAR(10)
							AS
							BEGIN
								SET NOCOUNT ON;

								DECLARE @context VARBINARY(10)
								SET @context = CONVERT(BINARY(10), @cod_usuario)

								SET CONTEXT_INFO @context
							END

					
GO
USE [master]
GO
ALTER DATABASE [differencial] SET  READ_WRITE 
GO
