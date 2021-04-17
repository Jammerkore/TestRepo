if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_HIS_WK_TYPE')
drop TYPE [dbo].[MID_CHN_HIS_WK_TYPE]
GO
 
CREATE TYPE [dbo].[MID_CHN_HIS_WK_TYPE] AS TABLE(
  [HN_RID] [int],
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
PRIMARY KEY (HN_RID, TIME_ID)
)


GO


