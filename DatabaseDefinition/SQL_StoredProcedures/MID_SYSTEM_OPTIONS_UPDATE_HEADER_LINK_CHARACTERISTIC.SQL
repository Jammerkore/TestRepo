--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Updates the header link characteristic on the systems option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_UPDATE_HEADER_LINK_CHARACTERISTIC]
	@SYSTEM_OPTION_RID int,
	@HEADER_LINK_CHARACTERISTIC int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SYSTEM_OPTIONS 
	SET 
		HEADER_LINK_CHARACTERISTIC = @HEADER_LINK_CHARACTERISTIC
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID

	SELECT @@ROWCOUNT;

END
GO