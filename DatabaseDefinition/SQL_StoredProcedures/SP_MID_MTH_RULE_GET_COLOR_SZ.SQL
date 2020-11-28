--dv =============================================
--dv Create date:	3/31/2005 - STEVE TODD 
--dv Description:	ULTIMATELY RETURNS SIZE_CURVE_GROUP DATA FILTERED BY A SUPPLIED SIZE GROUP ID
--dv Usage:			EXEC SP_MID_MTH_RULE_GET_COLOR_SZ 305, 2, 101   
--dv History:		5/13/2015 -TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_MTH_RULE_GET_COLOR_SZ] 
	(
	@METHODRID INT,
	@ROWTYPEID INT,
	@SGRID INT,
	@SG_VERSION INT
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

	DECLARE @DefaultColorCodeRID INT,
		@DEFAULT_ROW_SGL_RID INT,
		@DefaultSizesRID INT,
		@DefaultDimensionsRID INT,
		@DefaultSizeCodeRID INT

	SELECT @DefaultColorCodeRID = - 1,
		@DEFAULT_ROW_SGL_RID = - 1,
		@DefaultSizesRID = - 1,
		@DefaultDimensionsRID = - 1,
		@DefaultSizeCodeRID = - 1

	CREATE TABLE #RULES (
		BAND_DSC VARCHAR(20) NULL,
		METHOD_RID INT NOT NULL,
		SGL_RID INT NOT NULL,
		COLOR_CODE_RID INT NOT NULL,
		SIZES_RID INT NOT NULL,
		DIMENSIONS_RID INT NOT NULL,
		SIZE_CODE_RID INT NOT NULL,
		SIZE_RULE VARCHAR(20) NULL,
		SIZE_QUANTITY INT NULL,
		ROW_TYPE_ID INT NOT NULL,
		SIZE_SEQ INT DEFAULT 0 NOT NULL
		)

	CREATE INDEX #RULES_IDX_1 ON #RULES (SGL_RID)

	/*FILL THE INITIAL VALUES IN THE TEMP TABLE*/
	INSERT INTO #RULES
	SELECT CASE @ROWTYPEID
			WHEN @AllColor
				THEN 'All Colors'
			ELSE NULL
			END AS BAND_DSC,
		MC.METHOD_RID,
		MC.SGL_RID,
		MC.COLOR_CODE_RID,
		MC.SIZES_RID,
		MC.DIMENSIONS_RID,
		MC.SIZE_CODE_RID,
		MC.SIZE_RULE,
		MC.SIZE_QUANTITY,
		MC.ROW_TYPE_ID,
		0
	FROM SIZE_MTH_RULE_COLOR_SIZE MC
	JOIN STORE_GROUP_JOIN SGL WITH (NOLOCK) ON SGL.SGL_RID = MC.SGL_RID  -- TT#1898-MD - JSmith - Version Testing - Select Workflow to view and receive System Arugument Exception
	WHERE METHOD_RID = @METHODRID
		AND MC.ROW_TYPE_ID = @ROWTYPEID
		AND SGL.SG_RID = @SGRID  -- TT#1898-MD - JSmith - Version Testing - Select Workflow to view and receive System Arugument Exception
		AND SGL.SG_VERSION = @SG_VERSION  -- TT#1898-MD - JSmith - Version Testing - Select Workflow to view and receive System Arugument Exception

	IF @ROWTYPEID = @AllColor
	BEGIN
		/*ADD ROWS SO ALL COLOR WILL BE DISPLAYED UNDER THE SETS THAT WERE NOT FOUND IN CONSTRAINT TABLES*/
		INSERT INTO #RULES
		SELECT 'All Colors',
			@METHODRID, --METHOD_RID
			SGL.SGL_RID, --SGL_RID
			@DefaultColorCodeRID, --COLOR_CODE_RID
			@DefaultSizesRID, --SIZES_RID
			@DefaultDimensionsRID, --DIMENSIONS_RID
			@DefaultSizeCodeRID, --SIZE_CODE_RID
			NULL, --SIZE_RULE
			NULL, --SIZE_QUANTITY
			@ROWTYPEID, --ROW INDICATOR
			0 --SIZE_SEQ
		FROM STORE_GROUP_JOIN SGL
		WHERE SGL.SG_RID = @SGRID
		  AND SGL.SG_VERSION = @SG_VERSION   -- TT#1896-MD - JSmith - Versioning_Test - Select Size Need or Fill Size with Static Attribute and recieve error message
			AND SGL.SGL_RID NOT IN (
				SELECT SGL_RID
				FROM #RULES
				)

		/*MAKE SURE ALL COLOR WILL BE THERE FOR THE DEFAULT ROW*/
		IF NOT EXISTS (
				SELECT *
				FROM #RULES
				WHERE SGL_RID = @DEFAULT_ROW_SGL_RID
				)
		BEGIN
			INSERT INTO #RULES
			SELECT 'All Colors',
				@METHODRID, --METHOD_RID
				@DEFAULT_ROW_SGL_RID, --SGL_RID
				@DefaultColorCodeRID, --COLOR_CODE_RID
				@DefaultSizesRID, --SIZES_RID
				@DefaultDimensionsRID, --DIMENSIONS_RID
				@DefaultSizeCodeRID, --SIZE_CODE_RID
				NULL, --SIZE_RULE
				NULL, --SIZE_QUANTITY
				@ROWTYPEID, --ROW INDICATOR
				0 --SIZE_SEQ
		END
	END

	SELECT *
	FROM #RULES

	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


