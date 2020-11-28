--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads size alternate primary sizes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALT_PRIMARY_SIZE_READ]
	@SIZE_ALTERNATE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SIZE_ALTERNATE_RID, 
		PRIMARY_SIZE_SEQ, 
		COALESCE(SIZES_RID, -1) SIZES_RID, 
		COALESCE(DIMENSIONS_RID, -1) DIMENSIONS_RID 
	FROM SIZE_ALT_PRIMARY_SIZE 
	WHERE SIZE_ALTERNATE_RID = @SIZE_ALTERNATE_RID
	ORDER BY PRIMARY_SIZE_SEQ

END
GO

