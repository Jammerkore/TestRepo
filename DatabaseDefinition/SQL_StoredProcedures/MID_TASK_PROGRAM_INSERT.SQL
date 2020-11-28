--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Inserts a program task
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_PROGRAM_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@PROGRAM_PATH varchar(150), 
	@PROGRAM_PARMS varchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO TASK_PROGRAM
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		PROGRAM_PATH, 
		PROGRAM_PARMS
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@PROGRAM_PATH, 
		@PROGRAM_PARMS
		)

	SELECT @@ROWCOUNT;
END
GO
