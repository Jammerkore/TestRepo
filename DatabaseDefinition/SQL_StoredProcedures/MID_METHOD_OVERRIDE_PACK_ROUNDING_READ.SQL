--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads pack rounding for the override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_PACK_ROUNDING_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CASE
			CAST(PACK_MULTIPLE_RID as varchar)
        WHEN '-1'  THEN (SELECT TEXT_VALUE FROM APPLICATION_TEXT WHERE TEXT_CODE = 900678)
        ELSE CAST(PACK_MULTIPLE_RID as varchar) 
		END as PackMultipletext,
        PACK_ROUNDING_1ST_PACK_PCT AS FstPack,
        PACK_ROUNDING_NTH_PACK_PCT AS NthPack,
        PACK_MULTIPLE_RID AS PackMultiple
    FROM METHOD_OVERRIDE_PACK_ROUNDING
    WHERE METHOD_RID = @METHOD_RID

END
GO