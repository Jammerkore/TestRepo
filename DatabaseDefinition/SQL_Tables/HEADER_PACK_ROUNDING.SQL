--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#923 - JSmith - Creating new database fails
--dv ============================================= 
CREATE TABLE [dbo].[HEADER_PACK_ROUNDING](
	[HDR_RID] [int] NOT NULL,
	[PACK_MULTIPLE_RID] [int] NOT NULL,
	[PACK_ROUNDING_1ST_PACK_PCT] [float] NULL,
	[PACK_ROUNDING_NTH_PACK_PCT] [float] NULL,
 CONSTRAINT [PK_HEADER_PACK_ROUNDING] PRIMARY KEY CLUSTERED 
(
	[HDR_RID] ASC,
	[PACK_MULTIPLE_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
