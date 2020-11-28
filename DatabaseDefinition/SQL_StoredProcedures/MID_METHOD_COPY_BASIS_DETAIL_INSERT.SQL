--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Inserts a row on the method copy basis detail table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_BASIS_DETAIL_INSERT]
	@METHOD_RID int,
	@SGL_RID int,
	@DETAIL_SEQ int,
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float, 
	@INCLUDE_EXCLUDE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_COPY_BASIS_DETAIL 
		(
		METHOD_RID, 
		SGL_RID, 
		DETAIL_SEQ,
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT, 
		INCLUDE_EXCLUDE
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@DETAIL_SEQ,
		@HN_RID, 
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT, 
		@INCLUDE_EXCLUDE
		)

	SELECT @@ROWCOUNT
END
GO