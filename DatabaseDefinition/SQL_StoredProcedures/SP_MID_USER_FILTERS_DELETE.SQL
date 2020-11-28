--dv =============================================
--dv Modified date:	11/21/2014
--dv Description:	Deletes filters for the user
--dv History:		TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
--dv				TT#1397-MD -jsobek -Fix SP_MID_USER_FILTERS_DELETE
--dv				TT#1399-MD -jsobek -Improve SQL error reporting in nested procedures
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_USER_FILTERS_DELETE] 
	@UserRID INT,
	@COMMIT_LIMIT INT = 9999999,
	@RETURN_ROWCOUNT BIT = 1,
	@debug BIT = 0,
	@RECORDS_DELETED INT output
AS
BEGIN
	IF (@debug = 0)
		SET NOCOUNT ON

	DECLARE @t TABLE (RID INT)
	DECLARE @RID INT

	SET @RECORDS_DELETED = 0

	-- remove all tasklists
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	INSERT INTO @t
	SELECT top (@COMMIT_LIMIT) FILTER_RID
	FROM [dbo].[FILTER]
	WHERE USER_RID = @UserRID

	SET @RECORDS_DELETED = @@rowcount

	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

	DECLARE @rowsDeleted INT;

	SET @rowsDeleted = (
			SELECT COUNT(*)
			FROM @t
			)

	SELECT TOP 1 @RID = RID
	FROM @t

	WHILE @@rowcount <> 0
	BEGIN
		EXEC [dbo].[SP_MID_FILTER_DELETE] @RID

		DELETE @t
		WHERE RID = @RID

		SELECT TOP 1 @RID = RID
		FROM @t
	END

	IF @RETURN_ROWCOUNT = 1 SELECT @rowsDeleted
END
GO


