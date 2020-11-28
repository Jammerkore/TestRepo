--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1413 -DOConnell -Chain Set Percent
--dv ============================================= 
CREATE TABLE [dbo].[CHAIN_SET_PERCENT_USER](
	[USER_RID] [int] NOT NULL,
	[SG_RID] [int] NOT NULL,
	[CDR_RID] [int] NOT NULL,
 CONSTRAINT [PK_CHAIN_SET_PERCENT_USER] PRIMARY KEY CLUSTERED 
(
	[USER_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
