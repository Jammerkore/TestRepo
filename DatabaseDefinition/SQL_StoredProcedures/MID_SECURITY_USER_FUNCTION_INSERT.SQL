--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Inserts a user security function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_FUNCTION_INSERT]
	@USER_RID int,
	@FUNC_ID int,
	@ACTION_ID int,
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_USER_FUNCTION
		(
		USER_RID, 
		FUNC_ID, 
		ACTION_ID, 
		SEC_LVL_ID
		)
	VALUES
		(
		@USER_RID, 
		@FUNC_ID, 
		@ACTION_ID, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO