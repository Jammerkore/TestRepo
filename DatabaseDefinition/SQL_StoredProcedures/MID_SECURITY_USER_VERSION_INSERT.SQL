--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Inserts a user security version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_VERSION_INSERT]
	@USER_RID int,
	@FV_RID int,
	@ACTION_ID int,
	@SEC_TYPE int,
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_USER_VERSION
		(
		USER_RID, 
		FV_RID, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
		)
	VALUES
		(
		@USER_RID, 
		@FV_RID, 
		@ACTION_ID, 
		@SEC_TYPE, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO