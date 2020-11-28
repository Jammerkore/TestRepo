--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes a user security function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_FUNCTION_DELETE]
	@USER_RID int,
	@FUNC_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM SECURITY_USER_FUNCTION 
	WHERE USER_RID = @USER_RID
		  AND FUNC_ID = @FUNC_ID

	SELECT @@ROWCOUNT;

END
GO