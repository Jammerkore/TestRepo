--dv =============================================
--dv Modified date: 5/29/2015
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[TASK_HEADER_RECONCILE](
	[TASK_SEQUENCE] [int] NOT NULL,
	[TASKLIST_RID] [int] NOT NULL,
	[INPUT_DIRECTORY] [varchar](400) NOT NULL,
	[OUTPUT_DIRECTORY] [varchar](400) NOT NULL,
	[TRIGGER_SUFFIX] [varchar](50) NOT NULL,
	[REMOVE_TRANS_FILE_NAME] [varchar](50) NOT NULL,
	[REMOVE_TRANS_TRIGGER_SUFFIX] [varchar](50) NOT NULL,
	[HEADER_TYPES] [varchar](400) NOT NULL,
	[HEADER_KEYS_FILE_NAME] [varchar](400) NOT NULL,
)	
go
