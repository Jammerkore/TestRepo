--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Inserts enqueue data for a product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT_FOR_HIERARCHY]
	@PH_RID int,
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_TYPE int
	SET @HIERARCHY_TYPE = 5

	INSERT INTO MID_ENQUEUE(ENQUEUE_TYPE, PH_RID, USER_RID, OWNING_THREADID) 
	VALUES (@HIERARCHY_TYPE, @PH_RID, @USER_RID, @OWNING_THREADID)

	SELECT @@ROWCOUNT;

END
GO
