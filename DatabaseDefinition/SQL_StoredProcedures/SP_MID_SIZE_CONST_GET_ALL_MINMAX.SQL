--dv =============================================
--dv Create date:	9/20/2013 - Steve Todd 
--dv Description:	Returns various tables for a dataset to construct Size Constraint Minmax data  
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_SIZE_CONST_GET_ALL_MINMAX] 
	(
	@SIZECONSTRAINTRID INT,
	@SGRID INT,
	@SG_VERSION INT   -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
	)
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON


	--Row types (defined in enums)
	DECLARE @AllColor INT,
		@Color INT,
		@AllColorSize INT,
		@ColorSize INT,
		@AllColorSizeDimension INT,
		@ColorSizeDimension INT,
		@DefaultRow INT

	SELECT @AllColor = 2,
		@Color = 3,
		@AllColorSize = 4,
		@ColorSize = 5,
		@AllColorSizeDimension = 6,
		@ColorSizeDimension = 7,
		@DefaultRow = 8

	EXEC SP_MID_SIZE_CONST_GET_GRPLVL @SIZECONSTRAINTRID,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@AllColor,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@AllColorSize,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@ColorSize,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@AllColorSizeDimension,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@ColorSizeDimension,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	/* Type 3 is intentionally called after type 7 due to FK constraints within the calling program */
	EXEC SP_MID_SIZE_CONST_GET_MINMAX @SIZECONSTRAINTRID,
		@Color,
		@SGRID,
	    @SG_VERSION  -- TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


