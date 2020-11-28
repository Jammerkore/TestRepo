--dv =============================================
--dv Create date: 06/26/2015
--dv Description: Delete a DC Carton Rounding method
--dv History:	  06/26/2015 TT#1652-MD - RMatelic - DC Carton Rounding
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_DC_CARTON_ROUNDING_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_DC_CARTON_ROUNDING
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
	 
END
GO









