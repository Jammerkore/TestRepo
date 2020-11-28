--dv Create date:	9/4/2013
--dv Description:	Gets child base node data for the given base node and hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_CHILD_INFO_FROM_BASE_NODE]
	@hierarchyRID int,
	@HN_RID int
AS
BEGIN

	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT 
		vw.*,
		COALESCE(bn.BN_ID,' ') HN_ID, 
		COALESCE(bn.BN_NAME,' ') HN_NAME,
		COALESCE(bn.BN_DESCRIPTION,' ') DESCRIPTION, 
		COALESCE(bn.BN_PRODUCT_TYPE,-1) PRODUCT_TYPE
	FROM VW_HIERARCHY_GET_DATA vw
	INNER JOIN HIER_NODE_JOIN hnj ON hnj.HN_RID = vw.HN_RID  
	INNER JOIN BASE_NODE bn ON bn.HN_RID = vw.HN_RID          
	WHERE hnj.PH_RID=@hierarchyRID AND hnj.PARENT_HN_RID=@HN_RID
	  AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy
		
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
END
GO
