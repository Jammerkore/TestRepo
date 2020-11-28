--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads all rows from the application services history table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_SERVICES_HISTORY_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM APPLICATION_SERVICES_HISTORY

END
GO


