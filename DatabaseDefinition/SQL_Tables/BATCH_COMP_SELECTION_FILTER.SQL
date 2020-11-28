--dv =============================================
--dv Modified date: 5/12/2015
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[BATCH_COMP_SELECTION_FILTER](
	[BATCH_COMP_RID] [int] NOT NULL,
	[SEQUENCE] [int] NOT NULL,
	[SELECTION_FILTER_TYPE] char NOT NULL,
	[SELECTION_FILTER] [varchar](4000) NOT NULL)

GO
