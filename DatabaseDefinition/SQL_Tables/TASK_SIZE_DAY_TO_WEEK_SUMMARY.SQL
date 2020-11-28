--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#391 -stodd -size day to week summary
--dv ============================================= 
CREATE TABLE [dbo].[TASK_SIZE_DAY_TO_WEEK_SUMMARY](
	[TASKLIST_RID] [int] NOT NULL,
	[TASK_SEQUENCE] [int] NOT NULL,
	[CDR_RID] [int] NULL,
	[HN_RID] [int] NULL,
 CONSTRAINT [PK_TASK_SIZE_DAY_TO_WEEK_SUMMARY] PRIMARY KEY CLUSTERED 
(
	[TASKLIST_RID] ASC,
	[TASK_SEQUENCE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
