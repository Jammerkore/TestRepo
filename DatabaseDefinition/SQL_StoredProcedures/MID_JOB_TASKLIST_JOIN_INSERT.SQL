--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a tasklist job join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_TASKLIST_JOIN_INSERT]
	@JOB_RID int,
	@TASKLIST_RID int, 
	@TASKLIST_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO JOB_TASKLIST_JOIN
		(
		JOB_RID, 
		TASKLIST_RID, 
		TASKLIST_SEQUENCE
		)
	VALUES
		(
		@JOB_RID, 
		@TASKLIST_RID, 
		@TASKLIST_SEQUENCE
		)

	SELECT @@ROWCOUNT;
END
GO