--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv ============================================= 

CREATE TYPE [dbo].[DC_FULFILLMENT_STORE_ORDER_TYPE] AS TABLE(
	[METHOD_RID] [int],
	[SEQ] [int],
	[DIST_CENTER] [text],
	[SCG_RID] [int]
	PRIMARY KEY (METHOD_RID, SEQ)
	)

GO
