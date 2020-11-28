--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a special request job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_DELETE]
	@SPECIAL_REQ_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM SPECIAL_REQUEST_JOB 
	WHERE SPECIAL_REQ_RID = @SPECIAL_REQ_RID

	SELECT @@ROWCOUNT;

END
GO