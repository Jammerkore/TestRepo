--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Updates the delete store in progress indicator on the systems option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_UPDATE_DELETE_STORE_IN_PROGRESS]
	@SYSTEM_OPTION_RID int,
	@STORE_DELETE_IN_PROGRESS_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SYSTEM_OPTIONS 
		SET STORE_DELETE_IN_PROGRESS_IND = @STORE_DELETE_IN_PROGRESS_IND
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID

	SELECT @@ROWCOUNT;

END
GO