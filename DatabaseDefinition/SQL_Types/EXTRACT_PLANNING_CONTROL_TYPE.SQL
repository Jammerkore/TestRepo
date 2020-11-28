CREATE TYPE [dbo].[EXTRACT_PLANNING_CONTROL_TYPE] AS TABLE(
	[HN_RID] [int],
	[FV_RID] [int],
	[TIME_ID] [int],
	[PLAN_TYPE] [int],
	[UPDATE_DATE] [datetime],
	[EXTRACT_DATE] [datetime]
	PRIMARY KEY ([HN_RID], [FV_RID], [TIME_ID], [PLAN_TYPE])
	)
GO
