--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[SPECIAL_REQUEST_JOB_JOIN](
	[SPECIAL_REQ_RID] [int] NOT NULL,
	[JOB_RID] [int] NOT NULL,
	[JOB_SEQUENCE] [int] NOT NULL,
 CONSTRAINT [SPECIAL_REQUEST_JOB_JOIN_PK] PRIMARY KEY CLUSTERED 
(
	[SPECIAL_REQ_RID] ASC,
	[JOB_RID] ASC,
	[JOB_SEQUENCE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
