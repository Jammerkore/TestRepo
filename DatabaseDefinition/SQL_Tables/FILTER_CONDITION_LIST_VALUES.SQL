--dv =============================================
--dv Modified date: 12/22/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[FILTER_CONDITION_LIST_VALUES](
	[LIST_VALUE_RID] [int] IDENTITY(1,1) NOT NULL,
	[CONDITION_RID] [int] NOT NULL,
	[LIST_VALUE_TYPE_INDEX] [int] NOT NULL,
	[LIST_VALUE_INDEX] [int] NOT NULL,
	[LIST_VALUE_PARENT_INDEX] [int] NULL
)

GO