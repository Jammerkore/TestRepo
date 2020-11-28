--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
CREATE TABLE [dbo].[DETERMINE_NODE_ACTIVITY_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[TOTAL_NODES] [int] NOT NULL,
	[ACTIVE_NODES] [int] NOT NULL,
	[INACTIVE_NODES] [int] NOT NULL,
	[TOTAL_ERRORS] [int] NOT NULL,
 CONSTRAINT [PK_DETERMINE_NODE_ACTIVITY_INFO] PRIMARY KEY CLUSTERED 
(
	[PROCESS_RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [AUDIT]

GO
