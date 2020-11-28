--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the velocity grade table with mins and maxes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VELOCITY_GRADE_INSERT_WITH_MINMAX]
	@HN_RID int,
	@BOUNDARY int,
	@MINIMUM_STOCK int,
	@MAXIMUM_STOCK int,
	@MINIMUM_AD int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO VELOCITY_GRADE(HN_RID, BOUNDARY, MINIMUM_STOCK, MAXIMUM_STOCK, MINIMUM_AD)
	VALUES (@HN_RID, @BOUNDARY, @MINIMUM_STOCK, @MAXIMUM_STOCK, @MINIMUM_AD)

	SELECT @@ROWCOUNT;

END
GO
