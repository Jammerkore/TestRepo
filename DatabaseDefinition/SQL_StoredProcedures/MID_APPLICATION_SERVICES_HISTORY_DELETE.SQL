--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Deletes a row from the application services history table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_SERVICES_HISTORY_DELETE]
	@SERVICE_EPROCESSES_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM APPLICATION_SERVICES_HISTORY WITH (ROWLOCK)
    WHERE SERVICE_EPROCESSES_ID = @SERVICE_EPROCESSES_ID
	
	SELECT @@ROWCOUNT;

END
GO

