--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#816-MD -DOConnell -Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
--dv ============================================= 
CREATE TABLE [dbo].[STORE_ELIGIBILITY_CRITERIA_LOAD_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[RECS_READ] [int] NULL,
	[RECS_WITH_ERRORS] [int] NULL,
	[CRITERIA_ADDED_UPDATED] [int] NULL,
 CONSTRAINT [STORE_ELIGIBILITY_CRIT_LOAD_INFO_PK] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
