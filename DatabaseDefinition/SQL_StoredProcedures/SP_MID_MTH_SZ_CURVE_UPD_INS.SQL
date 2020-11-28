CREATE PROCEDURE [dbo].[SP_MID_MTH_SZ_CURVE_UPD_INS] 
	(
	@METHOD_RID INT,
	@SIZE_GROUP_RID INT = NULL,
	--Begin TT#1076 - JScott - Size Curves by Set
	@SIZE_CURVES_BY_TYPE INT = NULL,
	@SIZE_CURVES_BY_SG_RID INT = NULL,
	--End TT#1076 - JScott - Size Curves by Set
	@MERCH_BASIS_EQUAL_WEIGHT_IND INT = NULL,
	@CURVE_BASIS_EQUAL_WEIGHT_IND INT = NULL,
	/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
	/*	 @APPLY_LOST_SALES_IND int = null) */
	@APPLY_LOST_SALES_IND INT = NULL,
	@MINIMUM_AVERAGE FLOAT = NULL,
	@SALES_TOLERANCE FLOAT = NULL,
	@INDEX_UNITS_TYPE INT = NULL,
	@MIN_TOLERANCE FLOAT = NULL,
	@MAX_TOLERANCE FLOAT = NULL,
	@APPLY_MIN_TO_ZERO_TOLERANCE_IND CHAR = NULL
	) -- TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue)
	/* End TT#155 - JScott - Add Size Curve info to Node Properties */
AS
BEGIN
	SET NOCOUNT ON
	SET ANSI_PADDING ON

	/*****************************************************************************************/
	/* CREATED: 5/28/2009 - JOHN SMITH                                                       */
	/* PURPOSE: INSERTS OR UPDATES DATA IN METHOD_SIZE_CURVE                                 */
	/*****************************************************************************************/
	IF NOT EXISTS (
			SELECT *
			FROM METHOD_SIZE_CURVE
			WHERE METHOD_RID = @METHOD_RID
			)
		--INSERT
	BEGIN
		INSERT INTO METHOD_SIZE_CURVE (
			METHOD_RID,
			SIZE_GROUP_RID,
			--Begin TT#1076 - JScott - Size Curves by Set
			SIZE_CURVES_BY_TYPE,
			SIZE_CURVES_BY_SG_RID,
			--End TT#1076 - JScott - Size Curves by Set
			MERCH_BASIS_EQUAL_WEIGHT_IND,
			CURVE_BASIS_EQUAL_WEIGHT_IND,
			/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
			/*							APPLY_LOST_SALES_IND) */
			APPLY_LOST_SALES_IND,
			MINIMUM_AVERAGE,
			SALES_TOLERANCE,
			INDEX_UNITS_TYPE,
			MIN_TOLERANCE,
			MAX_TOLERANCE,
			APPLY_MIN_TO_ZERO_TOLERANCE_IND
			) -- TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			/* End TT#155 - JScott - Add Size Curve info to Node Properties */
		VALUES (
			@METHOD_RID,
			@SIZE_GROUP_RID,
			--Begin TT#1076 - JScott - Size Curves by Set
			@SIZE_CURVES_BY_TYPE,
			@SIZE_CURVES_BY_SG_RID,
			--End TT#1076 - JScott - Size Curves by Set
			@MERCH_BASIS_EQUAL_WEIGHT_IND,
			@CURVE_BASIS_EQUAL_WEIGHT_IND,
			/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
			/*							@APPLY_LOST_SALES_IND) */
			@APPLY_LOST_SALES_IND,
			@MINIMUM_AVERAGE,
			@SALES_TOLERANCE,
			@INDEX_UNITS_TYPE,
			@MIN_TOLERANCE,
			@MAX_TOLERANCE,
			@APPLY_MIN_TO_ZERO_TOLERANCE_IND
			) -- TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			/* End TT#155 - JScott - Add Size Curve info to Node Properties */
	END
	ELSE
		--UPDATE
	BEGIN
		UPDATE METHOD_SIZE_CURVE
		SET SIZE_GROUP_RID = @SIZE_GROUP_RID,
			--Begin TT#1076 - JScott - Size Curves by Set
			SIZE_CURVES_BY_TYPE = @SIZE_CURVES_BY_TYPE,
			SIZE_CURVES_BY_SG_RID = @SIZE_CURVES_BY_SG_RID,
			--End TT#1076 - JScott - Size Curves by Set
			MERCH_BASIS_EQUAL_WEIGHT_IND = @MERCH_BASIS_EQUAL_WEIGHT_IND,
			CURVE_BASIS_EQUAL_WEIGHT_IND = @CURVE_BASIS_EQUAL_WEIGHT_IND,
			/* Begin TT#155 - JScott - Add Size Curve info to Node Properties */
			/*					APPLY_LOST_SALES_IND = @APPLY_LOST_SALES_IND */
			APPLY_LOST_SALES_IND = @APPLY_LOST_SALES_IND,
			MINIMUM_AVERAGE = @MINIMUM_AVERAGE,
			SALES_TOLERANCE = @SALES_TOLERANCE,
			INDEX_UNITS_TYPE = @INDEX_UNITS_TYPE,
			MIN_TOLERANCE = @MIN_TOLERANCE,
			MAX_TOLERANCE = @MAX_TOLERANCE,
			APPLY_MIN_TO_ZERO_TOLERANCE_IND = @APPLY_MIN_TO_ZERO_TOLERANCE_IND -- TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			/* End TT#155 - JScott - Add Size Curve info to Node Properties */
		WHERE METHOD_RID = @METHOD_RID
	END

	SELECT @@ROWCOUNT
	SET NOCOUNT OFF
	SET ANSI_PADDING OFF
END
GO


