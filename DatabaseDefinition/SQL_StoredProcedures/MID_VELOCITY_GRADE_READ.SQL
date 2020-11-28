--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads a row from the velocity grade table based on the hierarchy rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		BOUNDARY, 
		GRADE_CODE, 
		COALESCE(MINIMUM_STOCK, -1) MINIMUM_STOCK, 
		COALESCE(MAXIMUM_STOCK, -1) MAXIMUM_STOCK, 
        COALESCE(MINIMUM_AD, -1) MINIMUM_AD 
	FROM VELOCITY_GRADE 
	WHERE HN_RID=@HN_RID
	ORDER BY BOUNDARY DESC

END
GO
