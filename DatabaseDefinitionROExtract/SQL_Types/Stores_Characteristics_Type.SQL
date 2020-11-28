CREATE TYPE [dbo].[Stores_Characteristics_Type] AS TABLE(
	[ID] [varchar](50),
	[Characteristic] [varchar](50),
	[Value] [varchar](50)
	PRIMARY KEY ([ID], [Characteristic])
	)
GO
