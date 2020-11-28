--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Inserts a user into a group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_JOIN_INSERT]
	@GROUP_RID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO USER_GROUP_JOIN 
		(
		GROUP_RID, 
		USER_RID
		)
	VALUES
		(
		@GROUP_RID,
		@USER_RID
		)

	SELECT @@ROWCOUNT;
END
GO