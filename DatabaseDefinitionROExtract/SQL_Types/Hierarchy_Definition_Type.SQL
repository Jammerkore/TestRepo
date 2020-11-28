CREATE TYPE [dbo].[Hierarchy_Definition_Type] AS TABLE(
	[Type] [varchar](50),
	[Level_Number] [int],
	[Hierarchy_Level] [varchar](50) 
	PRIMARY KEY ([Type], [Hierarchy_Level])
	)
GO
