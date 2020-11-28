--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Inserts enqueue data for a hierarchy branch
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT_FOR_HIERARCHY_BRANCH]
	@PH_RID int,
	@HN_RID int,
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_BRANCH_TYPE int
	SET @HIERARCHY_BRANCH_TYPE = 10

	INSERT INTO MID_ENQUEUE(ENQUEUE_TYPE, PH_RID, HN_RID, USER_RID, OWNING_THREADID) 
	VALUES (@HIERARCHY_BRANCH_TYPE, @PH_RID, @HN_RID, @USER_RID, @OWNING_THREADID)

	SELECT @@ROWCOUNT;

END
GO
