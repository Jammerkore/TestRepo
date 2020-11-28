--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads assignments for the group and function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP_AND_FUNCTION]
	@GROUP_RID int,
	@FUNC_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID
	FROM SECURITY_GROUP_FUNCTION
	WHERE GROUP_RID = @GROUP_RID
	      AND FUNC_ID = @FUNC_ID

END
GO

