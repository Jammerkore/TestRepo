--dv =============================================
--dv Create date: 5/7/2013
--dv Modified: 1/15/2014
--dv Description:	Retrieves both rid and variable data within store day history withing the specified time period
--dv				Start and end times are in Julian format (YYYYDDD) where DDD=0 at the begining of this year
--dv				Inclusive of both the start_time and end_time
--dv				Only includes active stores and active nodes
--dv				Excludes the hidden 'Unknown Colors' colors (COLOR_CODE_RID=0) and their descendant size nodes 
--dv Notes:		doing a nested query to join on the fewest rows possible
--dv Benchmark1:	(Just Day0 joined with active stores) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~3,200,000 rows in 27 seconds
--dv Benchmark2:	(Just Day0 joined with active stores and active size nodes) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~1,500,000 rows in 12 seconds
--dv Benchmark3:	(Day0 thru Day9 joined with active stores and active size nodes) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~15,200,000 rows in 6 minutes 31 seconds
--dv Benchmark4:	(Day0 thru Day9 joined with active stores and active size nodes separately) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~15,200,000 rows in 2 minutes 9 seconds (as stored procs it took 6 minutes 31 seconds)
--dv Benchmark5:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~523,000 rows in 22 seconds
--dv Benchmark6:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on tables directly without specifying starting node) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~1,523,000 rows in 42 seconds (so slower)
--dv Benchmark7:	(Just Day0 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101, specifiy columns needed) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~523,000 rows in 4 seconds (2nd pass)
--dv Benchmark8:	(Day0 thru Day9 joined with active stores and active size nodes without hidden colors joining on UDF and specifying starting node of 101, specifiy columns needed) On midretail29:DotsProdDev, 7 days(2013027 to 2013034) brought back ~4,900,000 rows in 4 minutes 39 seconds (1st pass)
--dv History:	1/15/2014 Using the store history day view (with no locks and primary keys) to increase performance
--dv =============================================
CREATE FUNCTION [dbo].[UDF_STORE_HISTORY_DAY_GET_DATA_IN_TIME_PERIOD_FROM_NODE] 
(	 
	@START_TIME INT, @END_TIME INT, @SELECTED_NODE_RID INT, @STORE_RID INT = -1 
) 
RETURNS TABLE  
AS 
RETURN  
( 
	--	      SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY0](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY1](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY2](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY3](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY4](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY5](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY6](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY7](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY8](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 
	--UNION ALL SELECT * FROM [dbo].[UDF_STORE_HISTORY_GET_SIZE_DATA_IN_TIME_PERIOD_FROM_NODE_DAY9](@START_TIME,@END_TIME,@SELECTED_NODE_RID,@STORE_RID) 

	SELECT  
		vw.HN_RID, 
		TIME_ID, 
		vw.ST_RID,  
		SALES, 
		SALES_REG, 
		SALES_PROMO, 
		SALES_MKDN, 
		STOCK, 
		STOCK_REG, 
		STOCK_MKDN 
	FROM [dbo].[UDF_HIERARCHY_GET_ACTIVE_SIZE_DESCENDANT_NODE_RIDS](@SELECTED_NODE_RID) dn 
	INNER JOIN [dbo].[VW_STORE_HISTORY_DAY] vw on vw.HN_MOD = dn.HN_MOD and vw.HN_RID = dn.HN_RID
	INNER JOIN [dbo].[UDF_STORE_GET_ACTIVE_RIDS]() st ON st.ST_RID=vw.ST_RID AND (@STORE_RID=-1 OR st.ST_RID=@STORE_RID) 	   
	WHERE TIME_ID BETWEEN @START_TIME AND @END_TIME 


)
GO