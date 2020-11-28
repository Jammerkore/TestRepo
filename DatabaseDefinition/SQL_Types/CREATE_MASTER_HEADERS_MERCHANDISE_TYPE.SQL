--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv ============================================= 

CREATE TYPE [dbo].[CREATE_MASTER_HEADERS_MERCHANDISE_TYPE] AS TABLE(
	[METHOD_RID] [int],
	[SEQ] [int],
	[MERCHANDISE] [text],
	[HN_RID] [int],
	[FILTER] [int],
	[FILTER_RID] [int]
	PRIMARY KEY (METHOD_RID, HN_RID)
	)

GO
