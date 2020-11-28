--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[STORE_REMOVAL_TABLE_LIST](
	[TABLE_LIST_ID] [int] IDENTITY(1,1) NOT NULL,
	[TABLE_NAME] [varchar](4000) NOT NULL,
	[DELETE_SEQUENCE] [int] NOT NULL,
	[STORE_SET_IND] [bit]  default 0 NULL,
 CONSTRAINT [PK_STORE_REMOVAL_TABLE_LIST] PRIMARY KEY CLUSTERED 
(
	[TABLE_LIST_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
