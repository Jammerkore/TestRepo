--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads IMO override methods
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_IMO_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		MOI.METHOD_RID, 
		MOI.ST_RID, 
		MOI.IMO_MIN_SHIP_QTY, 
		MOI.IMO_PCT_PK_THRSHLD, 
		MOI.IMO_MAX_VALUE, 
		MO.IMO_APPLY_VSW
    FROM METHOD_OVERRIDE_IMO MOI
    LEFT JOIN METHOD_OVERRIDE MO ON MOI.METHOD_RID = MO.METHOD_RID
    WHERE MOI.METHOD_RID = @METHOD_RID
    ORDER BY ST_RID

END
GO