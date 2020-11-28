--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads VSW reverse on hand units for the header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VSW_REVERSE_ON_HAND_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		ST_RID, 
		VSW_REVERSE_ON_HAND_UNITS 
	FROM VSW_REVERSE_ON_HAND 
	WHERE HDR_RID = @HDR_RID

END
GO