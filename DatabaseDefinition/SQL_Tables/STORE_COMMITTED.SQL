--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1224 - stodd - add store committed
--dv ============================================= 
CREATE TABLE [dbo].[STORE_COMMITTED](
	[HN_RID] [int] NOT NULL,
	[TIME_ID] [int] NOT NULL,
	[ST_RID] [int] NOT NULL,
	[UNITS] [int] NULL,
 CONSTRAINT [STORE_COMMITTED_PK] PRIMARY KEY CLUSTERED 
(
	[TIME_ID] ASC,
	[HN_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
