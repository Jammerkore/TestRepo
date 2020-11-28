--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads rows from the application services history based on process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS]
	@SERVICE_EPROCESSES_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM APPLICATION_SERVICES_HISTORY
    WHERE SERVICE_EPROCESSES_ID = @SERVICE_EPROCESSES_ID

END
GO



