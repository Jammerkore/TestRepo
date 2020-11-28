--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a color node based on the product hierarchy, parent style and color ID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_READ_FOR_STYLE]
	@PH_RID int,
	@STYLE_HN_RID int,
	@COLOR_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		cn.HN_RID, cn.COLOR_DESCRIPTION
	FROM HIER_NODE_JOIN hnj WITH (NOLOCK), 
		 COLOR_NODE cn WITH (NOLOCK), 
		 COLOR_CODE cc WITH (NOLOCK) 
	WHERE hnj.PH_RID = @PH_RID
		  AND hnj.PARENT_HN_RID = @STYLE_HN_RID
		  AND cn.HN_RID = hnj.HN_RID
		  AND cc.COLOR_CODE_ID = @COLOR_CODE_ID
		  AND cn.COLOR_CODE_RID = cc.COLOR_CODE_RID

END
GO
