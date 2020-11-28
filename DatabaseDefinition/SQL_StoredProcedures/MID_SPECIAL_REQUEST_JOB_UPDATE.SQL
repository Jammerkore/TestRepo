--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Updates a special request job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_UPDATE]
	@SPECIAL_REQ_RID int,
	@SPECIAL_REQ_NAME varchar(50),
	@CONCURRENT_PROCESSES int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE SPECIAL_REQUEST_JOB 
	SET
		SPECIAL_REQ_NAME = @SPECIAL_REQ_NAME, 
		CONCURRENT_PROCESSES = @CONCURRENT_PROCESSES
	WHERE SPECIAL_REQ_RID = @SPECIAL_REQ_RID

	SELECT @@ROWCOUNT;

END
GO