--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads xref home rids for the product hierarchy for viewing hierarchies by dependency
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIERARCHY_READ_XREF]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		hn.HOME_PH_RID 
	FROM HIERARCHY_NODE hn, 
		 HIER_NODE_JOIN hnj 
	WHERE hnj.PH_RID = @PH_RID
		  AND hnj.HN_RID = hn.HN_RID
		  AND hn.HOME_PH_RID != @PH_RID

END
GO
