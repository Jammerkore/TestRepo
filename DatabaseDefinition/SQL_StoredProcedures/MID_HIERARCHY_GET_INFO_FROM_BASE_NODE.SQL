--dv Create date:	9/4/2013
--dv Description:	Gets merchandise hierarchy node property data for the given base or style node.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_INFO_FROM_BASE_NODE]
	@HN_RID int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		vw.*,
		COALESCE(bn.BN_ID,' ') HN_ID, 
		COALESCE(bn.BN_NAME,' ') HN_NAME,
		COALESCE(bn.BN_DESCRIPTION,' ') DESCRIPTION, 
		COALESCE(bn.BN_PRODUCT_TYPE,-1) PRODUCT_TYPE
	FROM VW_HIERARCHY_GET_DATA vw
	INNER JOIN BASE_NODE bn ON bn.HN_RID = vw.HN_RID          
	WHERE vw.HN_RID=@HN_RID
                	
END
GO
