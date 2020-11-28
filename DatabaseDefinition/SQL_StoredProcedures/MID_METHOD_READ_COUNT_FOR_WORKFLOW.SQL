--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads the count of the method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_COUNT_FOR_WORKFLOW] 
	@METHOD_NAME varchar(50),
	@METHOD_TYPE_ID int,
	@USER_RID int,
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM METHOD
	WHERE METHOD_NAME = @METHOD_NAME
		  AND METHOD_TYPE_ID = @METHOD_TYPE_ID
		  AND USER_RID = @USER_RID
	      AND METHOD_RID != @METHOD_RID

END
GO