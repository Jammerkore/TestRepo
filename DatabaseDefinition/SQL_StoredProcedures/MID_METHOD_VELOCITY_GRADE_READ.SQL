--dv =============================================
--dv Create date:	01/27/2014
--dv Description:	Reads a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_GRADE_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 as 'RowPosition',
		mvg.GRADE_CODE as 'GradeCode',
		mvg.BOUNDARY as 'Boundary',
		mvg.MINIMUM_STOCK as 'MinStock',
		mvg.MAXIMUM_STOCK as 'MaxStock',
		mvg.MINIMUM_AD as 'MinAd'
    FROM METHOD_VELOCITY_GRADE mvg
    WHERE METHOD_RID = @METHOD_RID
	ORDER BY 
		BOUNDARY DESC
END
GO