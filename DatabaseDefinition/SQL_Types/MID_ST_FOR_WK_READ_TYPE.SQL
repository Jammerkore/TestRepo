CREATE TYPE [dbo].[MID_ST_FOR_WK_READ_TYPE] AS TABLE(
  [HN_MOD] [int],
  [HN_RID] [int],
  [FV_RID] [int],
  [TIME_ID] [int],

PRIMARY KEY (HN_MOD, HN_RID, FV_RID, TIME_ID)
)
GO