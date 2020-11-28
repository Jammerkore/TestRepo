--dv =============================================
--dv Create date:	5/14/2015
--dv Description:	Inserts a Batch Comp task
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_BATCH_COMP_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@BATCH_COMP_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO TASK_BATCH_COMP
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		BATCH_COMP_RID
		)
    VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@BATCH_COMP_RID
		)

	SELECT @@ROWCOUNT;

END
GO
