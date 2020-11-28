--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[VSW_REVERSE_ON_HAND](
	[HDR_RID] [int] NOT NULL,
	[HN_RID] [int] NOT NULL,
	[ST_RID] [int] NOT NULL,
	[USTAT] [tinyint] NOT NULL,
	[VSW_REVERSE_ON_HAND_UNITS] [int] NULL,
 CONSTRAINT [VSW_REVERSE_ON_HAND_PK] PRIMARY KEY CLUSTERED 
(
	[HDR_RID] ASC,
	[HN_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

go