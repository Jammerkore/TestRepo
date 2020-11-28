--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a size curve generate task
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@GENERATE_SEQUENCE int, 
	@METHOD_NODE_TYPE int, 
	@HN_RID int, 
	@METHOD_RID int, 
	@EXECUTE_CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TASK_SIZE_CURVE_GENERATE
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		GENERATE_SEQUENCE,
		METHOD_NODE_TYPE, 
		HN_RID, 
		METHOD_RID, 
		EXECUTE_CDR_RID
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@GENERATE_SEQUENCE, 
		@METHOD_NODE_TYPE, 
		@HN_RID, 
		@METHOD_RID, 
		@EXECUTE_CDR_RID
		)

	SELECT @@ROWCOUNT;
END
GO