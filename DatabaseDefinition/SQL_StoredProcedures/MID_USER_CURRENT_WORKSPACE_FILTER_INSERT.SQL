--dv =============================================
--dv Create date: 10/14/2014
--dv Description:	Inserts user's current workspace filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_CURRENT_WORKSPACE_FILTER_INSERT]
	@WORKSPACE_TYPE INT,
	@USER_RID INT,
	@FILTER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_CURRENT_WORKSPACE_FILTER
		(
		WORKSPACE_TYPE,
		USER_RID,
		FILTER_RID
		)
	VALUES 
		(
		@WORKSPACE_TYPE,
		@USER_RID,
		@FILTER_RID
		)

	SELECT @@ROWCOUNT
END
GO




















