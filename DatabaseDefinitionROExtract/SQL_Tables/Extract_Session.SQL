--dv =============================================
--dv Modified date: 2/20/2018
--dv Notes:	
--dv History: 
--dv =============================================    
CREATE TABLE [dbo].[Extract_Session](
	[SessionKey] [BIGINT] NOT NULL,
	[SessionActiveInd] [INT] NULL,
	[SessionStartDateTime] [DATETIME] NULL,
	[SessionEndDateTime] [DATETIME] NULL
)

GO