--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[HEADER_STORE_GRADE_VALUES](
	[HDR_RID] [int] NOT NULL,
	[SGL_RID] [int] NOT NULL,
	[BOUNDARY] [int] NOT NULL,
	[MINIMUM_STOCK] [int] NULL,
	[MAXIMUM_STOCK] [int] NULL,
	[MINIMUM_AD] [int] NULL,
	[MINIMUM_COLOR] [int] NULL,
	[MAXIMUM_COLOR] [int] NULL,
	[SHIP_UP_TO] [int] NULL,
 CONSTRAINT [HEADER_STR_GRADE_VALUES_PK] PRIMARY KEY CLUSTERED 
(
	[HDR_RID] ASC,
	[SGL_RID] ASC,
	[BOUNDARY] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go
