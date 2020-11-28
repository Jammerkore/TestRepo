--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts an Assortment Store Summary
--dv History:		8/13/2014 -TT#1268-MD -jsobek -5.4 Merge
--dv        		11/17/2017 - TT#2103-MD - JSmith - Matrix Need % Calc is wrong.  Should be using (Apply To Need *100 )divided by the Apply to Sales.  Currently is using the Basis Sales and not Apply To Sales.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_STORE_SUMMARY_INSERT]
	@HDR_RID int, 
	@ST_RID int, 
	@VARIABLE_NUMBER int, 
	@UNITS int, 
	@INTRANSIT int, 
	@NEED int, 
	@PCT_NEED float, 
	@VARIABLE_TYPE int,
	@ONHAND int,
	@VSW_ONHAND int,
	@PLAN_SALES_UNITS int,
	@PLAN_STOCK_UNITS int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_STORE_SUMMARY
		(
		HDR_RID, 
		ST_RID, 
		VARIABLE_NUMBER, 
		UNITS, 
		INTRANSIT, 
		NEED, 
		PCT_NEED, 
		VARIABLE_TYPE,
		ONHAND,
		VSW_ONHAND,
		PLAN_SALES_UNITS,
		PLAN_STOCK_UNITS
		)		
	VALUES
		(
		@HDR_RID, 
		@ST_RID, 
		@VARIABLE_NUMBER, 
		@UNITS, 
		@INTRANSIT, 
		@NEED, 
		@PCT_NEED, 
		@VARIABLE_TYPE,
		@ONHAND,
		@VSW_ONHAND,
		@PLAN_SALES_UNITS,
		@PLAN_STOCK_UNITS
		)		

	SELECT @@ROWCOUNT;
END

GO