--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Reads enqueue data for a store week/chain week for planning
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FOR_PLAN]
	@ENQUEUE_TYPE int,
	@HN_RID int,
	@FV_RID int,
	@START_WEEK int,
	@END_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE=@ENQUEUE_TYPE
		  AND HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
		  AND START_WEEK <= @END_WEEK
		  AND END_WEEK >= @START_WEEK

END
GO
