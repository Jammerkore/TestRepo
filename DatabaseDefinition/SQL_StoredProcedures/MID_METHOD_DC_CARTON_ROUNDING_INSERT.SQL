--dv =============================================
--dv Create date: 06/26/2015
--dv Description: Insert a DC Carton Rounding method
--dv History:	  06/26/2015 TT#1652-MD - RMatelic - DC Carton Rounding
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_DC_CARTON_ROUNDING_INSERT] 
	@METHOD_RID int, 
	@APPLY_OVERAGE_TO char(1)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_DC_CARTON_ROUNDING
		(
		METHOD_RID, 
		APPLY_OVERAGE_TO
		)
	VALUES 
		(
		@METHOD_RID, 
		@APPLY_OVERAGE_TO 
		)

	SELECT @@ROWCOUNT;

END
GO







