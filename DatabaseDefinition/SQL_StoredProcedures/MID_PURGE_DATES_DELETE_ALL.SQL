--dv =============================================
--dv Create date:	1/30/2014
--dv Description:	Clears all purge dates
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;
	TRUNCATE TABLE PURGE_DATES
	SELECT @@ROWCOUNT;
END
GO