--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#485-MD -DOConnell
--dv ============================================= 
CREATE TABLE [dbo].[NODE_IMO](
	[HN_RID] [int] not null,
	[ST_RID] [int] not null,
	[IMO_MIN_SHIP_QTY] [int] NULL,
	[IMO_PCT_PK_THRSHLD] [float] NULL,
	[IMO_MAX_VALUE] [int] NULL,
	[IMO_PSH_BK_STK] [int] NULL,
	[IMO_FWOS_MAX] [FLOAT] NULL DEFAULT 0,
	[IMO_FWOS_MAX_RID] [int] NULL, 
	[IMO_FWOS_MAX_TYPE] [int] NULL, 
CONSTRAINT [PK_NODE_IMO] PRIMARY KEY CLUSTERED 
(
	[HN_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]	
) ON [PRIMARY]
GO
