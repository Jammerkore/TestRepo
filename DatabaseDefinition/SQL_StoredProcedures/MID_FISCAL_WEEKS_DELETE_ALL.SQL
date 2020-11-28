--dv =============================================
--dv Create date:	1/30/2014
--dv Description:	Clears all fiscal weeks
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FISCAL_WEEKS_DELETE_ALL]
AS
BEGIN
	SET NOCOUNT ON;
	TRUNCATE TABLE FISCAL_WEEKS
	SELECT @@ROWCOUNT;
END
GO
