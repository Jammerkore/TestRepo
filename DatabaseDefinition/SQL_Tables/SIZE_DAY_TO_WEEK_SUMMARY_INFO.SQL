--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#465 -stodd -size day to week summary
--dv =============================================
CREATE TABLE [dbo].[SIZE_DAY_TO_WEEK_SUMMARY_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[TOTAL_STYLES] [int] NULL,
	[TOTAL_COLORS] [int] NULL,
	[TOTAL_SIZES] [int] NULL,
	[TOTAL_VALUES_READ] [int] NULL,
	[TOTAL_RECS_READ] [int] NULL,
	[TOTAL_RECS_WRITTEN] [int] NULL,
	[TOTAL_ERRORS] [int] NULL,
 CONSTRAINT [PK_SIZE_DAY_TO_WEEK_SUMMARY_INFO] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
