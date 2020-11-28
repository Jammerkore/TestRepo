--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads special request job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_READ_FROM_RID]
	@SPECIAL_REQ_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM SPECIAL_REQUEST_JOB 
	WHERE SPECIAL_REQ_RID = @SPECIAL_REQ_RID

END
GO