--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a spread forecast method basis detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SPREAD_BASIS_DETAIL_INSERT] 
	@METHOD_RID int,
	@DETAIL_SEQ int,
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_SPREAD_BASIS_DETAIL
		(
		METHOD_RID, 
		DETAIL_SEQ,
		FV_RID, 
		CDR_RID, 
		WEIGHT
		)
	VALUES
		(
		@METHOD_RID, 
		@DETAIL_SEQ,
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT
		)

	SELECT @@ROWCOUNT;

END
GO



