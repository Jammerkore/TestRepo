--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates a row on the store grades table with mins and maxes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_UPDATE_WITH_MINMAX]
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

	UPDATE STORE_GRADES WITH (ROWLOCK) 
	SET 
		MINIMUM_STOCK = @MINIMUM_STOCK,
		MAXIMUM_STOCK = @MAXIMUM_STOCK, 
		MINIMUM_AD = @MINIMUM_AD,
		MINIMUM_COLOR = @MINIMUM_COLOR,
		MAXIMUM_COLOR = @MAXIMUM_COLOR, 
		SHIP_UP_TO = @SHIP_UP_TO
	WHERE HN_RID = @HN_RID 
		  AND BOUNDARY = @BOUNDARY

	SELECT @@ROWCOUNT;

END
GO
