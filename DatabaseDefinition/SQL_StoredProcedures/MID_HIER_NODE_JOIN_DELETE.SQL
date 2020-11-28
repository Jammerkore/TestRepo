--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Deletes a row from the hierarchy join table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_NODE_JOIN_DELETE]
	@PH_RID int,
	@PARENT_HN_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM HIER_NODE_JOIN WITH (ROWLOCK) 
	WHERE PH_RID = @PH_RID
	      AND PARENT_HN_RID = @PARENT_HN_RID
		  AND HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
