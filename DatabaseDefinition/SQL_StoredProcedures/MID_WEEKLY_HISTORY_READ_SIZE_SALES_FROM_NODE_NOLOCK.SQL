--dv =============================================
--dv Create date: 1/10/2014
--dv Description: Returns weekly sales history for descendant sizes, grouped by store and size code
--dv History: 2/5/2015 TT#4298 -jsobek -Size selling posted under 'Reg' is not recognized when generating size curves for Total Sales
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WEEKLY_HISTORY_READ_SIZE_SALES_FROM_NODE_NOLOCK] 
	@TIME_ID_LIST TIME_ID_TYPE READONLY,
	@SELECTED_NODE_RID INT,
	@OLL_RID INT,
	@USE_REG_SALES BIT
AS
BEGIN
	SET NOCOUNT ON;
	--Get the store table count for hn mod
	DECLARE @STORE_TABLE_COUNT int; 
	SELECT @STORE_TABLE_COUNT=STORE_TABLE_COUNT FROM SYSTEM_OPTIONS 

	DECLARE @GEN_SIZE_CURVE_USING int;
	SELECT @GEN_SIZE_CURVE_USING=GEN_SIZE_CURVE_USING FROM SYSTEM_OPTIONS

	DECLARE @eGenerateSizeCurveUsing_Sales int = 801720;
	DECLARE @eGenerateSizeCurveUsing_InStockSales int = 801721;

	--Get the descendant size nodes, excluding hidden colors/sizes, excluding lowlevel model where flagged
	DECLARE @NODE_DATA TABLE 
	( 
		HN_RID int,
		HN_MOD int,
		SIZE_CODE_RID int,
		TIME_ID int
	)
	INSERT INTO @NODE_DATA
		 SELECT 
			q.HN_RID, 
			q.HN_RID % @STORE_TABLE_COUNT AS HN_MOD,
			sn.SIZE_CODE_RID,
			tl.TIME_ID
		 FROM
		 (
			 SELECT HN_RID 
			 FROM [dbo].[UDF_HIERARCHY_GET_SIZE_DESCENDANT_NODE_RIDS_NOLOCK](@SELECTED_NODE_RID) dn --ON dn.HN_RID=q.HN_RID 
			 --WHERE dn.HN_RID NOT IN (SELECT HN_RID FROM OVERRIDE_LL_MODEL_DETAIL WITH (NOLOCK) WHERE OLL_RID = @OLL_RID AND EXCLUDE_IND = '1')  -- TT#3790 - JSmith - Size Curve method with Override
			 WHERE dn.HN_RID NOT IN (SELECT HN_RID FROM [dbo].[UDF_OVERRIDE_GET_ALL_DESCENDANT_NODES](@OLL_RID) WHERE EXCLUDE_IND = '1')  -- TT#3790 - JSmith - Size Curve method with Override
		 ) as q
		 INNER JOIN SIZE_NODE sn WITH (NOLOCK) ON sn.HN_RID = q.HN_RID
		 CROSS JOIN @TIME_ID_LIST tl
	--Get the weekly sales history for the sizes
	DECLARE @HISTORY_DATA TABLE 
	( 
		ST_RID int,
		SIZE_CODE_RID int,
		SALES int
	)
	INSERT INTO @HISTORY_DATA
		SELECT 
			shw.ST_RID, 
			nd.SIZE_CODE_RID, 
			CASE 
			WHEN (@GEN_SIZE_CURVE_USING=@eGenerateSizeCurveUsing_Sales AND @USE_REG_SALES=1) THEN
				SUM((CASE WHEN COALESCE(shw.SALES_REG, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES_REG, 0) END) + (CASE WHEN COALESCE(shw.SALES_PROMO, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES_PROMO, 0) END)) 
			WHEN (@GEN_SIZE_CURVE_USING=@eGenerateSizeCurveUsing_Sales AND @USE_REG_SALES=0) THEN
				--SUM((CASE WHEN COALESCE(shw.SALES, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES, 0) END)) --Sales Unit Total
				SUM((CASE WHEN COALESCE(shw.SALES_REG, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES_REG, 0) END) + (CASE WHEN COALESCE(shw.SALES_PROMO, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES_PROMO, 0) END) + (CASE WHEN COALESCE(shw.SALES_MKDN, 0) < 0 THEN 0 ELSE COALESCE(shw.SALES_MKDN, 0) END))  --Always sum
			WHEN (@GEN_SIZE_CURVE_USING=@eGenerateSizeCurveUsing_InStockSales AND @USE_REG_SALES=1) THEN
				SUM((CASE WHEN COALESCE(shw.IN_STOCK_SALES_REG, 0) < 0 THEN 0 ELSE COALESCE(shw.IN_STOCK_SALES_REG, 0) END) + (CASE WHEN COALESCE(shw.IN_STOCK_SALES_PROMO, 0) < 0 THEN 0 ELSE COALESCE(shw.IN_STOCK_SALES_PROMO, 0) END)) 
			WHEN (@GEN_SIZE_CURVE_USING=@eGenerateSizeCurveUsing_InStockSales AND @USE_REG_SALES=0) THEN
				SUM((CASE WHEN COALESCE(shw.IN_STOCK_SALES, 0) < 0 THEN 0 ELSE COALESCE(shw.IN_STOCK_SALES, 0) END)) --Sales Unit Total
			END
			AS SALES
		FROM @NODE_DATA nd
		INNER JOIN VW_STORE_HISTORY_WEEK shw WITH (NOLOCK) ON shw.HN_MOD = nd.HN_MOD and shw.HN_RID=nd.HN_RID AND shw.ST_RID > 0 AND shw.TIME_ID = nd.TIME_ID
		GROUP BY ST_RID, SIZE_CODE_RID
	--Select sales greater than zero
	SELECT
		ST_RID,
		SIZE_CODE_RID,
		SALES
	FROM @HISTORY_DATA
	WHERE SALES > 0
	ORDER BY ST_RID, SIZE_CODE_RID
	RETURN 0
END
GO