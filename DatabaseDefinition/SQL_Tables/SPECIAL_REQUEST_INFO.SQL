--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[SPECIAL_REQUEST_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[TOTAL_JOBS] [int] NOT NULL,
	[JOBS_PROCESSED] [int] NOT NULL,
	[JOBS_WITH_ERRORS] [int] NOT NULL,
	[SUCCESSFUL_JOBS] [int] NOT NULL,
 CONSTRAINT [PK_SPECIAL_REQUEST_JOB_INFO] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [AUDIT]

GO
