--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates a row on the velocity grade table with mins and maxes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_UPDATE_WITH_MINMAX]
	@HN_RID int,
	@BOUNDARY int,
	@MINIMUM_STOCK int,
	@MAXIMUM_STOCK int,
	@MINIMUM_AD int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE VELOCITY_GRADE WITH (ROWLOCK) 
	SET 
		MINIMUM_STOCK = @MINIMUM_STOCK,
		MAXIMUM_STOCK = @MAXIMUM_STOCK,
		MINIMUM_AD = @MINIMUM_AD
	WHERE HN_RID = @HN_RID
	AND BOUNDARY = @BOUNDARY

	SELECT @@ROWCOUNT;

END
GO
