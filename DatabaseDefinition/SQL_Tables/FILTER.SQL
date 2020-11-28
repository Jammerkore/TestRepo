--dv =============================================
--dv Modified date: 6/23/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[FILTER](
	[FILTER_RID] [int] IDENTITY(1,1) NOT NULL,
	[USER_RID] [int] NOT NULL,
	[OWNER_USER_RID] [int] NOT NULL,
	[FILTER_TYPE] [int] NOT NULL,
	[FILTER_NAME] [varchar](250) NOT NULL,
	[IS_LIMITED] [bit] NOT NULL,
	[RESULT_LIMIT] [int] NOT NULL
)
GO


