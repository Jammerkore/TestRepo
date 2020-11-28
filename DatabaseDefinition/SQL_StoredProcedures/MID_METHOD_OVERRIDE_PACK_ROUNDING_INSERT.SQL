--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts pack rounding for the overrride method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_PACK_ROUNDING_INSERT]
    @METHOD_RID int, 
	@PACK_MULTIPLE_RID int, 
	@PACK_ROUNDING_1ST_PACK_PCT float, 
	@PACK_ROUNDING_NTH_PACK_PCT float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_PACK_ROUNDING 
		(
        METHOD_RID, 
		PACK_MULTIPLE_RID, 
		PACK_ROUNDING_1ST_PACK_PCT, 
		PACK_ROUNDING_NTH_PACK_PCT
		)
    VALUES
		(
        @METHOD_RID, 
		@PACK_MULTIPLE_RID, 
		@PACK_ROUNDING_1ST_PACK_PCT, 
		@PACK_ROUNDING_NTH_PACK_PCT
		)

	SELECT @@ROWCOUNT;
END
GO