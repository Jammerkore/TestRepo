--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Inserts a hierarchy security user node action
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_INSERT]
	@USER_RID int, 
	@HN_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int, 
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_USER_HIERARCHY_NODE
		(
		USER_RID, 
		HN_RID, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
		)
	VALUES
		(
		@USER_RID, 
		@HN_RID, 
		@ACTION_ID, 
		@SEC_TYPE, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO
