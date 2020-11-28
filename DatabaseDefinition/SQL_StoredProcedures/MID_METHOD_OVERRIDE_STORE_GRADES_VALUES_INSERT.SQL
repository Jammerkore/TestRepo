--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts a grade value for the overrride method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_INSERT]
	@METHOD_RID int, 
	@BOUNDARY int, 
	@MINIMUM_STOCK int,
    @MAXIMUM_STOCK int, 
	@MINIMUM_AD int, 
	@MINIMUM_COLOR int, 
	@MAXIMUM_COLOR int, 
	@SHIP_UP_TO int, 
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_STORE_GRADES_VALUES 
		(
        METHOD_RID, 
		BOUNDARY, 
		MINIMUM_STOCK,
        MAXIMUM_STOCK, 
		MINIMUM_AD, 
		MINIMUM_COLOR, 
		MAXIMUM_COLOR, 
		SHIP_UP_TO, 
		SGL_RID
		)
	VALUES
		(
        @METHOD_RID, 
		@BOUNDARY, 
		@MINIMUM_STOCK,
        @MAXIMUM_STOCK, 
		@MINIMUM_AD, 
		@MINIMUM_COLOR, 
		@MAXIMUM_COLOR, 
		@SHIP_UP_TO, 
		@SGL_RID
		)

	SELECT @@ROWCOUNT;
END
GO