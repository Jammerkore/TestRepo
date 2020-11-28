--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads a tasklist
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASKLIST_READ]
	@TASKLIST_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @eProfileType_TaskList INT = 46;

	SELECT 
		t.TASKLIST_RID, 
		t.TASKLIST_NAME, 
		t.SYSTEM_GENERATED_IND,
		t.CREATED_BY_USER_RID, 
		t.CREATED_DATETIME, 
		t.LAST_MODIFIED_BY_USER_RID, 
		t.LAST_MODIFIED_DATETIME,
		ui.USER_RID, 
		ui.ITEM_TYPE, 
		ui.ITEM_RID, 
		ui.OWNER_USER_RID
	FROM TASKLIST t 
		 INNER JOIN USER_ITEM ui ON ui.ITEM_RID = t.TASKLIST_RID
	WHERE ui.ITEM_TYPE = @eProfileType_TaskList
		  AND TASKLIST_RID = @TASKLIST_RID

END
GO