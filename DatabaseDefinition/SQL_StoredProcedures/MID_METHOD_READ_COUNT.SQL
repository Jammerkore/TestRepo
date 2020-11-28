--dv =============================================
--dv Create date: 11/15/2013
--dv Description:	Get the Method Row Count for this user.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_READ_COUNT]
	@METHOD_TYPE_ID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @UndefinedVelocityMethodRID INT = 2;

	SELECT 
		COUNT(*) AS MyCount
	FROM METHOD m
	INNER JOIN USER_ITEM ui ON ui.ITEM_RID = m.METHOD_RID AND ui.ITEM_TYPE = m.PROFILE_TYPE_ID
	WHERE METHOD_TYPE_ID = @METHOD_TYPE_ID
		AND ui.USER_RID = @USER_RID
		AND METHOD_RID > @UndefinedVelocityMethodRID

END
GO
