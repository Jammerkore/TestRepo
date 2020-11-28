--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads all rows on the method override IMO table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_IMO_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		MOI.[METHOD_RID], 
		MOI.[ST_RID], 
		MOI.[IMO_MIN_SHIP_QTY], 
		MOI.[IMO_PCT_PK_THRSHLD], 
		MOI.[IMO_MAX_VALUE], 
		MO.[IMO_APPLY_VSW]
    FROM METHOD_OVERRIDE_IMO MOI
    LEFT OUTER JOIN METHOD_OVERRIDE MO ON MOI.METHOD_RID = MO.METHOD_RID

END
GO
