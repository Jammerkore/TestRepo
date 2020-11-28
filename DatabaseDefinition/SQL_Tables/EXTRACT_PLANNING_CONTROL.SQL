--dv =============================================
--dv Modified date: 3/5/2018
--dv Notes:	
--dv History: 
--dv =============================================    
CREATE TABLE [dbo].[EXTRACT_PLANNING_CONTROL](
	[HN_RID] [int] NOT NULL,
	[FV_RID] [int] NOT NULL,
	[TIME_ID] [int] NOT NULL,
	[PLAN_TYPE] [int] NOT NULL,
	[UPDATE_DATE] [datetime] NULL,
	[EXTRACT_DATE] [datetime] NULL
)

GO
