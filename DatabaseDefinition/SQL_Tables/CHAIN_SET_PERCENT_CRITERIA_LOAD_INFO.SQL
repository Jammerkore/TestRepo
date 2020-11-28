--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[CHAIN_SET_PERCENT_CRITERIA_LOAD_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[RECS_READ] [int] NULL,
	[RECS_WITH_ERRORS] [int] NULL,
	[CRITERIA_ADDED_UPDATED] [int] NULL,
 CONSTRAINT [CHN_SET_PCT_CRIT_LOAD_INFO_PK] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
