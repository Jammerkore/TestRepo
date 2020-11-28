--dv =============================================
--dv Create date:	8/14/2014
--dv Description:	Gets a license key 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_LICENSE_KEY_READ]
	@SYSTEM_OPTIONS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[UDF_MID_GET_LICENSE_KEY] (@SYSTEM_OPTIONS_RID) AS LICENSE_KEY

END
GO
