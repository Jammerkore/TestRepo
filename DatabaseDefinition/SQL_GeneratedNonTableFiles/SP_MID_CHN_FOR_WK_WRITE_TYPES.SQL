if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_FOR_WK_TYPE')
drop TYPE [dbo].[MID_CHN_FOR_WK_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_FOR_WK_LOCK_TYPE')
drop TYPE [dbo].[MID_CHN_FOR_WK_LOCK_TYPE]
GO
 
CREATE TYPE [dbo].[MID_CHN_FOR_WK_TYPE] AS TABLE(
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
  [SALES] [int], 
  [SALES_REG] [int], 
  [SALES_PROMO] [int], 
  [SALES_MKDN] [int], 
  [STOCK] [int], 
  [STOCK_REG] [int], 
  [STOCK_MKDN] [int], 
  [WOS] [real], 
  [WOS_REGPROMO] [real]
PRIMARY KEY (HN_RID, FV_RID, TIME_ID)
)
GO


CREATE TYPE [dbo].[MID_CHN_FOR_WK_LOCK_TYPE] AS TABLE(
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
  [SALES_LOCK] [char], 
  [SALES_REG_LOCK] [char], 
  [SALES_PROMO_LOCK] [char], 
  [SALES_MKDN_LOCK] [char], 
  [STOCK_LOCK] [char], 
  [STOCK_REG_LOCK] [char], 
  [STOCK_MKDN_LOCK] [char], 
  [WOS_LOCK] [char], 
  [WOS_REGPROMO_LOCK] [char]
PRIMARY KEY (HN_RID, FV_RID, TIME_ID)
)
GO


