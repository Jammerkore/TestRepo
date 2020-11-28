--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @eProfileType_TaskList INT = 46;

	SELECT 
		t.TASKLIST_RID, 
		t.TASK_SEQUENCE,
		t.TASK_TYPE, 
		t.MAX_MESSAGE_LEVEL,
		t.EMAIL_SUCCESS_FROM, 
		t.EMAIL_SUCCESS_TO, 
		t.EMAIL_SUCCESS_CC, 
		t.EMAIL_SUCCESS_BCC, 
		t.EMAIL_SUCCESS_SUBJECT, 
		t.EMAIL_SUCCESS_BODY,
		t.EMAIL_FAILURE_FROM, 
		t.EMAIL_FAILURE_TO, 
		t.EMAIL_FAILURE_CC, 
		t.EMAIL_FAILURE_BCC, 
		t.EMAIL_FAILURE_SUBJECT, 
		t.EMAIL_FAILURE_BODY,
		ui.USER_RID, 
		ui.ITEM_TYPE, 
		ui.ITEM_RID, 
		ui.OWNER_USER_RID
	FROM TASK t 
		 inner join USER_ITEM ui ON ui.ITEM_RID = t.TASKLIST_RID
	WHERE t.TASKLIST_RID = @TASKLIST_RID
		  AND ui.ITEM_TYPE = @eProfileType_TaskList

END
GO