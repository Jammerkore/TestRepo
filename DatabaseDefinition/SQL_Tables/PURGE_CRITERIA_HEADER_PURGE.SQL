--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
CREATE TABLE [dbo].[PURGE_CRITERIA_HEADER_PURGE](
	[HN_RID] [int] NOT NULL,
	[HEADER_TYPE] [int] NOT NULL,
	[PURGE_HEADERS_TIMEFRAME] [int] NULL,
	[PURGE_HEADERS] [int] NULL,
 CONSTRAINT [PURGE_CRITERIA_HEADER_PURGE_PK] PRIMARY KEY CLUSTERED 
(
	[HN_RID] ASC,
	[HEADER_TYPE] ASC
)ON [PRIMARY])

GO
