--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a row hierarchy join table based on product hierarchy, parent rid, and hn rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_JOIN_READ_RID_FOR_PARENT]
	@PH_RID int,
	@PARENT_HN_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT HN_RID 
	FROM HIER_NODE_JOIN 
	WHERE PH_RID = @PH_RID
		  AND PARENT_HN_RID = @PARENT_HN_RID
		  AND HN_RID = @HN_RID

END
GO
