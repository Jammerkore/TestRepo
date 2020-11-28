--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a special request job join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_JOIN_DELETE]
	@SPECIAL_REQ_RID int,
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM SPECIAL_REQUEST_JOB_JOIN 
	WHERE SPECIAL_REQ_RID = @SPECIAL_REQ_RID
		  AND JOB_RID = @JOB_RID

	SELECT @@ROWCOUNT;

END
GO