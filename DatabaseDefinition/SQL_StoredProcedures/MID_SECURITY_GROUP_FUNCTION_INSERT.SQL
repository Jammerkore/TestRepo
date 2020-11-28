--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Inserts a security group function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_INSERT]
	@GROUP_RID int, 
	@FUNC_ID int, 
	@ACTION_ID int, 
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_GROUP_FUNCTION
		(
		GROUP_RID, 
		FUNC_ID, 
		ACTION_ID, 
		SEC_LVL_ID
		)
	VALUES
		(
		@GROUP_RID, 
		@FUNC_ID, 
		@ACTION_ID, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO
