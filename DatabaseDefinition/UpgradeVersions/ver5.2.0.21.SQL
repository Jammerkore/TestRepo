/*RELEASE 5.2.0.21 07-23-2013 -TT#739-MD -jsobek -Delete Stores -Post Conversion */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_ALOCTN_ARCHIVE_DELETE]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_ALOCTN_ARCHIVE_DELETE]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_HEADER]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_HEADER]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_BULK_COLOR_ALLOCATION]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_BULK_COLOR_ALLOCATION]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_BULK_SIZE_ALLOCATION]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_BULK_SIZE_ALLOCATION]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_HEADER_BULK_COLOR]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_HEADER_BULK_COLOR]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_HEADER_PACK]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_HEADER_PACK]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_PACK_ALLOCATION]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_PACK_ALLOCATION]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ARCHIVE_SUMMARY_ALLOCATION]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ARCHIVE_SUMMARY_ALLOCATION]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TOTAL_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TOTAL_ALLOCATION_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DETAIL_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DETAIL_ALLOCATION_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BULK_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[BULK_ALLOCATION_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PACK_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PACK_ALLOCATION_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[COLOR_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[COLOR_ALLOCATION_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[COLOR_SIZE_ALLOCATION_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[COLOR_SIZE_ALLOCATION_BIN]
GO

IF NOT EXISTS(SELECT * FROM SECURITY_FUNCTION_JOIN where FUNC_ID = 870560)
BEGIN 
	INSERT INTO SECURITY_FUNCTION_JOIN(FUNC_ID, FUNC_TYPE, FUNC_LEVEL, FUNC_PARENT) VALUES(870560, 0, 1, 870000);   --ReportsAllocationByStore TT#554-MD
	INSERT INTO SECURITY_FUNCTION_ACTIONS(FUNC_ID, ACTION_ID) VALUES(870560, 801160);   --ReportsAllocationByStore : Full Control -TT#554-MD
END
GO
