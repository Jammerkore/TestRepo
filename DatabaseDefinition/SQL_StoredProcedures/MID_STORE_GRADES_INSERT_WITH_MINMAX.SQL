--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row on the store grades table with mins and maxes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_INSERT_WITH_MINMAX]
	@HN_RID int,
	@BOUNDARY int,
	@MINIMUM_STOCK int,
	@MAXIMUM_STOCK int,
	@MINIMUM_AD int,
	@MINIMUM_COLOR int,
	@MAXIMUM_COLOR int,
	@SHIP_UP_TO int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STORE_GRADES(HN_RID, BOUNDARY, MINIMUM_STOCK, MAXIMUM_STOCK, MINIMUM_AD, MINIMUM_COLOR, MAXIMUM_COLOR, SHIP_UP_TO) 
	VALUES (@HN_RID, @BOUNDARY, @MINIMUM_STOCK, @MAXIMUM_STOCK, @MINIMUM_AD, @MINIMUM_COLOR, @MAXIMUM_COLOR, @SHIP_UP_TO)

	SELECT @@ROWCOUNT;

END
GO
