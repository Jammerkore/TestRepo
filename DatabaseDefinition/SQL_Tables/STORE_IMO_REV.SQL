--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[STORE_IMO_REV](
	[HN_RID] [int] NOT NULL,
	[ST_RID] [int] NOT NULL,
	[UNITS] [int] NULL,
	[FLASHBACK] [datetime] NOT NULL
 CONSTRAINT [PK_STORE_IMO_REV] PRIMARY KEY CLUSTERED 
(
	[FLASHBACK] ASC,
	[HN_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
