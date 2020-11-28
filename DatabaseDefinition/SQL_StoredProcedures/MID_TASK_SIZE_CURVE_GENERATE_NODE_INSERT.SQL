--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a size curve generate task node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@CONCURRENT_PROCESSES int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TASK_SIZE_CURVE_GENERATE_NODE
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		CONCURRENT_PROCESSES
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@CONCURRENT_PROCESSES
		)

	SELECT @@ROWCOUNT;
END
GO