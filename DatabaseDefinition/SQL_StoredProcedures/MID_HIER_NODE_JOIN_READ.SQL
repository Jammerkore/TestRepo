--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads the hierarchy join table based on product hierarchy passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_JOIN_READ]
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_NODE_JOIN 
	WHERE PH_RID = @PH_RID
	ORDER BY PARENT_HN_RID

END
GO
