--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
CREATE TABLE [dbo].[PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE](
	[PH_RID] [int] NOT NULL,
	[PHL_SEQUENCE] [int] NOT NULL,
	[HEADER_TYPE] [int] NOT NULL,
	[PURGE_HEADERS_TIMEFRAME] [int] NULL,
	[PURGE_HEADERS] [int] NULL,
 CONSTRAINT [MERCHANDISE_HEIR_LEVELS_HEADER_PURGE_PK] PRIMARY KEY CLUSTERED 
(
	[PH_RID] ASC,
	[PHL_SEQUENCE] ASC,
	[HEADER_TYPE] ASC
)ON [PRIMARY])

GO
