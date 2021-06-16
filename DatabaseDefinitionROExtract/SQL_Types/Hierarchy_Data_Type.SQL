CREATE TYPE [dbo].[Hierarchy_Data_Type] AS TABLE(
	[ID] [varchar](100),
	[NodeDisplay] [varchar](250),
	[ProductType] [varchar](50),
	[Hierarchy] [varchar](50),
	[Parent] [varchar](50),
	[Name] [varchar](50),
	[Description] [varchar](250)
	PRIMARY KEY ([ID])
	)
GO
