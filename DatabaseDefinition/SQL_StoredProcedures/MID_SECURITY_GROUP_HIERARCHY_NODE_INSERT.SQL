--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Inserts a hierarchy node security gorup
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_INSERT]
	@GROUP_RID int, 
	@HN_RID int, 
	@ACTION_ID int, 
	@SEC_TYPE int, 
	@SEC_LVL_ID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SECURITY_GROUP_HIERARCHY_NODE
		(
		GROUP_RID, 
		HN_RID, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
		) 
	VALUES
		(
		@GROUP_RID, 
		@HN_RID, 
		@ACTION_ID, 
		@SEC_TYPE, 
		@SEC_LVL_ID
		)

	SELECT @@ROWCOUNT;

END
GO

