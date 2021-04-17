if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_FOR_WK_READ_LOCK_TYPE')
drop TYPE [dbo].[MID_CHN_FOR_WK_READ_LOCK_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_FOR_WK_READ_TYPE')
drop TYPE [dbo].[MID_CHN_FOR_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_HIS_WK_READ_TYPE')
drop TYPE [dbo].[MID_CHN_HIS_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_CHN_MOD_WK_READ_TYPE')
drop TYPE [dbo].[MID_CHN_MOD_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_FOR_WK_READ_LOCK_TYPE')
drop TYPE [dbo].[MID_ST_FOR_WK_READ_LOCK_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_FOR_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_FOR_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_HIS_DAY_READ_TYPE')
drop TYPE [dbo].[MID_ST_HIS_DAY_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_HIS_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_HIS_WK_READ_TYPE]
GO
 
if exists (select * from sys.types WHERE is_table_type = 1 AND name = 'MID_ST_MOD_WK_READ_TYPE')
drop TYPE [dbo].[MID_ST_MOD_WK_READ_TYPE]
GO
 
