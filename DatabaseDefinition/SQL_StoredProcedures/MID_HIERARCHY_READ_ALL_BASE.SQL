--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Reads all base rows from the hierarchy (style and above)
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_ALL_BASE]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		vw.*,
		COALESCE(bn.BN_ID, ' ') HN_ID, 
		COALESCE(bn.BN_NAME, ' ') HN_NAME,
		COALESCE(bn.BN_DESCRIPTION, ' ') DESCRIPTION, 
		COALESCE(bn.BN_PRODUCT_TYPE,-1) PRODUCT_TYPE
	
	FROM VW_HIERARCHY_GET_DATA vw
		 INNER JOIN BASE_NODE bn ON bn.HN_RID = vw.HN_RID
    ORDER BY vw.HOME_LEVEL

END
GO
