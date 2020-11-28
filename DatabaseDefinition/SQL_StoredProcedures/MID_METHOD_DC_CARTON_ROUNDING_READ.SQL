--dv =============================================
--dv Create date: 06/26/2015
--dv Description: Read a DC Carton Rounding method
--dv History:	  06/26/2015 TT#1652-MD - RMatelic - DC Carton Rounding
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_DC_CARTON_ROUNDING_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COALESCE(APPLY_OVERAGE_TO,'0') as APPLY_OVERAGE_TO
	FROM METHOD_DC_CARTON_ROUNDING 
	WHERE METHOD_RID = @METHOD_RID

END
GO






