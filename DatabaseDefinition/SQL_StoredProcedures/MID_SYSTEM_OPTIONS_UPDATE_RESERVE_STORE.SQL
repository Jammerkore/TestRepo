--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Updates the reserve store on the systems option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_UPDATE_RESERVE_STORE]
	@SYSTEM_OPTION_RID int,
	@RESERVE_ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SYSTEM_OPTIONS 
		SET RESERVE_ST_RID = @RESERVE_ST_RID
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID

	SELECT @@ROWCOUNT;

END
GO