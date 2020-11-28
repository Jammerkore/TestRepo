--dv =============================================
--dv Create date:	9/4/2013
--dv Description:	Gets child size node data for the given base node and hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_GET_CHILD_SIZE_INFO_FROM_NODE]
	@hierarchyRID int,
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
	INNER JOIN HIER_NODE_JOIN hnj ON hnj.HN_RID = vw.HN_RID  
    INNER JOIN SIZE_NODE sn ON hnj.HN_RID = sn.HN_RID
    INNER JOIN SIZE_CODE sc ON sn.SIZE_CODE_RID = sc.SIZE_CODE_RID
	WHERE hnj.PH_RID=@hierarchyRID AND hnj.PARENT_HN_RID=@HN_RID
		
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
END
GO
