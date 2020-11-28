--dv =============================================
--dv Create date:	9/4/2013
--dv Description:	Gets merchandise hierarchy node property data for the given size node.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_INFO_FROM_SIZE_NODE]
	@HN_RID int
AS
BEGIN

	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT 
		vw.*,
		COALESCE(sn.SIZE_CODE_RID,-1) SIZE_CODE_RID,
		COALESCE(sc.SIZE_CODE_ID,' ') SIZE_CODE_ID
	FROM VW_HIERARCHY_GET_DATA vw
    INNER JOIN SIZE_NODE sn ON sn.HN_RID = vw.HN_RID
    INNER JOIN SIZE_CODE sc ON sn.SIZE_CODE_RID = sc.SIZE_CODE_RID        
	WHERE vw.HN_RID=@HN_RID
                
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
		
END
GO
