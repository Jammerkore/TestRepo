--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads group assignment nodes based on the group, node and security type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_READ_FROM_GROUP_NODE_AND_TYPE]
	@GROUP_RID int,
	@HN_RID int,
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eDatabaseSecurityTypes_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_LVL_ID, @eDatabaseSecurityTypes_NotSpecified) SEC_LVL_ID
	FROM SECURITY_GROUP_HIERARCHY_NODE
	WHERE GROUP_RID = @GROUP_RID 
		  AND HN_RID = @HN_RID
		  AND SEC_TYPE = @SEC_TYPE

END
GO

