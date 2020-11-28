--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Clears all virtual locks for global start
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VIRTUAL_LOCK_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE VIRTUAL_LOCK
	SELECT @@ROWCOUNT;
END
GO
