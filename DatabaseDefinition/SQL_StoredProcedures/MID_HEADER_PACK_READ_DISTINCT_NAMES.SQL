--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Gets distinct header pack names
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_READ_DISTINCT_NAMES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		HDR_PACK_NAME 
	FROM HEADER_PACK

END
GO
