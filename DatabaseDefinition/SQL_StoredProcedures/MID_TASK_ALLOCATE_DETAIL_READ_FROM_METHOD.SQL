--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads taskslist that have allocate details which contain the method provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eWorkflowMethodType_Method INT = 801701;

	SELECT 
		TASKLIST_NAME, 
		TL.TASKLIST_RID, 
		TL.USER_RID, 
		AU.USER_NAME 
	from TASKLIST TL, 
		 APPLICATION_USER AU, TASK_ALLOCATE_DETAIL TAD 
	where TAD.TASKLIST_RID = TL.TASKLIST_RID 
		  and AU.USER_RID = TL.USER_RID 
		  and TAD.WORKFLOW_METHOD_IND = @eWorkflowMethodType_Method 
		  and TAD.METHOD_RID = @METHOD_RID

END
GO