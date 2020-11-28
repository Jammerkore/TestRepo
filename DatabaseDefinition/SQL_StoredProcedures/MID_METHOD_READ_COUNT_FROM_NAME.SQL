--dv =============================================
--dv Create date: 11/19/2013
--dv Description: Get the count for the number of methods by a certain name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_COUNT_FROM_NAME]
	@USER_RID int,
	@METHOD_TYPE_ID int,
	@METHOD_NAME varchar(50)
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) AS MyCount
	FROM METHOD
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID
		AND USER_RID = @USER_RID
		AND METHOD_NAME = @METHOD_NAME

END
GO