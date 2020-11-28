--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row from the size curve generate info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_GENERATE_INFO_INSERT]
	@PROCESS_RID int,
	@MTHDS_EXECUTED int, 
	@MTHDS_SUCCESSFUL int, 
	@MTHDS_FAILED int, 
	@MTHDS_NO_ACTION int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SIZE_CURVE_GENERATE_INFO
		(
		PROCESS_RID, 
		MTHDS_EXECUTED, 
		MTHDS_SUCCESSFUL, 
		MTHDS_FAILED, 
		MTHDS_NO_ACTION
		)
	VALUES 
		(
		@PROCESS_RID, 
		@MTHDS_EXECUTED, 
		@MTHDS_SUCCESSFUL, 
		@MTHDS_FAILED, 
		@MTHDS_NO_ACTION
		)

	SELECT @@ROWCOUNT;

END
GO
