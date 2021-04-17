if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_HIS_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_HIS_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_HIS_DAY_READ_TYPE')
drop TYPE [dbo].[MID_ST_HIS_DAY_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_MOD_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_MOD_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_FOR_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_FOR_WK_READ_TYPE]
GO
 
CREATE TYPE [dbo].[MID_ST_HIS_WK_READ_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [TIME_ID] [int],
PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)
)


GO


CREATE TYPE [dbo].[MID_ST_HIS_DAY_READ_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [TIME_ID] [int],
PRIMARY KEY (HN_MOD, HN_RID, TIME_ID)
)


GO


CREATE TYPE [dbo].[MID_ST_MOD_WK_READ_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)
)


GO


CREATE TYPE [dbo].[MID_ST_FOR_WK_READ_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],
PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)
)


GO


