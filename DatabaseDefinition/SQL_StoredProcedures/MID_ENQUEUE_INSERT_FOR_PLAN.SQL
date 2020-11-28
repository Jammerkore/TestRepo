--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Inserts enqueue data for a store week/chain week for planning
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT_FOR_PLAN]
	@ENQUEUE_TYPE int,
	@HN_RID int,
	@FV_RID int,
	@START_WEEK int,
	@END_WEEK int,
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO MID_ENQUEUE(ENQUEUE_TYPE, HN_RID, FV_RID, START_WEEK, END_WEEK, USER_RID, OWNING_THREADID) 
	VALUES (@ENQUEUE_TYPE, @HN_RID, @FV_RID, @START_WEEK, @END_WEEK, @USER_RID, @OWNING_THREADID)

	SELECT @@ROWCOUNT;
END
GO
