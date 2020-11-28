--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[SPECIAL_REQUEST_JOB](
	[SPECIAL_REQ_RID] [int] IDENTITY(1,1) NOT NULL,
	[SPECIAL_REQ_NAME] [varchar](50) NOT NULL,
	[CONCURRENT_PROCESSES] [int] NOT NULL,
 CONSTRAINT [PK_SPECIAL_REQUEST_JOB] PRIMARY KEY CLUSTERED 
(
	[SPECIAL_REQ_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
