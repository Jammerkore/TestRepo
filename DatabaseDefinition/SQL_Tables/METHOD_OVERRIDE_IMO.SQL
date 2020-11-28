--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[METHOD_OVERRIDE_IMO](
	[METHOD_RID] [int] NOT NULL,
	[ST_RID] [int] not null,
	[IMO_MIN_SHIP_QTY] [int] NULL,
	[IMO_PCT_PK_THRSHLD] [float] NULL,
	[IMO_MAX_VALUE] [int] NULL
CONSTRAINT [PK_METHOD_OVERRIDE_IMO] PRIMARY KEY CLUSTERED 
(
	[METHOD_RID] ASC,
	[ST_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]	
) ON [PRIMARY]
GO
