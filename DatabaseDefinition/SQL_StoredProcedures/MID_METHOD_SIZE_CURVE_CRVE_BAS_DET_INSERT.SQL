--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts size curve method basis details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_CRVE_BAS_DET_INSERT]
    @METHOD_RID int,
	@BASIS_SEQ int, 
	@SIZE_CURVE_GROUP_RID int, 
	@WEIGHT float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_SIZE_CURVE_CRVE_BAS_DET 
		(
		METHOD_RID, 
		BASIS_SEQ, 
		SIZE_CURVE_GROUP_RID, 
		WEIGHT
		)
	VALUES 
		(
		@METHOD_RID, 
		@BASIS_SEQ, 
		@SIZE_CURVE_GROUP_RID, 
		@WEIGHT
		)

	SELECT @@ROWCOUNT;
END
GO