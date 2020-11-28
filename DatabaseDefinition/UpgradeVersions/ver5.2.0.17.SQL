/*RELEASE 5.2.0.17 07-01-2013 -TT#739-MD -jsobek -Delete Stores -Post Conversion */
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VARIABLE_MAP_BIN_TO_MODEL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[VARIABLE_MAP_BIN_TO_MODEL]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VARIABLE_MODEL_ENTRY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[VARIABLE_MODEL_ENTRY]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VARIABLE_MODEL]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[VARIABLE_MODEL]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_VARIABLE_MODEL_INSERT]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_VARIABLE_MODEL_INSERT]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STORE_WEEK_HISTORY_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[STORE_WEEK_HISTORY_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[STORE_DAY_HISTORY_BIN]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[STORE_DAY_HISTORY_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_UPDT_STORE_HISTWEEK_BIN]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_UPDT_STORE_HISTWEEK_BIN]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_UPDT_STORE_HISTDAY_BIN]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_UPDT_STORE_HISTDAY_BIN]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VSW_REVERSE_ONHAND]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[VSW_REVERSE_ONHAND]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_UPDT_VSW_OHND_REVERSE]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_UPDT_VSW_OHND_REVERSE]
GO


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_UPDT_ARCHV_SUM_ALOC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_UPDT_ARCHV_SUM_ALOC]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_UPDT_ARCHV_PACK_ALOC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_UPDT_ARCHV_PACK_ALOC]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_UPDT_ARCHV_COLR_ALOC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_UPDT_ARCHV_COLR_ALOC]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_MID_UPDT_ARCHV_COLRSZ_ALOC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_MID_UPDT_ARCHV_COLRSZ_ALOC]
GO
