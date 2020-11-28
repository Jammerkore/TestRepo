--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes enqueue data for a store week/chain week for planning
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_PLAN]
	@ENQUEUE_TYPE int,
	@HN_RID int,
	@FV_RID int,
	@START_WEEK int,
	@END_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
		  AND START_WEEK = @START_WEEK
		  AND END_WEEK = @END_WEEK

	SELECT @@ROWCOUNT;

END
GO
