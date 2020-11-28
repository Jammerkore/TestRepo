--dv =============================================
--dv Modified date: 4/18/2017
--dv Notes:	
--dv History: TT#1966-MD - JSmith- DC Fulfillment
--dv ============================================= 

CREATE TYPE [dbo].[CREATE_MASTER_HEADERS_OVERRIDE_TYPE] AS TABLE(
	[METHOD_RID] [int],
	[SEQ] [int],
	[OVERRIDE_TYPE] [char],
	[HEADER_CHAR_FIELD] [int],
	[HEADER_FIELD] [int],
	[HCG_RID] [int],
	[OVERRIDE_VALUE] [text],
	PRIMARY KEY (METHOD_RID, SEQ)
	)

GO

