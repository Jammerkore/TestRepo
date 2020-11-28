--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Read styles rids (parent of color) based on the color code passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_STYLES_FROM_COLOR_CODE]
	@COLOR_CODE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		hnj.PARENT_HN_RID AS SYTLE_HN_RID, 
		hnj.HN_RID AS COLOR_HN_RID
    FROM COLOR_NODE cn WITH (NOLOCK)
         INNER JOIN HIER_NODE_JOIN hnj WITH (NOLOCK) ON cn.HN_RID = hnj.HN_RID
    WHERE cn.COLOR_CODE_RID = @COLOR_CODE_RID

END
GO
