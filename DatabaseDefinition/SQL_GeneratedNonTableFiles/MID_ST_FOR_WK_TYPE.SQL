if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_FOR_WK_TYPE')
drop TYPE [dbo].[MID_ST_FOR_WK_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_FOR_WK_LOCK_TYPE')
drop TYPE [dbo].[MID_ST_FOR_WK_LOCK_TYPE]
GO
 
CREATE TYPE [dbo].[MID_ST_FOR_WK_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
  [ST_RID] [int],
  [SALES] [int], 
  [SALES_REG] [int], 
  [SALES_PROMO] [int], 
  [SALES_MKDN] [int], 
  [STOCK] [int], 
  [STOCK_REG] [int], 
  [STOCK_MKDN] [int], 
  [IN_STOCK_SALES] [int], 
  [IN_STOCK_SALES_REG] [int], 
  [IN_STOCK_SALES_PROMO] [int], 
  [IN_STOCK_SALES_MKDN] [int], 
  [ACCUM_SELL_THRU_SALES] [int], 
  [ACCUM_SELL_THRU_STOCK] [int], 
  [DAYS_IN_STOCK] [int], 
  [RECEIVED_STOCK] [int]
PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID)
)


GO


CREATE TYPE [dbo].[MID_ST_FOR_WK_LOCK_TYPE] AS TABLE(
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
  [ST_RID] [int],
  [SALES_LOCK] [char], 
  [SALES_REG_LOCK] [char], 
  [SALES_PROMO_LOCK] [char], 
  [SALES_MKDN_LOCK] [char], 
  [STOCK_LOCK] [char], 
  [STOCK_REG_LOCK] [char], 
  [STOCK_MKDN_LOCK] [char], 
  [IN_STOCK_SALES_LOCK] [char], 
  [IN_STOCK_SALES_REG_LOCK] [char], 
  [IN_STOCK_SALES_PROMO_LOCK] [char], 
  [IN_STOCK_SALES_MKDN_LOCK] [char], 
  [ACCUM_SELL_THRU_SALES_LOCK] [char], 
  [ACCUM_SELL_THRU_STOCK_LOCK] [char], 
  [DAYS_IN_STOCK_LOCK] [char], 
  [RECEIVED_STOCK_LOCK] [char]
PRIMARY KEY (HN_RID, FV_RID, TIME_ID, ST_RID)
)


GO


