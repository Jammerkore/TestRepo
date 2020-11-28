--dv =============================================
--dv Create date: 10/14/2014
--dv Description:	Get the user's current workspace filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_WORKSPACE_FILTER_READ]
	@USER_RID INT,
	@WORKSPACE_TYPE INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM USER_CURRENT_WORKSPACE_FILTER
	WHERE USER_RID = @USER_RID
		AND WORKSPACE_TYPE = @WORKSPACE_TYPE

END
GO




















