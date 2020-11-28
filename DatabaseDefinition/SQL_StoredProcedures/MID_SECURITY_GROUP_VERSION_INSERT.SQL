--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Inserts a security group version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_VERSION_INSERT]
	@GROUP_RID int, 
	@FV_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int, 
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_GROUP_VERSION
		(
		GROUP_RID, 
		FV_RID, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
		)
	VALUES
		(
		@GROUP_RID, 
		@FV_RID, 
		@ACTION_ID, 
		@SEC_TYPE, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO
