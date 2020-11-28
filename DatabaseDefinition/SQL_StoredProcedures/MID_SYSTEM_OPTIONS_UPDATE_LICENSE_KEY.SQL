--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Updates the license key on the systems options table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_UPDATE_LICENSE_KEY]
	@SYSTEM_OPTION_RID int,
	@LICENSE_KEY varchar(140)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SYSTEM_OPTIONS 
	SET 
		LICENSE_KEY = @LICENSE_KEY
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID
		
	SELECT @@ROWCOUNT;

END
GO