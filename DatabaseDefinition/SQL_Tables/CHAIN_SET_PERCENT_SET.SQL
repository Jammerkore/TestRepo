--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[CHAIN_SET_PERCENT_SET](
	[HN_RID] [int] NOT NULL,
	[SGL_RID] [int] NOT NULL,
	[TIME_ID] [int] NOT NULL,
	[PERCENTAGE] [float] NULL,
 CONSTRAINT [PK_CHAIN_SET_PERCENT_SET] PRIMARY KEY CLUSTERED 
(
	[HN_RID] ASC,
	[SGL_RID] ASC,
	[TIME_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
