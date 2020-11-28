CREATE TYPE [dbo].[Stores_Data_Type] AS TABLE(
	[ID] [varchar](50),
	[StoreDisplay] [varchar](200),
	[Name] [varchar](50),
	[Description] [varchar](50),
	[City] [varchar](50),
	[State] [varchar](50),
	[SellingSquareFeet] [varchar](50),
	[SellingOpenDate] [varchar](50),
	[SellingCloseDate] [varchar](50),
	[StockOpenDate] [varchar](50),
	[StockCloseDate] [varchar](50),
	[ActiveIndicator] [varchar](50),
	[Status] [varchar](50),
	[LeadTime] [varchar](50),
	[ShipDate] [varchar](50),
	[VSWID] [varchar](50) 
	PRIMARY KEY ([ID])
	)
GO
