--dv =============================================
--dv Create date:	8/19/2015
--dv Description:	Returns the RID of the root node of the hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_ROOT_NODE_RID]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID 
	FROM HIERARCHY_NODE 
	WHERE HOME_PH_RID = @PH_RID
	  AND HOME_LEVEL = 0

END
GO
