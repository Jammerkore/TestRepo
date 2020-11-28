--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Deletes enqueue data for a hierarchy branch
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_HIERARCHY_BRANCH]
	@PH_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_BRANCH_TYPE int
	SET @HIERARCHY_BRANCH_TYPE = 10

	DELETE FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE = @HIERARCHY_BRANCH_TYPE
		  AND PH_RID = @PH_RID
		  AND HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
