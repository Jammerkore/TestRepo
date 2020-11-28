--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Reads all state abbreviations
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STATE_PROVINCE_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SP_ABBREVIATION
	FROM STATE_PROVINCE 
	ORDER BY SP_ABBREVIATION

END
GO