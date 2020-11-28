/*RELEASE 5.2.1.19 09-24-2014 -TT#4041 -jsobek -Node Properties Override report doesn't show decimals or FWOS Max Models */
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_GET_VSW_REPORT]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[SP_GET_VSW_REPORT]
GO
