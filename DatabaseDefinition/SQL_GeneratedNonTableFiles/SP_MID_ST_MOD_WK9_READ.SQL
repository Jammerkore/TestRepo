CREATE PROCEDURE [dbo].[SP_MID_ST_MOD_WK9_READ]
@dt MID_ST_MOD_WK_READ_TYPE READONLY,
@Rollup CHAR(1) = NULL
AS
SET NOCOUNT ON
DECLARE @Tables INT,
        @Loop INT,
        @HN_TYPE INT,
        @HN_RID INT,
        @FV_RID INT,
        @HN_MOD INT,
        @ROLL_OPTION INT,
        @LoopCount INT,
        @NextLoopCount INT
SET @Tables = 10
SELECT DISTINCT @HN_TYPE = ph.PH_TYPE, @HN_RID = t.HN_RID, @HN_MOD = t.HN_RID % @Tables,
                @ROLL_OPTION = COALESCE(ph.HISTORY_ROLL_OPTION, 800251), @FV_RID = t.FV_RID
        FROM HIERARCHY_NODE hn (NOLOCK)
        JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
        JOIN @dt t on t.HN_RID = hn.HN_RID
 
 -- build temp table of values and locks for modified version
 create table #TEMPMOD 
 (HN_RID  int not null,
  FV_RID  int not null,
  TIME_ID int not null,
  ST_RID int not null,
    SALES int   null,
    SALES_REG int   null,
    SALES_PROMO int   null,
    SALES_MKDN int   null,
    STOCK int   null,
    STOCK_REG int   null,
    STOCK_MKDN int   null,
    IN_STOCK_SALES int   null,
    IN_STOCK_SALES_REG int   null,
    IN_STOCK_SALES_PROMO int   null,
    IN_STOCK_SALES_MKDN int   null,
    ACCUM_SELL_THRU_SALES int   null,
    ACCUM_SELL_THRU_STOCK int   null,
    DAYS_IN_STOCK int   null,
    RECEIVED_STOCK int   null,
    SALES_LOCK  char(1) null, 
    SALES_REG_LOCK  char(1) null, 
    SALES_PROMO_LOCK  char(1) null, 
    SALES_MKDN_LOCK  char(1) null, 
    STOCK_LOCK  char(1) null, 
    STOCK_REG_LOCK  char(1) null, 
    STOCK_MKDN_LOCK  char(1) null, 
    IN_STOCK_SALES_LOCK  char(1) null, 
    IN_STOCK_SALES_REG_LOCK  char(1) null, 
    IN_STOCK_SALES_PROMO_LOCK  char(1) null, 
    IN_STOCK_SALES_MKDN_LOCK  char(1) null, 
    ACCUM_SELL_THRU_SALES_LOCK  char(1) null, 
    ACCUM_SELL_THRU_STOCK_LOCK  char(1) null, 
    DAYS_IN_STOCK_LOCK  char(1) null, 
    RECEIVED_STOCK_LOCK  char(1) null)
  
 
 insert into #TEMPMOD
  select sfw.HN_RID, sfw.FV_RID, sfw.TIME_ID, sfw.ST_RID,
    sfw.SALES,   sfw.SALES_REG,   sfw.SALES_PROMO,   sfw.SALES_MKDN,   sfw.STOCK, 
    sfw.STOCK_REG,   sfw.STOCK_MKDN,   sfw.IN_STOCK_SALES,   sfw.IN_STOCK_SALES_REG, 
    sfw.IN_STOCK_SALES_PROMO,   sfw.IN_STOCK_SALES_MKDN,   sfw.ACCUM_SELL_THRU_SALES, 
    sfw.ACCUM_SELL_THRU_STOCK,   sfw.DAYS_IN_STOCK,   sfw.RECEIVED_STOCK, 
    COALESCE(sfwl.SALES_LOCK, 0) SALES_LOCK, 
    COALESCE(sfwl.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
    COALESCE(sfwl.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
    COALESCE(sfwl.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
    COALESCE(sfwl.STOCK_LOCK, 0) STOCK_LOCK, 
    COALESCE(sfwl.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
    COALESCE(sfwl.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_LOCK, 0) IN_STOCK_SALES_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_REG_LOCK, 0) IN_STOCK_SALES_REG_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_PROMO_LOCK, 0) IN_STOCK_SALES_PROMO_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_MKDN_LOCK, 0) IN_STOCK_SALES_MKDN_LOCK, 
    COALESCE(sfwl.ACCUM_SELL_THRU_SALES_LOCK, 0) ACCUM_SELL_THRU_SALES_LOCK, 
    COALESCE(sfwl.ACCUM_SELL_THRU_STOCK_LOCK, 0) ACCUM_SELL_THRU_STOCK_LOCK, 
    COALESCE(sfwl.DAYS_IN_STOCK_LOCK, 0) DAYS_IN_STOCK_LOCK, 
    COALESCE(sfwl.RECEIVED_STOCK_LOCK, 0) RECEIVED_STOCK_LOCK
   FROM @dt t
   JOIN VW_STORE_FORECAST_WEEK sfw (NOLOCK)
    on sfw.HN_RID = t.HN_RID
    and sfw.FV_RID = t.FV_RID
    and sfw.TIME_ID = t.TIME_ID
   left outer join STORE_FORECAST_WEEK_LOCK sfwl (NOLOCK)
    on sfwl.HN_RID = sfw.HN_RID
    and sfwl.FV_RID =sfw.FV_RID
    and sfwl.TIME_ID = sfw.TIME_ID
    and sfwl.ST_RID = sfw.ST_RID
  union
  select sfwl.HN_RID, sfwl.FV_RID, sfwl.TIME_ID, sfwl.ST_RID, 
    sfw.SALES,   sfw.SALES_REG,   sfw.SALES_PROMO,   sfw.SALES_MKDN,   sfw.STOCK, 
    sfw.STOCK_REG,   sfw.STOCK_MKDN,   sfw.IN_STOCK_SALES,   sfw.IN_STOCK_SALES_REG, 
    sfw.IN_STOCK_SALES_PROMO,   sfw.IN_STOCK_SALES_MKDN,   sfw.ACCUM_SELL_THRU_SALES, 
    sfw.ACCUM_SELL_THRU_STOCK,   sfw.DAYS_IN_STOCK,   sfw.RECEIVED_STOCK, 
    COALESCE(sfwl.SALES_LOCK, 0) SALES_LOCK, 
    COALESCE(sfwl.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
    COALESCE(sfwl.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
    COALESCE(sfwl.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
    COALESCE(sfwl.STOCK_LOCK, 0) STOCK_LOCK, 
    COALESCE(sfwl.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
    COALESCE(sfwl.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_LOCK, 0) IN_STOCK_SALES_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_REG_LOCK, 0) IN_STOCK_SALES_REG_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_PROMO_LOCK, 0) IN_STOCK_SALES_PROMO_LOCK, 
    COALESCE(sfwl.IN_STOCK_SALES_MKDN_LOCK, 0) IN_STOCK_SALES_MKDN_LOCK, 
    COALESCE(sfwl.ACCUM_SELL_THRU_SALES_LOCK, 0) ACCUM_SELL_THRU_SALES_LOCK, 
    COALESCE(sfwl.ACCUM_SELL_THRU_STOCK_LOCK, 0) ACCUM_SELL_THRU_STOCK_LOCK, 
    COALESCE(sfwl.DAYS_IN_STOCK_LOCK, 0) DAYS_IN_STOCK_LOCK, 
    COALESCE(sfwl.RECEIVED_STOCK_LOCK, 0) RECEIVED_STOCK_LOCK
 FROM @dt t
 JOIN STORE_FORECAST_WEEK_LOCK sfwl (NOLOCK)
  on sfwl.HN_RID = t.HN_RID
  and sfwl.FV_RID = t.FV_RID
  and sfwl.TIME_ID = t.TIME_ID
 left outer join VW_STORE_FORECAST_WEEK sfw (NOLOCK)
  on sfw.HN_RID = sfwl.HN_RID
  and sfw.FV_RID =sfwl.FV_RID
  and sfw.TIME_ID = sfwl.TIME_ID
  and sfw.ST_RID = sfwl.ST_RID
 
 -- create temp table for history values
 create table #TEMPHISTORY 
 (HN_RID  int not null,
  FV_RID  int not null,
  TIME_ID int not null,
  ST_RID int not null,
    SALES int null, 
    SALES_REG int null, 
    SALES_PROMO int null, 
    SALES_MKDN int null, 
    STOCK int null, 
    STOCK_REG int null, 
    STOCK_MKDN int null, 
    IN_STOCK_SALES int null, 
    IN_STOCK_SALES_REG int null, 
    IN_STOCK_SALES_PROMO int null, 
    IN_STOCK_SALES_MKDN int null, 
    ACCUM_SELL_THRU_SALES int null, 
    ACCUM_SELL_THRU_STOCK int null, 
    DAYS_IN_STOCK int null, 
    RECEIVED_STOCK int null)
 
 -- alternate and real time roll
IF @HN_TYPE = 800001 and @ROLL_OPTION = 800251
   BEGIN
   SET @LoopCount = 0
   CREATE TABLE #TREE (LOOPCOUNT INT NOT NULL, PARENT_HN_RID INT NOT NULL, HOME_PH_RID INT, PH_TYPE INT, CHILD_HN_RID INT NOT NULL, CHILD_HN_MOD INT NOT NULL)
   -- insert the children of the node into the temp table
   INSERT #TREE (LOOPCOUNT, PARENT_HN_RID, HOME_PH_RID, PH_TYPE, CHILD_HN_RID, CHILD_HN_MOD) 
       select @LoopCount as LOOPCOUNT, @HN_RID as PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables
         from HIER_NODE_JOIN hnj (NOLOCK)
           JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID
           JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
       where @HN_RID = hnj.PARENT_HN_RID
   SET @Loop = @@ROWCOUNT
   -- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf
   SET @NextLoopCount = @LoopCount + 1
   WHILE @Loop > 0
   BEGIN
      INSERT #TREE
        select @NextLoopCount as LOOPCOUNT, hnj.PARENT_HN_RID, hn.HOME_PH_RID, ph.PH_TYPE, hnj.HN_RID, hnj.HN_RID % @Tables
          from HIER_NODE_JOIN hnj (NOLOCK)
            JOIN HIERARCHY_NODE hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID
            JOIN PRODUCT_HIERARCHY ph (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
            JOIN #TREE t ON hnj.PARENT_HN_RID = t.CHILD_HN_RID
        WHERE t.LOOPCOUNT =  @LoopCount AND t.PH_TYPE <> 800000
      SET @Loop = @@ROWCOUNT
      SET @LoopCount = @LoopCount + 1
      SET @NextLoopCount = @LoopCount + 1
   END
   -- join with dates from xml selecting only nodes from the main hierarchy or lowest leaf alternates
   SELECT * 
     INTO #TEMP2
     FROM #TREE
     CROSS JOIN @dt
   where LOOPCOUNT = @LoopCount - 1
	     or PH_TYPE = 800000
       -- build temp table of summed history values 
       insert into #TEMPHISTORY 
       select @HN_RID as HN_RID, @FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, 
          SUM(shw.SALES) SALES, 
          SUM(shw.SALES_REG) SALES_REG, 
          SUM(shw.SALES_PROMO) SALES_PROMO, 
          SUM(shw.SALES_MKDN) SALES_MKDN, 
          SUM(shw.STOCK) STOCK, 
          SUM(shw.STOCK_REG) STOCK_REG, 
          SUM(shw.STOCK_MKDN) STOCK_MKDN, 
          SUM(shw.IN_STOCK_SALES) IN_STOCK_SALES, 
          SUM(shw.IN_STOCK_SALES_REG) IN_STOCK_SALES_REG, 
          SUM(shw.IN_STOCK_SALES_PROMO) IN_STOCK_SALES_PROMO, 
          SUM(shw.IN_STOCK_SALES_MKDN) IN_STOCK_SALES_MKDN, 
          SUM(shw.ACCUM_SELL_THRU_SALES) ACCUM_SELL_THRU_SALES, 
          SUM(shw.ACCUM_SELL_THRU_STOCK) ACCUM_SELL_THRU_STOCK, 
          SUM(shw.DAYS_IN_STOCK) DAYS_IN_STOCK, 
          SUM(shw.RECEIVED_STOCK) RECEIVED_STOCK
		FROM #TEMP2 t
	    JOIN VW_STORE_HISTORY_WEEK shw (NOLOCK) ON t.CHILD_HN_RID = shw.HN_RID
		    AND shw.TIME_ID = t.TIME_ID
	    GROUP BY shw.TIME_ID, ST_RID
 END 
 ELSE 
 BEGIN 
       -- build temp table of history values 
       insert into #TEMPHISTORY 
       select shw.HN_RID,  t.FV_RID as FV_RID, shw.TIME_ID, shw.ST_RID, 
          shw.SALES as SALES, 
          shw.SALES_REG as SALES_REG, 
          shw.SALES_PROMO as SALES_PROMO, 
          shw.SALES_MKDN as SALES_MKDN, 
          shw.STOCK as STOCK, 
          shw.STOCK_REG as STOCK_REG, 
          shw.STOCK_MKDN as STOCK_MKDN, 
          shw.IN_STOCK_SALES as IN_STOCK_SALES, 
          shw.IN_STOCK_SALES_REG as IN_STOCK_SALES_REG, 
          shw.IN_STOCK_SALES_PROMO as IN_STOCK_SALES_PROMO, 
          shw.IN_STOCK_SALES_MKDN as IN_STOCK_SALES_MKDN, 
          shw.ACCUM_SELL_THRU_SALES as ACCUM_SELL_THRU_SALES, 
          shw.ACCUM_SELL_THRU_STOCK as ACCUM_SELL_THRU_STOCK, 
          shw.DAYS_IN_STOCK as DAYS_IN_STOCK, 
          shw.RECEIVED_STOCK as RECEIVED_STOCK
  FROM @dt t
  JOIN VW_STORE_HISTORY_WEEK shw (NOLOCK)
    on shw.HN_RID = t.HN_RID
   and shw.TIME_ID = t.TIME_ID
 END 
 
  --select * from #TEMPMOD
  --select * from #TEMPHISTORY
   -- combine modified values with history
 select tmpmod.HN_RID, tmpmod.FV_RID, tmpmod.TIME_ID, tmpmod.ST_RID,
          COALESCE(COALESCE(tmpmod.SALES, tmphis.SALES), 0) SALES, 
          COALESCE(COALESCE(tmpmod.SALES_REG, tmphis.SALES_REG), 0) SALES_REG, 
          COALESCE(COALESCE(tmpmod.SALES_PROMO, tmphis.SALES_PROMO), 0) SALES_PROMO, 
          COALESCE(COALESCE(tmpmod.SALES_MKDN, tmphis.SALES_MKDN), 0) SALES_MKDN, 
          COALESCE(COALESCE(tmpmod.STOCK, tmphis.STOCK), 0) STOCK, 
          COALESCE(COALESCE(tmpmod.STOCK_REG, tmphis.STOCK_REG), 0) STOCK_REG, 
          COALESCE(COALESCE(tmpmod.STOCK_MKDN, tmphis.STOCK_MKDN), 0) STOCK_MKDN, 
          COALESCE(COALESCE(tmpmod.IN_STOCK_SALES, tmphis.IN_STOCK_SALES), 0) IN_STOCK_SALES, 
          COALESCE(COALESCE(tmpmod.IN_STOCK_SALES_REG, tmphis.IN_STOCK_SALES_REG), 0) IN_STOCK_SALES_REG, 
          COALESCE(COALESCE(tmpmod.IN_STOCK_SALES_PROMO, tmphis.IN_STOCK_SALES_PROMO), 0) IN_STOCK_SALES_PROMO, 
          COALESCE(COALESCE(tmpmod.IN_STOCK_SALES_MKDN, tmphis.IN_STOCK_SALES_MKDN), 0) IN_STOCK_SALES_MKDN, 
          COALESCE(COALESCE(tmpmod.ACCUM_SELL_THRU_SALES, tmphis.ACCUM_SELL_THRU_SALES), 0) ACCUM_SELL_THRU_SALES, 
          COALESCE(COALESCE(tmpmod.ACCUM_SELL_THRU_STOCK, tmphis.ACCUM_SELL_THRU_STOCK), 0) ACCUM_SELL_THRU_STOCK, 
          COALESCE(COALESCE(tmpmod.DAYS_IN_STOCK, tmphis.DAYS_IN_STOCK), 0) DAYS_IN_STOCK, 
          COALESCE(COALESCE(tmpmod.RECEIVED_STOCK, tmphis.RECEIVED_STOCK), 0) RECEIVED_STOCK, 
          COALESCE(tmpmod.SALES_LOCK, 0) SALES_LOCK, 
          COALESCE(tmpmod.SALES_REG_LOCK, 0) SALES_REG_LOCK, 
          COALESCE(tmpmod.SALES_PROMO_LOCK, 0) SALES_PROMO_LOCK, 
          COALESCE(tmpmod.SALES_MKDN_LOCK, 0) SALES_MKDN_LOCK, 
          COALESCE(tmpmod.STOCK_LOCK, 0) STOCK_LOCK, 
          COALESCE(tmpmod.STOCK_REG_LOCK, 0) STOCK_REG_LOCK, 
          COALESCE(tmpmod.STOCK_MKDN_LOCK, 0) STOCK_MKDN_LOCK, 
          COALESCE(tmpmod.IN_STOCK_SALES_LOCK, 0) IN_STOCK_SALES_LOCK, 
          COALESCE(tmpmod.IN_STOCK_SALES_REG_LOCK, 0) IN_STOCK_SALES_REG_LOCK, 
          COALESCE(tmpmod.IN_STOCK_SALES_PROMO_LOCK, 0) IN_STOCK_SALES_PROMO_LOCK, 
          COALESCE(tmpmod.IN_STOCK_SALES_MKDN_LOCK, 0) IN_STOCK_SALES_MKDN_LOCK, 
          COALESCE(tmpmod.ACCUM_SELL_THRU_SALES_LOCK, 0) ACCUM_SELL_THRU_SALES_LOCK, 
          COALESCE(tmpmod.ACCUM_SELL_THRU_STOCK_LOCK, 0) ACCUM_SELL_THRU_STOCK_LOCK, 
          COALESCE(tmpmod.DAYS_IN_STOCK_LOCK, 0) DAYS_IN_STOCK_LOCK, 
          COALESCE(tmpmod.RECEIVED_STOCK_LOCK, 0) RECEIVED_STOCK_LOCK
    into  #TEMPMOD2
 FROM #TEMPMOD tmpmod
 left outer JOIN #TEMPHISTORY tmphis  ON tmphis.HN_RID = tmpmod.HN_RID
 AND tmphis.FV_RID = tmpmod.FV_RID
 AND tmphis.TIME_ID = tmpmod.TIME_ID
 AND tmphis.ST_RID = tmpmod.ST_RID
 
 -- remove duplicate rows from history table
  delete #TEMPHISTORY 
    from #TEMPHISTORY, #TEMPMOD
    where #TEMPHISTORY.HN_RID = #TEMPMOD.HN_RID
         and #TEMPHISTORY.FV_RID = #TEMPMOD.FV_RID
         and #TEMPHISTORY.TIME_ID = #TEMPMOD.TIME_ID
         and #TEMPHISTORY.ST_RID = #TEMPMOD.ST_RID
 
 --select * from #TEMPHISTORY
 insert into #TEMPMOD2
 select tmphis.HN_RID, tmphis.FV_RID, tmphis.TIME_ID, tmphis.ST_RID,
          COALESCE(tmphis.SALES, 0) SALES, 
          COALESCE(tmphis.SALES_REG, 0) SALES_REG, 
          COALESCE(tmphis.SALES_PROMO, 0) SALES_PROMO, 
          COALESCE(tmphis.SALES_MKDN, 0) SALES_MKDN, 
          COALESCE(tmphis.STOCK, 0) STOCK, 
          COALESCE(tmphis.STOCK_REG, 0) STOCK_REG, 
          COALESCE(tmphis.STOCK_MKDN, 0) STOCK_MKDN, 
          COALESCE(tmphis.IN_STOCK_SALES, 0) IN_STOCK_SALES, 
          COALESCE(tmphis.IN_STOCK_SALES_REG, 0) IN_STOCK_SALES_REG, 
          COALESCE(tmphis.IN_STOCK_SALES_PROMO, 0) IN_STOCK_SALES_PROMO, 
          COALESCE(tmphis.IN_STOCK_SALES_MKDN, 0) IN_STOCK_SALES_MKDN, 
          COALESCE(tmphis.ACCUM_SELL_THRU_SALES, 0) ACCUM_SELL_THRU_SALES, 
          COALESCE(tmphis.ACCUM_SELL_THRU_STOCK, 0) ACCUM_SELL_THRU_STOCK, 
          COALESCE(tmphis.DAYS_IN_STOCK, 0) DAYS_IN_STOCK, 
          COALESCE(tmphis.RECEIVED_STOCK, 0) RECEIVED_STOCK, 
          0 as SALES_LOCK, 
          0 as SALES_REG_LOCK, 
          0 as SALES_PROMO_LOCK, 
          0 as SALES_MKDN_LOCK, 
          0 as STOCK_LOCK, 
          0 as STOCK_REG_LOCK, 
          0 as STOCK_MKDN_LOCK, 
          0 as IN_STOCK_SALES_LOCK, 
          0 as IN_STOCK_SALES_REG_LOCK, 
          0 as IN_STOCK_SALES_PROMO_LOCK, 
          0 as IN_STOCK_SALES_MKDN_LOCK, 
          0 as ACCUM_SELL_THRU_SALES_LOCK, 
          0 as ACCUM_SELL_THRU_STOCK_LOCK, 
          0 as DAYS_IN_STOCK_LOCK, 
          0 as RECEIVED_STOCK_LOCK
 from #TEMPHISTORY tmphis
 
 select * from #TEMPMOD2 order by TIME_ID, ST_RID
 
 OPTION (MAXDOP 1)


if (select object_id('tempdb.dbo.#TEMP2')) > 0 DROP TABLE #TEMP2
if (select object_id('tempdb.dbo.#TEMPMOD')) > 0 DROP TABLE #TEMPMOD
if (select object_id('tempdb.dbo.#TEMPMOD2')) > 0 DROP TABLE #TEMPMOD2
if (select object_id('tempdb.dbo.#TEMPHISTORY')) > 0 DROP TABLE #TEMPHISTORY




GO


