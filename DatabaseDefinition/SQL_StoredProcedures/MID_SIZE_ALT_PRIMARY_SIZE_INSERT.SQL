--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Inserts a size alternate primary size
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALT_PRIMARY_SIZE_INSERT]
	@SIZE_ALTERNATE_RID int,
	@PRIMARY_SIZE_SEQ int, 
	@SIZES_RID int, 
	@DIMENSIONS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SIZE_ALT_PRIMARY_SIZE
		(
		SIZE_ALTERNATE_RID, 
		PRIMARY_SIZE_SEQ, 
		SIZES_RID, 
		DIMENSIONS_RID
		)
	VALUES 
		(
		@SIZE_ALTERNATE_RID, 
		@PRIMARY_SIZE_SEQ, 
		@SIZES_RID, 
		@DIMENSIONS_RID
		)

	SELECT @@ROWCOUNT;
END
GO

