--dv =============================================
--dv Modified date: 3/26/2015
--dv Notes:	
--dv History: 
--dv =============================================  
CREATE TABLE [dbo].[HEADER_RECONCILE_INFO](
	[PROCESS_RID] [int] NOT NULL,
	[HEADER_FILES_READ] [int] NULL,
	[HEADER_TRANS_READ] [int] NULL,
	[HEADER_TRANS_WRITTEN] [int] NULL,
	[HEADER_FILES_WRITTEN] [int] NULL,
	[REMOVE_TRANS_WRITTEN] [int] NULL,
	[REMOVE_FILES_WRITTEN] [int] NULL,
	[HEADER_TRANS_DUPLICATES] [int] NULL,
	[HEADER_TRANS_SKIPPED] [int] NULL) ON 'ALLOCATION'
  
GO
