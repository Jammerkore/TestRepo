--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Inserts a size basis substitute
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_BASIS_SUBSTITUTES_INSERT]
	@METHOD_RID int,
	@SIZE_TYPE_RID int,
	@SUBSTITUTE_RID int,
	@SIZE_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_SIZE_BASIS_SUBSTITUTES
		(
		METHOD_RID, 
		SIZE_TYPE_RID,
		SUBSTITUTE_RID, 
		SIZE_TYPE
		)
	VALUES
		(
		@METHOD_RID, 
		@SIZE_TYPE_RID,
		@SUBSTITUTE_RID, 
		@SIZE_TYPE
		)

	SELECT @@ROWCOUNT;
END
GO