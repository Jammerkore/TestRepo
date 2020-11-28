--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Clears all enqueue data for global start
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE MID_ENQUEUE
	SELECT @@ROWCOUNT;
END
GO
