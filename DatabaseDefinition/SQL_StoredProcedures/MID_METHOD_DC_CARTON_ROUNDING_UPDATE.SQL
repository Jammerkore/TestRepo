--dv =============================================
--dv Create date: 06/26/2015
--dv Description: Update a DC Carton Rounding method
--dv History:	  06/26/2015 TT#1652-MD - RMatelic - DC Carton Rounding
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_DC_CARTON_ROUNDING_UPDATE] 
	@METHOD_RID int, 
	@APPLY_OVERAGE_TO char(1)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_DC_CARTON_ROUNDING
	SET
		APPLY_OVERAGE_TO = @APPLY_OVERAGE_TO
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
	 
END
GO








