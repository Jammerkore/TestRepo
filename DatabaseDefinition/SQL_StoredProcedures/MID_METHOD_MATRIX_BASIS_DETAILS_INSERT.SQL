--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Inserts a row into the method matrix basis details table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_BASIS_DETAILS_INSERT]
	@METHOD_RID int, 
	@SEQ_ID int,
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float, 
	@IS_INCLUDED_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_MATRIX_BASIS_DETAILS
		(
		METHOD_RID, 
		SEQ_ID,
		FV_RID, 
		CDR_RID, 
		WEIGHT, 
		IS_INCLUDED_IND
		)
	VALUES
		(
		@METHOD_RID, 
		@SEQ_ID,
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT, 
		@IS_INCLUDED_IND
		)

	SELECT @@ROWCOUNT

END
GO
