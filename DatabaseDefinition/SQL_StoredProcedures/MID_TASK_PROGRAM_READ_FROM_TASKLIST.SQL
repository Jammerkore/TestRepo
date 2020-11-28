--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Reads program tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_PROGRAM_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_PROGRAM 
	WHERE TASKLIST_RID = @TASKLIST_RID 

END
GO
