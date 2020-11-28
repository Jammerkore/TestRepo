--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 -stodd -add resevation stores (IMO)
--dv =============================================   
CREATE TABLE [dbo].[PUSH_TO_BACK_STOCK_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[HDRS_READ] [int] NULL,
	[HDRS_WITH_ERRORS] [int] NULL,
	[HDRS_PROCESSED] [int] NULL,
	[HDRS_SKIPPED] [int] NULL,
 CONSTRAINT [PUSH_TO_BACK_STOCK_INFO_PK] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
