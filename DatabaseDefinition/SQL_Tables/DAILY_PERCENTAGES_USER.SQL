--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[DAILY_PERCENTAGES_USER](
	[USER_RID] [int] NOT NULL,
	[SG_RID] [int] NOT NULL,
	[CDR_RID] [int] NOT NULL,
 CONSTRAINT [PK_DAILY_PERCENTAGES_USER] PRIMARY KEY CLUSTERED 
(
	[USER_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
