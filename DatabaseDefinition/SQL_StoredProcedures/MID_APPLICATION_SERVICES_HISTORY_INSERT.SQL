--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Inserts a row into the application services history table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_SERVICES_HISTORY_INSERT]
	@SERVICE_EPROCESSES_ID int, 
	@SERVICE_START_DATETIME datetime, 
	@SERVICE_VERSION varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO APPLICATION_SERVICES_HISTORY
		(
		SERVICE_EPROCESSES_ID, 
		SERVICE_START_DATETIME, 
		SERVICE_VERSION
		)
	VALUES
		(
		@SERVICE_EPROCESSES_ID, 
		@SERVICE_START_DATETIME, 
		@SERVICE_VERSION
		)
	
	SELECT @@ROWCOUNT;

END
GO
