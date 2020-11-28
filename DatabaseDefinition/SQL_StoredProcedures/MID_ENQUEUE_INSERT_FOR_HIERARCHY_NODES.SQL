--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Inserts enqueue data for the hierarchy nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT_FOR_HIERARCHY_NODES]
	@HN_RID int,
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_NODE_TYPE int
	SET @HIERARCHY_NODE_TYPE = 6

	INSERT INTO MID_ENQUEUE(ENQUEUE_TYPE, HN_RID, USER_RID, OWNING_THREADID) 
	VALUES (@HIERARCHY_NODE_TYPE, @HN_RID, @USER_RID, @OWNING_THREADID)

	SELECT @@ROWCOUNT;

END
GO
