--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Deletes security group actions, based on the group and function passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_DELETE_FROM_GROUP_AND_FUNCTION]
	@GROUP_RID int, 
	@FUNC_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_GROUP_FUNCTION 
	WHERE GROUP_RID = @GROUP_RID 
		  AND FUNC_ID = @FUNC_ID

	SELECT @@ROWCOUNT;

END
GO

