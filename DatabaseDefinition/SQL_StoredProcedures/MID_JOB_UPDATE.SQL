--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Updates the job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_UPDATE]
	@JOB_RID int,
	@JOB_NAME varchar(100),
	@LAST_MODIFIED_BY_USER_RID int,
	@LAST_MODIFIED_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE JOB 
	SET 
		JOB_NAME = @JOB_NAME, 
		LAST_MODIFIED_BY_USER_RID = @LAST_MODIFIED_BY_USER_RID, 
		LAST_MODIFIED_DATETIME = @LAST_MODIFIED_DATETIME
	WHERE JOB_RID = @JOB_RID

	SELECT @@ROWCOUNT;
END
GO