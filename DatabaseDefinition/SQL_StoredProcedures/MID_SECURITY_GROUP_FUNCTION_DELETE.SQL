--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes a security group function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_DELETE]
	@GROUP_RID int, 
	@FUNC_ID int, 
	@ACTION_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM SECURITY_GROUP_FUNCTION 
	WHERE GROUP_RID = @GROUP_RID 
		  AND FUNC_ID = @FUNC_ID
		  AND ACTION_ID = @ACTION_ID

	SELECT @@ROWCOUNT;

END
GO
