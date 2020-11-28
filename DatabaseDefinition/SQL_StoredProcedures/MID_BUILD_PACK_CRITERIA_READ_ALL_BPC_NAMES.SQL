--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads all criteria names
--dv =============================================
CREATE PROCEDURE [dbo].[MID_BUILD_PACK_CRITERIA_READ_ALL_BPC_NAMES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT BPC_NAME 
	FROM BUILD_PACK_CRITERIA

END
GO