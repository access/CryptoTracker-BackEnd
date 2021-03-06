USE [master]
GO
/****** Object:  Database [CryptocurrencyTracker]    Script Date: 06.04.2021 11:43:38 ******/
CREATE DATABASE [CryptocurrencyTracker]
CONTAINMENT = NONE
GO
ALTER DATABASE [CryptocurrencyTracker] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
	EXEC [CryptocurrencyTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CryptocurrencyTracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CryptocurrencyTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CryptocurrencyTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CryptocurrencyTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CryptocurrencyTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CryptocurrencyTracker] SET  MULTI_USER 
GO
ALTER DATABASE [CryptocurrencyTracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CryptocurrencyTracker] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CryptocurrencyTracker] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CryptocurrencyTracker] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CryptocurrencyTracker] SET DELAYED_DURABILITY = DISABLED 
GO
USE [CryptocurrencyTracker]
GO
/****** Object:  Table [dbo].[CryptoCurrencyItems]    Script Date: 06.04.2021 11:43:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CryptoCurrencyItems](
	[CryptoName] [nvarchar](50) NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LastTradeRate] [decimal](10, 8) NOT NULL,
	[TradeRateDate] [datetime] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
	[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [AK_CryptoName] UNIQUE NONCLUSTERED 
	(
	[CryptoName] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CryptoCurrencyValues]    Script Date: 06.04.2021 11:43:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CryptoCurrencyValues](
	[CurrencyID] [int] NOT NULL,
	[MarketValue] [decimal](10, 8) NOT NULL,
	[HistoryDate] [datetime] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	CONSTRAINT [PK_CryptoCurrencyValues] PRIMARY KEY CLUSTERED 
	(
	[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [CryptocurrencyTracker] SET  READ_WRITE 
GO

INSERT INTO  [CryptocurrencyTracker].[dbo].[CryptoCurrencyItems] (CryptoName,LastTradeRate,TradeRateDate) VALUES ('LTC', '0.00376365','2021-04-06 11:54:50.127');
INSERT INTO  [CryptocurrencyTracker].[dbo].[CryptoCurrencyItems] (CryptoName,LastTradeRate,TradeRateDate) VALUES ('XLM', '0.00000921','2021-04-06 11:54:50.127');
INSERT INTO  [CryptocurrencyTracker].[dbo].[CryptoCurrencyItems] (CryptoName,LastTradeRate,TradeRateDate) VALUES ('ETH', '0.03571920','2021-04-06 11:54:50.127');
INSERT INTO  [CryptocurrencyTracker].[dbo].[CryptoCurrencyItems] (CryptoName,LastTradeRate,TradeRateDate) VALUES ('NEO', '0.00107009','2021-04-06 11:54:50.127');
INSERT INTO  [CryptocurrencyTracker].[dbo].[CryptoCurrencyItems] (CryptoName,LastTradeRate,TradeRateDate) VALUES ('ADA', '0.00002047','2021-04-06 11:54:50.127');
