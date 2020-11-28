--dv =============================================
--dv Modified date: 3/27/2015
--dv Notes:	
--dv History: 
--dv =============================================   
CREATE TABLE [dbo].[API_PROCESS_CONTROL_RULES](
	[API_ID] [int] NOT NULL,
	[PROCESS_MUST_BE_RUNNING_IND] [char](1) NULL,
	[PROCESS_CANNOT_BE_RUNNING_IND] [char](1) NULL,
	[PROCESS_ID] [int] NOT NULL,
	[LAST_MODIFIED_DATETIME] [datetime] NULL,
	[LAST_MODIFIED_BY] varchar(250) NULL)  
GO
