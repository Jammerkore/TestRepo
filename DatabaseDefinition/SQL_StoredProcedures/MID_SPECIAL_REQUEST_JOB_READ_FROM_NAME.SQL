--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads special request job from the name provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME]
	@SPECIAL_REQ_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		SPECIAL_REQ_RID 
	FROM SPECIAL_REQUEST_JOB 
	WHERE SPECIAL_REQ_NAME = @SPECIAL_REQ_NAME

END
GO