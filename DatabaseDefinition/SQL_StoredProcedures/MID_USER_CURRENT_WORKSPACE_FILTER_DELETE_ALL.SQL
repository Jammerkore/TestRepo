--dv =============================================
--dv Create date: 11/11/2014
--dv Description:	DELETES the user workspace filter for any user using a deleted filter.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_WORKSPACE_FILTER_DELETE_ALL]
	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM USER_CURRENT_WORKSPACE_FILTER 
	WHERE FILTER_RID = @FILTER_RID

	SELECT @@ROWCOUNT
END
GO




















