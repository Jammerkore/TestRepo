--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Inserts a row into the enqueue table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT]
	@ENQUEUE_TYPE int,
	@RID int,
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO MID_ENQUEUE(ENQUEUE_TYPE, RID, USER_RID, OWNING_THREADID) 
	VALUES (@ENQUEUE_TYPE, @RID, @USER_RID, @OWNING_THREADID)

	SELECT @@ROWCOUNT;
END
GO
