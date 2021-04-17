if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_CHN_FOR_WK_WRITE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_CHN_FOR_WK_WRITE]
GO
 
